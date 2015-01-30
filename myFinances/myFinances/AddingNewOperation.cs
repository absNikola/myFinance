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
    public partial class AddingNewOperation : Form
    {
        private KeyValuePair<int, string> NewOperationText = new KeyValuePair<int, string>(-2, "< Добавить новую категорию >");
        private KeyValuePair<int, string> NewOperationSeparator = new KeyValuePair<int, string>(0, "-----------------------------------------------------");
        public string NewOperationGroupName { get; set; }

        public AddingNewOperation()
        {
            InitializeComponent();
        }

        private void comboBox1_SetData()
        {
            var parentForm = Application.OpenForms[0] as MainForm;
            var idBill = -1;
            if (parentForm.comboBox1.SelectedIndex >= 0)
                idBill = ((KeyValuePair<int, string>)parentForm.comboBox1.Items[parentForm.comboBox1.SelectedIndex]).Key;


            // Перезаполнили значения в контроле
            comboBox1.Items.Clear();
            comboBox1.Items.Add(NewOperationText);
            comboBox1.Items.Add(NewOperationSeparator);

            if (ManageDb.CheckConnectionDb())
            {
                var listOperation = ManageDb.GetListOperation(idBill, this.Text);
                foreach (var operation in listOperation)
                    comboBox1.Items.Add(new KeyValuePair<int, string>(operation.Id, operation.Name));
            }


            // Определяем активный выбор строки
            var idOperation = -1;
            for (var i=0; i < Globals.DefaultId.Count; i++)
                if (Globals.DefaultId[i].IdBill == idBill) 
                {
                    if (this.Text == "Добавить доход") idOperation = Globals.DefaultId[i].IdIncomeOperation;
                    if (this.Text == "Отметить расход") idOperation = Globals.DefaultId[i].IdExpenseOperation;
                }
            var index = -1;
            for (var i = 0; i < this.comboBox1.Items.Count; i++)
                if (((KeyValuePair<int, string>)this.comboBox1.Items[i]).Key == idOperation)
                    index = i;
            comboBox1.SelectedIndex = index;
            button1.Select();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("")) textBox1.Text = "0";

            // Тут проверим что введены лишь цифры и выкинем лишнее
            var tmpString = string.Empty;
            var correctSymbols = "0123456789";
            for (var i = 0; i < textBox1.Text.Length; i++)
            {
                if (correctSymbols.Contains(textBox1.Text[i].ToString())) 
                    tmpString += textBox1.Text[i].ToString();
            }
            textBox1.Text = tmpString;

            // Проверим что число нормальное
            while (textBox1.Text[0] == '0')
            {
                if (textBox1.Text.Equals("0")) break;
                textBox1.Text = textBox1.Text.Substring(1, textBox1.Text.Length - 1);
            }

            // Тут проверяем строку на длину
            if (textBox1.Text.Length > 12) 
                textBox1.Text = textBox1.Text.Substring(0, 12);
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
            var parentForm = Application.OpenForms[0] as MainForm;
            var idOperation = -1;
            if (this.comboBox1.SelectedIndex >= 0)
                idOperation = ((KeyValuePair<int, string>) comboBox1.Items[comboBox1.SelectedIndex]).Key;

            if (!textBox1.Text.Equals("0"))
            {
                if (idOperation != -1)
                {
                    var newOperation = new OperationDto()
                    {
                        IdOperation = idOperation,
                        Amount = Convert.ToInt64(textBox1.Text),
                        Comment = textBox2.Text,
                    };
                    // Если операция сегодня - то знаем и дату и время, если в прошлом - то только дата
                    if (checkBox1.Checked) newOperation.Date = DateTime.Now;
                    else newOperation.Date = dateTimePicker1.Value.Date;
                    // Если же вдруг задали дату в будущем - кинем ошибку, операцию не сохраняем
                    if (newOperation.Date > DateTime.Now)
                    {
                        dateTimePicker1.Value = DateTime.Now;
                        MessageSender.SendMessage(this, "Нельзя отмечать траты в будущем", "Ошибка");
                        return;
                    }

                    var resultOperation = ManageDb.SaveOperationtoDb(newOperation, this.Text);
                    if (resultOperation.Equals("Success"))
                    {
                        MessageSender.SendMessage(this, "Данные успешно внесены", "Успешно");
                        Close();
                    }
                    else MessageSender.SendMessage(this, resultOperation, "Ошибка");
                    // В формочку проставили нормальные значения
                    comboBox1_SetData();
                }
                else MessageSender.SendMessage(this, "       Выбрана неверная операция", "Ошибка");
            }
            else MessageSender.SendMessage(this, "   Необходимо внести сумму операции", "Ошибка");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
                if (comboBox1.Items[comboBox1.SelectedIndex].Equals(NewOperationSeparator))
                    comboBox1.SelectedIndex = -1;

            if (comboBox1.SelectedIndex != -1)
                if (((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key == NewOperationText.Key)
                {
                    var newForm = new AddingNewOperationGroup();
                    newForm.StartPosition = FormStartPosition.CenterParent;
                    newForm.ShowDialog();
                    // Перезаполнили значения в контроле
                    comboBox1_SetData();


                    var newIdOperation = ManageDb.GetIdOperationbyName(NewOperationGroupName, this.Text);
                    // Определяем активный выбор строки для новой записи
                    var index = -1;
                    for (var i = 0; i < this.comboBox1.Items.Count; i++)
                        if (((KeyValuePair<int, string>)this.comboBox1.Items[i]).Key == newIdOperation)
                            index = i;
                    comboBox1.SelectedIndex = index;
                }
        }

        private void AddingNewOperation_Load(object sender, EventArgs e)
        {
            // Значения по-умолчанию
            label1.Text = "Сумма";
            label2.Text = "Комментарий";
            textBox1.Text = "0";
            textBox2.Text = string.Empty;
            comboBox1_SetData();
            comboBox1.DisplayMember = "Value";
            dateTimePicker1.Value = DateTime.Now;

            // Активность контролов
            textBox1.Select();
            textBox1.SelectionStart = textBox1.Text.Length;
            dateTimePicker1.Enabled = false;
            checkBox1.Checked = true;
        }
    }
}
