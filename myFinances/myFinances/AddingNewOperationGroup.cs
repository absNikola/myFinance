using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myFinances
{
    public partial class AddingNewOperationGroup : Form
    {
        private int MaxLenghtTextField = 28;

        public AddingNewOperationGroup()
        {
            InitializeComponent();
            comboBox1_SetData();
            comboBox1.DisplayMember = "Value";
            comboBox2_SetData();
            comboBox2.DisplayMember = "Value";
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


            var parentForm = Application.OpenForms[0] as MainForm;
            var idBill = -1;
            if (parentForm.comboBox1.SelectedIndex >= 0)
                idBill = ((KeyValuePair<int, string>)parentForm.comboBox1.Items[parentForm.comboBox1.SelectedIndex]).Key;
            // Определяем активный выбор строки
            var index = -1;
            for (var i = 0; i < comboBox1.Items.Count; i++)
                if (((KeyValuePair<int, string>)comboBox1.Items[i]).Key == idBill)
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
                var parentFormOperation = Application.OpenForms[1] as AddingNewOperation;
                var listOperation = ManageDb.GetListOperation(idBill, parentFormOperation.Text);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > MaxLenghtTextField)
                textBox1.Text = textBox1.Text.Substring(0, MaxLenghtTextField);
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > MaxLenghtTextField)
                textBox2.Text = textBox2.Text.Substring(0, MaxLenghtTextField);
            textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var idBill = -1;
            if (this.comboBox1.Enabled)
                idBill = ((KeyValuePair<int, string>)this.comboBox1.Items[this.comboBox1.SelectedIndex]).Key;
            var idParent = -1;
            if (this.comboBox2.Enabled)
                idParent = ((KeyValuePair<int, string>)this.comboBox2.Items[this.comboBox2.SelectedIndex]).Key;

            // Сначала проверить символы на корректность
            // Полагаем, что все символы хорошие
            if (textBox1.Text == string.Empty)
            {
                MessageSender.SendMessage(this, "Название группы не может быть пустым", "Ошибка");
            }
            else
            {
                if (ManageDb.CheckConnectionDb())
                {
                    // Затем добавить запись в БД
                    var structure = new StructureDto()
                    {
                        ParentId = idParent,
                        IdBill = idBill,
                        Name = textBox1.Text,
                        Comment = textBox2.Text,
                    };
                    var parentFormOperation = Application.OpenForms[1] as AddingNewOperation;
                    var saveStructureResult = ManageDb.SaveStructuretoDb(structure, parentFormOperation.Text);
                    if (!saveStructureResult.Equals("Success"))
                        MessageSender.SendMessage(this, saveStructureResult, "Ошибка");
                    else parentFormOperation.NewOperationGroupName = structure.Name;
                }
                else MessageSender.SendMessage(this, "Проверьте настройки подключения", "Ошибка");
                Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2_SetData();
        }
    }
}
