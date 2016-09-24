using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        const int SubCategoryCollapsedHeight = 32;
        private void PopulateSubcategories()
        {
            gSubCategories.Children.Clear();
            gSubCategories.RowDefinitions.Clear();
            var subCategories = _database.GetSubCategories(Category).ToArray();
            for (int i = 0; i < subCategories.Length; i++)
            {
                var subGrid = CreateSubCategoryElement(subCategories[i], i);
                subGrid.RowDefinitions.Add(new RowDefinition() {});
                var cobtentEl = new ContentControl();
                var records = _database.GetRecords(Category, subCategories[i]);
                cobtentEl.DataContext = records;
                cobtentEl.ContentTemplate = (DataTemplate)Resources["RecordTemplate"];
                subGrid.Children.Add(cobtentEl);
            }
        }

        private void AddSubCategoryClick(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            var subCat = GenegateNewSubCategoryName(Category);
            var curCategory = CBCategory.Text;
            _database.AddSubCategory(Category, subCat);
            PopulateSubcategories();
            CBCategory.Text = curCategory;
        }

        private void EditSubCategoryClick(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            var exp = FindParent<Expander>((DependencyObject)e.OriginalSource);
            var subCategory = FindChild<TextBox>(exp);
            subCategory.Background = (LinearGradientBrush)Resources["YellowTextBrush"];
            SetEnabledAllExcept(gMain, false, subCategory);
            subCategory.IsReadOnly = false;
            isSubCategoryEditMode = true;
            subCategory.ToolTip = new ToolTip() { Content = "Edit sub category and push enter" };
            _editedSubCategory = subCategory.Text;
        }

        bool isSubCategoryEditMode = false;
        string _editedSubCategory = string.Empty;
        private void SubCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (!isSubCategoryEditMode)
            {
                return;
            }
            isSubCategoryEditMode = false;
            var tb = (TextBox)sender;
            tb.Background = null;
            var text = tb.Text;
            if (string.IsNullOrEmpty(text.Trim()))
            {
                MessageBox.Show("Sub category name is not correct!");
                return;
            }
            if (_database.GetSubCategories(Category).Contains(text) && _editedSubCategory != text)
            {
                MessageBox.Show("This sub category already exists!");
                return;
            }
            var category = CBCategory.Text;
            _database.EditSubCategory(Category, _editedSubCategory, text);
            _database.UpdateData();
            CBCategory.Text = category;
            SetEnabledAllExcept(gMain, true);
        }

        private void DeleteSubCategoryClick(object sender, RoutedEventArgs e)
        {
            _isMadeChanges = true;
            if (gSubCategories.RowDefinitions.Count == 1)
            {
                MessageBox.Show("Only one sub category exists in this category. You can only edit it");
                return;
            }
            var result = MessageBox.Show("Do you really want to delete this sub category?", "Delete sub category", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            var expander = FindParent<Expander>((Button)sender);
            var category = CBCategory.Text;
            _database.DeleteSubCategory(Category, expander.Header.ToString());
            _database.UpdateData();
            CBCategory.Text = category;
        }

        private Grid CreateSubCategoryElement(string name, int index)
        {
            gSubCategories.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(SubCategoryCollapsedHeight) });
            var curRow = gSubCategories.RowDefinitions.Count - 1;
            var subCategoryExpander = new Expander();
            subCategoryExpander.SetValue(Grid.RowProperty, curRow);
            subCategoryExpander.HeaderTemplate = (DataTemplate)Resources["Header"];
            subCategoryExpander.Header = name;
            subCategoryExpander.Style = (Style)Resources["SubCategoryStyle"];
            subCategoryExpander.BorderThickness = new Thickness(3);
            gSubCategories.Children.Add(subCategoryExpander);
            subCategoryExpander.Content = new Grid();
            subCategoryExpander.Tag = index;
            subCategoryExpander.Expanded += (o, e) => 
            {
                FindParent<Grid>((DependencyObject)o).RowDefinitions[(int)((Expander)o).Tag].Height = GridLength.Auto;
            } ;
            subCategoryExpander.Collapsed += (o, e) => { FindParent<Grid>((DependencyObject)o).RowDefinitions[(int)((Expander)o).Tag].Height = new GridLength(SubCategoryCollapsedHeight); };
            return (Grid)subCategoryExpander.Content;
        }
    }
}
