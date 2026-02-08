using System;
using System.IO;
using System.Xml.Serialization;

namespace SerializationApp
{
    public static class SingleXmlSerializer
    {
        private static readonly object _sync = new object();

        public static void Save<T>(string filePath, T obj)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            string dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            var xs = new XmlSerializer(typeof(T));
            string tmp = filePath + ".tmp";

            lock (_sync)
            {
                using (var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                    xs.Serialize(fs, obj);

                if (File.Exists(filePath)) File.Delete(filePath);
                File.Move(tmp, filePath);
            }
        }

        public static T Load<T>(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) return default(T);

            var xs = new XmlSerializer(typeof(T));
            lock (_sync)
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try { return (T)xs.Deserialize(fs); }
                    catch { return default(T); }
                }
            }
        }
    }
}