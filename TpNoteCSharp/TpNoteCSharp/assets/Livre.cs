using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpNoteCSharp.assets
{
    internal class Livre
    {
        private string _titre;
        private string _auteur;
        private DateTime _anneePublication;
        private int _isbn;
        private Categorie _categorie;
        private DateTime _dateAjout;

        public Livre(string titre, string auteur, DateTime anneePublication, int isbn, Categorie categorie, DateTime dateAjout)
        {
            _titre = titre;
            _auteur = auteur;
            _anneePublication = anneePublication;
            _isbn = isbn;
            _categorie = categorie;
            _dateAjout = dateAjout;
        }

        public string Titre
        {
            get { return _titre; }
            set { _titre = value; }
        }

        public string Auteur
        {
            get { return _auteur; }
            set { _auteur = value; }
        }

        public DateTime AnneePublication
        {
            get { return _anneePublication; }
            set { _anneePublication = value; }
        }

        public int ISBN
        {
            get { return _isbn; }
            set { _isbn = value; }
        }

        public Categorie Categorie
        {
            get { return _categorie; }
            set { _categorie = value; }
        }

        public DateTime DateAjout
        {
            get { return _dateAjout; }
            set { _dateAjout = value; }
        }

    }
}
