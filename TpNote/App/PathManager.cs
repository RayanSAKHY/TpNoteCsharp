// App/PathManager.cs
using System;
using System.IO;

namespace App
{
    internal static class PathManager
    {
        /// <summary>
        ///     Remonte depuis le dossier d'exécution jusqu'à trouver un .sln (racine solution).
        /// </summary>
        public static string GetSolutionRootOrBase()
        {
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (dir != null)
            {
                var sln = Array.Find(dir.GetFiles("*.sln"), _ => true);
                var slnx = Array.Find(dir.GetFiles("*.slnx"), _ => true);
                if (sln != null) return dir.Parent?.FullName;
                dir = dir.Parent;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        ///     …/TpNoteCSharp/Biblioteque
        /// </summary>
        public static string GetLibraryRoot()
        {
            string root = GetSolutionRootOrBase();
            string folder = Path.Combine(root, "Bibliotheque");
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static string Sanitize(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "unknown";
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Trim();
        }

        /// <summary>
        ///     Returns the path for the user folder WITHOUT creating it.
        ///     …/TpNoteCSharp/Biblioteque/&lt;username&gt;
        /// </summary>
        public static string GetUserFolderPath(string username)
        {
            string user = Sanitize(username);
            string folder = Path.Combine(GetLibraryRoot(), user);
            return folder;
        }

        /// <summary>
        ///     Returns the path for the user folder and ensures it exists.
        /// </summary>
        public static string GetUserFolder(string username)
        {
            string folder = GetUserFolderPath(username);
            Directory.CreateDirectory(folder);
            return folder;
        }

        // NOTE: these path helpers now use the non-creating GetUserFolderPath
        // so read/load operations do not cause creation of the folder.
        public static string GetUserProfilePath(string username)
            => Path.Combine(GetUserFolderPath(username), "user.xml");

        public static string GetUserBooksPath(string username, string extension /* .xml | .bin */)
            => Path.Combine(GetUserFolderPath(username), "livres" + extension);
    }
}