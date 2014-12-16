using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace myFinances
{
    public class Globals
    {
        public static string ServerName { get; set; }
        public static int ServerPort { get; set; }
        public static string DbName { get; set; }
        public static string DbUserName { get; set; }
        public static string DbUserPassword { get; set; }
        public static string ErrorText { get; set; }


        public static int DefaultIdBill { get; set; }
        public static int DefaultIdIncome { get; set; }
        public static int DefaultIdExpence { get; set; }
    }

    public class BillDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }

    public class StructureDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }

    public class OperationDto
    {
        public int Id { get; set; }
        public int IdBill { get; set; }
        public long Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }

    public class MessageSender
    {
        public static void SendError(Form form, string errorText)
        {
            Globals.ErrorText = errorText;
            var newForm = new ErrorWindow()
            {
                Owner = form,
                StartPosition = FormStartPosition.CenterParent,
            };
            newForm.ShowDialog();
        }
        public static void SendOk(Form form, string okText)
        {
            Globals.ErrorText = okText;
            var newForm = new OkWindow()
            {
                Owner = form,
                StartPosition = FormStartPosition.CenterParent,
            };
            newForm.ShowDialog();
        }
    }

    static class Program
    {
        private static void InitData()
        {
            try
            {
                var document = XDocument.Load("Settings.xml");
                Globals.ServerName = document.Descendants("ServerName").First().Value;
                Globals.ServerPort = Convert.ToInt32(document.Descendants("ServerPort").First().Value);

                Globals.DbName = document.Descendants("DbName").First().Value;
                Globals.DbUserName = document.Descendants("DbUserName").First().Value;
                Globals.DbUserPassword = document.Descendants("DbUserPassword").First().Value;

                Globals.DefaultIdBill = Convert.ToInt32(document.Descendants("DefaultIdBill").First().Value);
                Globals.DefaultIdExpence = Convert.ToInt32(document.Descendants("DefaultIdExpence").First().Value);
                Globals.DefaultIdIncome = Convert.ToInt32(document.Descendants("DefaultIdIncome").First().Value);
            }
            catch(Exception ex)
            {
                throw new Exception("Что-то пошло не так при считывании настроек: " + ex.Message);
            }

            Globals.ErrorText = "Что-то пошло не так";
        }

        [STAThread]
        static void Main()
        {
            InitData();

            // Запуск приложения
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
