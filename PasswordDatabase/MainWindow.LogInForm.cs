using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PasswordDatabase
{
    public partial class MainWindow : Window
    {
        private void bApply_Click(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            if (cbAccount.Text.Length == 0)
            {
                MessageBox.Show("Enter your new account name!");
                return;
            }
            if (pbPassword.Password.Length == 0)
            {
                MessageBox.Show("Enter your new account password!");
                return;
            }
            if (_database.GetAccounts().Contains(cbAccount.Text))
            {
                MessageBox.Show("This account already exists!");
                return;
            }
            _database.UserAccount = cbAccount.Text;
            _database.UserPassword = pbPassword.SecurePassword;
            _database.IsUsedEncryption = true;
            _database.Account.Rows.Add(cbAccount.Text, Convert.ToBase64String(Encrypter.GetHashCode512(pbPassword.Password)));
            _database.UpdateData();
            RefreshAccountsNames();
            pbPassword.Password = string.Empty;
            BSignOut.IsEnabled = true;
            bSignIn.IsEnabled = false;
            bDeleteAccount.IsEnabled = false;
            bApply.Visibility = Visibility.Hidden;
            TabData.IsEnabled = true;
            bSignUp.IsEnabled = false;
            Tabs.SelectedIndex = 1;
            cbAccount.IsEditable = false;
            cbAccount.IsReadOnly = false;
            cbAccount.SelectedItem = _database.UserAccount;
            cbAccount.IsEnabled = false;
            cbAccount.Background = null;
            PopulateData();
        }

        private void BSignOut_Click(object sender, RoutedEventArgs e)
        {
            BSignOut.IsEnabled = false;
            bSignUp.IsEnabled = true;
            bSignIn.IsEnabled = true;
            bApply.Visibility = Visibility.Hidden;
            TabData.IsEnabled = false;
            _database.UpdateData();
            cbAccount.IsEnabled = true;
            bDeleteAccount.IsEnabled = true;
            _database.UserAccount = null;
            _database.UserPassword = null;
            _database.IsUsedEncryption = false;
        }

        private void BSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (cbAccount.Text.Length == 0)
            {
                MessageBox.Show("Enter your account name!");
                return;
            }
            if (pbPassword.Password.Length == 0)
            {
                MessageBox.Show("Enter your account password!");
                return;
            }
            var accountPass = (string)_database.Account.Select().First(row => (string)row[0] == cbAccount.Text)[1];
            var password = Convert.ToBase64String(Encrypter.GetHashCode512(pbPassword.Password));
            if (password != accountPass)
            {
                MessageBox.Show("Password is incorrect!");
                return;
            }
            _database.UserPassword = pbPassword.SecurePassword;
            _database.IsUsedEncryption = true;
            _database.UserAccount = cbAccount.Text;
            _database.ReadData();
            pbPassword.Password = string.Empty;
            BSignOut.IsEnabled = true;
            bSignIn.IsEnabled = false;
            bSignUp.IsEnabled = false;
            bApply.Visibility = Visibility.Hidden;
            TabData.IsEnabled = true;
            bDeleteAccount.IsEnabled = false;
            Tabs.SelectedIndex = 1;
            cbAccount.IsEditable = false;
            cbAccount.IsReadOnly = false;
            cbAccount.Text = cbAccount.Text;
            cbAccount.IsEnabled = false;
            PopulateData();
        }

        private void BSignUp_Click(object sender, RoutedEventArgs e)
        {
            cbAccount.IsEditable = true;
            cbAccount.IsReadOnly = false;
            cbAccount.Background = (LinearGradientBrush)Resources["YellowTextBrush"];
            cbAccount.Text = string.Empty;
            bApply.Visibility = Visibility.Visible;
            bSignIn.IsEnabled = false;
            bDeleteAccount.IsEnabled = false;

        }

        private void RefreshAccountsNames()
        {
            var accounts = _database.GetAccounts();
            cbAccount.ItemsSource = accounts;
            if (accounts.Any())
            {
                bSignIn.IsEnabled = true;
                bDeleteAccount.IsEnabled = true;
                cbAccount.SelectedIndex = 0;
            }
            else
            {
                bSignIn.IsEnabled = false;
                bDeleteAccount.IsEnabled = false;
            }
        }
    }
}
