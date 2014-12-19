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
            comboBox1.DisplayMember = "Value";
            label7.Text = "Операция добавления";
            comboBox2_SetData();
            comboBox2.DisplayMember = "Value";
            label8.Text = "Расходная операция";
            comboBox3_SetData();
            comboBox3.DisplayMember = "Value";

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
                foreach (var bill in listBill)
                    comboBox1.Items.Add(new KeyValuePair<int, string>(bill.Id, bill.Name));
            }
            if (comboBox1.Items.Count == 0)
            {
                comboBox1.Enabled = false;
                comboBox1.SelectedIndex = -1;
            }

            // Определяем активный выбор строки
            var index = -1;
            for (var i = 0; i < comboBox1.Items.Count; i++)
                if (((KeyValuePair<int, string>)comboBox1.Items[i]).Key == Globals.DefaultIdBill)
                    index = i;
            comboBox1.SelectedIndex = index;
            button2.Select();
        }

        private void comboBox2_SetData()
        {
            // Вычисляем выбранный счет
            // И по нему определяем дефолтное заданное значение операции
            var idBill = -1;
            if (comboBox1.Enabled) 
                idBill = ((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key;
            var idDefaultOperation = -1;
            for (var i = 0; i < Globals.DefaultId.Count; i++)
                if (Globals.DefaultId[i].IdBill == idBill) 
                    idDefaultOperation = Globals.DefaultId[i].IdIncomeOperation;

            // Перезаполнили значения в контроле
            comboBox2.Items.Clear();
            if (ManageDb.CheckConnectionDb())
            {
                var listOperation = ManageDb.GetListOperation(idBill, "Добавить доход");
                foreach (var operation in listOperation) 
                    comboBox2.Items.Add(new KeyValuePair<int, string>(operation.Id, operation.Name));
            }
            if (comboBox2.Items.Count == 0)
            {
                comboBox2.Enabled = false;
                comboBox2.SelectedIndex = -1;
            }

            // Определяем активный выбор строки
            // Пробеум найти значение по-дефолту и выставить эту строку активной, если нет - то задаем -1
            var index = -1;
            for (var i = 0; i < comboBox2.Items.Count; i++)
                if (((KeyValuePair<int, string>)comboBox2.Items[i]).Key == idDefaultOperation)
                    index = i;
            comboBox2.SelectedIndex = index;
            button2.Select();
        }

        private void comboBox3_SetData()
        {
            // Вычисляем выбранный счет
            // И по нему определяем дефолтное заданное значение операции
            var idBill = -1;
            if (comboBox1.Enabled) 
                idBill = ((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key;
            var idDefaultOperation = -1;
            for (var i = 0; i < Globals.DefaultId.Count; i++)
                if (Globals.DefaultId[i].IdBill == idBill)
                    idDefaultOperation = Globals.DefaultId[i].IdExpenseOperation;

            // Перезаполнили значения в контроле
            comboBox3.Items.Clear();
            if (ManageDb.CheckConnectionDb())
            {
                var listOperation = ManageDb.GetListOperation(idBill, "Отметить расход");
                foreach (var operation in listOperation)
                    comboBox3.Items.Add(new KeyValuePair<int, string>(operation.Id, operation.Name));
            }
            if (comboBox3.Items.Count == 0)
            {
                comboBox3.Enabled = false;
                comboBox3.SelectedIndex = -1;
            }

            // Определяем активный выбор строки
            // Пробеум найти значение по-дефолту и выставить эту строку активной, если нет - то задаем -1
            var index = -1;
            for (var i = 0; i < comboBox3.Items.Count; i++)
                if (((KeyValuePair<int, string>)comboBox3.Items[i]).Key == idDefaultOperation)
                    index = i;
            comboBox3.SelectedIndex = index;
            button2.Select();
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
            if (textBox1.Text.Length > MaxLenghtTextField) 
                textBox1.Text = textBox1.Text.Substring(0, MaxLenghtTextField);
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals("")) textBox2.Text = "0";
            // Тут проверим что введены лишь цифры и выкинем лишнее
            var tmpString = string.Empty;
            var correctSymbols = "0123456789";
            for (var i = 0; i < textBox2.Text.Length; i++)
                if (correctSymbols.Contains(textBox2.Text[i].ToString())) 
                    tmpString += textBox2.Text[i].ToString();
            textBox2.Text = tmpString;

            // Проверим что число нормальное
            if (textBox2.Text.Length > 0)
                while (textBox2.Text[0] == '0')
                {
                    if (textBox2.Text.Equals("0")) break;
                    textBox2.Text = textBox2.Text.Substring(1, textBox2.Text.Length - 1);
                }

            // Тут проверяем строку на длину
            if (textBox2.Text.Length > MaxLenghtTextField) 
                textBox2.Text = textBox2.Text.Substring(0, MaxLenghtTextField);
            textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox3.Text.Length > MaxLenghtTextField) 
                textBox3.Text = textBox3.Text.Substring(0, MaxLenghtTextField);
            textBox3.SelectionStart = textBox3.Text.Length;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox4.Text.Length > MaxLenghtTextField) 
                textBox4.Text = textBox4.Text.Substring(0, MaxLenghtTextField);
            textBox4.SelectionStart = textBox4.Text.Length;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Тут проверяем строку на длину
            if (textBox5.Text.Length > MaxLenghtTextField) 
                textBox5.Text = textBox5.Text.Substring(0, MaxLenghtTextField);
            textBox5.SelectionStart = textBox5.Text.Length;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Сохраняем все данные в файл конфигурации
            Globals.ServerName = this.textBox1.Text;
            Globals.ServerPort = Convert.ToInt32(this.textBox2.Text);
            Globals.DbName = this.textBox3.Text;
            Globals.DbUserName = this.textBox4.Text;
            Globals.DbUserPassword = this.textBox5.Text;

            var idBill = -1;
            if (this.comboBox1.Enabled)
                idBill = ((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key;
            var idIncome = -1;
            if (this.comboBox2.Enabled)
                idIncome = ((KeyValuePair<int, string>)comboBox2.Items[comboBox2.SelectedIndex]).Key;
            var idExpense = -1;
            if (this.comboBox3.Enabled)
                idExpense = ((KeyValuePair<int, string>)comboBox3.Items[comboBox3.SelectedIndex]).Key;

            var isChange = false;
            // в случае если у нас уже есть настроки для данного счета - обновляем их
            for (int i = 0; i < Globals.DefaultId.Count; i++)
                if (Globals.DefaultId[i].IdBill == idBill && idBill != -1)
                {
                    isChange = true;
                    Globals.DefaultId[i].IdIncomeOperation = idIncome;
                    Globals.DefaultId[i].IdExpenseOperation = idExpense;
                }
            // в случае если для данного счета еще не было настроек - добавим их
            if (!isChange && idBill != -1)
            {
                var oneSetting = new SettingDto()
                {
                    IdBill = idBill,
                    IdIncomeOperation = idIncome,
                    IdExpenseOperation = idExpense,
                };
                Globals.DefaultId.Add(oneSetting);
            }
            
            ManageSetting.SaveSetting();
            MessageSender.SendMessage(this, "                Данные успешно обновлены", "Успешно");
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Сначала задаем новые введенные значения
            Globals.ServerName = textBox1.Text;
            Globals.ServerPort = Convert.ToInt32(textBox2.Text);
            Globals.DbName = textBox3.Text;
            Globals.DbUserName = textBox4.Text;
            Globals.DbUserPassword = textBox5.Text;

            if (ManageDb.CheckConnectionDb())
                MessageSender.SendMessage(this, "               Подключение успешно", "Успешно");
            else 
                MessageSender.SendMessage(this, "Настройки подключения заданы неверно", "Ошибка");
            // Затем возвращаем обратно дефолтные настройки
            ManageSetting.ReadSetting();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2_SetData();
            comboBox3_SetData();
        }
    }
}
