using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpNoteCSharp.assets
{
    internal class Utilisateur
    {
        private string _nom;
        private string _prenom;
        private string _email;
        private DateTime _dateInscription;
        private Livre[] _livresEmpruntes;

        public Utilisateur(string nom, string prenom, string email, DateTime dateInscription, Livre[] livresEmpruntes)
        {
            _nom = nom;
            _prenom = prenom;
            _email = email;
            _dateInscription = dateInscription;
            _livresEmpruntes = livresEmpruntes;
        }

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }

        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public DateTime DateInscription
        {
            get { return _dateInscription; }
            set { _dateInscription = value; }
        }

        public Livre[] LivresEmpruntes
        {
            get { return _livresEmpruntes; }
            set { _livresEmpruntes = value; }
        }

    }
}
