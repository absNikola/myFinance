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
        public static List<SettingDto> DefaultId { get; set; }
    }

    public class SettingDto
    {
        public int IdBill { get; set; }
        public int IdIncomeOperation { get; set; }
        public int IdExpenseOperation { get; set; }
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
        public int IdBill { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }

    public class OperationDto
    {
        public int OperationId { get; set; }
        public int OperationStructureId { get; set; }
        public long Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; }
    }

    public class ShowedDataDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Comment { get; set; }
    }

    public class MessageSender
    {
        public static void SendMessage(Form form, string messageText, string typeMessage)
        {
            Globals.ErrorText = messageText;
            var newForm = new MessageWindow()
            {
                Owner = form,
                StartPosition = FormStartPosition.CenterParent,
                Text = typeMessage,
            };
            newForm.ShowDialog();
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            ManageSetting.ReadSetting();

            // Запуск приложения
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
