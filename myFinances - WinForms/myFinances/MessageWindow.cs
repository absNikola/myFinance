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
    public partial class MessageWindow : Form
    {
        public MessageWindow()
        {
            InitializeComponent();
            label1.Text = Globals.ErrorText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
