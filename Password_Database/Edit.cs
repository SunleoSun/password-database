using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Password_Database
{
    public partial class Edit : Form
    {
        public string OldName = "";
        public DatabaseClass Database1;
        public Edit(string Name1, string Account, string Password, string Else)
        {
            InitializeComponent();
            if (Name1 != "")
            {
                tbName.Text = Name1;
                OldName = Name1;
            }
            if (Account != "")
                tbAccount.Text = Account;
            else
                tbAccount.Text = "*************";
            if (Password != "")
                tbPassword.Text = Password;
            else
                tbPassword.Text = "*************";
            if (Else != "")
                tbElse.Text = Else;
            else
                tbElse.Text = "*************";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbName.Text != null || tbName.Text != "")
            {
                if (Database1.ErrText(tbName.Text) || Database1.ErrText(tbAccount.Text) || Database1.ErrText(tbPassword.Text) || Database1.ErrText(tbElse.Text))
                {
                    MessageBox.Show("You shouldn't use this symbol - \"`\" !");
                    return;
                }
                Database1.EditData(Database1.CurrentCategory, OldName, tbName.Text, tbAccount.Text, tbPassword.Text, tbElse.Text);
                Close();
            }
        }
    }
}
