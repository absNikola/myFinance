using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myFinances
{
    public partial class AddingNewIncome : Form
    {
        public string NewOperationText = "< Добавить новую категорию >";
        public string NewOperationSeparator = "-----------------------------------------------------";
        public int SelectedIdBill = -1;

        public AddingNewIncome()
        {
            InitializeComponent();

            var parentForm = Application.OpenForms[0] as MainForm;
            var index = parentForm.comboBox1.SelectedIndex;
            SelectedIdBill = LoadDataDB.GetIdBillbyName(parentForm.comboBox1.Items[index].ToString());

            InitializeData();
        }

        private void InitializeData() 
        {
            // Значения по-умолчанию
            label1.Text = "Сумма";
            label2.Text = "Комментарий";
            textBox1.Text = "0";
            textBox2.Text = string.Empty;
            comboBox1_SetData();
            dateTimePicker1.Value = DateTime.Now;

            // Активность контролов
            comboBox1.Select();
            dateTimePicker1.Enabled = false;
            checkBox1.Checked = true;
        }

        private void comboBox1_SetData()
        {
            // Перезаполнили значения в контроле
            comboBox1.Items.Clear();
            //закомментировано тлк для отладки
            comboBox1.Items.Add(NewOperationText);
            comboBox1.Items.Add(NewOperationSeparator);
            var listOperation = LoadDataDB.GetListIncomeOperation(SelectedIdBill);
            foreach (var operation in listOperation) comboBox1.Items.Add(operation.Name);
            
            if (comboBox1.Items.Count == 1) comboBox1.SelectedIndex = -1;
            else comboBox1.SelectedIndex = Globals.DefaultIdIncome;

            button1.Select();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Тут проверим что введены лишь цифры и выкинем лишнее
            var tmpString = string.Empty;
            var correctSymbols = "0123456789";
            for (var i = 0; i < textBox1.Text.Length; i++)
            {
                if (correctSymbols.Contains(textBox1.Text[i].ToString())) tmpString += textBox1.Text[i].ToString();
            }
            textBox1.Text = tmpString;

            // Проверим что число нормальное
            while (textBox1.Text[0] == '0')
            {
                if (textBox1.Text.Equals("0")) break;
                textBox1.Text = textBox1.Text.Substring(1, textBox1.Text.Length - 1);
            }

            // Тут проверяем строку на длину
            if (textBox1.Text.Length > 12) textBox1.Text = tmpString.Substring(0, 12);
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                dateTimePicker1.Enabled = false;
            else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker1.Value = DateTime.Now;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals("0") && SelectedIdBill != -1)
            {
                var newIncome = new OperationDto()
                {
                    IdBill = SelectedIdBill,
                    Amount = Convert.ToInt64(textBox1.Text),
                    Comment = textBox2.Text,
                };
                if (checkBox1.Checked) newIncome.Date = DateTime.Today;
                else newIncome.Date = dateTimePicker1.Value;
                var resultOperation = SaveDataDB.SaveIncomeOperationtoDb(newIncome);
                if (resultOperation.Equals("Success"))
                {
                    MessageSender.SendOk(this, "Данные успешно внесены");
                }
                else
                {
                    MessageSender.SendError(this, resultOperation);
                }
                // В формочку проставили нормальные значения
                InitializeData();
            }
            else
            {
                MessageSender.SendError(this, "    Необходимо внести сумму операции");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
                if (comboBox1.Items[comboBox1.SelectedIndex].Equals(NewOperationSeparator))
                    comboBox1.SelectedIndex = -1;

            if (comboBox1.SelectedIndex != -1)
                if (comboBox1.Items[comboBox1.SelectedIndex].Equals(NewOperationText))
                {
                    var newForm = new AddingNewIncomeGroup();
                    newForm.StartPosition = FormStartPosition.CenterParent;
                    newForm.ShowDialog();
                    // Перезаполнили значения
                    InitializeData();
                }
        }
    }
}
