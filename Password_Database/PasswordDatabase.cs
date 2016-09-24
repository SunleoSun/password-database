using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Password_Database
{
    public partial class Password_Database : Form
    {
        public DatabaseClass Database;
        public Password_Database()
        {
            InitializeComponent();
            Database = new DatabaseClass();
            Database.ViewCategory(comboBox1.Text, listView1);
            comboBox1.Items.AddRange(Database.FindAllCategoriesInFile());
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.Text = comboBox1.Items[0].ToString();
                Database.CurrentCategory = comboBox1.Text;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Database.ViewCategory(comboBox1.Text, listView1);
            Database.CurrentCategory = comboBox1.Text;
        }

        private void bAddCategorie_Click(object sender, EventArgs e)
        {
            AddCategorie AddC = new AddCategorie();
            AddC.Database1 = Database;
            AddC.ShowDialog();
            if (AddC.NewCategory != null)
            {
                comboBox1.Items.Add(AddC.NewCategory);
                comboBox1.Text = AddC.NewCategory;
            }
        }

        private void bDeleteCategory_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Do you really want to delete this category?", "Deleting category", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Database.DeleteDataCategory(comboBox1.Text);
                    comboBox1.Items.Remove(comboBox1.Text);
                    if (comboBox1.Items.Count != 0)
                        comboBox1.Text = comboBox1.Items[0].ToString();
                }
                if (dr == DialogResult.No)
                    return;
            }
            Database.ViewCategory(Database.CurrentCategory,listView1);
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            AddData Add = new AddData();
            Add.Database1 = Database;
            Add.CurrentCategory = Database.CurrentCategory;
            if (comboBox1.Text != null)
                Database.CurrentCategory = comboBox1.Text;
            Add.ShowDialog();
            Database.ViewCategory(Database.CurrentCategory, listView1);
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            string Name="";
            ListView.ListViewItemCollection lvic = new ListView.ListViewItemCollection(listView1);
            ListView.SelectedListViewItemCollection sel = listView1.SelectedItems;
            foreach(ListViewItem item in  lvic)
            {
                if(sel.Contains(item))
                {
                    Name = item.Text;
                }
            }
            Database.DeleteData(Database.CurrentCategory, Name);
            Database.ViewCategory(Database.CurrentCategory, listView1);
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            string Name1 = "", Account1 = "", Password1 = "", Else1 = "";
            Edit edit;
            ListView.ListViewItemCollection lvic = new ListView.ListViewItemCollection(listView1);
            ListView.SelectedListViewItemCollection sel = listView1.SelectedItems;
            if (sel.Count==0)
                return;
            foreach (ListViewItem item in lvic)
            {
                if (sel.Contains(item))
                {
                    ListViewItem.ListViewSubItemCollection sub =  item.SubItems;
                    Name1 = sub[0].Text;
                    if(item.SubItems.Count >=2)
                        Account1 = sub[1].Text;
                    Password1 = sub[2].Text;
                    Else1 = sub[3].Text;
                    break;
                }
            }
            edit = new Edit(Name1, Account1, Password1, Else1);
            edit.Database1 = Database;
            edit.ShowDialog();
            Database.ViewCategory(Database.CurrentCategory, listView1);
        }

        private void bSaveDatabase_Click(object sender, EventArgs e)
        {
            if (Database.SaveData())
            {
                MessageBox.Show("Saved successfully!");
            }
            else
                MessageBox.Show("Saved failed!");
        }

        private void CloseProgram(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Save database?", "Saving", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                if (Database.SaveData())
                {
                    MessageBox.Show("Saved successfully!");
                }
                else
                    MessageBox.Show("Saved failed!");
        }



    }
}
