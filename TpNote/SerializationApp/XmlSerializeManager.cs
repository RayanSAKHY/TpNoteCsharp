using System;
using System.IO;
using System.Xml.Serialization;

namespace SerializationApp
{
    public sealed class XmlSerializerManager : ISerializer
    {
        public string DefaultExtension => ".xml";

        public void Save<T>(string filePath, T data)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.Serialize(stream, data);
            }
        }

        public T Load<T>(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) return default(T);

            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}