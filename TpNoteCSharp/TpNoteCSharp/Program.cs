using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TpNoteCSharp.assets;
using Microsoft.Extensions.Logging;


namespace TpNoteCSharp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ILogger logger = factory.CreateLogger("Program");
            logger.LogInformation("Hello World! Logging is {Description}.", "fun");
            Livre livre = new Livre("Titre Exemple", "Auteur Exemple", new DateTime(2020, 1, 1), 1234567890, "Science", DateTime.Now);
            logger.LogInformation("\nLivre d'exemple "+livre.ToString());
            Application.Run(new Form1());
        }
    }
}
