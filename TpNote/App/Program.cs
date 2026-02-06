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
        private static readonly List<Livre> _livres = new List<Livre>();

        private static void Menu()
        {
            Console.WriteLine("\n--- BIBLIOTHEQUE (Demo Serialization) ---");
            Console.WriteLine("1) Ajouter un livre");
            Console.WriteLine("2) Lister les livres");
            Console.WriteLine("3) Sauvegarder (XML)");
            Console.WriteLine("4) Charger (XML)");
            Console.WriteLine("5) Sauvegarder (Binaire)");
            Console.WriteLine("6) Charger (Binaire)");
            Console.WriteLine("0) Quitter");
            Console.Write("> Choix: ");
        }

        private static string Ask(string label)
        {
            Console.Write(label);
            return Console.ReadLine();
        }

        private static void AddBook()
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
            _livres.Add(livre);
            Console.WriteLine("Livre ajouté.");
        }

        private static void ListBooks()
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

        private static void Save(SerializationFormat format)
        {
            var serializer = SerializerFactory.Create(format);
            string fileName = "livres" + serializer.DefaultExtension;
            string path = PathManager.CombineWithFolder(fileName);
            serializer.Save(path, _livres);
            Console.WriteLine($"Sauvegardé: {path}");
        }

        private static void Load(SerializationFormat format)
        {
            var serializer = SerializerFactory.Create(format);
            string fileName = "livres" + serializer.DefaultExtension;
            string path = PathManager.CombineWithFolder(fileName);
            var loaded = serializer.Load<List<Livre>>(path);
            if (loaded == null)
            {
                Console.WriteLine("Rien chargé (fichier absent ou vide).");
                return;
            }
            _livres.Clear();
            _livres.AddRange(loaded);
            Console.WriteLine($"Chargé: {path} ( {_livres.Count} livres )");
        }

        private static void Run()
        {
            while (true)
            {
                Menu();
                var choix = Console.ReadLine();
                switch (choix)
                {
                    case "1": AddBook(); break;
                    case "2": ListBooks(); break;
                    case "3": Save(SerializationFormat.Xml); break;
                    case "4": Load(SerializationFormat.Xml); break;
                    case "5": Save(SerializationFormat.Binary); break;
                    case "6": Load(SerializationFormat.Binary); break;
                    case "0": return;
                    default: Console.WriteLine("Choix inconnu."); break;
                }
            }
        }
    
    [STAThread]
        static void Main()
        {
            ILoggerFactory factory = null;
            ILogger logger = null; 

            try
            {
                factory = LoggerFactory.Create(builder => builder.AddConsole());
                logger = factory.CreateLogger("Program");
                logger.LogInformation("Hello World! Logging is {Description}.", "fun");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Livre livre = new Livre("Titre Exemple",
                                      "Auteur Exemple",
                                      new DateTime(2020, 1, 1),
                                      1234567890,
                                      "Science",
                                      DateTime.Now);

                if (logger != null)
                    logger.LogInformation("Livre d'exemple: {Livre}", livre.ToString());

                Application.Run(new Form1()); 
            }
            finally
            {
                if (factory != null)
                    factory.Dispose();
            }

            Console.WriteLine("Dossier de données: " + PathManager.GetLibraryFolder()); 
            Run();
        }
    }
}
