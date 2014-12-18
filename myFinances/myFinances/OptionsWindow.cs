using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace myFinances
{
    public partial class OptionsWindow : Form
    {
        private int MaxLenghtTextField = 45;
        
        public OptionsWindow()
        {
            InitializeComponent();
            ManageSetting.ReadSetting();
            InitializeData();
        }

        private void InitializeData()
        {
            label1.Text = "Имя сервера";
            textBox1.Text = Globals.ServerName;
            label2.Text = "Порт сервера";
            textBox2.Text = Globals.ServerPort.ToString();
            label3.Text = "Название БД";
            textBox3.Text = Globals.DbName;
            label4.Text = "Логин пользователя";
            textBox4.Text = Globals.DbUserName;
            label5.Text = "Пароль пользователя";
            textBox5.Text = Globals.DbUserPassword;

            groupBox1.Text = "Значения по-умолчанию";
            label6.Text = "Счёт";
            comboBox1_SetData();
            label7.Text = "Операция добавления";
            comboBox2_SetData();
            label8.Text = "Расходная операция";
            comboBox3_SetData();

            button1.Text = "Отмена";
            button2.Text = "Применить";
            button3.Text = "Проверить подключение";

            button2.Select();
        }

        private void comboBox1_SetData()
        {
            // Перезаполнили значения в контроле
            comboBox1.Items.Clear();
            if (ManageDb.CheckConnectionDb())
            {
                var listBill = ManageDb.GetListBill();
                foreach (var bill in listBill) comboBox1.Items.Add(bill.Name);
            }
            
            if (comboBox1.Items.Count == 0)
            {
                comboBox1.SelectedIndex = -1;
                comboBox1.Enabled = false;
            }
            else if (Globals.DefaultIdBill - 1 <= comboBox1.Items.Count) comboBox1.SelectedIndex = Globals.DefaultIdBill - 1;
                 else comboBox1.SelectedIndex = -1;
        }

        private void comboBox2_SetData()
        {
            // Перезаполнили значения в контроле
            comboBox2.Items.Clear();
            if (ManageDb.CheckConnectionDb())
            {
                var listOperation = ManageDb.GetListOperation(-1, "Добавить доход");
                foreach (var operation in listOperation) comboBox2.Items.Add(operation.Name);
            }

            if (comboBox2.Items.Count == 0)
            {
                comboBox2.SelectedIndex = -1;
                comboBox2.Enabled = false;
            }
            else if (Globals.DefaultIdIncome - 1 <= comboBox2.Items.Count) comboBox2.SelectedIndex = Globals.DefaultIdIncome - 1;
            else comboBox2.SelectedIndex = -1;
        }

        private void comboBox3_SetData()
        {
            // Перезаполнили значения в контроле
            comboBox3.Items.Clear();
            if (ManageDb.CheckConnectionDb())
            {
                var listOperation = ManageDb.GetListOperation(-1, "Отметить расход");
                foreach (var operation in listOperation) comboBox3.Items.Add(operation.Name);
            }

            if (comboBox3.Items.Count == 0)
            {
                comboBox3.SelectedIndex = -1;
                comboBox3.Enabled = false;
            }
            else if (Globals.DefaultIdExpence - 1 <= comboBox3.Items.Count) comboBox3.SelectedIndex = Globals.DefaultIdExpence - 1;
            else comboBox3.SelectedIndex = -1;
        }

        private void OptionsWindow_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox1.Text.Length > MaxLenghtTextField) textBox1.Text = textBox1.Text.Substring(0, MaxLenghtTextField);
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Тут проверим что введены лишь цифры и выкинем лишнее
            var tmpString = string.Empty;
            var correctSymbols = "0123456789";
            for (var i = 0; i < textBox2.Text.Length; i++)
            {
                if (correctSymbols.Contains(textBox2.Text[i].ToString())) tmpString += textBox2.Text[i].ToString();
            }
            textBox2.Text = tmpString;

            // Проверим что число нормальное
            if (textBox2.Text.Length > 0)
                while (textBox2.Text[0] == '0')
                {
                    if (textBox2.Text.Equals("0")) break;
                    textBox2.Text = textBox2.Text.Substring(1, textBox2.Text.Length - 1);
                }

            // Тут проверяем строку на длину
            if (textBox2.Text.Length > MaxLenghtTextField) textBox2.Text = textBox2.Text.Substring(0, MaxLenghtTextField);
            textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox3.Text.Length > MaxLenghtTextField) textBox3.Text = textBox3.Text.Substring(0, MaxLenghtTextField);
            textBox3.SelectionStart = textBox3.Text.Length;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox4.Text.Length > MaxLenghtTextField) textBox4.Text = textBox4.Text.Substring(0, MaxLenghtTextField);
            textBox4.SelectionStart = textBox4.Text.Length;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox5.Text.Length > MaxLenghtTextField) textBox5.Text = textBox5.Text.Substring(0, MaxLenghtTextField);
            textBox5.SelectionStart = textBox5.Text.Length;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Сохраняем все данные в файл конфигурации
            var xConnection = new XElement("connection",
                new XElement("ServerName", this.textBox1.Text),
                new XElement("ServerPort", this.textBox2.Text),
                new XElement("DbName", this.textBox3.Text),
                new XElement("DbUserName", this.textBox4.Text),
                new XElement("DbUserPassword", this.textBox5.Text));

            var xSettings = new XElement("defaultSettings", 
                new XElement("DefaultIdBill", Globals.DefaultIdBill),
                new XElement("DefaultIdIncome", Globals.DefaultIdIncome),
                new XElement("DefaultIdExpence", Globals.DefaultIdExpence));

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("settings", xConnection, xSettings));

            ManageSetting.SaveSetting(doc);
            MessageSender.SendMessage(this, "                Данные успешно обновлены", "Успешно");
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Globals.ServerName = textBox1.Text;
            Globals.ServerPort = Convert.ToInt32(textBox2.Text);
            Globals.DbName = textBox3.Text;
            Globals.DbUserName = textBox4.Text;
            Globals.DbUserPassword = textBox5.Text;

            if (ManageDb.CheckConnectionDb())
            {
                MessageSender.SendMessage(this, "                   Подключение успешно", "Успешно");
            }
            else
            {
                MessageSender.SendMessage(this, "Настройки подключения заданы неверно", "Ошибка");
            }

            ManageSetting.ReadSetting();
        }
    }
}
