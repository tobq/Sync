using System;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Drawing;
using Google.Apis.Services;
using System.Net;
using System.Drawing.Drawing2D;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Drive.v3;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.VisualBasic;

namespace Sync
{
    public partial class GIO
    {
        public Uri OAuth { get { return AppData.Client.GetAuth(Scopes); } }
        UserCredential credential;
        DriveService Service;
        public static User user;
        public static MD5 md5 = MD5.Create();
        const string Scopes = "email https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.appdata https://www.googleapis.com/auth/activity";
        public const string GAppMIME = "application/vnd.google-apps.";
        public const string FolderMIME = GAppMIME + "folder";


        public void Trash(string ID, bool val)
        {
            Service.Files.Update(new Google.Apis.Drive.v3.Data.File { Trashed = val }, ID).Execute();
        }
        public IOrderedEnumerable<Google.Apis.Drive.v3.Data.File> GetPrioritised()
        {
            var request = Service.Files.List();
            request.Q = $"appProperties has {{ key='prioritised' and value='true' }}";
            request.Fields = "files(name,id,appProperties,mimeType)";
            request.PageSize = 999;
            return request.Execute().Files.OrderBy(x => x.AppProperties["priority"]);
        }

        public static void AddOperation(Action action, StateNotification? cancelToken, int? priority)
        {
            var operation = new Operation { Action = action, Cancel = cancelToken, Priority = priority };
            lock (Queue)
            {
                if (priority.HasValue && Queue.Count > 0)
                {
                    var node = Queue.First;
                    while (node.Value.Priority <= priority)
                    {
                        if (node.Next == null)
                        {
                            Queue.AddLast(operation);
                            return;
                        }
                        node = node.Next;
                    }
                    Queue.AddBefore(node, operation);
                }
                else Queue.AddLast(operation);
            }
        }

