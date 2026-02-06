using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationApp
{
    public sealed class BinarySerializerManager : ISerializer
    {
        public string DefaultExtension => ".bin";

#pragma warning disable SYSLIB0011 // BinaryFormatter obsolète sur .NET moderne, OK en .NET Framework pour besoin pédagogique
        public void Save<T>(string filePath, T data)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, data);
            }
        }

        public T Load<T>(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) return default(T);

            var formatter = new BinaryFormatter();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                object obj = formatter.Deserialize(stream);
                return (T)obj;
            }
        }
#pragma warning restore SYSLIB0011
    }
}