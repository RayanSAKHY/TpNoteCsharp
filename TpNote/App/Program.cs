using App;
using DataApp;
using Microsoft.Extensions.Logging;
using SerializationApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



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
     
        private static Utilisateur CreateUserFlow()
        {
            string u = Ask("Username : ");
            string p = Ask("Mot de passe (provisoire, en clair) : ");
            string nom = Ask("Nom : ");
            string prenom = Ask("Prenom : ");
            string email = Ask("Email : ");

            var user = new Utilisateur(u, p, nom, prenom, email, DateTime.Now, new Livre[0]);
            var repo = new UserRepository(user.Username);
            repo.SaveProfile(user);

            Console.WriteLine($"Utilisateur '{user.Username}' créé et sauvegardé dans :");
            Console.WriteLine("  " + PathManager.GetUserProfilePath(user.Username));
            return user;
        }

        private static bool LoginFlow(out UserRepository repo, out Utilisateur user)
        {
            repo = null; user = null;

            string u = Ask("Username : ");
            string p = Ask("Mot de passe : ");

            var r = new UserRepository(u);
            var existing = r.LoadProfile();
            if (existing == null)
            {
                Console.WriteLine("Utilisateur inconnu.");
                return false;
            }
            if (!string.Equals(existing.MotDePasse, p, StringComparison.Ordinal))
            {
                Console.WriteLine("Mot de passe incorrect (comparaison en clair, provisoire).");
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

                    case "3":
                        repo.SaveBooks(livres, SerializationFormat.Xml); 
                        Console.WriteLine("Sauvegardé (XML) dans : " + 
                            PathManager.GetUserBooksPath(username, ".xml"));
                        break;

                    case "4":
                        livres = repo.LoadBooks(SerializationFormat.Xml); 
                        Console.WriteLine($"Chargé (XML) : {livres.Count} livre(s).");
                        break;

                    case "5":
                        repo.SaveBooks(livres, SerializationFormat.Binary); 
                        Console.WriteLine("Sauvegardé (BIN) dans : " + 
                            PathManager.GetUserBooksPath(username, ".bin"));
                        break;

                    case "6":
                        livres = repo.LoadBooks(SerializationFormat.Binary); 
                        Console.WriteLine($"Chargé (BIN) : {livres.Count} livre(s).");
                        break;

                    default:
                        Console.WriteLine("Choix inconnu.");
                        break;
                }
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
                        if (LoginFlow(out var repo, out var logged))
                        {
                            BooksSubmenu(repo, logged.Username);
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
            logger.LogInformation("Hello World! Logging is {Description}.", "fun");

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
