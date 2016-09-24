using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordDatabase
{
    /// <summary>
    /// Interaction logic for DeleteAccount.xaml
    /// </summary>
    public partial class DeleteAccount : Window
    {
        public string AccountName
        {
            set
            {
                lLabel.Text = string.Format("Do you really want to delete account '{0}' and all data associated with it? If so, write 'DELETE' and press OK.", value);
            }
        }
        public DeleteAccount()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbText.Text == "DELETE")
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
