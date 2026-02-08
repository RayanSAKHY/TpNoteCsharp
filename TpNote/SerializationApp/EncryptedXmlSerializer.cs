using System;
using System.IO;
using System.Xml.Serialization;

namespace SerializationApp
{
    public static class EncryptedXmlSerializer
    {
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

        public static T Load<T>(string filePath, string password)
        {
            // Backwards-compatible: return default(T) on any error
            T res;
            string err;
            if (TryLoad(filePath, password, out res, out err))
                return res;
            return default(T);
        }

        // TryLoad returns false and an error describing the failure.
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