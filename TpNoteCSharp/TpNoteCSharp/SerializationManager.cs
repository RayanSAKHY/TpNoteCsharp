using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TpNoteCSharp.assets
{
    internal static class SerializationManager<T>
    {
        private static readonly object _sync = new object();

        /// <summary>
        /// Sauvegarde une liste d'objets de type T dans un fichier XML.
        /// </summary>
        public static void Save(string filePath, List<T> items)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (items == null) throw new ArgumentNullException(nameof(items));

            var serializer = new XmlSerializer(typeof(List<T>));
            lock (_sync)
            {
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    serializer.Serialize(stream, items);
                }
            }
        }

        /// <summary>
        /// Charge une liste d'objets de type T depuis un fichier XML. Retourne une liste vide si le fichier n'existe pas
        /// ou si la désérialisation échoue.
        /// </summary>
        public static List<T> Load(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) return new List<T>();

            var serializer = new XmlSerializer(typeof(List<T>));
            lock (_sync)
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        return (List<T>)serializer.Deserialize(stream);
                    }
                    catch (InvalidOperationException)
                    {
                        return new List<T>();
                    }
                }
            }
        }
    }
}
