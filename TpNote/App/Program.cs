using App;
using DataApp;
using Microsoft.Extensions.Logging;
using SerializationApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;



namespace App
{
    internal static class Program
    {

        private static string Ask(string label)
        {
            Console.Write(label);
            return Console.ReadLine();
        }

        private static void AddBook(List<Livre> _livres)
        {
            string titre = Ask("Titre: ");
            string auteur = Ask("Auteur: ");
            int isbn;
            while (!int.TryParse(Ask("ISBN (int): "), out isbn))
                Console.WriteLine("ISBN invalide. Reessaie.");

            string categorie = Ask("Categorie: ");
            DateTime anneePub;
            while (!DateTime.TryParse(Ask("Date publication (yyyy-mm-dd): "), out anneePub))
                Console.WriteLine("Date invalide. Reessaie.");

            var livre = new Livre(titre, auteur, anneePub, isbn, categorie, DateTime.Now);

            // 3) Ajouter à la liste en mémoire
            _livres.Add(livre);

            Console.WriteLine("Livre ajouté.");
        }

        private static void ListBooks(List<Livre> _livres)
        {
            if (_livres.Count == 0)
            {
                Console.WriteLine("(aucun livre)");
                return;
            }
            for (int i = 0; i < _livres.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {_livres[i]}");
            }
        }
     
        // Create user: prompt for key and allow retry on encryption error
        private static Utilisateur CreateUserFlow()
        {
            string u = Ask("Username : ");
            string p = Ask("Mot de passe (provisoire, en clair) : ");
            string nom = Ask("Nom : ");
            string prenom = Ask("Prenom : ");
            string email = Ask("Email : ");

            var user = new Utilisateur(u, p, nom, prenom, email, DateTime.Now, new Livre[0]);
            var repo = new UserRepository(user.Username);

            while (true)
            {
                string key = Ask("Encryption key (leave empty to use Windows SID): ");
                try
                {
                    repo.SaveProfile(user, key);
                    Console.WriteLine($"Utilisateur '{user.Username}' créé et sauvegardé dans :");
                    Console.WriteLine("  " + PathManager.GetUserProfilePath(user.Username));
                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la sauvegarde chiffrée : " + ex.Message);
                    var retry = Ask("Réessayer la sauvegarde avec une autre clé ? (o/n) : ");
                    if (!string.Equals(retry, "o", StringComparison.OrdinalIgnoreCase)) throw;
                }
            }
        }

        // Login: prompt key, on decryption failure propose retry
        private static bool LoginFlow(out UserRepository repo, out Utilisateur user, out string attemptedUser)
        {
            repo = null; user = null;
            attemptedUser = Ask("Username : ");
            string p = Ask("Mot de passe : ");

            var r = new UserRepository(attemptedUser);

            string key = Ask("Encryption key (leave empty to use Windows SID): ");
            Utilisateur existing = null;
            while (true)
            {
                try
                {
                    existing = r.LoadProfile(key);
                    break;
                }
                catch (DecryptionException dex)
                {
                    Console.WriteLine("Erreur de décryptage : " + dex.Message);
                    var retry = Ask("Réessayer avec une autre clé ? (o/n) : ");
                    if (string.Equals(retry, "o", StringComparison.OrdinalIgnoreCase))
                    {
                        key = Ask("Encryption key: ");
                        continue;
                    }
                    // user chose not to retry — treat as unknown
                    existing = null;
                    break;
                }
            }

            if (existing == null)
            {
                Console.WriteLine("Utilisateur inconnu.");
                return false;
            }
            if (!string.Equals(existing.MotDePasse, p, StringComparison.Ordinal) )
            {
                Console.WriteLine("Identifiant/mot de passe incorrect. Veuillez réessayerr.");
                return false;
            }

            repo = r; user = existing;
            Console.WriteLine("Connecté.");
            return true;
        }

