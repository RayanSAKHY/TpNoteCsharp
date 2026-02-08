using System;
using System.IO;
using System.Xml.Serialization;

namespace SerializationApp
{
    /// <summary>
    /// Sérialiseur XML avec chiffrement/déchiffrement via CryptoHelper.
    /// Sérialise en mémoire puis chiffre/déchiffre en fichier.
    /// </summary>
    public static class EncryptedXmlSerializer
    {
        /// <summary>
        /// Sérialise l'objet en XML puis chiffre et écrit le fichier.
        /// </summary>
        /// <typeparam name="T">Type de l'objet.</typeparam>
        /// <param name="filePath">Chemin complet du fichier de sortie.</param>
        /// <param name="obj">Objet à sérialiser.</param>
        /// <param name="password">Clé de chiffrement (vide => SID Windows).</param>
        public static void Save<T>(string filePath, T obj, string password)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            // Serialize to memory
            var xs = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                xs.Serialize(ms, obj);
                ms.Position = 0;
                CryptoHelper.EncryptStreamToFile(ms, filePath, password);
            }
        }

        /// <summary>
        /// Charge et déchiffre le fichier XML puis désérialise en T.
        /// Retourne default(T) en cas d'erreur.
        /// </summary>
        /// <typeparam name="T">Type attendu.</typeparam>
        /// <param name="filePath">Chemin du fichier chiffré.</param>
        /// <param name="password">Clé de déchiffrement.</param>
        /// <returns>Instance de T ou default(T) en cas d'erreur.</returns>
        public static T Load<T>(string filePath, string password)
        {
            // Backwards-compatible: return default(T) on any error
            T res;
            string err;
            if (TryLoad(filePath, password, out res, out err))
                return res;
            return default(T);
        }

        /// <summary>
        /// Tentative de chargement qui renvoie un bool et un message d'erreur lisible en cas d'échec.
        /// </summary>
        /// <typeparam name="T">Type attendu.</typeparam>
        /// <param name="filePath">Chemin du fichier chiffré.</param>
        /// <param name="password">Clé de déchiffrement.</param>
        /// <param name="result">Résultat désérialisé si true.</param>
        /// <param name="error">Code d'erreur si false.</param>
        /// <returns>true si succès, false sinon.</returns>
        public static bool TryLoad<T>(string filePath, string password, out T result, out string error)
        {
            result = default(T);
            error = null;

            MemoryStream ms;
            if (!CryptoHelper.TryDecryptFileToMemoryStream(filePath, password, out ms, out error))
            {
                // error contains FileNotFound, InvalidFormat, WrongKeyOrCorrupt, OtherError:...
                return false;
            }

            try
            {
                using (ms)
                {
                    var xs = new XmlSerializer(typeof(T));
                    result = (T)xs.Deserialize(ms);
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = "DeserializeError: " + ex.Message;
                result = default(T);
                return false;
            }
        }
    }
}