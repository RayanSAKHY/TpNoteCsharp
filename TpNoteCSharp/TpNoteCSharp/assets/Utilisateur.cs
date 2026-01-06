using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TpNoteCSharp.assets
{
    [Serializable]
    [XmlRoot("Utilisateur")]
    internal class Utilisateur
    {
        private string _nom;
        private string _prenom;
        private string _email;
        private DateTime _dateInscription;
        private Livre[] _livresEmpruntes;

        // Constructeur sans paramètres requis pour la désérialisation
        public Utilisateur()
        {
        }

        public Utilisateur(string nom, string prenom, string email, DateTime dateInscription, Livre[] livresEmpruntes)
        {
            _nom = nom;
            _prenom = prenom;
            _email = email;
            _dateInscription = dateInscription;
            _livresEmpruntes = livresEmpruntes;
        }

        [XmlElement("Nom")]
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        [XmlElement("Prenom")]
        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }

        [XmlElement("Email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [XmlElement("DateInscription")]
        public DateTime DateInscription
        {
            get { return _dateInscription; }
            set { _dateInscription = value; }
        }

        [XmlArray("LivresEmpruntes")]
        [XmlArrayItem("Livre")]
        public Livre[] LivresEmpruntes
        {
            get { return _livresEmpruntes; }
            set { _livresEmpruntes = value; }
        }

    }
}
