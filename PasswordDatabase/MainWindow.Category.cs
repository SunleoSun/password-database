using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PasswordDatabase
{
    public partial class MainWindow : Window
    {
        public string Category
        {
            get
            {
                if (CBCategory.SelectedIndex != -1)
                {
                    return (string)((DataRowView)CBCategory.Items[CBCategory.SelectedIndex])["Category"];
                }
                return null;
            }
        }


        bool isCategoryInAddMode = false;
        bool isCategoryInEditMode = false;
        string _editedCategory = string.Empty;
        private void BAddCategory_Click(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            SetCategoryEditable();
            isCategoryInAddMode = true;
        }
        private void BEditCategory_Click(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            if (CBCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Category not selected");
                return;
            }
            _editedCategory = CBCategory.Text;
            SetCategoryEditable();
            isCategoryInEditMode = true;
        }

        private void BDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            if (CBCategory.SelectedIndex == -1)
            {
                return;
            }
            var dialogResult = MessageBox.Show(
                string.Format(
                    "Do you really want to delete \"{0}\" category and all data associated with it?",
                    CBCategory.SelectedValue),
                "Delete category",
                MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.No)
            {
                return;
            }
            _database.DeleteCategory(CBCategory.SelectedValue.ToString());
            _database.UpdateData();
            PopulateCategories();
        }

        private void SetCategoryEditable()
        {
            CBCategory.IsEditable = true;
            CBCategory.ToolTip = new ToolTip() { Content = "Write category name and push enter" };
            CBCategory.Background = (LinearGradientBrush)Resources["YellowTextBrush"];
            SetEnabledAllExcept(gMain, false, CBCategory);
        }

        private void CBCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            var curText = CBCategory.Text;
            SetEnabledAllExcept(gMain, true);
            if (isCategoryInAddMode)
            {
                isCategoryInAddMode = false;
                CBCategory.IsEditable = false;
                if (string.IsNullOrEmpty(curText.Trim()))
                {
                    MessageBox.Show("Category name not correct!");
                    return;
                }
                if (_database.GetCategories().Contains(curText))
                {
                    MessageBox.Show("This category already exists!");
                    return;
                }
                _database.AddCategory(curText);
                var subCat = GenegateNewSubCategoryName(curText);
                _database.AddSubCategory(curText, subCat);
            }
            if (isCategoryInEditMode)
            {
                isCategoryInEditMode = false;
                CBCategory.IsEditable = false;
                if (string.IsNullOrEmpty(curText.Trim()))
                {
                    MessageBox.Show("Category name not correct!");
                    return;
                }
                if (_database.GetCategories().Contains(curText))
                {
                    MessageBox.Show("This category already exists!");
                    return;
                }
                _database.EditCategory(_editedCategory, curText);
                _database.UpdateData();
            }
            CBCategory.Background = null;
            CBCategory.ToolTip = null;
            CBCategory.SelectedValue = curText;
        }

        private void CBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = ((DataView)CBCategory.ItemsSource);
            if (CBCategory.SelectedIndex == -1 && data.Count > 0)
            {
                CBCategory.SelectedIndex = 0;
            }
            PopulateSubcategories();
        }

        private void PopulateCategories(string selectedItem = null)
        {
            CBCategory.DataContext = _database.GetCategoriesRows();
            if (CBCategory.Items.Count > 0)
            {
                if (selectedItem != null)
                {
                    CBCategory.SelectedValue = selectedItem;
                }
                else
                {
                    CBCategory.SelectedIndex = 0;
                }
            }
        }

    }
}
