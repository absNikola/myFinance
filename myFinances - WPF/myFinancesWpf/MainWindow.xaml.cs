using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace myFinancesWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Che

            this.Button_Settings.Content = "Настройки";
            this.Button_Income.Content = "Добавить доход";
            this.Label_Bill.Content = "Выберите счёт";
            this.Button_Expence.Content = "Отметить расход";

            this.DatePicker_Start.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            this.Label_Period.Content = "Период";
            this.DatePicker_End.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, 1).AddDays(-1);

            this.Button_ChangeOperation.Content = "";
        }
    }
}
