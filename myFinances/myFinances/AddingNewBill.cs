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
                    if (!saveBillResult.Equals("Success")) MessageSender.SendMessage(this, saveBillResult, "Ошибка");
                }
                else MessageSender.SendMessage(this, "Проверьте настройки подключения", "Ошибка");
                Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var tmpString = textBox1.Text;
            var tmpLenght = textBox1.Text.Length;
            if (tmpString.Length > 28) tmpLenght = 28;

            textBox1.Text = tmpString.Substring(0, tmpLenght);
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var tmpString = textBox2.Text;
            var tmpLenght = textBox2.Text.Length;
            if (tmpString.Length > 28) tmpLenght = 28;

            textBox2.Text = tmpString.Substring(0, tmpLenght);
            textBox2.SelectionStart = textBox2.Text.Length;
        }
    }
}
