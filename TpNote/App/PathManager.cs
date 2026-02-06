using System;
using System.IO;

namespace App
{
    internal static class PathManager
    {
        public static string GetLibraryFolder()
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = Path.Combine(root, "TpNoteCSharp","Bibliotheque");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        public static string CombineWithFolder(string fileName)
        {
            return Path.Combine(GetLibraryFolder(), fileName);
        }
    }
}