using System;

namespace SerializationApp
{
    /// <summary>
    /// Interface pour les sérialiseurs (XML / Binaire / autres).
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Extension de fichier par défaut pour ce sérialiseur (ex: ".xml", ".bin").
        /// </summary>
        string DefaultExtension { get; }

        /// <summary>
        /// Sauvegarde un objet de type T dans le chemin fourni.
        /// Doit créer le dossier si nécessaire.
        /// </summary>
        /// <typeparam name="T">Type de l'objet à sauvegarder.</typeparam>
        /// <param name="filePath">Chemin complet du fichier cible.</param>
        void Save<T>(string filePath, T data);

        /// <summary>
        /// Charge un objet de type T depuis le chemin fourni.
        /// Doit retourner default(T) si le fichier n'existe pas ou la désérialisation échoue.
        /// </summary>
        /// <typeparam name="T">Type attendu.</typeparam>
        /// <param name="filePath">Chemin complet du fichier source.</param>
        /// <returns>Instance de T ou default(T).</returns>
        T Load<T>(string filePath);
    }
}