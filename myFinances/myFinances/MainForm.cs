using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MySql.Data.MySqlClient;

namespace myFinances
{
    public partial class MainForm : Form
    {
        private KeyValuePair<int, string> NewBillText = new KeyValuePair<int, string>(-2, "< Добавить новый счёт >");
        private KeyValuePair<int, string> NewBillSeparator = new KeyValuePair<int, string>(0, "-----------------------------------------------------");

        public MainForm()
        {
            InitializeComponent();
            comboBox1_SetData();
            comboBox1.DisplayMember = "Value";
            label1.Text = "Выберите счёт";
        }

        private void comboBox1_SetData()
        {
            // Перезаполнили значения в контроле
            comboBox1.Items.Clear();
            comboBox1.Items.Add(NewBillText);
            comboBox1.Items.Add(NewBillSeparator);

            if (ManageDb.CheckConnectionDb())
            {
                var listBill = ManageDb.GetListBill();
                foreach (var bill in listBill) 
                    comboBox1.Items.Add(new KeyValuePair<int, string>(bill.Id, bill.Name));
            }

            // Определяем активный выбор строки
            var index = -1;
            for (var i = 0; i<comboBox1.Items.Count; i++)
                if (((KeyValuePair<int, string>) comboBox1.Items[i]).Key == Globals.DefaultIdBill) 
                    index = i;
            comboBox1.SelectedIndex = index;
            button1.Select();
        }

        private void myFinances_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) 
                MessageSender.SendMessage(this, "Не выбран счёт для совершения операций", "Ошибка");
            else
            {
                // Проверяем что для выбранной строки есть запись в БД
                var listBill = ManageDb.GetListBill();
                var flag = false;
                foreach (var bill in listBill)
                    if (bill.Id == ((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key) flag = true;

                if (flag)
                {
                    // Здесь необходимо добавить запись в БД
                    var newForm = new AddingNewOperation()
                    {
                        Text = "Добавить доход",
                        StartPosition = FormStartPosition.CenterParent,
                    };
                    newForm.ShowDialog();
                }else MessageSender.SendMessage(this, "           Выбран несуществующий счёт \n" +
                                                 "               для совершения операций", "Ошибка");  
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
                if (comboBox1.Items[comboBox1.SelectedIndex].Equals(NewBillSeparator))
                    comboBox1.SelectedIndex = -1;

            if (comboBox1.SelectedIndex != -1)
                if (((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key == NewBillText.Key)
                {
                    var newForm = new AddingNewBill();
                    newForm.StartPosition = FormStartPosition.CenterParent;
                    newForm.ShowDialog();
                    // Перезаполнили значения в контроле
                    comboBox1_SetData();
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                MessageSender.SendMessage(this, "Не выбран счёт для совершения операций", "Ошибка");
            else
            {
                // Проверяем что для выбранной строки есть запись в БД
                var listBill = ManageDb.GetListBill();
                var flag = false;
                foreach (var bill in listBill)
                    if (bill.Id == ((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key) flag = true;

                if (flag)
                {
                    // Здесь необходимо добавить запись в БД
                    var newForm = new AddingNewOperation()
                    {
                        Text = "Отметить расход",
                        StartPosition = FormStartPosition.CenterParent,
                    };
                    newForm.ShowDialog();
                }
                else MessageSender.SendMessage(this, "           Выбран несуществующий счёт \n" +
                                                "               для совершения операций", "Ошибка");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var newForm = new OptionsWindow()
            {
                Text = "Настройки приложения",
                StartPosition = FormStartPosition.CenterParent,
            };
            newForm.ShowDialog();

            ManageSetting.ReadSetting();
            comboBox1_SetData();
        }
    }
}
