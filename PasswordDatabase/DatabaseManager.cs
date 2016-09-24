using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Security;
using System.Runtime.InteropServices;

namespace PasswordDatabase
{
    class DatabaseManager : DataSet
    {
        private const string CategoryTableSelectCommand = "SELECT * FROM Category;";
        private const string AccountTableSelectCommand = "SELECT * FROM Account;";
        private const string SubCategoryTableSelectCommand = "SELECT * FROM SubCategory;";
        private const string RecordTableSelectCommand = "SELECT * FROM Record;";

        private const string DatabaseFileName = @"\mydatabase.sqlite";
        private readonly SQLiteConnection _connection;
        private SQLiteTransaction _transaction;
        public SecureString UserPassword { get; set; }
        public string UserAccount { get; set; }
        public bool IsUsedEncryption { get; set; } = false;

        public DatabaseManager()
        {
            _connection = new SQLiteConnection(ConnectionString);
            _connection.Open();
        }

        public string ConnectionString
        {
            get
            {
                var curFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var databaseFile = curFolder + DatabaseFileName;
                return "Data Source=" + databaseFile + ";Version=3;";
            }
        }

        public long LastAutoIncrement
        {
            get
            {
                using (var con = new SQLiteConnection(ConnectionString))
                {
                    con.Open();
                    var sql = "SELECT last_insert_rowid()";
                    var command = new SQLiteCommand(sql, con);
                    return (long)command.ExecuteScalar();
                }
            }
        }

        private char[] HandleSecureString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            var res = new char[value.Length];
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                for (int i = 0; i < value.Length; i++)
                {
                    res[i] = (char)Marshal.ReadInt16(valuePtr, i * 2);
                }
                return res;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public DataTable EncryptTable(DataTable table, params string[] exceptColumns)
        {
            if (!IsUsedEncryption)
            {
                return table;
            }
            return ConvertTableData(table, (el) => Encrypter.Encrypt(HandleSecureString(UserPassword), el), exceptColumns);
        }

        public DataTable DecryptTable(DataTable table, params string[] exceptColumns)
        {
            if (!IsUsedEncryption)
            {
                return table;
            }
            return ConvertTableData(table, (el) => Encrypter.Decrypt(HandleSecureString(UserPassword), el), exceptColumns);
        }

