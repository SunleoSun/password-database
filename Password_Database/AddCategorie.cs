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
    public partial class AddCategorie : Form
    {
        public DatabaseClass Database1;
        public string NewCategory;
        public AddCategorie()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (Database1.ErrText(textBox1.Text))
                {
                    MessageBox.Show("You shouldn't use this symbol - \"`\" !");
                    return;
                }
                Database1.CreateDataCategorie(textBox1.Text);
                NewCategory = textBox1.Text;
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
