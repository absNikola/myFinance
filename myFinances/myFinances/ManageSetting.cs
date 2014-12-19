using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace myFinances
{
    class ManageSetting
    {
        public static void ReadSetting()
        {
            try
            {
                var document = XDocument.Load("Settings.xml");
                Globals.ServerName = document.Descendants("ServerName").First().Value;
                Globals.ServerPort = Convert.ToInt32(document.Descendants("ServerPort").First().Value);

                Globals.DbName = document.Descendants("DbName").First().Value;
                Globals.DbUserName = document.Descendants("DbUserName").First().Value;
                Globals.DbUserPassword = document.Descendants("DbUserPassword").First().Value;

                Globals.DefaultIdBill = Convert.ToInt32(document.Descendants("IdBill").First().Value);
                if (!ManageDb.IsBillExist(Globals.DefaultIdBill)) Globals.DefaultIdBill = -1;

                Globals.DefaultId = new List<SettingDto>();
                var listBill = document.Descendants("Bill");
                foreach (var bill in listBill)
                {
                    var oneSetting = new SettingDto()
                    {
                        IdBill = Convert.ToInt32(bill.Descendants("IdBill").First().Value),
                        IdIncomeOperation = Convert.ToInt32(bill.Descendants("IdIncome").First().Value),
                        IdExpenseOperation = Convert.ToInt32(bill.Descendants("IdExpense").First().Value),
                    };
                    // Здесь проверить, что все Id правильные
                    if (ManageDb.IsBillExist(oneSetting.IdBill))
                    {
                        if (!ManageDb.IsOperationBelongBill(oneSetting.IdIncomeOperation, oneSetting.IdBill, "Добавить доход")) 
                            oneSetting.IdIncomeOperation = -1;
                        if (!ManageDb.IsOperationBelongBill(oneSetting.IdExpenseOperation, oneSetting.IdBill, "Отметить расход"))
                            oneSetting.IdExpenseOperation = -1;
                        Globals.DefaultId.Add(oneSetting);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Что-то пошло не так при считывании настроек: " + ex.Message);
            }
            Globals.ErrorText = "Что-то пошло не так";
        }

        public static void SaveSetting()
        {
            // Сохраняем все данные в файл конфигурации
            var xConnection = new XElement("connection",
                new XElement("ServerName", Globals.ServerName),
                new XElement("ServerPort", Globals.ServerPort),
                new XElement("DbName", Globals.DbName),
                new XElement("DbUserName", Globals.DbUserName),
                new XElement("DbUserPassword", Globals.DbUserPassword));

            var xSettings = new XElement("defaultSettings", new XElement("IdBill", Globals.DefaultIdBill));
            for (int i = 0; i < Globals.DefaultId.Count; i++)
            {
                var xBill = new XElement("Bill",
                    new XElement("IdBill", Globals.DefaultId[i].IdBill),
                    new XElement("IdIncome", Globals.DefaultId[i].IdIncomeOperation),
                    new XElement("IdExpense", Globals.DefaultId[i].IdExpenseOperation));
                xSettings.Add(xBill);
            }

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("settings", xConnection, xSettings));
            using (var writer = new XmlTextWriter("Settings.xml", new UTF8Encoding(false)))
            {
                doc.Save(writer);
            }
        }
    }
}
