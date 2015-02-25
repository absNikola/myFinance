using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myFinances
{
    public partial class AddingNewBill : Form
    {
        private int MaxLenghtTextField = 28;

        public AddingNewBill()
        {
            InitializeComponent();

            label1.Text = "Название счёта";
            label2.Text = "Комментарий";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сначала проверить символы на корректность
            // Полагаем, что все символы хорошие
            if (textBox1.Text==string.Empty)
            {
                MessageSender.SendMessage(this, "Название счёта не может быть пустым", "Ошибка");
            }
            else
            {
                if (ManageDb.CheckConnectionDb())
                {
                    // Затем добавить запись в БД
                    var bill = new BillDto()
                    {
                        Name = textBox1.Text,
                        Comment = textBox2.Text,
                    };
                    var saveBillResult = ManageDb.SaveBilltoDb(bill);
                    if (!saveBillResult.Equals("Success")) 
                        MessageSender.SendMessage(this, saveBillResult, "Ошибка");
                }
                else MessageSender.SendMessage(this, "Проверьте настройки подключения", "Ошибка");
                Close();
            }
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
    }
}
