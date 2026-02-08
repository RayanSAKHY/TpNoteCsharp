// App/Diagnostics.cs
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace App
{
    /// <summary>
    /// Résultat structuré du diagnostic.
    /// </summary>
    internal sealed class FolderDiagResult
    {
        public string UsernameRaw { get; set; } = "";
        public string UsernameSanitized { get; set; } = "";
        public string LibraryRoot { get; set; } = "";
        public bool LibraryRootExists { get; set; }
        public string UserFolder { get; set; } = "";
        public bool UserFolderExists { get; set; }
        public string ParentFolder { get; set; } = "";
        public string[] ParentSubdirs { get; set; } = Array.Empty<string>();
        public string WitnessFile { get; set; } = "";
        public bool WitnessWritten { get; set; }
        public string WitnessError { get; set; } = "";
        public string ProcessIdentity { get; set; } = "";
        public string ProcessSid { get; set; } = "";
        public bool AclFetched { get; set; }
        public string AclError { get; set; } = "";

        public bool Success =>
            LibraryRootExists &&
            UserFolderExists &&
            WitnessWritten &&
            string.IsNullOrEmpty(WitnessError);
    }

    internal static class Diagnostics
    {
        /// <summary>
        /// Lance un diagnostic complet de création du dossier utilisateur.
        ///  - Crée le dossier user si besoin
        ///  - Écrit un fichier témoin dans le dossier
        ///  - Liste le parent et les sous-dossiers
        ///  - Tente de lire les ACL et l'identité du process
        /// </summary>
        public static FolderDiagResult TestUserFolderCreation(string username, bool verbose = true)
        {
            var result = new FolderDiagResult
            {
                UsernameRaw = username ?? "<null>",
                UsernameSanitized = PathManager.Sanitize(username)
            };

            try
            {
                result.LibraryRoot = PathManager.GetLibraryRoot();
                result.LibraryRootExists = Directory.Exists(result.LibraryRoot);

                // Force la création et récupère le chemin final
                result.UserFolder = PathManager.GetUserFolder(username);
                result.UserFolderExists = Directory.Exists(result.UserFolder);

                // Parent + sous-dossiers pour comparaison visuelle
                var parent = Directory.GetParent(result.UserFolder);
                result.ParentFolder = parent?.FullName ?? "<no parent>";
                if (parent != null && parent.Exists)
                {
                    result.ParentSubdirs = Directory.GetDirectories(result.ParentFolder);
                }

                // Fichier témoin
                try
                {
                    result.WitnessFile = Path.Combine(result.UserFolder, ".__witness.txt");
                    File.WriteAllText(result.WitnessFile, "witness " + DateTime.Now.ToString("s"));
                    result.WitnessWritten = true;
                }
                catch (Exception ex)
                {
                    result.WitnessError = $"{ex.GetType().Name} - {ex.Message}";
                    result.WitnessWritten = false;
                }

                // Identité process + ACL
                try
                {
                    var wi = WindowsIdentity.GetCurrent();
                    result.ProcessIdentity = wi?.Name ?? "<unknown>";
                    result.ProcessSid = wi?.User?.Value ?? "<unknown>";
                }
                catch (Exception ex)
                {
                    result.ProcessIdentity = "<error>";
                    result.ProcessSid = $"{ex.GetType().Name} - {ex.Message}";
                }

                try
                {
                    var di = new DirectoryInfo(result.UserFolder);
                    _ = di.GetAccessControl(); // On ne détaille pas les règles ici, on vérifie juste qu'on y a accès
                    result.AclFetched = true;
                }
                catch (Exception ex)
                {
                    result.AclFetched = false;
                    result.AclError = $"{ex.GetType().Name} - {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                // En cas d’exception globale inattendue, on capture dans WitnessError
                result.WitnessError = $"[GLOBAL] {ex.GetType().Name} - {ex.Message}";
                result.WitnessWritten = false;
            }

            if (verbose)
                Dump(result);

            return result;
        }

        /// <summary>
        /// Affiche un dump lisible en console.
        /// </summary>
        public static void Dump(FolderDiagResult r)
        {
            Console.WriteLine("=== DIAGNOSTIC: TestUserFolderCreation ===");
            Console.WriteLine($"User (brut)         : {r.UsernameRaw}");
            Console.WriteLine($"User (sanitized)    : {r.UsernameSanitized}");
            Console.WriteLine($"LibraryRoot         : {r.LibraryRoot}");
            Console.WriteLine($"LibraryRoot exists? : {r.LibraryRootExists}");
            Console.WriteLine($"UserFolder          : {r.UserFolder}");
            Console.WriteLine($"UserFolder exists?  : {r.UserFolderExists}");
            Console.WriteLine($"Parent              : {r.ParentFolder}");
            Console.WriteLine($"Subdirs sous parent : {r.ParentSubdirs.Length}");
            foreach (var d in r.ParentSubdirs) Console.WriteLine(" - " + d);
            Console.WriteLine($"Witness path        : {r.WitnessFile}");
            Console.WriteLine($"Witness written?    : {r.WitnessWritten}");
            if (!string.IsNullOrEmpty(r.WitnessError))
                Console.WriteLine($"Witness error       : {r.WitnessError}");
            Console.WriteLine($"Process identity    : {r.ProcessIdentity}");
            Console.WriteLine($"Process SID         : {r.ProcessSid}");
            Console.WriteLine($"ACL fetched?        : {r.AclFetched}");
            if (!string.IsNullOrEmpty(r.AclError))
                Console.WriteLine($"ACL error           : {r.AclError}");
            Console.WriteLine($"SUCCESS             : {r.Success}");
            Console.WriteLine("=== FIN DIAGNOSTIC ===");
        }

        /// <summary>
        /// Aide : lève une exception si le diagnostic échoue (pratique en debug/build scripts).
        /// </summary>
        public static void AssertOrThrow(FolderDiagResult r)
        {
            if (!r.Success)
            {
                throw new InvalidOperationException(
                    "Diagnostic de création de dossier utilisateur échoué. " +
                    $"LibraryRootExists={r.LibraryRootExists}, " +
                    $"UserFolderExists={r.UserFolderExists}, " +
                    $"WitnessWritten={r.WitnessWritten}, " +
                    $"WitnessError={r.WitnessError}"
                );
            }
        }
    }
}