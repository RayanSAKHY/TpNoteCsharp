using System;

namespace SerializationApp
{
    public interface ISerializer
    {
        /// <summary>
        ///     Extension par défaut (ex: ".xml", ".bin").
        /// </summary>
        string DefaultExtension { get; }

        /// <summary>
        ///     Sauvegarde un objet de type T.
        /// </summary>
        void Save<T>(string filePath, T data);

        /// <summary>
        ///     Charge un objet de type T.
        /// </summary>
        T Load<T>(string filePath);
    }
}