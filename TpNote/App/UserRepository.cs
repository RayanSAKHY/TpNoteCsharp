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
        public void setLivres(List<Livre> livres) => _livres = livres;
        private string UserFolder => PathManager.GetUserFolder(_username);
        private string ProfilePath => PathManager.GetUserProfilePath(_username);
        private string BooksPath(ISerializer ser) => PathManager.GetUserBooksPath(_username, ser.DefaultExtension);

        // --- Profil utilisateur : XML unitaire ---
        public void SaveProfile(Utilisateur user) => SingleXmlSerializer.Save(ProfilePath, user);
        public Utilisateur LoadProfile() => SingleXmlSerializer.Load<Utilisateur>(ProfilePath);

        // --- Livres : via Factory (XML/Binaire) ---
        public void SaveBooks(IEnumerable<Livre> livres, SerializationFormat format)
        {
            var ser = SerializerFactory.Create(format);
            ser.Save(BooksPath(ser), new List<Livre>(livres));
        }

        public List<Livre> LoadBooks(SerializationFormat format)
        {
            var ser = SerializerFactory.Create(format);
            return ser.Load<List<Livre>>(BooksPath(ser)) ?? new List<Livre>();
        }
    }
}