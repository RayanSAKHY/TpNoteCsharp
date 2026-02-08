using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataApp
{
    /// <summary>
    /// Représente un utilisateur de la bibliothèque.
    /// Classe sérialisable en XML pour stocker le profil utilisateur.
    /// </summary>
    [Serializable]
    [XmlRoot("Utilisateur")]
    public class Utilisateur
    {
        private string _nom;
        private string _prenom;
        private string _email;
        private DateTime _dateInscription;
        private Livre[] _livresEmpruntes;
        private string _username;
        private string _password;

        /// <summary>
        /// Constructeur sans paramètres requis pour la désérialisation.
        /// </summary>
        public Utilisateur()
        {
        }

        /// <summary>
        /// Constructeur complet utilisé lors de la création d'un profil.
        /// </summary>
        /// <param name="username">Nom d'utilisateur.</param>
        /// <param name="motDePasse">Mot de passe (stocké tel quel ici pour le TP).</param>
        /// <param name="nom">Nom de famille.</param>
        /// <param name="prenom">Prénom.</param>
        /// <param name="email">Adresse e‑mail.</param>
        /// <param name="dateInscription">Date d'inscription.</param>
        /// <param name="livresEmpruntes">Tableau des livres empruntés.</param>
        public Utilisateur(string username, string motDePasse, string nom, string prenom, string email, DateTime dateInscription, Livre[] livresEmpruntes)
        {
            _username = username;
            _password = motDePasse;
            _nom = nom;
            _prenom = prenom;
            _email = email;
            _dateInscription = dateInscription;
            _livresEmpruntes = livresEmpruntes;
        }

        /// <summary>
        /// Nom d'utilisateur (clé logique pour le profil).
        /// </summary>
        [XmlElement("Username")]
        public string Username
        {
            get => _username;
            set => _username = value;
        }

        /// <summary>
        /// Mot de passe (dans ce TP il est stocké en clair dans l'objet ; attention en production).
        /// </summary>
        [XmlElement("MotDePasse")]
        public string MotDePasse
        {
            get => _password;
            set => _password = value;
        }

        /// <summary>
        /// Nom de famille.
        /// </summary>
        [XmlElement("Nom")]
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        /// <summary>
        /// Prénom.
        /// </summary>
        [XmlElement("Prenom")]
        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }

        /// <summary>
        /// Adresse e‑mail.
        /// </summary>
        [XmlElement("Email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Date d'inscription de l'utilisateur.
        /// </summary>
        [XmlElement("DateInscription")]
        public DateTime DateInscription
        {
            get { return _dateInscription; }
            set { _dateInscription = value; }
        }

        /// <summary>
        /// Liste des livres empruntés (tableau sérialisé en XML).
        /// </summary>
        [XmlArray("LivresEmpruntes")]
        [XmlArrayItem("Livre")]
        public Livre[] LivresEmpruntes
        {
            get { return _livresEmpruntes; }
            set { _livresEmpruntes = value; }
        }

    }
}
