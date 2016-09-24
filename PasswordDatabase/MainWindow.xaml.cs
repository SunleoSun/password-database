using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PasswordDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int SubCategoryHeight = 200;
        DatabaseManager _database;
        bool _isMadeChanges = false;
        const string NewSubCategoryText = "New Sub Category ";
        List<DataGrid> _grids = new List<DataGrid>();

        public MainWindow()
        {
            InitializeComponent();
            CBCategory.Background = null;
            cbAccount.Background = null;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _database = new DatabaseManager();
            _database.CreateRestorePoint();
            _database.ReadData();
            RefreshAccountsNames();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isMadeChanges)
            {
                return;
            }
            var result = MessageBox.Show("Save changes?", "Save", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _database.UpdateData();
                _database.Save();
            }
            else
            {
                _database.RollabckToRestorePoint();
            }
            if (_database.UserPassword != null)
            {
                _database.UserPassword.Dispose();
            }
            _database.Dispose();
            GC.Collect();
        }

        private bool SetEnabledAllExcept(DependencyObject root, bool isEnabled, params UIElement[] elements)
        {
            bool result = false;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i) as UIElement;
                if (child == null)
                {
                    continue;
                }
                if (!elements.Contains(child) && !SetEnabledAllExcept(child, isEnabled, elements))
                {
                    child.IsEnabled = isEnabled;
                }
                else
                {
                    child.IsEnabled = !isEnabled;
                    result = true;
                }
            }
            return result;
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent as T != null)
                return (T)parent;
            else
                return FindParent<T>(parent);
        }

        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent is T)
            {
                return (T)parent;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var childFound = FindChild<T>(child);
                if (childFound != null)
                    return childFound;
            }
            return null;
        }

        public string GenegateNewSubCategoryName(string category)
        {
            var subCategories = _database.GetSubCategories(category);
            int nextSubCatNum = 1;
            foreach (var subCat in subCategories)
            {
                if (new Regex("^" + NewSubCategoryText + "\\d{1,8}$").Match(subCat).Success)
                {
                    var num = int.Parse(new Regex("\\d{1,8}$").Match(subCat).Value);
                    if (nextSubCatNum <= num)
                    {
                        nextSubCatNum = num + 1;
                    }
                }
            }
            return NewSubCategoryText + nextSubCatNum.ToString();
        }

        public void PopulateData()
        {
            PopulateCategories();
            PopulateSubcategories();
        }

        private void BAddRecordClick(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            var datagrid = FindParent<DataGrid>((DependencyObject)sender);
            var view = (DataView)datagrid.ItemsSource;
            var row = view.AddNew();
            var subCategory = FindParent<Expander>((DependencyObject)sender).Header.ToString();
            row["Account"] = _database.UserAccount;
            row["Category_ID"] = _database.GetCategoryID(Category);
            row["SubCategory_ID"] = _database.GetSubCategoryID(Category, subCategory);
            row.EndEdit();
        }

        private void BRemoveRecordClick(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            var datagrid = FindParent<DataGrid>((DependencyObject)sender);
            var view = (DataView)datagrid.ItemsSource;
            var gridRow = FindParent<DataGridRow>((DependencyObject)sender);
            view.Delete(datagrid.Items.IndexOf(gridRow.Item));
        }

        private void DataGrid_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            _isMadeChanges = true;
            var row = ((DataRowView)((DataGrid)sender).CurrentCell.Item).Row;
            row.EndEdit();
        }

        private void bDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var acc = cbAccount.SelectedItem.ToString();
            var dialog = new DeleteAccount();
            dialog.AccountName = acc;
            var res = dialog.ShowDialog();
            if (res.Value)
            {
                _isMadeChanges = true;
                _database.DeleteAccount(acc);
                _database.UpdateData();
                RefreshAccountsNames();
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)sender;
            _grids.Add(grid);
            grid.MaxWidth = ActualWidth - 80;
            grid.UpdateLayout();
            UpdateGridColumns(grid);
        }

        private void UpdateGridColumns(DataGrid grid)
        {
            foreach (var col in grid.Columns)
            {
                if (col is DataGridTemplateColumn)
                {
                    continue;
                }
                col.Width = col.MaxWidth = grid.ActualWidth / 3 - grid.Columns[3].ActualWidth / 3 - 2;
                grid.UpdateLayout();
            }
        }

        private void DataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            _grids.Remove((DataGrid)sender);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
            foreach (var grid in _grids)
            {
                grid.MaxWidth = e.NewSize.Width - 80;
                grid.UpdateLayout();
                UpdateGridColumns(grid);
            }
        }
    }
}
