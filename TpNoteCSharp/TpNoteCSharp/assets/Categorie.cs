using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpNoteCSharp.assets
{
    internal class Categorie
    {
        private string _nom;
        private Livre[] _livres;

        public Categorie(string nom, Livre[] livres)
        {
            _nom = nom;
            _livres = livres;
        }

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public Livre[] Livres
        {
            get { return _livres; }
            set { _livres = value; }
        }
    }
}
