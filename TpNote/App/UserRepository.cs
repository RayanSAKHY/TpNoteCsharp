using System;
using System.Collections.Generic;
using System.IO;
using DataApp;
using SerializationApp;

namespace App
{
    internal sealed class UserRepository
    {
        private readonly string _username;
        private List<Livre> _livres;

        public UserRepository(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username requis.", nameof(username));
            _username = PathManager.Sanitize(username);
            _livres = new List<Livre>();
        }

        public List<Livre> getLivres() => _livres;

        private string UserFolder => PathManager.GetUserFolder(_username);
        private string ProfilePath => PathManager.GetUserProfilePath(_username);
        private string BooksPath(ISerializer ser) => PathManager.GetUserBooksPath(_username, ser.DefaultExtension);

        // --- Profil utilisateur : XML unitaire ---
        public void SaveProfile(Utilisateur user, string key)
        {
            // Ensure directory exists before saving
            PathManager.GetUserFolder(_username);
            EncryptedXmlSerializer.Save(ProfilePath, user, key);
        }
        public void SaveProfile(Utilisateur user) => SaveProfile(user, null);

        public Utilisateur LoadProfile(string key)
        {
            Utilisateur user;
            string error;
            if (EncryptedXmlSerializer.TryLoad(ProfilePath, key, out user, out error))
            {
                return user;
            }

            if (string.Equals(error, "FileNotFound", StringComparison.OrdinalIgnoreCase))
            {
                // no profile file
                return null;
            }

            // file exists but couldn't be decrypted/parsed
            throw new DecryptionException(error ?? "Unknown decryption error");
        }
        public Utilisateur LoadProfile() => LoadProfile(null);

        // --- Livres : via Factory (XML/Binaire) ---
        public void SaveBooks(IEnumerable<Livre> livres, SerializationFormat format, string key)
        {
            if (format == SerializationFormat.Xml)
            {
                var path = PathManager.GetUserBooksPath(_username, ".xml");
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                EncryptedXmlSerializer.Save(path, new List<Livre>(livres), key);
            }
            else
            {
                var path = PathManager.GetUserBooksPath(_username, ".bin");
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                EncryptedBinarySerializer.Save(path, new List<Livre>(livres), key);
            }
        }

        public List<Livre> LoadBooks(SerializationFormat format, string key)
        {
            if (format == SerializationFormat.Xml)
            {
                var path = PathManager.GetUserBooksPath(_username, ".xml");
                List<Livre> livres;
                string error;
                if (EncryptedXmlSerializer.TryLoad(path, key, out livres, out error))
                    return livres ?? new List<Livre>();

                if (string.Equals(error, "FileNotFound", StringComparison.OrdinalIgnoreCase))
                    return new List<Livre>();

                throw new DecryptionException(error ?? "Unknown decryption error");
            }
            else
            {
                var path = PathManager.GetUserBooksPath(_username, ".bin");
                List<Livre> livres;
                string error;
                if (EncryptedBinarySerializer.TryLoad(path, key, out livres, out error))
                    return livres ?? new List<Livre>();

                if (string.Equals(error, "FileNotFound", StringComparison.OrdinalIgnoreCase))
                    return new List<Livre>();

                throw new DecryptionException(error ?? "Unknown decryption error");
            }
        }

        public void SaveBooks(IEnumerable<Livre> livres, SerializationFormat format) => SaveBooks(livres, format, null);
        public List<Livre> LoadBooks(SerializationFormat format) => LoadBooks(format, null);
    }
}