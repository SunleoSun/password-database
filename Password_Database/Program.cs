using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Password_Database
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Password_Database PD = new Password_Database();
            Application.Run(new Password_Database());
        }
    }
}
