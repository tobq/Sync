using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Runtime.InteropServices;
using Shell32;
using Microsoft.CSharp.RuntimeBinder;

namespace Sync
{
    static class Utils
    {
        public static Folder RecyclingBin = new Shell().NameSpace(10);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);


        public unsafe static bool SetBrowser()
        {
            int version, option = 3;
            int* optionPtr = &option;
            var ok = InternetSetOption(0, 81, new IntPtr(optionPtr), sizeof(int));

            using (var browser = new System.Windows.Forms.WebBrowser()) version = browser.Version.Major;
            if (version >= 11) option = 11001;
            else if (version == 10) option = 10001;
            else if (version == 9) option = 9999;
            else if (version == 8) option = 8888;
            else option = 7000;
            try
            {
                RegistryKey Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", option, RegistryValueKind.DWord);
                Key.Close();
            }
            catch { return false; }
            return ok;
        }
        public static string Post(string URL, string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL);
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
        public static string Cap(string str)
        {
            if (str == null) return null;
            if (str.Length > 1) return char.ToUpper(str[0]) + str.Substring(1);
            return str.ToUpper();
        }
        public static string JExtract(string JSON, string key)
        {
            JSON = Regex.Replace(JSON, @"\s", "");
            JSON = JSON.Substring(JSON.IndexOf("\"" + key + "\"") + key.Length + 4);
            return JSON.Substring(0, JSON.IndexOf("\""));
        }
        public static Image getImage(string url)
        {
            var wc = new WebClient();
            byte[] bytes = wc.DownloadData(url);
            using (var ms = new MemoryStream(bytes)) return Image.FromStream(ms);
        }
        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
        public static string Normalise(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToLowerInvariant();
        }
        public static string MD5String(byte[] md5)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < md5.Length; i++) sb.Append(md5[i].ToString("x2"));
            return sb.ToString();
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] bytes, byte[] Bytes, long count);
        public static bool HashComp(byte[] h, byte[] H) { return h.Length == H.Length && memcmp(h, H, h.Length) == 0; }

        static public string UniquePath(string path)
        {
            path = path.Trim();
            if (File.Exists(path) || Directory.Exists(path))
            {
                var parent = Path.GetDirectoryName(path);
                var extension = Path.GetExtension(path);
                var version = 1;

                var regex = Regex.Match(path, @"(.+) \((\d+)\)(\.\w+)?");
                var fileName = (regex.Success) ? regex.Groups[1].Value : Path.GetFileNameWithoutExtension(path);

                do path = Path.Combine(parent, $"{fileName} ({version++}){extension}");
                while (File.Exists(path) || Directory.Exists(path));
            }

            return path;
        }

        public static bool RestoreItem(string ItemPath)
        {
            foreach (FolderItem folderItem in RecyclingBin.Items())
            {
                var FileName = RecyclingBin.GetDetailsOf(folderItem, 0);
                if (Path.GetExtension(FileName) == "") FileName += Path.GetExtension(folderItem.Path);
                var FilePath = RecyclingBin.GetDetailsOf(folderItem, 1);
                if (ItemPath == Path.Combine(FilePath, FileName))
                {
                    folderItem.InvokeVerb("R&estore");
                    return true;
                }
            }
            return false;
        }
    }
}
