namespace App
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageAccount;
        private System.Windows.Forms.TabPage tabPageBooks;

        // Account controls
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.Label lblPrenom;
        private System.Windows.Forms.TextBox txtPrenom;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblKeyAccount;
        private System.Windows.Forms.TextBox txtKeyAccount;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnLogin;

        // Books controls
        private System.Windows.Forms.ListBox listBoxBooks;
        private System.Windows.Forms.GroupBox groupBoxAdd;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.TextBox txtTitre;
        private System.Windows.Forms.Label lblAuteur;
        private System.Windows.Forms.TextBox txtAuteur;
        private System.Windows.Forms.Label lblIsbn;
        private System.Windows.Forms.TextBox txtIsbn;
        private System.Windows.Forms.Label lblDatePub;
        private System.Windows.Forms.TextBox txtDatePub;
        private System.Windows.Forms.Label lblCategorie;
        private System.Windows.Forms.TextBox txtCategorie;
        private System.Windows.Forms.Button btnAddBook;
        private System.Windows.Forms.TextBox txtKeyBooks;
        private System.Windows.Forms.Label lblKeyBooks;
        private System.Windows.Forms.Button btnSaveXml;
        private System.Windows.Forms.Button btnLoadXml;
        private System.Windows.Forms.Button btnSaveBin;
        private System.Windows.Forms.Button btnLoadBin;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageAccount = new System.Windows.Forms.TabPage();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblNom = new System.Windows.Forms.Label();
            this.txtNom = new System.Windows.Forms.TextBox();
            this.lblPrenom = new System.Windows.Forms.Label();
            this.txtPrenom = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblKeyAccount = new System.Windows.Forms.Label();
            this.txtKeyAccount = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tabPageBooks = new System.Windows.Forms.TabPage();
            this.listBoxBooks = new System.Windows.Forms.ListBox();
            this.groupBoxAdd = new System.Windows.Forms.GroupBox();
            this.lblTitre = new System.Windows.Forms.Label();
            this.txtTitre = new System.Windows.Forms.TextBox();
            this.lblAuteur = new System.Windows.Forms.Label();
            this.txtAuteur = new System.Windows.Forms.TextBox();
            this.lblIsbn = new System.Windows.Forms.Label();
            this.txtIsbn = new System.Windows.Forms.TextBox();
            this.lblDatePub = new System.Windows.Forms.Label();
            this.txtDatePub = new System.Windows.Forms.TextBox();
            this.lblCategorie = new System.Windows.Forms.Label();
            this.txtCategorie = new System.Windows.Forms.TextBox();
            this.btnAddBook = new System.Windows.Forms.Button();
            this.lblKeyBooks = new System.Windows.Forms.Label();
            this.txtKeyBooks = new System.Windows.Forms.TextBox();
            this.btnSaveXml = new System.Windows.Forms.Button();
            this.btnLoadXml = new System.Windows.Forms.Button();
            this.btnSaveBin = new System.Windows.Forms.Button();
            this.btnLoadBin = new System.Windows.Forms.Button();
            this.tabControlMain.SuspendLayout();
            this.tabPageAccount.SuspendLayout();
            this.tabPageBooks.SuspendLayout();
            this.groupBoxAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageAccount);
            this.tabControlMain.Controls.Add(this.tabPageBooks);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(820, 520);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageAccount
            // 
            this.tabPageAccount.Controls.Add(this.lblUsername);
            this.tabPageAccount.Controls.Add(this.txtUsername);
            this.tabPageAccount.Controls.Add(this.lblPassword);
            this.tabPageAccount.Controls.Add(this.txtPassword);
            this.tabPageAccount.Controls.Add(this.lblNom);
            this.tabPageAccount.Controls.Add(this.txtNom);
            this.tabPageAccount.Controls.Add(this.lblPrenom);
            this.tabPageAccount.Controls.Add(this.txtPrenom);
            this.tabPageAccount.Controls.Add(this.lblEmail);
            this.tabPageAccount.Controls.Add(this.txtEmail);
            this.tabPageAccount.Controls.Add(this.lblKeyAccount);
            this.tabPageAccount.Controls.Add(this.txtKeyAccount);
            this.tabPageAccount.Controls.Add(this.btnCreate);
            this.tabPageAccount.Controls.Add(this.btnLogin);
            this.tabPageAccount.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccount.Name = "tabPageAccount";
            this.tabPageAccount.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageAccount.Size = new System.Drawing.Size(812, 494);
            this.tabPageAccount.TabIndex = 0;
            this.tabPageAccount.Text = "Compte";
            this.tabPageAccount.UseVisualStyleBackColor = true;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 10);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(140, 7);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(220, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(10, 40);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(74, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Mot de passe:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(140, 37);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(220, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblNom
            // 
            this.lblNom.AutoSize = true;
            this.lblNom.Location = new System.Drawing.Point(10, 70);
            this.lblNom.Name = "lblNom";
            this.lblNom.Size = new System.Drawing.Size(32, 13);
            this.lblNom.TabIndex = 4;
            this.lblNom.Text = "Nom:";
            // 
            // txtNom
            // 
            this.txtNom.Location = new System.Drawing.Point(140, 67);
            this.txtNom.Name = "txtNom";
            this.txtNom.Size = new System.Drawing.Size(220, 20);
            this.txtNom.TabIndex = 5;
            // 
            // lblPrenom
            // 
            this.lblPrenom.AutoSize = true;
            this.lblPrenom.Location = new System.Drawing.Point(10, 100);
            this.lblPrenom.Name = "lblPrenom";
            this.lblPrenom.Size = new System.Drawing.Size(46, 13);
            this.lblPrenom.TabIndex = 6;
            this.lblPrenom.Text = "Prénom:";
            // 
            // txtPrenom
            // 
            this.txtPrenom.Location = new System.Drawing.Point(140, 97);
            this.txtPrenom.Name = "txtPrenom";
            this.txtPrenom.Size = new System.Drawing.Size(220, 20);
            this.txtPrenom.TabIndex = 7;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(10, 130);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 8;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(140, 127);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(220, 20);
            this.txtEmail.TabIndex = 9;
            // 
            // lblKeyAccount
            // 
            this.lblKeyAccount.AutoSize = true;
            this.lblKeyAccount.Location = new System.Drawing.Point(10, 160);
            this.lblKeyAccount.Name = "lblKeyAccount";
            this.lblKeyAccount.Size = new System.Drawing.Size(80, 13);
            this.lblKeyAccount.TabIndex = 10;
            this.lblKeyAccount.Text = "Clé chiffrement:";
            // 
            // txtKeyAccount
            // 
            this.txtKeyAccount.Location = new System.Drawing.Point(140, 157);
            this.txtKeyAccount.Name = "txtKeyAccount";
            this.txtKeyAccount.Size = new System.Drawing.Size(220, 20);
            this.txtKeyAccount.TabIndex = 11;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(140, 196);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(140, 28);
            this.btnCreate.TabIndex = 12;
            this.btnCreate.Text = "Créer utilisateur";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(290, 196);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(120, 28);
            this.btnLogin.TabIndex = 13;
            this.btnLogin.Text = "Connexion";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tabPageBooks
            // 
            this.tabPageBooks.Controls.Add(this.listBoxBooks);
            this.tabPageBooks.Controls.Add(this.groupBoxAdd);
            this.tabPageBooks.Controls.Add(this.lblKeyBooks);
            this.tabPageBooks.Controls.Add(this.txtKeyBooks);
            this.tabPageBooks.Controls.Add(this.btnSaveXml);
            this.tabPageBooks.Controls.Add(this.btnLoadXml);
            this.tabPageBooks.Controls.Add(this.btnSaveBin);
            this.tabPageBooks.Controls.Add(this.btnLoadBin);
            this.tabPageBooks.Location = new System.Drawing.Point(4, 22);
            this.tabPageBooks.Name = "tabPageBooks";
            this.tabPageBooks.Size = new System.Drawing.Size(812, 494);
            this.tabPageBooks.TabIndex = 1;
            this.tabPageBooks.Text = "Livres";
            this.tabPageBooks.UseVisualStyleBackColor = true;
            // 
            // listBoxBooks
            // 
            this.listBoxBooks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxBooks.Location = new System.Drawing.Point(10, 10);
            this.listBoxBooks.Name = "listBoxBooks";
            this.listBoxBooks.Size = new System.Drawing.Size(480, 810);
            this.listBoxBooks.TabIndex = 0;
            // 
            // groupBoxAdd
            // 
            this.groupBoxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAdd.Controls.Add(this.lblTitre);
            this.groupBoxAdd.Controls.Add(this.txtTitre);
            this.groupBoxAdd.Controls.Add(this.lblAuteur);
            this.groupBoxAdd.Controls.Add(this.txtAuteur);
            this.groupBoxAdd.Controls.Add(this.lblIsbn);
            this.groupBoxAdd.Controls.Add(this.txtIsbn);
            this.groupBoxAdd.Controls.Add(this.lblDatePub);
            this.groupBoxAdd.Controls.Add(this.txtDatePub);
            this.groupBoxAdd.Controls.Add(this.lblCategorie);
            this.groupBoxAdd.Controls.Add(this.txtCategorie);
            this.groupBoxAdd.Controls.Add(this.btnAddBook);
            this.groupBoxAdd.Location = new System.Drawing.Point(510, 10);
            this.groupBoxAdd.Name = "groupBoxAdd";
            this.groupBoxAdd.Size = new System.Drawing.Size(280, 220);
            this.groupBoxAdd.TabIndex = 1;
            this.groupBoxAdd.TabStop = false;
            this.groupBoxAdd.Text = "Ajouter un livre";
            // 
            // lblTitre
            // 
            this.lblTitre.AutoSize = true;
            this.lblTitre.Location = new System.Drawing.Point(10, 20);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(31, 13);
            this.lblTitre.TabIndex = 0;
            this.lblTitre.Text = "Titre:";
            // 
            // txtTitre
            // 
            this.txtTitre.Location = new System.Drawing.Point(80, 17);
            this.txtTitre.Name = "txtTitre";
            this.txtTitre.Size = new System.Drawing.Size(190, 20);
            this.txtTitre.TabIndex = 1;
            // 
            // lblAuteur
            // 
            this.lblAuteur.AutoSize = true;
            this.lblAuteur.Location = new System.Drawing.Point(10, 48);
            this.lblAuteur.Name = "lblAuteur";
            this.lblAuteur.Size = new System.Drawing.Size(41, 13);
            this.lblAuteur.TabIndex = 2;
            this.lblAuteur.Text = "Auteur:";
            // 
            // txtAuteur
            // 
            this.txtAuteur.Location = new System.Drawing.Point(80, 45);
            this.txtAuteur.Name = "txtAuteur";
            this.txtAuteur.Size = new System.Drawing.Size(190, 20);
            this.txtAuteur.TabIndex = 3;
            // 
            // lblIsbn
            // 
            this.lblIsbn.AutoSize = true;
            this.lblIsbn.Location = new System.Drawing.Point(10, 76);
            this.lblIsbn.Name = "lblIsbn";
            this.lblIsbn.Size = new System.Drawing.Size(73, 13);
            this.lblIsbn.TabIndex = 4;
            this.lblIsbn.Text = "ISBN (entier) :";
            // 
            // txtIsbn
            // 
            this.txtIsbn.Location = new System.Drawing.Point(80, 73);
            this.txtIsbn.Name = "txtIsbn";
            this.txtIsbn.Size = new System.Drawing.Size(190, 20);
            this.txtIsbn.TabIndex = 5;
            // 
            // lblDatePub
            // 
            this.lblDatePub.AutoSize = true;
            this.lblDatePub.Location = new System.Drawing.Point(10, 104);
            this.lblDatePub.Name = "lblDatePub";
            this.lblDatePub.Size = new System.Drawing.Size(117, 13);
            this.lblDatePub.TabIndex = 6;
            this.lblDatePub.Text = "Date pub (yyyy-mm-dd):";
            // 
            // txtDatePub
            // 
            this.txtDatePub.Location = new System.Drawing.Point(170, 101);
            this.txtDatePub.Name = "txtDatePub";
            this.txtDatePub.Size = new System.Drawing.Size(100, 20);
            this.txtDatePub.TabIndex = 7;
            // 
            // lblCategorie
            // 
            this.lblCategorie.AutoSize = true;
            this.lblCategorie.Location = new System.Drawing.Point(10, 132);
            this.lblCategorie.Name = "lblCategorie";
            this.lblCategorie.Size = new System.Drawing.Size(55, 13);
            this.lblCategorie.TabIndex = 8;
            this.lblCategorie.Text = "Catégorie:";
            // 
            // txtCategorie
            // 
            this.txtCategorie.Location = new System.Drawing.Point(80, 129);
            this.txtCategorie.Name = "txtCategorie";
            this.txtCategorie.Size = new System.Drawing.Size(190, 20);
            this.txtCategorie.TabIndex = 9;
            // 
            // btnAddBook
            // 
            this.btnAddBook.Location = new System.Drawing.Point(80, 160);
            this.btnAddBook.Name = "btnAddBook";
            this.btnAddBook.Size = new System.Drawing.Size(120, 28);
            this.btnAddBook.TabIndex = 10;
            this.btnAddBook.Text = "Ajouter";
            this.btnAddBook.Click += new System.EventHandler(this.btnAddBook_Click);
            // 
            // lblKeyBooks
            // 
            this.lblKeyBooks.AutoSize = true;
            this.lblKeyBooks.Location = new System.Drawing.Point(500, 240);
            this.lblKeyBooks.Name = "lblKeyBooks";
            this.lblKeyBooks.Size = new System.Drawing.Size(58, 13);
            this.lblKeyBooks.TabIndex = 2;
            this.lblKeyBooks.Text = "Clé (livres):";
            // 
            // txtKeyBooks
            // 
            this.txtKeyBooks.Location = new System.Drawing.Point(590, 236);
            this.txtKeyBooks.Name = "txtKeyBooks";
            this.txtKeyBooks.Size = new System.Drawing.Size(190, 20);
            this.txtKeyBooks.TabIndex = 3;
            // 
            // btnSaveXml
            // 
            this.btnSaveXml.Location = new System.Drawing.Point(500, 270);
            this.btnSaveXml.Name = "btnSaveXml";
            this.btnSaveXml.Size = new System.Drawing.Size(120, 28);
            this.btnSaveXml.TabIndex = 4;
            this.btnSaveXml.Text = "Save XML";
            this.btnSaveXml.Click += new System.EventHandler(this.btnSaveXml_Click);
            // 
            // btnLoadXml
            // 
            this.btnLoadXml.Location = new System.Drawing.Point(660, 270);
            this.btnLoadXml.Name = "btnLoadXml";
            this.btnLoadXml.Size = new System.Drawing.Size(120, 28);
            this.btnLoadXml.TabIndex = 5;
            this.btnLoadXml.Text = "Load XML";
            this.btnLoadXml.Click += new System.EventHandler(this.btnLoadXml_Click);
            // 
            // btnSaveBin
            // 
            this.btnSaveBin.Location = new System.Drawing.Point(500, 310);
            this.btnSaveBin.Name = "btnSaveBin";
            this.btnSaveBin.Size = new System.Drawing.Size(120, 28);
            this.btnSaveBin.TabIndex = 6;
            this.btnSaveBin.Text = "Save BIN";
            this.btnSaveBin.Click += new System.EventHandler(this.btnSaveBin_Click);
            // 
            // btnLoadBin
            // 
            this.btnLoadBin.Location = new System.Drawing.Point(660, 310);
            this.btnLoadBin.Name = "btnLoadBin";
            this.btnLoadBin.Size = new System.Drawing.Size(120, 28);
            this.btnLoadBin.TabIndex = 7;
            this.btnLoadBin.Text = "Load BIN";
            this.btnLoadBin.Click += new System.EventHandler(this.btnLoadBin_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(820, 520);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bibliothèque - Interface graphique";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageAccount.ResumeLayout(false);
            this.tabPageAccount.PerformLayout();
            this.tabPageBooks.ResumeLayout(false);
            this.tabPageBooks.PerformLayout();
            this.groupBoxAdd.ResumeLayout(false);
            this.groupBoxAdd.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}

