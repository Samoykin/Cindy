using P3.Model;
using P3.Updater;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P3.ViewModel
{
    class MainViewModel
    {

        DBconnect dbc = new DBconnect();
        String path = @"DBTels.sqlite";

        #region Settings
        String _localPropPath = "LocalProp.xml";
        #endregion

        #region Properties
        //public People person { get; set; }
        public Misc misc { get; set; }
        public ObservableCollection<Division> div { get; set; }
        //public ObservableCollection<People> people { get; set; }

        public ObservableCollection<Employee> employeeLst { get; set; }
        public ObservableCollection<Employee> employeeLstT { get; set; }

        public ObservableCollection<Customer> customerLst { get; set; }
        public ObservableCollection<Customer> customerLstT { get; set; }

        public ObservableCollection<DinnerList> dinnerLst { get; set; }
        public Employee selectedDiv { get; set; }



        #endregion

        #region Commands

        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommandCust { get; set; }
        public ICommand ClickCommand2 { get; set; }
        public ICommand ClickCommand2Cust { get; set; }
        public ICommand ClickCommand3 { get; set; }
        public ICommand ClickCommand4 { get; set; }
        public ICommand ClickCommand5 { get; set; }
        public ICommand ClickCommand6 { get; set; }

        public ICommand ClickCommandAddPerson { get; set; }

        #endregion

        public MainViewModel()
        {
            XMLcode xml = new XMLcode(_localPropPath); ;
            if (!File.Exists(_localPropPath))
            {
                xml.CreateXml();
                xml.CreateNodesXml();
                
            }


            try
            {
                SoftUpdater upd = new SoftUpdater(xml);
                upd.UpdateSoft();
            }
            catch { }

            if (!File.Exists(path))
            {
                dbc.CreateBase();
                dbc.EmployeeCreateTable();
                dbc.CustomerCreateTable();
                dbc.InfoCreateTable();
                dbc.StatusCreateTable();

            }


            employeeLst = new ObservableCollection<Employee> { };
            employeeLstT = new ObservableCollection<Employee> { };

            customerLst = new ObservableCollection<Customer> { };
            customerLstT = new ObservableCollection<Customer> { };

            dinnerLst = new ObservableCollection<DinnerList> { };

            //people = new ObservableCollection<People>{};
            misc = new Misc { };
            div = new ObservableCollection<Division> { };
            selectedDiv = new Employee { };


            ClickCommand = new Command(arg => ClickMethod());
            ClickCommandCust = new Command(arg => ClickMethodCust());
            ClickCommand2 = new Command(arg => ClickMethod2());
            ClickCommand2Cust = new Command(arg => ClickMethod2Cust());
            ClickCommand3 = new Command(arg => ClickMethod3());

            ClickCommand4 = new Command(arg => ClickMethod4());
            ClickCommand5 = new Command(arg => ClickMethod5());
            ClickCommand6 = new Command(arg => ClickMethod6());
            ClickCommandAddPerson = new Command(arg => ClickMethodAddPerson());

            //person = new People { };
            //person.Name = "Fry";
            //person.Age = "23";
            //person.Phone = "1212121212";
            //person.Descr = "dfgdsgdf";

            //people.Add(person);

            //person = new People { };
            //person.Name = "Andrey";
            //person.Age = "28";
            //person.Phone = "8877";
            //person.Descr = "jjk";

            //people.Add(person);


            employeeLst = dbc.EmployeeRead();

            customerLst = dbc.CustomerRead();

            dinnerLst = dbc.DinnerListRead();

            //---подразделения---
            List<String> divList = new List<string>();
            Division temp;

            foreach (Employee ee in employeeLst)
            {
                employeeLstT.Add(ee);
                divList.Add(ee.Division);
            }


            var result = (from m in divList select m).Distinct().ToList();

            temp = new Division();
            temp.Value = "1. Все";
            div.Add(temp);

            foreach (var s in result)
            {
                temp = new Division();
                temp.Value = s;
                div.Add(temp);
            }

            selectedDiv.Division = "1. Все";
            //----------------------

            foreach (Customer ee in customerLst)
            {
                customerLstT.Add(ee);
            }


            VacCount();
            //employeeLst.Where(x => x.FullName == "анд");

            //misc.onCount += Filter;
            

            //employeeLst[0].FullName
        }

        //Статусы сотрудников
        List<String> tVacation = new List<string>();
        List<String> tSick = new List<string>();
        List<String> tBTrip = new List<string>();
        List<String> tOther = new List<string>();

        public void VacCount()
        {
            tSick.Clear();
            tVacation.Clear();
            tBTrip.Clear();
            tOther.Clear();
            dbc.EmployeeReadStatusCount();
            tSick = dbc.tSick;
            tVacation = dbc.tVacation;
            tBTrip = dbc.tBTrip;
            tOther = dbc.tOther;
            misc.Vacation = tVacation.Count;
            misc.BTrip = tBTrip.Count;
            misc.Sick = tSick.Count;
        }

        //Фильтр по имени
        private void ClickMethod()
        {
           employeeLst.Clear();
            foreach(Employee ee in employeeLstT)
            {
                if (ee.FullName.IndexOf(misc.TextChange, StringComparison.OrdinalIgnoreCase) != -1)
                    employeeLst.Add(ee);
            }

            misc.SelectedIndex = 0;
        }

        private void ClickMethodCust()
        {
            customerLst.Clear();
            foreach (Customer ee in customerLstT)
            {
                if (ee.FullName.IndexOf(misc.TextChangeCust, StringComparison.OrdinalIgnoreCase) != -1)
                    customerLst.Add(ee);
            }

            misc.SelectedIndex = 0;
        }

        private void ClickMethod2()
        {
            employeeLst.Clear();
            foreach (Employee ee in employeeLstT)
            {
                employeeLst.Add(ee);
            }

            misc.SelectedIndex = 0;

            misc.TextChange = "";
            selectedDiv.Division = "1. Все";

        }

        private void ClickMethod2Cust()
        {
            customerLst.Clear();
            foreach (Customer ee in customerLstT)
            {
                customerLst.Add(ee);
            }

            misc.SelectedIndexCust = 0;

            misc.TextChangeCust = "";
        }

        //Фильтр по подразделениям
        private void ClickMethod3()
        {
            employeeLst.Clear();
            if (selectedDiv.Division == "1. Все")
            {
                foreach (Employee ee in employeeLstT)
                {
                    employeeLst.Add(ee);
                }

            }
            else
            {
                foreach (Employee ee in employeeLstT)
                {
                    if (ee.Division.IndexOf(selectedDiv.Division, StringComparison.OrdinalIgnoreCase) != -1)
                        employeeLst.Add(ee);
                }
            }            

            misc.SelectedIndex = 0;
        }

        private void Filter()
        {
            employeeLst.Clear();


            
        }

        private void ClickMethod4()
        {
            misc.Page1State = "Collapsed";
            misc.Page2State = "Visible";
            misc.Page3State = "Collapsed";
        }

        private void ClickMethod5()
        {
            misc.Page2State = "Collapsed";
            misc.Page1State = "Visible";
            misc.Page3State = "Collapsed";
        }

        private void ClickMethod6()
        {
            misc.Page2State = "Collapsed";
            misc.Page1State = "Collapsed";
            misc.Page3State = "Visible";
        }

        private void ClickMethodAddPerson()
        {

        }




    }
}
