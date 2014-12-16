using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        [STAThread]
        static void Main()
        {
            //Globals.ServerName = "bogoyavlensky.dlinkddns.com";
            Globals.ServerName = "bogoyavlensky.com";
            Globals.ServerPort = 3306;
            Globals.DbName = "financialDB";
            Globals.DbUserName = "root";
            Globals.DbUserPassword = "123";
            Globals.ErrorText = "Что-то пошло не так";

            // Ноль для пустого списка счетов
            Globals.DefaultIdBill = 3;
            Globals.DefaultIdIncome = 3;
            Globals.DefaultIdExpence = 10;

            // Запуск приложения
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
