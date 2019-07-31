using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
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

            label1.Text = "Выберите счёт";
            label2.Text = "Период включительно";
            DateTime startDate = Convert.ToDateTime("01.01.1900");
            DateTime finishDate = Convert.ToDateTime("31.12.2200");
            if (DateTime.Today.Day >= 1 && DateTime.Today.Day <= 9)
            {
                startDate = (DateTime.Today.Month > 1 )
                    ? new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 25)
                    : new DateTime(DateTime.Today.Year - 1, 12, 25);
                finishDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 9);
            } 
            else if (DateTime.Today.Day >= 10 && DateTime.Today.Day <= 24)
            {
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 10);
                finishDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 24);
            } 
            else if (DateTime.Today.Day >= 25)
            {
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 25);
                finishDate = (DateTime.Today.Month < 12)
                        ? new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 9)
                        : new DateTime(DateTime.Today.Year + 1, 1, 9);
            }
            dateTimePicker1.Value = startDate;
            dateTimePicker2.Value = finishDate;
            button4.Text = "Расходы";

            comboBox1_SetData();
            comboBox1.DisplayMember = "Value";

            dataGrid1_SetData();
        }

        private void CountAmount(List<ShowedDataDto> listData, int parentIdList)
        {
            for (var i = parentIdList + 1; i < listData.Count; i++)
                if (listData[i].ParentId == listData[parentIdList].Id)
                {
                    if (listData[i].Id != -1) CountAmount(listData, i);
                    listData[parentIdList].Amount += listData[i].Amount;
                }
        }
        private void dataGrid1_SetData()
        {
            dataGridView1.Rows.Clear();

            var action = string.Empty;
            if (button4.Text.Equals("Расходы")) action = "Отметить расход";
            if (button4.Text.Equals("Доходы")) action = "Добавить доход";

            var showedOperation = new List<ShowedDataDto>();
            // Сначала скушали структуру таблицы
            // в зависимости от выбранного счета idBill
            if (comboBox1.SelectedIndex == -1) return;
            var listOperation = ManageDb.GetListOperationStructure(((KeyValuePair<int, string>)comboBox1.Items[comboBox1.SelectedIndex]).Key, action);
            foreach (var operation in listOperation)
            {
                var newOperation = new ShowedDataDto()
                {
                    Id = operation.Id,
                    ParentId = operation.ParentId,
                    Name = operation.Name,
                    Amount = 0,
                    Comment = operation.Comment,
                    Date = null,
                };
                showedOperation.Add(newOperation);
            }

            // Самое время примапить данные
            var listWasted = ManageDb.GetListOperationByDate(dateTimePicker1.Value, dateTimePicker2.Value, action);
            foreach (var operation in listWasted)
            {
                for (var i = 0; i < showedOperation.Count; i++)
                    if (operation.OperationStructureId == showedOperation[i].Id)
                    {
                        var newOperation = new ShowedDataDto();
                        newOperation.Id = -1;
                        newOperation.ParentId = operation.OperationStructureId;
                        newOperation.Name = string.Empty;
                        newOperation.Amount = operation.Amount;
                        newOperation.Comment = operation.Comment;
                        newOperation.Date = operation.Date;
                        showedOperation.Insert(i + 1, newOperation);
                    }
            }

            // И теперь сосчитали сумму в родительских элементах
            for (var i = 0; i < showedOperation.Count; i++)
                if (showedOperation[i].Id == showedOperation[i].ParentId)
                    CountAmount(showedOperation, i);

            // Отображаем лишь те категории, где сумма ненулевая
            for (var i = 0; i < showedOperation.Count; i++)
            {
                var date = string.Empty;
                if (showedOperation[i].Date != null)
                    date = Convert.ToDateTime(showedOperation[i].Date).ToString("dd.MM.yyyy");

                var item = string.Empty;
                var percentage = string.Empty;
                if (showedOperation[i].Id != -1)
                {
                    item = "-";
                    var percentageValue = showedOperation[0].Amount <= 0
                        ? 0
                        : 100 * ((double)showedOperation[i].Amount / showedOperation[0].Amount);
                    percentage = percentageValue.ToString("N2");
                }

                if (showedOperation[i].Amount > 0)
                    dataGridView1.Rows.Add(item, showedOperation[i].Name, showedOperation[i].Amount, showedOperation[i].Comment, date, percentage);
            }
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
            for (var i = 0; i < comboBox1.Items.Count; i++)
                if (((KeyValuePair<int, string>)comboBox1.Items[i]).Key == Globals.DefaultIdBill)
                    index = i;
            comboBox1.SelectedIndex = index;
            button1.Select();
        }

        private void myFinances_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) MessageSender.SendMessage(this, "Не выбран счёт для совершения операций", "Ошибка");
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
                    dataGrid1_SetData();
                }
                else MessageSender.SendMessage(this, "".PadLeft(11) + "Выбран несуществующий счёт \n" + "".PadLeft(15) + "для совершения операций", "Ошибка");
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

            dataGrid1_SetData();
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
                    dataGrid1_SetData();
                }
                else MessageSender.SendMessage(this, "".PadLeft(11) + "Выбран несуществующий счёт \n" + "".PadLeft(15) + "для совершения операций", "Ошибка");
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value < Convert.ToDateTime("01.01.1900"))
                dateTimePicker1.Value = Convert.ToDateTime("01.01.1900");
            if (dateTimePicker1.Value > Convert.ToDateTime("31.12.2200"))
                dateTimePicker1.Value = Convert.ToDateTime("31.12.2200");
            if (dateTimePicker2.Value < dateTimePicker1.Value)
                dateTimePicker1.Value = dateTimePicker2.Value;

            // И перезаполнили значения в таблице
            dataGrid1_SetData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value < Convert.ToDateTime("01.01.1900"))
                dateTimePicker2.Value = Convert.ToDateTime("01.01.1900");
            if (dateTimePicker2.Value > Convert.ToDateTime("31.12.2200"))
                dateTimePicker2.Value = Convert.ToDateTime("31.12.2200");
            if (dateTimePicker2.Value < dateTimePicker1.Value)
                dateTimePicker2.Value = dateTimePicker1.Value;

            // И перезаполнили значения в таблице
            dataGrid1_SetData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text.Equals("Расходы")) button4.Text = "Доходы";
            else button4.Text = "Расходы";

            dataGrid1_SetData();
        }
    }
}
