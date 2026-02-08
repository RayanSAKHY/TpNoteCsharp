using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationApp
{
    /// <summary>
    /// Sérialiseur binaire chiffré (utilise BinaryFormatter pour le TP).
    /// Sérialise en mémoire puis chiffre/déchiffre via CryptoHelper.
    /// </summary>
    public static class EncryptedBinarySerializer
    {
        /// <summary>
        /// Sérialise en binaire puis chiffre et écrit le fichier.
        /// </summary>
        /// <typeparam name="T">Type de l'objet à sérialiser.</typeparam>
        /// <param name="filePath">Chemin du fichier de sortie.</param>
        /// <param name="data">Données à sauvegarder.</param>
        /// <param name="password">Clé de chiffrement (vide => SID Windows).</param>
        public static void Save<T>(string filePath, T data, string password)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, data);
                ms.Position = 0;
                CryptoHelper.EncryptStreamToFile(ms, filePath, password);
            }
        }

        /// <summary>
        /// Charge et déchiffre le fichier binaire puis désérialise en T.
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
        /// Tentative de chargement binaire chiffré, renvoie un bool et un message d'erreur si échec.
        /// </summary>
        /// <typeparam name="T">Type attendu.</typeparam>
        /// <param name="filePath">Chemin du fichier chiffré.</param>
        /// <param name="password">Clé de déchiffrement.</param>
        /// <param name="result">Résultat désérialisé si true.</param>
        /// <param name="error">Code d'erreur si false.</param>
        /// <returns>true si succès.</returns>
        public static bool TryLoad<T>(string filePath, string password, out T result, out string error)
        {
            result = default(T);
            error = null;

            MemoryStream ms;
            if (!CryptoHelper.TryDecryptFileToMemoryStream(filePath, password, out ms, out error))
            {
                return false;
            }

            try
            {
                using (ms)
                {
                    var formatter = new BinaryFormatter();
                    object obj = formatter.Deserialize(ms);
                    result = (T)obj;
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