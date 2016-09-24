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
    public partial class AddData : Form
    {
        public DatabaseClass Database1;
        public string CurrentCategory;
        public AddData()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(tbName.Text == null)
            {
                MessageBox.Show("Field Name must be filled!");
                return;
            }
            if (Database1.ErrText(tbName.Text) || Database1.ErrText(tbAccount.Text) || Database1.ErrText(tbPassword.Text) || Database1.ErrText(tbElse.Text))
            {
                MessageBox.Show("You shouldn't use this symbol - \"`\" !");
                return;
            }
            Database1.AddData(CurrentCategory, tbName.Text, tbAccount.Text, tbPassword.Text, tbElse.Text);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
