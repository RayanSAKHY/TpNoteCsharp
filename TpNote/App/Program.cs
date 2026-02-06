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
        }
    }
}
