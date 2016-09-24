namespace Password_Database
{
    partial class Password_Database
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bAddCategory = new System.Windows.Forms.Button();
            this.bAdd = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bDeleteCategory = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Name = new System.Windows.Forms.ColumnHeader();
            this.Account = new System.Windows.Forms.ColumnHeader();
            this.Password = new System.Windows.Forms.ColumnHeader();
            this.Else = new System.Windows.Forms.ColumnHeader();
            this.ch = new System.Windows.Forms.ColumnHeader("Password_Database");
            this.bSaveDatabase = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.Location = new System.Drawing.Point(40, 53);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(455, 27);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // bAddCategory
            // 
            this.bAddCategory.Location = new System.Drawing.Point(508, 53);
            this.bAddCategory.Name = "bAddCategory";
            this.bAddCategory.Size = new System.Drawing.Size(178, 27);
            this.bAddCategory.TabIndex = 4;
            this.bAddCategory.Text = "Add Category";
            this.bAddCategory.UseVisualStyleBackColor = true;
            this.bAddCategory.Click += new System.EventHandler(this.bAddCategorie_Click);
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(42, 450);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(209, 48);
            this.bAdd.TabIndex = 5;
            this.bAdd.Text = "Add";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(279, 450);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(215, 47);
            this.bDelete.TabIndex = 6;
            this.bDelete.Text = "Delete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(528, 450);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(194, 46);
            this.bEdit.TabIndex = 7;
            this.bEdit.Text = "Edit";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // bDeleteCategory
            // 
            this.bDeleteCategory.Location = new System.Drawing.Point(719, 53);
            this.bDeleteCategory.Name = "bDeleteCategory";
            this.bDeleteCategory.Size = new System.Drawing.Size(178, 27);
            this.bDeleteCategory.TabIndex = 8;
            this.bDeleteCategory.Text = "Delete Category";
            this.bDeleteCategory.UseVisualStyleBackColor = true;
            this.bDeleteCategory.Click += new System.EventHandler(this.bDeleteCategory_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Name,
            this.Account,
            this.Password,
            this.Else});
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(29, 87);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1033, 317);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Name
            // 
            this.Name.Text = "Name";
            this.Name.Width = 257;
            // 
            // Account
            // 
            this.Account.Text = "Account";
            this.Account.Width = 189;
            // 
            // Password
            // 
            this.Password.Text = "Password";
            this.Password.Width = 256;
            // 
            // Else
            // 
            this.Else.Text = "Else";
            this.Else.Width = 325;
            // 
            // bSaveDatabase
            // 
            this.bSaveDatabase.Location = new System.Drawing.Point(508, 12);
            this.bSaveDatabase.Name = "bSaveDatabase";
            this.bSaveDatabase.Size = new System.Drawing.Size(389, 26);
            this.bSaveDatabase.TabIndex = 10;
            this.bSaveDatabase.Text = "Save Database";
            this.bSaveDatabase.UseVisualStyleBackColor = true;
            this.bSaveDatabase.Click += new System.EventHandler(this.bSaveDatabase_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "Categories:";
            // 
            // Password_Database
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 524);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bSaveDatabase);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.bDeleteCategory);
            this.Controls.Add(this.bEdit);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.bAddCategory);
            this.Controls.Add(this.comboBox1);
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Text = "PasswordDatabase";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CloseProgram);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bAddCategory;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bDeleteCategory;
        internal System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Name;
        private System.Windows.Forms.ColumnHeader Account;
        private System.Windows.Forms.ColumnHeader Password;
        private System.Windows.Forms.ColumnHeader Else;
        private System.Windows.Forms.ColumnHeader ch;
        private System.Windows.Forms.Button bSaveDatabase;
        private System.Windows.Forms.Label label1;
    }
}

