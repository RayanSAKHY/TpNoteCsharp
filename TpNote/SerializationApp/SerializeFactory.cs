using System;

namespace SerializationApp
{
    public static class SerializerFactory
    {
        public static ISerializer Create(SerializationFormat format)
        {
            switch (format)
            {
                case SerializationFormat.Xml:
                    return new XmlSerializerManager();
                case SerializationFormat.Binary:
                    return new BinarySerializerManager();
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, "Format non supporté.");
            }
        }
    }
}