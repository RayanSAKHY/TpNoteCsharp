using System;
using System.IO;

namespace App
{
    internal static class PathManager
    {
        /// <summary>
        /// Remonte depuis le dossier d'exécution jusqu'à trouver un .sln (racine solution).
        /// Retourne le chemin parent du dossier contenant le .sln trouvé.
        /// </summary>
        /// <returns>Chemin racine ou AppDomain.BaseDirectory si non trouvé.</returns>
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
        /// Retourne le dossier racine de la bibliothèque (créé s'il n'existe pas).
        /// Exemple: …/TpNoteCSharp/Bibliotheque
        /// </summary>
        /// <returns>Chemin du dossier Bibliotheque.</returns>
        public static string GetLibraryRoot()
        {
            string root = GetSolutionRootOrBase();
            string folder = Path.Combine(root, "Bibliotheque");
            Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        /// Sanitize le nom pour en faire un nom de fichier valide (remplace les caractères invalides).
        /// </summary>
        /// <param name="name">Nom brut.</param>
        /// <returns>Nom sanitizé.</returns>
        public static string Sanitize(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "unknown";
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Trim();
        }

        /// <summary>
        /// Retourne le chemin du dossier utilisateur WITHOUT creating it.
        /// Exemple: …/TpNoteCSharp/Bibliotheque/&lt;username&gt;
        /// </summary>
        /// <param name="username">Nom d'utilisateur brut.</param>
        /// <returns>Chemin du dossier utilisateur (pas de création de dossier).</returns>
        public static string GetUserFolderPath(string username)
        {
            string user = Sanitize(username);
            string folder = Path.Combine(GetLibraryRoot(), user);
            return folder;
        }

        /// <summary>
        /// Retourne le chemin du dossier utilisateur en s'assurant qu'il existe (crée si besoin).
        /// </summary>
        /// <param name="username">Nom d'utilisateur.</param>
        /// <returns>Chemin du dossier utilisateur (créé si nécessaire).</returns>
        public static string GetUserFolder(string username)
        {
            string folder = GetUserFolderPath(username);
            Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>
        /// Chemin vers le profil utilisateur (user.xml) — n'induit pas la création du dossier.
        /// </summary>
        /// <param name="username">Nom d'utilisateur.</param>
        /// <returns>Chemin complet du fichier de profil XML.</returns>
        public static string GetUserProfilePath(string username)
            => Path.Combine(GetUserFolderPath(username), "user.xml");

        /// <summary>
        /// Chemin vers le fichier livres de l'utilisateur (avec extension fournie) — n'induit pas la création du dossier.
        /// </summary>
        /// <param name="username">Nom d'utilisateur.</param>
        /// <param name="extension">Extension (.xml ou .bin).</param>
        /// <returns>Chemin complet du fichier livres.</returns>
        public static string GetUserBooksPath(string username, string extension /* .xml | .bin */)
            => Path.Combine(GetUserFolderPath(username), "livres" + extension);
    }
}