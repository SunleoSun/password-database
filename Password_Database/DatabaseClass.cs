using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Password_Database
{
    public class DatabaseClass
    {
        string sAllData;
        string[] sAllDatabase;
        int NumDimensions = 0;
        public string CurrentCategory;
        //===========================================
        public DatabaseClass()
        {
            StreamReader sr = new StreamReader("Database.txt");
            sAllData = sr.ReadToEnd();
            sr.Close();
            sAllDatabase = sAllData.Split(new char[] {'\n','\r'}, StringSplitOptions.RemoveEmptyEntries);
            NumDimensions = 0;
            foreach (string line in sAllDatabase)
            {
                NumDimensions++;
            }
        }
        //============================================
        public void ViewCategory(string NameCategory, ListView listview)
        {
            if (NameCategory == "")
                listview.Items.Clear();
            NumDimensions = 0;
            foreach (string line in sAllDatabase)
            {
                NumDimensions++;
            }
            listview.Items.Clear();
            int CurrentLine = -1;
            foreach (string line in sAllDatabase)
            {
                CurrentLine++;
                if (!line.StartsWith("`"))
                {
                    if (line == NameCategory)
                    {
                        CurrentLine++;
                        while (NumDimensions > CurrentLine && sAllDatabase[CurrentLine].StartsWith("`"))
                        {
                            string[] EditedLine = sAllDatabase[CurrentLine].Split(new char[] { '`' });
                            int Counter = -1;
                            int NumDimensionsEditedLine = 0;
                            foreach (string line1 in EditedLine)
                            {
                                NumDimensionsEditedLine++;
                            }
                            foreach (string line1 in EditedLine)
                            {
                                Counter++;
                                if (Counter + 1 > NumDimensionsEditedLine-1)
                                    break;
                                EditedLine[Counter] = EditedLine[Counter + 1];
                            }
                            int FillStrings = 0;
                            foreach (string line1 in EditedLine)
                            {
                                if (line1.Length != 0)
                                    FillStrings++;
                                else
                                    break;
                            }
                            Array.Resize<string>(ref EditedLine, FillStrings);
                            ListViewItem lvi = new ListViewItem(EditedLine);
                            listview.Items.Add(lvi);
                            CurrentLine++;
                            if (CurrentLine >= NumDimensions)
                            {
                                CurrentLine = -1;
                                break;
                            }
                            //DataGridViewSelectedRowCollection r = new DataGridViewSelectedRowCollection();
                        }
                    }
                }
            }
            CurrentLine = -1;
        }
        //=======================================
        public void AddData(string NameCategory, string Name, string Account, string Password, string Else)
        {
            if (NameCategory == "")
                return;
            if (Name == "")
            {
                MessageBox.Show("Name must not be empty!");
                return;
            }
            if (Account == "")
                Account = "*************";
            if (Password == "")
                Password = "*************";
            if (Else == "")
                Else = "*************";

            int SentenceNumber=-1;

            foreach (string line in sAllDatabase)
            {
                SentenceNumber++;
                if (NameCategory == line)
                {
                    if (SentenceNumber + 1 == NumDimensions)
                        break;
                    string Tmp = sAllDatabase[SentenceNumber+1];
                    while (Tmp.StartsWith("`"))
                    {
                        if (Tmp.StartsWith("`" + Name + "`"))
                        {
                            MessageBox.Show("This name is already exists!");
                            return;
                        }
                        else
                        {
                            SentenceNumber++;
                            if (SentenceNumber + 1 == NumDimensions)
                                break;
                            Tmp = sAllDatabase[SentenceNumber + 1];
                        }
                    }
                }
            }
            int NumSentence = 0;
            foreach (string line in sAllDatabase)
            {
                NumSentence++;
                if (line == NameCategory)
                {
                    Array.Resize<string>(ref sAllDatabase, NumDimensions + 1);
                    //Замена и смещение элементов базового массива на одно измерение
                    for (int x = NumDimensions; x > NumSentence - 1; x--)
                    {
                        sAllDatabase[x] = sAllDatabase[x-1]; 
                    }
                    sAllDatabase[NumSentence] = "`" + Name + "`" + Account + "`" + Password + "`" + Else + "`";
                }
            }
        }
        //==============================================
        public void DeleteData(string NameCategory, string Name)
        {
            if (NameCategory == null || Name == "")
                return;
            string Tmp;
            int NumSentence = -1;
            NumDimensions = 0;
            foreach (string line in sAllDatabase)
            {
                NumDimensions++;
            }
            foreach (string line in sAllDatabase)
            {
                NumSentence++;
                if (NameCategory == line)
                {
                    Tmp = sAllDatabase[NumSentence+1];
                    bool b = true;
                    while (b)
                    {
                        NumSentence++;
                        if (Tmp.StartsWith("`" + Name))
                        {
                            b = false;
                            if (NumSentence == NumDimensions - 1)
                            {
                                Array.Resize<string>(ref sAllDatabase, NumDimensions - 1);
                                return;
                            }
                            for (int x = NumSentence; x < NumDimensions - 1; x++)
                            {
                                sAllDatabase[x] = sAllDatabase[x + 1];
                            }
                        }
                        else
                        {
                            Tmp = sAllDatabase[NumSentence + 1];
                            b = true;
                        }
                    }
                }
            }
            Array.Resize<string>(ref sAllDatabase, NumDimensions - 1);
            
        }
        //==============================================
        public void EditData(string NameCategory, string Name, string NewName,string NewAccount,string NewPassword,string NewElse)
        {
            if (NameCategory == ""|| Name == ""|| NewName == "")
                return;
            if (NewAccount == "")
                NewAccount = "*************";
            if (NewPassword == "")
                NewPassword = "*************";
            if (NewElse == "")
                NewElse = "*************";
            NumDimensions = 0;
            foreach (string line in sAllDatabase)
            {
                NumDimensions++;
            }
            int NumSentence=-1;
            int SentenceNumber = -1;
            int NumOfRepeatedSentence = 0;
            foreach (string line in sAllDatabase)
            {
                SentenceNumber++;
                if (NameCategory == line)
                {
                    if (SentenceNumber + 1 == NumDimensions)
                        break;
                    string Tmp = sAllDatabase[SentenceNumber + 1];
                    while (Tmp.StartsWith("`"))
                    {
                        if (Tmp.StartsWith("`" + NewName + "`"))
                        {
                            NumOfRepeatedSentence = SentenceNumber + 1;
                            break;
                        }
                        else
                        {
                            SentenceNumber++;
                            if (SentenceNumber + 1 == NumDimensions)
                                break;
                            Tmp = sAllDatabase[SentenceNumber + 1];
                        }
                    }
                }
            }
            foreach (string line in sAllDatabase)
            {
                NumSentence++;
                if (line == NameCategory)
                {
                    while (sAllDatabase[NumSentence+1].StartsWith("`"))
                    {
                        if (sAllDatabase[NumSentence+1].StartsWith("`" + Name + "`"))
                        {
                            if (NumSentence + 1 == NumOfRepeatedSentence)
                            {
                                sAllDatabase[NumSentence + 1] = "`" + NewName + "`" + NewAccount + "`" + NewPassword + "`" + NewElse + "`";
                                break;
                            }
                            else
                            {
                                MessageBox.Show("This name already exists!");
                                return;
                            }
                        }
                        else
                        {
                            NumSentence++;
                            if (NumSentence >= NumDimensions-1)
                                break;
                        }
                    }
                }
            }
        }
        public bool SaveData()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Database.txt"))
                {
                    foreach (string line in sAllDatabase)
                    {
                        sw.WriteLine(line);
                    }
                    return true;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        //==============================================
        public void CreateDataCategorie(string NameCategory)
        {
            foreach (string line in sAllDatabase)
            {
                if (line == NameCategory)
                {
                    MessageBox.Show("This category already exists!");
                    return;
                }
            }
            CurrentCategory = NameCategory;
            if (NameCategory != null)
            {
                //Если есть уже такая категория,- то выход из ф.
                foreach (string line in sAllDatabase)
                {
                    if (line == NameCategory)
                    {
                        MessageBox.Show("This category already exists!");
                        return;
                    }
                }
                NumDimensions = 0;
                foreach (string line in sAllDatabase)
                {
                    NumDimensions++;
                }
                Array.Resize<string>(ref sAllDatabase, NumDimensions + 1);
                sAllDatabase[NumDimensions] = NameCategory;
                return;
            }
        }
        //=======================================
        public void DeleteDataCategory(string NameCategory)
        {
            NumDimensions = 0;
            foreach (string line in sAllDatabase)
            {
                NumDimensions++;
            }
            if (NameCategory != null)
            {
                int NumberOfDeletingSentence = -1;
                int NumberOfAllDeletingSentences = 1;
                int Counter=0;
                //Определение количества удаляемых строк и их номера
                foreach (string line in sAllDatabase)
                {
                    NumberOfDeletingSentence++;
                    if (line == NameCategory)
                    {
                        Counter = NumberOfDeletingSentence;
                        if (Counter+1 < NumDimensions)
                            while (sAllDatabase[Counter+1].StartsWith("`"))
                            {
                                NumberOfAllDeletingSentences++;
                                Counter++;
                                if (Counter+1 >= NumDimensions)
                                    break;
                            }
                        break;
                    }
                }
                //Удаление строк
                Counter = 0;
                for (int x = 0; x < NumDimensions; x++)
                {
                    //Если второй индекс меньше кол-ва измерений массива
                    if((NumberOfDeletingSentence + NumberOfAllDeletingSentences + x) < NumDimensions)
                        sAllDatabase[x+NumberOfDeletingSentence] = sAllDatabase[(NumberOfDeletingSentence + NumberOfAllDeletingSentences + x)];
                    else
                    {
                        //Если второй индекс больше кол-ва измерений массива то затереть оставшиеся string
                        for (int y = x + NumberOfDeletingSentence; y < NumDimensions; y++)
                        {
                            Counter++;
                            sAllDatabase[y] = "";
                        }
                        break;
                    }
                }
                //Изменить размер массива, удаляя все пустые строки
                //if (Counter == 0)
                //    Counter = 1;
                Counter = NumDimensions - Counter;
                Array.Resize<string>(ref sAllDatabase, Counter);
            }
        }

        //=============================================
        public string[] FindAllCategoriesInFile()
        {
            int NumberCategories =  0;
            foreach (string line in sAllDatabase)
            {
                if (!line.StartsWith("`"))
                {
                    NumberCategories++;
                }
            }
            string[] Categories = new string[NumberCategories];
            int Counter = 0;
            foreach (string line in sAllDatabase)
            {
                if (!line.StartsWith("`"))
                {
                    Categories[Counter] = line;
                    Counter++;
                }
            }
            return Categories;
        }
        //=========================================
        public bool ErrText(string Text)
        {
            for (int x = 0; x < Text.Length; x++)
                if (Text[x] == '`')
                    return true;
            return false;
        }

    }
}
