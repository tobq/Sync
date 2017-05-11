using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Sync
{
    static class AppData
    {
        static readonly public string AppDataFolder = Environment.ExpandEnvironmentVariables(@"%appdata%\Sync\");
        static readonly string settingsPath = new DirectoryInfo(AppDataFolder + "\\settings.json").FullName;
        public static List<string> Trashed = new List<string>();
        public static Dictionary<string, _File> Files = new Dictionary<string, _File>();
        public static string Path { get; set; }
        public static access Access { get; set; }
        public static client Client { get; set; }
        public static string SettingID { get; set; }
        public static string PageToken { get; set; }
        public static bool Save()
        {
            var tries = 10;
            do
            {
                try
                {
                    File.WriteAllText(settingsPath, JsonConvert.SerializeObject(new
                    {
                        Access = Access,
                        PageToken = PageToken,
                        SettingID = SettingID,
                        Files = Files,
                        Trashed = Trashed,
                        Path = Path
                    }));
                    return true;
                }
                catch { System.Threading.Thread.Sleep(2000 / tries); }
            }
            while (--tries > 0);
            return false;
        }
        public static bool Load(string ClientJson = "client.json")
        {
            try
            {
                Client = JsonConvert.DeserializeObject<client>(File.ReadAllText(ClientJson));
                var json = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(settingsPath));
                Files = json["Files"].ToObject<Dictionary<string, _File>>();
                PageToken = json["PageToken"].ToObject<string>();
                SettingID = json["SettingID"].ToObject<string>();
                Path = json["Path"].ToObject<string>();
                Trashed = json["Trashed"].ToObject<List<string>>();
                Access = json["Access"].ToObject<access>();
                return true;
            }
            catch { return false; }
        }
        public static bool Clear()
        {
            Access.refresh_token = null;
            Files = null;
            PageToken = null;
            Trashed.Clear();
            File.Delete(settingsPath);
            Program.Watcher.EnableRaisingEvents = false;
            return false;
        }
    }
    public class client
    {
        public string client_id;
        public string auth_uri;
        public string token_uri;
        public string client_secret;
        public string[] redirect_uris;
        public Uri GetAuth(string scopes)
        {
            return new Uri($"{AppData.Client.auth_uri}?client_id={client_id}&redirect_uri={redirect_uris[0]}&scope={scopes}&response_type=code");
        }
    }
    public class access
    {
        string Access_token;
        public string access_token
        {
            get
            {
                if (DateTime.Now.Subtract(created).Hours >= 1) return Refresh();
                return Access_token;
            }
            set { Access_token = value; }
        }
        public string refresh_token;
        public DateTime created;
        public string Refresh()
        {
            string response = Utils.Post(
                "https://accounts.google.com/o/oauth2/token",
                $"client_id={AppData.Client.client_id}&client_secret={AppData.Client.client_secret}&refresh_token={refresh_token}&grant_type=refresh_token"
            );
            created = DateTime.Now;
            return access_token = Utils.JExtract(response, "access_token");
        }
    }

    public enum StateCode
    {
        Error,
        Misc,
        Pending,
        OK
    }
    public struct folder { public StateCode State; public string Message, Path; }
    public class _File
    {
        public string ID;
        public string MD5;
    }
}