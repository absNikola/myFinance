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
        public string NewBillText = "< Добавить новый счёт >";
        public string NewBillSeparator = "-----------------------------------------------------";
        public MainForm()
        {
            InitializeComponent();

            // Значения в выпадашку
            comboBox1_SetData();
            label1.Text = "Выберите счёт";
        }

        private void comboBox1_SetData()
        {
            // Перезаполнили значения в контроле
            comboBox1.Items.Clear();
            comboBox1.Items.Add(NewBillText);
            comboBox1.Items.Add(NewBillSeparator);
            var listBill = LoadDataDB.GetListBill();
            foreach (var bill in listBill) comboBox1.Items.Add(bill.Name);

            
            if (comboBox1.Items.Count == 2) comboBox1.SelectedIndex = -1; 
            else comboBox1.SelectedIndex = Globals.DefaultIdBill;

            button1.Select();
        }

        private void myFinances_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageSender.SendError(this, "Не выбран счёт для совершения операций");
            }
            else
            {
                if (LoadDataDB.GetIdBillbyName(comboBox1.Items[comboBox1.SelectedIndex].ToString()) != -1)
                {
                    // Здесь необходимо добавить запись в БД
                    var newForm = new AddingNewIncome();
                    newForm.StartPosition = FormStartPosition.CenterParent;
                    newForm.ShowDialog();
                }
                else MessageSender.SendError(this, "           Выбран несуществующий счёт \n" +
                                                 "               для совершения операций");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
                if (comboBox1.Items[comboBox1.SelectedIndex].Equals(NewBillSeparator))
                    comboBox1.SelectedIndex = -1;

            if (comboBox1.SelectedIndex != -1)
                if (comboBox1.Items[comboBox1.SelectedIndex].Equals(NewBillText))
                {
                    var newForm = new AddingNewBill();
                    newForm.StartPosition = FormStartPosition.CenterParent;
                    newForm.ShowDialog();
                    // Перезаполнили значения в контроле
                    comboBox1_SetData();
                }
        }
    }
}
