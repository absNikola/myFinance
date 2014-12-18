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

                Globals.DefaultIdBill = Convert.ToInt32(document.Descendants("DefaultIdBill").First().Value);
                Globals.DefaultIdExpence = Convert.ToInt32(document.Descendants("DefaultIdExpence").First().Value);
                Globals.DefaultIdIncome = Convert.ToInt32(document.Descendants("DefaultIdIncome").First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Что-то пошло не так при считывании настроек: " + ex.Message);
            }

            Globals.ErrorText = "Что-то пошло не так";
        }

        public static void SaveSetting(XDocument doc)
        {
            using (var writer = new XmlTextWriter("Settings.xml", new UTF8Encoding(false)))
            {
                doc.Save(writer);
            }
        }
    }
}