        public DataTable ConvertTableData(DataTable table, Func<string, string> convert, params string[] exceptColumns)
        {
            var exeptRowIndexes = table.Columns.Cast<DataColumn>().Select(
            (el, indx) =>
            {
                if (exceptColumns.Contains(el.ColumnName))
                    return indx;
                return -1;
            });
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted || row["Account"].ToString() != UserAccount)
                {
                    continue;
                }
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    if (!exeptRowIndexes.Contains(i))
                    {
                        row[i] = convert(row.ItemArray[i].ToString());
                    }
                }
            }
            return table;
        }

        public void CreateRestorePoint()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void RollabckToRestorePoint()
        {
            _transaction.Rollback();
        }

        public void Save()
        {
            _transaction.Commit();
        }

        public void DeleteAccount(string account)
        {
            Account.Select(string.Format("Account='{0}'", account)).First().Delete();
        }

        public void EditCategory(string category, string newValue)
        {
            GetCategory(category)["Category"] = newValue;
        }

        public void DeleteCategory(string category)
        {
            GetCategory(category).Delete();
        }

        public DataRow GetCategory(string category)
        {
            return Category.Select(string.Format("Account='{0}' AND Category='{1}'", UserAccount, category)).First();
        }

        public IEnumerable<string> GetAccounts()
        {
            return Account.Select().Select(row => row[0].ToString()).ToArray();
        }

        public IEnumerable<string> GetCategories()
        {
            return Category.Select("Account='" + UserAccount + "'").Select(el => (string)el["Category"]);
        }

        public DataView GetCategoriesRows()
        {
            var dv = new DataView(Category);
            dv.RowFilter = "Account='" + UserAccount + "'";
            dv.Sort = "Category";
            return dv;
        }

        public IEnumerable<string> GetSubCategories(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return new string[0];
            }
            var subCat = SubCategory
                .Select(string.Format("Account='{0}' AND Category_ID='{1}'", UserAccount, GetCategoryID(category)))
                .Select(row => (string)row["SubCategory"]).ToList();
            subCat.Sort();
            return subCat;
        }

        public void AddCategory(string category)
        {
            Category.Rows.Add(null, UserAccount, category);
            UpdateData();
        }

        public void AddSubCategory(string category, string subCatName)
        {
            var catID = GetCategoryID(category);
            SubCategory.Rows.Add(null, UserAccount, catID, subCatName);
            UpdateData();
            Record.Rows.Add(null, UserAccount, catID, GetSubCategoryID(category, subCatName), "", "", "");
            UpdateData();
        }

        public void EditSubCategory(string category, string subCategory, string newValue)
        {
            GetSubCategory(category, subCategory)["SubCategory"] = newValue;
        }

        public void DeleteSubCategory(string category, string subCategory)
        {
            GetSubCategory(category, subCategory).Delete();
        }

        public int GetCategoryID(string name)
        {
            return int.Parse(
                Category
                .Select(string.Format("Account='{0}' AND Category='{1}'", UserAccount, name))
                .First()["Category_ID"].ToString());
        }

        public int GetSubCategoryID(string category, string subCategory)
        {
            return int.Parse(
                SubCategory
                .Select(string.Format(
                    "Account='{0}' AND Category_ID='{1}' AND SubCategory='{2}'", 
                    UserAccount,
                    GetCategoryID(category), 
                    subCategory))
                .First()["SubCategory_ID"].ToString());
        }

        private DataRow GetSubCategory(string category, string subCategory)
        {
            return SubCategory
                     .Select(
                          string.Format(
                                    "Account='{0}' AND Category_ID='{1}' AND SubCategory_ID='{2}'",
                                    UserAccount,
                                    GetCategoryID(category),
                                    GetSubCategoryID(category, subCategory))).First();
        }

        public DataView GetRecords(string category, string subCategory)
        {
            var dv = new DataView(Record);
            dv.RowFilter = string.Format(
                        "Account='{0}' AND Category_ID='{1}' AND SubCategory_ID='{2}'",
                        UserAccount,
                        GetCategoryID(category),
                        GetSubCategoryID(category, subCategory));
            return dv;
        }

        public DataTable Account
        {
            get
            {
                return Tables["Account"];
            }
        }

        public DataTable Category
        {
            get
            {
                return Tables["Category"];
            }
        }

        public DataTable SubCategory
        {
            get
            {
                return Tables["SubCategory"];
            }
        }

        public DataTable Record
        {
            get
            {
                return Tables["Record"];
            }
        }

        public void ReadData()
        {
            ClearTable(Record);
            ClearTable(SubCategory);
            ClearTable(Category);
            ClearTable(Account);
            FillAccaunts();
            FillCategory();
            FillSubCategory();
            FillRecord();
        }

        public void FillAccaunts()
        {
            var adapter = new SQLiteDataAdapter(AccountTableSelectCommand, _connection);
            adapter.Fill(this, "Account");
        }

        public void FillCategory()
        {
            var adapter = new SQLiteDataAdapter(CategoryTableSelectCommand, _connection);
            FillTable(adapter, "Category", "Category_ID", "Account");
            var accountCategory = new ForeignKeyConstraint(
                "AccountToCategory",
                Account.Columns["Account"],
                Category.Columns["Account"]);
            Category.Constraints.Add(accountCategory);
        }
        public void FillSubCategory()
        {
            var adapter = new SQLiteDataAdapter(SubCategoryTableSelectCommand, _connection);
            FillTable(adapter, "SubCategory", "Category_ID", "SubCategory_ID", "Account");
            var categorySubCategory = new ForeignKeyConstraint(
                "CategoryToSubCat",
                new DataColumn[] { Category.Columns["Account"], Category.Columns["Category_ID"] },
                new DataColumn[] { SubCategory.Columns["Account"], SubCategory.Columns["Category_ID"] });
            SubCategory.Constraints.Add(categorySubCategory);
        }
        private void FillRecord()
        {
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(RecordTableSelectCommand, _connection);
            FillTable(adapter, "Record", "Record_ID", "Account", "Category_ID", "SubCategory_ID");
            var recordConstraint = new ForeignKeyConstraint(
                "SubCatToRecord",
                new DataColumn[] { SubCategory.Columns["Account"], SubCategory.Columns["Category_ID"], SubCategory.Columns["SubCategory_ID"] },
                new DataColumn[] { Record.Columns["Account"], Record.Columns["Category_ID"], Record.Columns["SubCategory_ID"] });
            Record.Constraints.Add(recordConstraint);
        }

        private void FillTable(SQLiteDataAdapter adapter, string tableName, params string[] exceptColumns)
        {
            if (Tables.Contains(tableName))
            {
                Tables[tableName].Clear();
            }
            else
            {
                Tables.Add(tableName);
            }
            var table = new DataTable();
            adapter.Fill(table);
            Tables[tableName].Load(new DataTableReader(DecryptTable(table, exceptColumns)));
        }


        public void UpdateData()
        {
            UpdateTable(RecordTableSelectCommand, Record, "Account", "Category_ID", "SubCategory_ID", "Record_ID");
            UpdateTable(SubCategoryTableSelectCommand, SubCategory, "Category_ID", "SubCategory_ID", "Account");
            UpdateTable(CategoryTableSelectCommand, Category, "Category_ID", "Account");
            UpdateTable(AccountTableSelectCommand, Account, "Account", "Password");
            ReadData();
        }

        public void UpdateTable(string selectCommand, DataTable table, params string[] exceptColumns)
        {
            var adapter = CreateAdapter(selectCommand, _connection);
            adapter.Update(EncryptTable(table, exceptColumns));
        }

        private static void ClearTable(DataTable table)
        {
            if (table == null)
            {
                return;
            }
            table.Constraints.Clear();
            table.Clear();
        }

        private SQLiteDataAdapter CreateAdapter(string selectCmd, SQLiteConnection con)
        {
            var adapter = new SQLiteDataAdapter(selectCmd, con);
            var builder = new SQLiteCommandBuilder(adapter);
            builder.ConflictOption = ConflictOption.OverwriteChanges;
            adapter.InsertCommand = builder.GetInsertCommand();
            adapter.UpdateCommand = builder.GetUpdateCommand();
            adapter.DeleteCommand = builder.GetDeleteCommand();
            return adapter;
        }
    }
}
