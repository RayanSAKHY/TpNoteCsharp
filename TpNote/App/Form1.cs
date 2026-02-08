using System;
using System.Collections.Generic;
using System.Windows.Forms;
using App;
using DataApp;
using SerializationApp;

namespace App
{
    public partial class Form1 : Form
    {
        // Current repository and user once connected
        private UserRepository _repo;
        private Utilisateur _user;
        private List<Livre> _currentBooks = new List<Livre>();

        public Form1()
        {
            InitializeComponent();

            // Keep tab visible to designer, but hide/disable at runtime until login.
            tabPageBooks.Visible = false;
            tabPageBooks.Enabled = false;
        }

        // Helper to show a short message
        private void ShowInfo(string msg) => MessageBox.Show(this, msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void ShowError(string msg) => MessageBox.Show(this, msg, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Create user handler
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                // Read fields from the UI
                string u = txtUsername.Text.Trim();
                string p = txtPassword.Text;
                string nom = txtNom.Text;
                string prenom = txtPrenom.Text;
                string email = txtEmail.Text;
                string key = txtKeyAccount.Text; // encryption key (may be empty -> SID fallback)

                if (string.IsNullOrWhiteSpace(u))
                {
                    ShowError("Username requis.");
                    return;
                }

                var user = new Utilisateur(u, p, nom, prenom, email, DateTime.Now, new Livre[0]);
                var repo = new UserRepository(user.Username);

                // Save profile with key (may throw on IO but we catch)
                repo.SaveProfile(user, key);
                ShowInfo($"Utilisateur '{user.Username}' créé et sauvegardé dans :\n{PathManager.GetUserProfilePath(user.Username)}");
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de la création : " + ex.Message);
            }
        }

        // Login handler
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string attemptedUser = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string key = txtKeyAccount.Text;

            if (string.IsNullOrWhiteSpace(attemptedUser))
            {
                ShowError("Saisir le username pour la connexion.");
                return;
            }

            var r = new UserRepository(attemptedUser);

            // Try load profile and allow retry on decryption error (similar to console)
            while (true)
            {
                try
                {
                    var existing = r.LoadProfile(key);
                    if (existing == null)
                    {
                        ShowError("Utilisateur inconnu.");
                        return;
                    }
                    if (!string.Equals(existing.MotDePasse, password, StringComparison.Ordinal))
                    {
                        ShowError("Mot de passe incorrect.");
                        return;
                    }

                    // success
                    _repo = r;
                    _user = existing;

                    // Load books
                    _currentBooks = _repo.LoadBooks(SerializationFormat.Xml) ?? new List<Livre>();

                    // Show and enable the books tab at runtime
                    tabPageBooks.Visible = true;
                    tabPageBooks.Enabled = true;

                    // Ensure tab control shows it
                    tabControlMain.SelectedTab = tabPageBooks;

                    // Enable controls explicitly (defensive)
                    listBoxBooks.Enabled = true;
                    groupBoxAdd.Enabled = true;
                    txtTitre.Enabled = true;
                    txtAuteur.Enabled = true;
                    txtIsbn.Enabled = true;
                    txtDatePub.Enabled = true;
                    txtCategorie.Enabled = true;
                    txtKeyBooks.Enabled = true;
                    btnAddBook.Enabled = true;
                    btnSaveXml.Enabled = true;
                    btnLoadXml.Enabled = true;
                    btnSaveBin.Enabled = true;
                    btnLoadBin.Enabled = true;

                    // Refresh UI
                    RefreshBookList();
                    txtTitre.Focus();

                    ShowInfo($"Connecté: {_user.Username}");
                    return;
                }
                catch (DecryptionException dex)
                {
                    var retry = MessageBox.Show(this, "Erreur de décryptage : " + dex.Message + "\nRéessayer avec une autre clé ?", "Décryptage", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (retry == DialogResult.Retry)
                    {
                        // Ask user for another key via input dialog (simple)
                        using (var frm = new InputKeyForm("Entrez une autre clé de chiffrement (vide => SID):"))
                        {
                            if (frm.ShowDialog(this) == DialogResult.OK) key = frm.Key;
                            else return;
                        }
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Erreur pendant la connexion: " + ex.Message);
                    return;
                }
            }
        }

        // Refresh UI listbox of books
        private void RefreshBookList()
        {
            listBoxBooks.BeginUpdate();
            listBoxBooks.Items.Clear();
            foreach (var b in _currentBooks)
            {
                // if Livre overrides ToString it's used, else show basic info
                listBoxBooks.Items.Add(b?.ToString() ?? $"{b?.Titre ?? "<no title>"} - {b?.Auteur ?? "<no author>"}");
            }
            listBoxBooks.EndUpdate();
        }

        // Add book from UI fields
        private void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                // DEBUG: uncomment the next line if you want to see raw input while diagnosing
                // MessageBox.Show($"Titre='{txtTitre.Text}'\nAuteur='{txtAuteur.Text}'\nISBN='{txtIsbn.Text}'\nDate='{txtDatePub.Text}'\nCat='{txtCategorie.Text}'");

                string titre = txtTitre.Text?.Trim();
                string auteur = txtAuteur.Text?.Trim();

                if (string.IsNullOrEmpty(titre) || string.IsNullOrEmpty(auteur))
                {
                    ShowError("Titre et Auteur sont requis.");
                    return;
                }

                // ISBN is optional now — default to 0 if empty
                int isbn = 0;
                var isbnText = txtIsbn.Text?.Trim();
                if (!string.IsNullOrEmpty(isbnText))
                {
                    if (!int.TryParse(isbnText, out isbn))
                    {
                        ShowError("ISBN invalide (doit être un entier).");
                        return;
                    }
                }

                // Date is optional — default to today if empty or invalid
                DateTime datePub;
                var dateText = txtDatePub.Text?.Trim();
                if (string.IsNullOrEmpty(dateText) || !DateTime.TryParse(dateText, out datePub))
                {
                    datePub = DateTime.Now;
                }

                string categorie = txtCategorie.Text?.Trim() ?? string.Empty;

                var livre = new Livre(titre, auteur, datePub, isbn, categorie, DateTime.Now);
                _currentBooks.Add(livre);
                RefreshBookList();
                ShowInfo($"Livre ajouté. Total = {_currentBooks.Count}");
            }
            catch (Exception ex)
            {
                ShowError("Erreur ajout livre: " + ex.Message);
            }
        }