        private static void BooksSubmenu(UserRepository repo, string username)
        {
            var livres = repo.getLivres();

            while (true)
            {
                Console.WriteLine("\nLivres —\n  1) Ajouter\n  2) Lister\n  3) Save XML\n  4) Load XML\n  5) Save BIN\n  6) Load BIN\n  0) Retour\n");
                Console.Write("> Choix: ");
                var sc = Console.ReadLine();
                if (sc == "0") break;

                switch (sc)
                {
                    case "1":
                        AddBook(livres);
                        break;

                    case "2":
                        ListBooks(livres);
                        break;

                    // Books submenu: handle load/save decryption errors with retry prompts
                    case "3":
                    {
                        while (true)
                        {
                            string key = Ask("Encryption key for save (leave empty to use Windows SID): ");
                            try
                            {
                                repo.SaveBooks(livres, SerializationFormat.Xml, key);
                                Console.WriteLine("Sauvegardé (XML) dans : " + PathManager.GetUserBooksPath(username, ".xml"));
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erreur lors de la sauvegarde chiffrée : " + ex.Message);
                                var retry = Ask("Réessayer la sauvegarde ? (o/n) : ");
                                if (!string.Equals(retry, "o", StringComparison.OrdinalIgnoreCase)) break;
                            }
                        }
                        break;
                    }
                    case "4":
                    {
                        while (true)
                        {
                            string key = Ask("Encryption key for load (leave empty to use Windows SID): ");
                            try
                            {
                                livres = repo.LoadBooks(SerializationFormat.Xml, key);
                                Console.WriteLine($"Chargé (XML) : {livres.Count} livre(s).");
                                break;
                            }
                            catch (DecryptionException dex)
                            {
                                Console.WriteLine("Erreur de décryptage : " + dex.Message);
                                var retry = Ask("Réessayer avec une autre clé ? (o/n) : ");
                                if (!string.Equals(retry, "o", StringComparison.OrdinalIgnoreCase)) break;
                            }
                        }
                        break;
                    }
                    case "5":
                    {
                        while (true)
                        {
                            string key = Ask("Encryption key for save (leave empty to use Windows SID): ");
                            try
                            {
                                repo.SaveBooks(livres, SerializationFormat.Binary, key);
                                Console.WriteLine("Sauvegardé (BIN) dans : " + PathManager.GetUserBooksPath(username, ".bin"));
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Erreur lors de la sauvegarde chiffrée : " + ex.Message);
                                var retry = Ask("Réessayer la sauvegarde ? (o/n) : ");
                                if (!string.Equals(retry, "o", StringComparison.OrdinalIgnoreCase)) break;
                            }
                        }
                        break;
                    }
                    case "6":
                    {
                        while (true)
                        {
                            string key = Ask("Encryption key for load (leave empty to use Windows SID): ");
                            try
                            {
                                livres = repo.LoadBooks(SerializationFormat.Binary, key);
                                Console.WriteLine($"Chargé (BIN) : {livres.Count} livre(s).");
                                break;
                            }
                            catch (DecryptionException dex)
                            {
                                Console.WriteLine("Erreur de décryptage : " + dex.Message);
                                var retry = Ask("Réessayer avec une autre clé ? (o/n) : ");
                                if (!string.Equals(retry, "o", StringComparison.OrdinalIgnoreCase)) break;
                            }
                        }
                        break;
                    }
                    default:
                        Console.WriteLine("Choix inconnu.");
                        break;
                }
            }
        }


        private static void DeleteUserFolder(string username)
        {
            try
            {
                // Use the non-creating path to avoid creating the folder when attempting to delete
                string userFolder = PathManager.GetUserFolderPath(username);

                // Sécurité : ne supprime pas en dehors de la racine Bibliotheque
                string root = PathManager.GetLibraryRoot();
                string fullFolder = Path.GetFullPath(userFolder)
                                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string fullRoot = Path.GetFullPath(root)
                                     .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                if (!fullFolder.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Suppression refusée : chemin hors de la bibliothèque.");
                    return;
                }

                if (!Directory.Exists(fullFolder))
                {
                    Console.WriteLine("Aucun dossier à supprimer pour cet utilisateur.");
                    return;
                }

                // Enlève l’attribut ReadOnly sur tous les fichiers
                foreach (var file in Directory.GetFiles(fullFolder, "*", SearchOption.AllDirectories))
                {
                    try { File.SetAttributes(file, FileAttributes.Normal); } catch { /* ignore */ }
                }

                Directory.Delete(fullFolder, recursive: true);
                Console.WriteLine($"Dossier utilisateur supprimé : {fullFolder}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du dossier utilisateur : " + ex.Message);
            }
        }
        public static void RunGui() 
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }

        public static void RunConsole()
        {
            int nbEssai = 3;
            while (true)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1) Créer utilisateur");
                Console.WriteLine("2) Connexion utilisateur");
                Console.WriteLine("0) Quitter");
                Console.Write("> Choix: ");
                var c = Console.ReadLine();

                if (c == "0") break;

                switch (c)
                {
                    case "1":
                        var user = CreateUserFlow();
                        break;

                    case "2":
                        if (LoginFlow(out var repo, out var logged,out var attemptedUser))
                        {
                            nbEssai = 3;
                            BooksSubmenu(repo, logged.Username);
                        }
                        else
                        {
                            nbEssai--;
                            Console.WriteLine("Nombre d'essai restant : " + nbEssai);
                            if (nbEssai <=0)
                            {
                                Console.WriteLine("Trop d'essais infructueux. Supression des fichiers associé.");
                                DeleteUserFolder(attemptedUser);
                                nbEssai = 3;
                            }
                        }
                    break;

                    default:
                        Console.WriteLine("Choix inconnu.");
                        break;
                }
            }
        }

        private static void Main()
        {
            //var diag = Diagnostics.TestUserFolderCreation("rayan", verbose: true);

            ILoggerFactory factory = factory = LoggerFactory.Create(builder => builder.AddConsole()); ;
            ILogger logger = factory.CreateLogger("Program");
            //logger.LogInformation("Hello World! Logging is {Description}.", "fun");

            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1) Interface graphique");
            Console.WriteLine("2) Interface Console");
            Console.Write("> Choix: ");
            var c = Console.ReadLine();

            switch (c)
            {
                case "1":
                    RunGui();
                    break;
                case "2":
                    RunConsole();
                    break;
                default:
                    Console.WriteLine("Choix inconnu.");
                    break;
            }
        }
    }
}