        public void Rename(string ID, string newName)
        {
            Service.Files.Update(new Google.Apis.Drive.v3.Data.File { Name = newName }, ID).Execute();
        }
        public void Update(string path)
        {
            _File file;
            if (AppData.Files.TryGetValue(path, out file))
            {
                var check = Service.Files.Get(file.ID);
                check.Fields = "id, md5Checksum";
                var tries = 5;
                do
                {
                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var checksum = Utils.MD5String(md5.ComputeHash(stream));
                            stream.Position = 0;
                            Google.Apis.Drive.v3.Data.File File;
                            try { File = check.Execute(); }
                            catch
                            {
                                Upload(path, Path.GetDirectoryName(path));
                                return;
                            }
                            if (check.Execute().Md5Checksum == AppData.Files[path].MD5)
                            {
                                AppData.Files[path].MD5 = checksum;
                                Service.Files.Update(
                                    new Google.Apis.Drive.v3.Data.File(),
                                    AppData.Files[path].ID,
                                    stream,
                                    MIME.GetMimeType(Path.GetExtension(path))
                                ).Upload();
                                UpdateProperties(AppData.Files[path].ID, "lastMd5", checksum);
                                return;
                            }
                            Upload(path, AppData.Files[Path.GetDirectoryName(path)].ID);
                            var UNRESOLVED = Program.AddOngoing(1, StateCode.Error, $"There is a duplicate of: {Path.GetFileName(path)}, because it was remotely changed");
                            return;
                        }
                    }
                    catch (IOException) { Thread.Sleep(2000 / tries); }
                }
                while (--tries > 0);
            }
        }
        public void Move(string oldPath, string newPath)
        {
            var request = Service.Files.Update(
                new Google.Apis.Drive.v3.Data.File { Name = Path.GetFileName(newPath) },
                AppData.Files[oldPath].ID
            );
            request.RemoveParents = AppData.Files[Path.GetDirectoryName(oldPath)].ID;
            request.AddParents = AppData.Files[Path.GetDirectoryName(newPath)].ID;
            request.Fields = "id, md5Checksum";
            var moved = request.Execute();
            UpdateProperties(moved.Id, "lastMd5", moved.Md5Checksum);
            AppData.Files[newPath] = new _File
            {
                ID = moved.Id,
                MD5 = moved.Md5Checksum
            };
            AppData.Files.Remove(oldPath);
        }

        public string NewFolder(string name, string parentID)
        {
            var request = Service.Files.Create(new Google.Apis.Drive.v3.Data.File
            {
                Name = name,
                MimeType = FolderMIME,
                Parents = new List<string> { parentID }
            });
            request.Fields = "id";
            return request.Execute().Id;
        }

        public void FillRemote(string parentID, string path)
        {
            AppData.Files[path] = new _File { ID = parentID };
            foreach (string directory in Directory.GetDirectories(path))
                FillRemote(NewFolder(new DirectoryInfo(directory).Name, parentID), directory);

            foreach (var file in Directory.GetFiles(path))
                using (var cancel = Program.AddOngoing(1, StateCode.Pending, $"Uploading {Path.GetFileName(file)}"))
                    Upload(file, parentID);
        }
        public void Upload(string path, string parentID)
        {
            if (Directory.Exists(path))
                FillRemote(NewFolder(new DirectoryInfo(path).Name, parentID), path);

            else if (File.Exists(path))
            {
                var tries = 5;
                do
                {
                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var checksum = Utils.MD5String(md5.ComputeHash(stream));
                            var searchRequest = Service.Files.List();
                            searchRequest.Fields = "files(id, md5Checksum)";
                            searchRequest.Q = $"appProperties has {{ key='lastMd5' and value='{checksum}' }}";
                            foreach (var match in searchRequest.Execute().Files)
                                if (match.Md5Checksum == checksum)
                                {
                                    var request = Service.Files.Copy(
                                        new Google.Apis.Drive.v3.Data.File
                                        {
                                            Parents = new List<string> { AppData.Files[Path.GetDirectoryName(path)].ID },
                                            Name = Path.GetFileName(path)
                                        }, match.Id
                                    );
                                    request.Fields = "id, md5Checksum";
                                    var copy = request.Execute();
                                    UpdateProperties(copy.Id, "lastMd5", copy.Md5Checksum);
                                    AppData.Files[path] = new _File { ID = copy.Id, MD5 = copy.Md5Checksum };
                                    return;
                                }
                                else UpdateProperties(match.Id, "lastMd5", match.Md5Checksum);
                            stream.Position = 0;
                            var uploadRequest = Service.Files.Create(new Google.Apis.Drive.v3.Data.File
                            {
                                Name = Path.GetFileName(path),
                                Parents = new List<string> { parentID }
                            }, stream, MIME.GetMimeType(Path.GetExtension(path)));
                            uploadRequest.Fields = "id, md5Checksum";
                            uploadRequest.Upload();
                            AppData.Files[path] = new _File
                            {
                                ID = uploadRequest.ResponseBody.Id,
                                MD5 = uploadRequest.ResponseBody.Md5Checksum
                            };
                            UpdateProperties(uploadRequest.ResponseBody.Id, "lastMd5", uploadRequest.ResponseBody.Md5Checksum);
                            return;
                        }
                    }
                    catch (IOException) { Thread.Sleep(2000 / tries); }
                }
                while (--tries > 0);
                var UNRESOLVED = Program.AddOngoing(1, StateCode.Error, $"Failed to upload {Path.GetFileName(path)} as it could not be accessed");
            }
        }

        void GetRemoteItems(string id, ref List<string> itemList)
        {
            itemList.Add(id);
            var list = new List<string>();
            ForEachChild(id, child => GetRemoteItems(child.Id, ref list));
            itemList.AddRange(list);
        }

        void SyncLocal(string itemPath)
        {
            _File Item;

            if (AppData.Files.TryGetValue(itemPath, out Item))
            {
                var parentPath = Path.GetDirectoryName(itemPath);
                var item = Service.Files.Get(Item.ID).Execute();

                var itemList = new List<string>();

                if (Directory.Exists(itemPath))
                {
                    var name = new DirectoryInfo(itemPath).Name;
                    if (item.Name != name) Directory.Move(itemPath, Path.Combine(parentPath, item.Name));

                    ForEachChild(item.Id, child =>
                    {
                        string childPath = AppData.Files.FirstOrDefault(f => f.Value.ID == child.Id).Key;
                        if (childPath != null) SyncLocal(childPath);
                    });
                }
                else if (File.Exists(itemPath))
                {
                    var name = Path.GetFileName(itemPath);
                    if (item.Name != name) File.Move(itemPath, Path.Combine(parentPath, item.Name));

                    if (item.Trashed != true && item.Md5Checksum != Utils.MD5String(md5.ComputeHash(File.ReadAllBytes(itemPath))))
                        using (var stream = new FileStream(itemPath, FileMode.Create, FileAccess.Write))
                            Service.Files.Get(item.Id).Download(stream);
                }
                else Download(item, parentPath);
            }
        }
        public void Download(Google.Apis.Drive.v3.Data.File file, string parentPath)
        {
            if (!parentPath.StartsWith(AppData.Path)) return;
            var newDir = validPath(parentPath, file);
            if (file.MimeType == FolderMIME) FillLocal(newDir, file.Id);

            if (file.Md5Checksum == null) return;
            AppData.Files[newDir] = new _File { ID = file.Id, MD5 = file.Md5Checksum };
            var copyPath = AppData.Files.FirstOrDefault(copyMatch(file)).Key;
            if (!string.IsNullOrEmpty(copyPath))
                using (var copyNoti = Program.AddOngoing(0, StateCode.Pending, $"Copying {Path.GetFileName(copyPath)}"))
                {
                    File.Copy(copyPath, newDir);
                    return;
                }
            using (var stream = new FileStream(newDir, FileMode.Create, FileAccess.Write))
                Service.Files.Get(file.Id).Download(stream);
        }

        void validatePath(ref string path, Google.Apis.Drive.v3.Data.File item)
        {
            path = Utils.UniquePath(path);
            if (Path.GetFileName(path) != item.Name)
                Service.Files.Update(new Google.Apis.Drive.v3.Data.File
                { Name = Path.GetFileName(path) }, item.Id).Execute();
        }

        string validPath(string parentPath, Google.Apis.Drive.v3.Data.File item)
        {
            var vPath = Utils.UniquePath(Path.Combine(parentPath, item.Name));
            var x = Path.GetFileName(vPath);
            if (Path.GetFileName(vPath) != item.Name)
                Service.Files.Update(new Google.Apis.Drive.v3.Data.File
                { Name = Path.GetFileName(vPath) }, item.Id).Execute();
            return vPath;
        }
        void ForEachChild(string remoteID, Action<Google.Apis.Drive.v3.Data.File> action)
        {
            var request = Service.Files.List();
            request.PageSize = 999;
            request.Fields = "files(name,id,mimeType,md5Checksum)";
            request.Q = $"not trashed and '{remoteID}' in parents";
            do
            {
                var response = request.Execute();
                foreach (var item in response.Files) action(item);
                request.PageToken = response.NextPageToken;
            } while (request.PageToken != null);
        }
        public void FillLocal(string localPath, string remoteID)
        {
            AppData.Files[localPath] = new _File { ID = remoteID };
            Directory.CreateDirectory(localPath);
            ForEachChild(remoteID, item =>
            {
                if (item.MimeType == FolderMIME) FillLocal(validPath(localPath, item), item.Id);
                else if (item.Md5Checksum != null)
                    using (var noti = Program.AddOngoing(0, StateCode.Pending,
                        $"Downloading {item.Name}"))
                        Download(item, localPath);
            });
        }
        public string IDToName(string id)
        {
            try
            {
                return Service.Files.Get(id).Execute().Name;
            }
            catch { return null; }
        }
        public static void Logout(System.Windows.Forms.Form opener = null)
        {
            AddOperation(() =>
            {
                Canvas.Tray.Visible = false;
                Program.Ongoing[0].Clear();
                Program.Ongoing[1].Clear();
                Program.RecentlyDeleted = new List<string>();
                Program.Ongoing = new[] {
                    new Dictionary<long, StateInfo>(),
                    new Dictionary<long, StateInfo>()
                };

                AppData.Clear();
            }, null, null);
            Program.Login.show(opener ?? Program.Settings);
        }
        public int? GetPriority(string fileID)
        {
            var request = Service.Files.Get(fileID);
            request.Fields = "id, appProperties";
            string  prio ="";
            request.Execute().AppProperties?.TryGetValue("priority", out prio);
            int priority;
            return int.TryParse(prio, out priority) ? priority : (int?)null;
        }
        public IDictionary<string, string> GetSettings()
        {
            if (AppData.SettingID != null)
            {
                Google.Apis.Drive.v3.Data.File properties = null;
                try
                {
                    var getRequest = Service.Files.Get(AppData.SettingID);
                    getRequest.Fields = "appProperties";
                    properties = getRequest.Execute();
                }
                catch { }
                if (properties != null) return properties.AppProperties;
            }
            var request = Service.Files.List();
            request.Spaces = "appDataFolder";
            request.Q = "name = 'Sync.settings'";
            request.Fields = "files(id, appProperties)";
            var settingsFiles = request.Execute().Files;
            if (settingsFiles.Count > 0)
            {
                while (settingsFiles.Count > 1)
                {
                    Service.Files.Delete(settingsFiles[0].Id).Execute();
                    settingsFiles.RemoveAt(0);
                }
                AppData.SettingID = settingsFiles[0].Id;
                AppData.Save();
                return settingsFiles[0].AppProperties;
            }
            var createRequest = Service.Files.Create(new Google.Apis.Drive.v3.Data.File
            {
                Name = "Sync.settings",
                AppProperties = new Dictionary<string, string> { { "quietHours", "00000000" } },
                Parents = new List<string> { "appDataFolder" }
            });
            createRequest.Fields = "id, appProperties";
            var newSettings = createRequest.Execute();
            AppData.SettingID = newSettings.Id;
            AppData.Save();
            return newSettings.AppProperties;
        }
        public GIO()
        {
            Poller = new Thread(new ThreadStart(Poll));
            if (AppData.Load() && Logged)
            {
                AppData.Access.Refresh();
                AppData.Save();
                load();
            }
            Poller.Start();
        }
        public void UpdateProperties(string id, string key, string val)
        {
            Service.Files.Update(new Google.Apis.Drive.v3.Data.File
            {
                AppProperties = new Dictionary<string, string> { [key] = val }
            }, id).Execute();
        }

        public void UpdateProperties(string id, Dictionary<string, string> prop)
        {
            Service.Files.Update(new Google.Apis.Drive.v3.Data.File { AppProperties = prop }, id).Execute();
        }

        public void UpdateFolder(string id)
        {
            foreach (var priority in GetPrioritised()) UpdateProperties(priority.Id, "prioritised", "false");
            Service.Files.Update(new Google.Apis.Drive.v3.Data.File
            {
                AppProperties = new Dictionary<string, string> {
                    { "folder", id}
                }
            }, AppData.SettingID).Execute();
        }

        public void PatchRemote()
        {
            foreach (var path in AppData.Files.Keys.ToList())
                if (!File.Exists(path) && !Directory.Exists(path) && !AppData.Trashed.Contains(path))
                    Program.onDelete(null, new FileSystemEventArgs(WatcherChangeTypes.Changed, Path.GetDirectoryName(path), Path.GetFileName(path)));
            var items = new List<string>();
            Utils.GetLocalItems(AppData.Path, ref items);
            items.ForEach(item =>
            {
                var eventArgs = new FileSystemEventArgs(WatcherChangeTypes.Changed, Path.GetDirectoryName(item), Path.GetFileName(item));
                if (AppData.Files.ContainsKey(item)) Program.onChange(null, eventArgs);
                else Program.onCreate(null, eventArgs);
            });
        }

        public void Login(string successCode)
        {
            AppData.Access = JsonConvert.DeserializeObject<access>(Utils.Post(
                AppData.Client.token_uri,
                $"code={successCode}&client_id={AppData.Client.client_id}&client_secret={AppData.Client.client_secret}&redirect_uri={AppData.Client.redirect_uris[0]}&grant_type=authorization_code"
            ));
            AppData.Access.Refresh();
            AppData.Save();
            load();
            if (GetSettings().ContainsKey("folder"))
            {
                Program.ClearLocal(false, StateCode.Misc, "Set this computer's local folder");
                Program.StateChanged(new StateNotification { Folder = 1 });
            }
            else
            {
                Program.ClearDrive();
                Program.ClearLocal();
            }
        }
        void load()
        {
            try
            {
                credential = new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets { ClientId = AppData.Client.client_id, ClientSecret = AppData.Client.client_secret },
                }), "user", new TokenResponse { RefreshToken = AppData.Access.refresh_token });
                Service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Sync"
                });
                user = JsonConvert.DeserializeObject<User>(Utils.Post(
                    "https://www.googleapis.com/oauth2/v1/tokeninfo",
                    $"access_token={AppData.Access.access_token}"
                ));
            }
            catch { AppData.Clear(); }
        }
        public bool Logged
        {
            get { return AppData.Access?.refresh_token != null; }
        }
        public IList<Google.Apis.Drive.v3.Data.File> FolderList(string parent)
        {
            var request = Service.Files.List();
            request.PageSize = 999;
            request.Fields = "files(name,id,mimeType,shared)";
            request.Q = $"mimeType = '{FolderMIME}' and trashed = false and '{parent}' in parents";
            return request.Execute().Files;
        }
        public bool HasChildren(string parent)
        {
            if (parent == null) return false;
            var request = Service.Files.List();
            request.PageSize = 1;
            request.Fields = "files(id)";
            request.Q = $"trashed = false and '{parent}' in parents";
            return request.Execute().Files.Count > 0;
        }

        public class User
        {
            public string user_id;
            public string email;
            public Image getPhoto(int size = 100, bool crop = false, Color? fill = null)
            {
                var request = (HttpWebRequest)WebRequest.Create($"https://www.googleapis.com/plus/v1/people/{user_id}?access_token={AppData.Access.access_token}&fields=image");
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var URL = Utils.JExtract(responseString, "url");
                var s = URL.IndexOf("sz=");
                if (s > 0)
                {
                    var e = URL.IndexOf('&', s);
                    if (e > 0) URL = URL.Remove(s + 3, e - s - 3).Insert(s + 3, size.ToString());
                    else URL = URL.Remove(s + 3) + size;
                }
                else URL = $"{URL}&sz={size}";
                Image photo = Utils.GetImage(URL);
                if (crop)
                {
                    var wh = Math.Min(photo.Height, photo.Width);
                    Image cropped = new Bitmap(wh, wh);
                    Graphics canvas = Graphics.FromImage(cropped);
                    canvas.SmoothingMode = SmoothingMode.AntiAlias;
                    canvas.InterpolationMode = InterpolationMode.High;
                    canvas.CompositingQuality = CompositingQuality.HighQuality;
                    using (Brush brush = new SolidBrush(fill ?? Color.Transparent)) canvas.FillRectangle(brush, 0, 0, wh, wh);
                    GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(0, 0, wh, wh);
                    canvas.SetClip(path);
                    canvas.DrawImage(photo, 0, 0);

                    return cropped;
                }
                return photo;
            }
        }
        static Func<KeyValuePair<string, _File>, bool> copyMatch(Google.Apis.Drive.v3.Data.File toMatch)
        {
            return new Func<KeyValuePair<string, _File>, bool>((file) =>
            {
                return file.Value.MD5 == toMatch.Md5Checksum &&
                File.Exists(file.Key) &&
                file.Value.MD5 == Utils.MD5String(md5.ComputeHash(File.ReadAllBytes(file.Key)));
            });
        }
        public void SetQuietHours(string time = null)
        {
            string remoteQI = string.Empty;
            string QI = time ?? (GetSettings().TryGetValue("quietHours", out remoteQI) ? remoteQI : "00000000");
            QuietHours[0] = TimeSpan.ParseExact(QI.Substring(0, 2) + ":" + QI.Substring(2, 2), "c", null);
            QuietHours[1] = TimeSpan.ParseExact(QI.Substring(4, 2) + ":" + QI.Substring(6, 2), "c", null);
        }
    }
    public struct Operation
    {
        public Action Action;
        public StateNotification? Cancel;
        public int? Priority;
    }
}