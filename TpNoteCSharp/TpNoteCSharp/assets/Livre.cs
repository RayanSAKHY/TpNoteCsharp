using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TpNoteCSharp.assets
{
    [Serializable]
    [XmlRoot("Livre")]
    internal class Livre
    {
        private string _titre;
        private string _auteur;
        private DateTime _anneePublication;
        private int _isbn;
        private string _categorie;
        private DateTime _dateAjout;

        // Constructeur sans paramètres requis pour la désérialisation
        public Livre()
        {
        }

        public Livre(string titre, string auteur, DateTime anneePublication, int isbn, string categorie, DateTime dateAjout)
        {
            _titre = titre;
            _auteur = auteur;
            _anneePublication = anneePublication;
            _isbn = isbn;
            _categorie = categorie;
            _dateAjout = dateAjout;
        }

        [XmlElement("Titre")]
        public string Titre
        {
            get { return _titre; }
            set { _titre = value; }
        }

        [XmlElement("Auteur")]
        public string Auteur
        {
            get { return _auteur; }
            set { _auteur = value; }
        }

        [XmlElement("AnneePublication")]
        public DateTime AnneePublication
        {
            get { return _anneePublication; }
            set { _anneePublication = value; }
        }

        [XmlElement("ISBN")]
        public int ISBN
        {
            get { return _isbn; }
            set { _isbn = value; }
        }

        [XmlElement("Categorie")]
        public string Categorie
        {
            get { return _categorie; }
            set { _categorie = value; }
        }

        [XmlElement("DateAjout")]
        public DateTime DateAjout
        {
            get { return _dateAjout; }
            set { _dateAjout = value; }
        }

        override public string ToString()
        {
            return $"Titre: {_titre}, Auteur: {_auteur}, Année de Publication: {_anneePublication.Year}, ISBN: {_isbn}, Catégorie: {_categorie}, Date d'Ajout: {_dateAjout.ToShortDateString()}";
        }
    }
}
