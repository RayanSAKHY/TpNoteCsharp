using System;

namespace SerializationApp
{
    /// <summary>
    /// Exception utilisée pour indiquer qu'un fichier existe mais n'a pas pu être déchiffré/lu.
    /// </summary>
    public class DecryptionException : Exception
    {
        /// <summary>
        /// Crée une instance avec un message d'erreur.
        /// </summary>
        /// <param name="message">Description de l'erreur de déchiffrement.</param>
        public DecryptionException(string message) : base(message) { }
    }
}