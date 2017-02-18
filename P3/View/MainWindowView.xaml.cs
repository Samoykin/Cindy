using P3.DataUpd;
using P3.Model; //
using P3.Utils; //
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; //
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

namespace P3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindowView : Window
    {
        DBconnect dbc = new DBconnect();
        List<String> tID = new List<String>();

        public MainWindowView()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IDData upd = new IDData();
            upd.ParseHTML();
        }

        public void testMet()
        {
            ObservableCollection<Employee> employeeLst = new ObservableCollection<Employee>();

            employeeLst = dbc.EmployeeRead();

            foreach (Employee st in employeeLst)
            {
                tID.Add(st.ID);
            }

            UpdPersonalData upd = new UpdPersonalData();
            upd.ParseHTML();

            Status stEmpl2 = new Status(); // статусы
            stEmpl2.ParseHTML(); //статусы

            dDays dd = new dDays(); //дата прихода и рождения
            dd.tID = tID;
            dd.ParseHTML();

            dPic dlP = new dPic(); //фотки
            dlP.tID = tID;
            dlP.ParseHTML();
        }

    }
}