        // Save XML with optional key
        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            if (_repo == null) { ShowError("Se connecter d'abord."); return; }
            string key = txtKeyBooks.Text;
            try
            {
                _repo.SaveBooks(_currentBooks, SerializationFormat.Xml, key);
                ShowInfo("Sauvegarde XML réussie.\n" + PathManager.GetUserBooksPath(_repo.getLivres() != null ? _repo.getLivres().Count.ToString() : _user.Username, ".xml"));
            }
            catch (Exception ex)
            {
                ShowError("Erreur sauvegarde XML : " + ex.Message);
            }
        }

        // Load XML with optional key
        private void btnLoadXml_Click(object sender, EventArgs e)
        {
            if (_repo == null) { ShowError("Se connecter d'abord."); return; }
            string key = txtKeyBooks.Text;
            while (true)
            {
                try
                {
                    _currentBooks = _repo.LoadBooks(SerializationFormat.Xml, key);
                    RefreshBookList();
                    ShowInfo($"Chargé (XML) : {_currentBooks.Count} livre(s).");
                    return;
                }
                catch (DecryptionException dex)
                {
                    var dr = MessageBox.Show("Erreur de décryptage : " + dex.Message + "\nRéessayer ?", "Décryptage", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Retry)
                    {
                        using (var frm = new InputKeyForm("Entrez clé de déchiffrement :"))
                        {
                            if (frm.ShowDialog(this) == DialogResult.OK) key = frm.Key;
                            else return;
                        }
                        continue;
                    }
                    else return;
                }
                catch (Exception ex)
                {
                    ShowError("Erreur chargement XML : " + ex.Message);
                    return;
                }
            }
        }

        // Save binary
        private void btnSaveBin_Click(object sender, EventArgs e)
        {
            if (_repo == null) { ShowError("Se connecter d'abord."); return; }
            string key = txtKeyBooks.Text;
            try
            {
                _repo.SaveBooks(_currentBooks, SerializationFormat.Binary, key);
                ShowInfo("Sauvegarde BIN réussie.\n" + PathManager.GetUserBooksPath(_user.Username, ".bin"));
            }
            catch (Exception ex)
            {
                ShowError("Erreur sauvegarde BIN : " + ex.Message);
            }
        }

        // Load binary
        private void btnLoadBin_Click(object sender, EventArgs e)
        {
            if (_repo == null) { ShowError("Se connecter d'abord."); return; }
            string key = txtKeyBooks.Text;
            while (true)
            {
                try
                {
                    _currentBooks = _repo.LoadBooks(SerializationFormat.Binary, key);
                    RefreshBookList();
                    ShowInfo($"Chargé (BIN) : {_currentBooks.Count} livre(s).");
                    return;
                }
                catch (DecryptionException dex)
                {
                    var dr = MessageBox.Show("Erreur de décryptage : " + dex.Message + "\nRéessayer ?", "Décryptage", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Retry)
                    {
                        using (var frm = new InputKeyForm("Entrez clé de déchiffrement :"))
                        {
                            if (frm.ShowDialog(this) == DialogResult.OK) key = frm.Key;
                            else return;
                        }
                        continue;
                    }
                    else return;
                }
                catch (Exception ex)
                {
                    ShowError("Erreur chargement BIN : " + ex.Message);
                    return;
                }
            }
        }
    }

    // Small helper form to prompt for a key (modal) — minimal and reusable
    internal class InputKeyForm : Form
    {
        private TextBox txt;
        private Button ok;
        private Button cancel;
        public string Key => txt.Text;

        public InputKeyForm(string prompt)
        {
            Text = "Clé de chiffrement";
            Width = 400;
            Height = 140;
            StartPosition = FormStartPosition.CenterParent;

            var lbl = new Label() { Left = 10, Top = 10, Width = 360, Text = prompt };
            txt = new TextBox() { Left = 10, Top = 30, Width = 360 };
            ok = new Button() { Text = "OK", Left = 200, Width = 80, Top = 60, DialogResult = DialogResult.OK };
            cancel = new Button() { Text = "Annuler", Left = 290, Width = 80, Top = 60, DialogResult = DialogResult.Cancel };

            Controls.Add(lbl);
            Controls.Add(txt);
            Controls.Add(ok);
            Controls.Add(cancel);

            AcceptButton = ok;
            CancelButton = cancel;
        }
    }
}
