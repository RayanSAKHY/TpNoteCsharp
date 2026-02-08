using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationApp
{
    public static class EncryptedBinarySerializer
    {
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

        public static T Load<T>(string filePath, string password)
        {
            // Backwards-compatible: return default(T) on any error
            T res;
            string err;
            if (TryLoad(filePath, password, out res, out err))
                return res;
            return default(T);
        }

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