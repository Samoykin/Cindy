using P3.Contacts;
using P3.DataUpd;
using P3.Model;
using P3.Updater;
using P3.Utils;
using P3.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace P3.ViewModel
{
    class MainViewModel 
    {

        DBconnect dbc = new DBconnect();
        DBDinnerConnect dbDinner = new DBDinnerConnect();
        UpdateContacts updCont = new UpdateContacts();
        String path = @"DBTels.sqlite";
        String pathTemp = "DBTelsTemp.sqlite";

        LogFile logFile = new LogFile();
        LogInfo logInfo = new LogInfo();

        System.Windows.Forms.Timer timerForUpd = new System.Windows.Forms.Timer();
        DispatcherTimer t1 = new DispatcherTimer();
        IDData upd = new IDData();

        #region Settings
        String _localPropPath = "LocalProp.xml";
        #endregion

        #region Properties
        //public People person { get; set; }
        public Misc misc { get; set; }

        public ObservableCollection<New> futureNews2 { get; set; } 
        public ObservableCollection<Division> news { get; set; } //новости (не обращаем внимания на Division
        public ObservableCollection<Division> futureNews { get; set; } //предстоящие события
        public Statistic statistic { get; set; }
        public Customer newContact { get; set; }
        public ObservableCollection<Division> div { get; set; }
        //public ObservableCollection<People> people { get; set; }

        public ObservableCollection<Employee> employeeLst { get; set; }
        public ObservableCollection<Employee> employeeLstT { get; set; }
        public ObservableCollection<Employee> employeeNewLst { get; set; }

        public ObservableCollection<Customer> customerLst { get; set; }
        public ObservableCollection<Customer> customerLstT { get; set; }

        public ObservableCollection<DinnerList> dinnerLst { get; set; }
        public Employee selectedDiv { get; set; }

        public ObservableCollection<Division> listDinner { get; set; }

        EmplStatistic emplStat;
        #endregion

        #region Commands
        public ICommand FutureNewsCommand { get; set; }
        public ICommand DataUpdCommand { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommandCust { get; set; }
        public ICommand ClickCommand2 { get; set; }        
        public ICommand ClickCommand3 { get; set; }
        public ICommand ClickCommandAddPerson { get; set; }

        public ICommand UpdateNewersCommand { get; set; }

        public ICommand ListSelChangCommand { get; set; }

        //Контакты
        public ICommand ClickCommand2Cust { get; set; } //сброс фильтра
        public ICommand UpdCustCommand { get; set; }
        public ICommand SaveCustCommand { get; set; }
        public ICommand SaveNewCust { get; set; }
        public ICommand SortNameContAscCommand { get; set; } //сортировка по имени
        public ICommand SortNameContDescCommand { get; set; } //сортировка по имени

        //Меню
        public ICommand ClickCommand4 { get; set; }
        public ICommand ClickCommand5 { get; set; }
        public ICommand ClickCommand6 { get; set; }
        public ICommand MenuStatistic { get; set; }

        //Сортировка сотрудников
        public ICommand SortNameAscCommand { get; set; }
        public ICommand SortNameDescCommand { get; set; }
        public ICommand SortBirthAscCommand { get; set; }
        public ICommand SortBirthDescCommand { get; set; }
        public ICommand SortStartAscCommand { get; set; }
        public ICommand SortStartDescCommand { get; set; }


        //Отправка E-mail
        private ICommand _sendMail;
        public ICommand SendMail
        {
            get
            {
                if (_sendMail == null)
                { _sendMail = new RelayCommand<object>(this.SendMail_Execute); }
                return _sendMail;
            }
        }


        //private ICommand _myCommand;
        //public ICommand MyCommand
        //{
        //    get
        //    {
        //        if (_myCommand == null)
        //        { _myCommand = new RelayCommand<object>(this.MyCommand_Execute); }
        //        return _myCommand;
        //    }
        //}

        //Сохранение контакта
        private ICommand _saveData;
        public ICommand SaveData
        {
            get
            {
                if (_saveData == null)
                { _saveData = new RelayCommand<object>(this.SaveData_Execute); }
                return _saveData;
            }
        }

        private void SaveData_Execute(object parameter)
        {
            String name = parameter.ToString();
            Customer chageContact = new Customer();
            Boolean flag = false;

            foreach(Customer c in customerLst)
            {
                if (c.FullName == name)
                {
                    chageContact.FullName = c.FullName;
                    chageContact.Position = c.Position;
                    chageContact.PhoneMobile = c.PhoneMobile;
                    chageContact.PhoneWork = c.PhoneWork;
                    chageContact.Email = c.Email;
                    chageContact.Company = c.Company;

                }
            }
            //сохранение в Excel
            flag = updCont.saveData2(parameter.ToString(), chageContact);
            //сохранение в БД
            dbc.CustomerUpdatePerson(chageContact);

            if (flag == true) 
            {
                misc.SaveStatus = "Данные контакта обновлены";
            }
            else
            {
                misc.SaveStatus = "Данные не обновлены";
            }
            

            timerForUpd.Interval = 2000;
            timerForUpd.Tick += new EventHandler(timer_Tick);
            timerForUpd.Start();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            misc.SaveStatus = "";
            timerForUpd.Stop();
        }


        #endregion

        private void SendMail_Execute(object parameter)
        {
            SendMail sm = new SendMail();
            sm.SendMailOutlook(parameter.ToString());
        }

        

        //private void MyCommand_Execute(object parameter)
        //{
        //    String aa = parameter.ToString();
        //    //MessageDialog msg = new MessageDialog(String.Format("Parameter: {0}", parameter));
        //    //await aa.ShowAsync();
        //}





        public MainViewModel()
        {
            

            logInfo.saveInfo(); //сохранение информации о запустившем программу

            String logText = DateTime.Now.ToString() + "|event| |Запуск приложения Phonebook 3 Cindy";
            logFile.WriteLog(logText);

            XMLcode xml = new XMLcode(_localPropPath); ;
            if (!File.Exists(_localPropPath))
            {
                xml.CreateXml();
                xml.CreateNodesXml();
                
            }

            //(Environment.UserDomainName) == "ELCOM"
            if (1==1)
            {
                

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
                File.Copy(path, pathTemp, true);

                employeeLst = new ObservableCollection<Employee> { };
                employeeLstT = new ObservableCollection<Employee> { };
                employeeNewLst = new ObservableCollection<Employee> { };

                customerLst = new ObservableCollection<Customer> { };
                customerLstT = new ObservableCollection<Customer> { };

                dinnerLst = new ObservableCollection<DinnerList> { };

                //people = new ObservableCollection<People>{};
                misc = new Misc { };
                newContact = new Customer { };

                news = new ObservableCollection<Division> { };  //новости
                futureNews = new ObservableCollection<Division> { };  //предстоящие события
                futureNews2 = new ObservableCollection<New> { };  //предстоящие события

                statistic = new Statistic { };

                div = new ObservableCollection<Division> { };
                listDinner = new ObservableCollection<Division> { };
                selectedDiv = new Employee { };

                FutureNewsCommand = new Command(arg => FutureNewsMethod());
                DataUpdCommand = new Command(arg => DataUpd()); //обновление данных с сайта

                ClickCommand = new Command(arg => ClickMethod());
                ClickCommandCust = new Command(arg => ClickMethodCust());
                ClickCommand2 = new Command(arg => ClickMethod2());

                ClickCommand3 = new Command(arg => ClickMethod3());

                UpdateNewersCommand = new Command(arg => ClickMethodUpdateNewers());

                ListSelChangCommand = new Command(arg => ClickMethodListSelChangCommand());

                ClickCommandAddPerson = new Command(arg => ClickMethodAddPerson());

                //Контакты
                ClickCommand2Cust = new Command(arg => ClickMethod2Cust());
                UpdCustCommand = new Command(arg => ClickMethodUpdCust());
                SaveCustCommand = new Command(arg => ClickMethodSaveCust());
                SaveNewCust = new Command(arg => ClickMethodSaveNewCust());
                SortNameContAscCommand = new Command(arg => ClickMethodSortNameContAsc());
                SortNameContDescCommand = new Command(arg => ClickMethodSortNameContDesc());

                //Меню
                ClickCommand4 = new Command(arg => ClickMethod4());
                ClickCommand5 = new Command(arg => ClickMethod5());
                ClickCommand6 = new Command(arg => ClickMethod6());
                MenuStatistic = new Command(arg => ClickMethodMenuStatistic());

                //Сортировка
                SortNameAscCommand = new Command(arg => ClickMethodSortNameAsc());
                SortNameDescCommand = new Command(arg => ClickMethodSortNameDesc());
                SortBirthAscCommand = new Command(arg => ClickMethodSortBirthAsc());
                SortBirthDescCommand = new Command(arg => ClickMethodSortBirthDesc());
                SortStartAscCommand = new Command(arg => ClickMethodSortStartAsc());
                SortStartDescCommand = new Command(arg => ClickMethodSortStartDesc());

                //SortClear();
                //emplSort.SortNameAsc = true;

                var tempEmpl = dbc.EmployeeRead();
                employeeLst = tempEmpl;

                //видимость стартовой страницы
                misc.StartPageVisible = "Visible";
                misc.FilterPageVisible = "Collapsed";

                //-----------------------------------------------------
                //Новости
                NewsData nData = new NewsData();

                //employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderBy(a => a.BirthDayShort));

                foreach (New n in nData.GetNews(employeeLst).OrderBy(a=>a.Date))
                {
                    futureNews2.Add(n);
                }




                foreach (Employee e in employeeLst)
                {
                    if (e.BirthDay.Month == DateTime.Now.Month && e.BirthDay.Day == DateTime.Now.Day)
                    {
                        Division newstemp = new Division();
                        newstemp.Value = "";
                        if (Convert.ToInt32(e.Age) % 5 == 0)
                        {
                            newstemp.Value = e.FullName.ToString() + " празднует Юбилей: " + e.Age.ToString() + " лет";
                        }
                        else
                        {
                            newstemp.Value = e.FullName.ToString() + " празднует " + e.Age.ToString() + "-й День рождения";
                        }

                        news.Add(newstemp);
                    }

                    if (e.StartDay.Month == DateTime.Now.Month && e.StartDay.Day == DateTime.Now.Day)
                    {
                        Division newstemp = new Division();
                        newstemp.Value = "";
                        if (e.TimeRecord != "менее года")
                        {
                            if (Convert.ToInt32(e.TimeRecord) % 5 == 0)
                            {
                                //юбилей
                                newstemp.Value = e.FullName.ToString() + " празднует Юбилей: " + e.TimeRecord + " лет работы в компании";
                            }
                            else
                            {
                                newstemp.Value = e.FullName.ToString() + " празднует " + e.TimeRecord + "-й год работы в компании";
                            }
                        }
                        else if (e.StartDay.Year == DateTime.Now.Year)
                        {
                            newstemp.Value = "Принят новый сотрудник " + e.FullName.ToString();
                        }


                        news.Add(newstemp);
                    }

                    //----------------------------------------------------------
                    //Предстоящие события
                    DateTime dt1 = DateTime.Now;
                    DateTime dt2 = e.BirthDay;
                    Int32 diff = dt1.Year - dt2.Year;

                    //var interval = new TimeSpan(5);
                    dt2 = e.BirthDay.AddYears(diff);

                    //день рождения
                    if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                    {
                        Division newstemp = new Division();

                        if (Convert.ToInt32(e.Age + 1) % 5 == 0)
                        {
                            newstemp.Value = e.FullName.ToString() + " " + e.BirthDay.ToString("dd.MM") + " празднует Юбилей: " + (e.Age + 1).ToString() + " лет";
                        }
                        else
                        {
                            newstemp.Value = e.FullName.ToString() + " " + e.BirthDay.ToString("dd.MM") + " празднует " + (e.Age + 1).ToString() + "-й День рождения";
                        }

                        futureNews.Add(newstemp);


                    }
                    //годовщина работы
                    DateTime dt3 = DateTime.Now;
                    DateTime dt4 = e.StartDay;
                    Int32 diff2 = dt3.Year - dt4.Year;

                    //var interval2 = new TimeSpan(5);
                    dt2 = e.StartDay.AddYears(diff2);

                    if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                    {
                        Division newstemp = new Division();

                        if (e.TimeRecord == "менее года")
                        {
                            newstemp.Value = e.FullName.ToString() + " " + e.StartDay.ToString("dd.MM") + " празднует 1-й год работы в компании";
                        }
                        else if (Convert.ToInt32(e.TimeRecord)+1 % 5 == 0)
                        {
                            //юбилей
                            newstemp.Value = e.FullName.ToString() + " " + e.StartDay.ToString("dd.MM") + " празднует Юбилей: " + (Convert.ToInt32(e.TimeRecord) + 1) + " лет работы в компании";
                        }
                        else
                        {
                            newstemp.Value = e.FullName.ToString() + " " + e.StartDay.ToString("dd.MM") + " празднует " + (Convert.ToInt32(e.TimeRecord) + 1) + "-й год работы в компании";
                        }
                        futureNews.Add(newstemp);
                    }



                    //----------------------------------------------------------




                    //if ((e.StartDay.Month > DateTime.Now.Month && e.StartDay.Day > DateTime.Now.Day) && (e.StartDay.Month < DateTime.Now.Month && e.StartDay.Day < DateTime.Now.Day + 7)
                    //{
                    //    Division newstemp = new Division();
                    //    newstemp.Value = e.FullName.ToString() + " " + e.StartDayShort + " годовщина работы в компании";
                    //    futureNews.Add(newstemp);
                    //}

                }




                //-----------------------------------------------------
                //Обеды

                //Division temp22 = new Division();
                //DinnerType dtype = new DinnerType();
                //temp22.Value = "Не обедает";
                //listDinner.Add(temp22);
                //temp22.Value = "Половинка";
                //listDinner.Add(temp22);
                //temp22.Value = "Полная";
                //listDinner.Add(temp22);

                //dinnerLst = dbDinner.DinnerListRead();

                //-----------------------------------------------------

                misc.EmployeeCount = employeeLst.Count() - 10; //количество сотрудников кроме Корпорат номера сотр Логистика, Офис в Иркутсе, Офис в Красноярске, Офис в Москве, Офис в США, Офис в ТВЗ, Офис в Томске, Охранник, Столовая

                int age = 0;
                int timeRec = 0;

                foreach (Employee em in employeeLst)
                {
                    age += em.Age;

                    int timeRecT = 0;
                    if (em.TimeRecord == "менее года")
                        timeRecT = 0;
                    else
                        timeRecT = Convert.ToInt32(em.TimeRecord);                    

                    timeRec += timeRecT;
                }
                misc.MiddleAge = age / misc.EmployeeCount;
                misc.MiddleTimeRecord = timeRec / misc.EmployeeCount;

                //---подразделения---
                List<String> divList = new List<string>();
                Division temp;

                foreach (Employee ee in employeeLst)
                {
                    employeeLstT.Add(ee);
                    divList.Add(ee.Division);
                }

                divList.Sort();

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
                emplStat = new EmplStatistic(employeeLst, statistic);
                statistic = emplStat.CalcCount();

                DateTime dtTemp = DateTime.Now;
                var employeeNewLstT = emplStat.Newers(statistic.StartNewers, dtTemp);

                employeeNewLst.Clear();

                foreach (Employee ee in employeeNewLstT)
                {
                    employeeNewLst.Add(ee);
                }

                //DateTime dtTemp = DateTime.Now;
                ////новички за месяц 
                //Newers(new DateTime(dtTemp.Year, dtTemp.Month, 1));
                //statistic.NewersCountMonth = employeeNewLst.Count();

                ////новички за квартал            
                //Newers(DateTime.Now.AddDays(-92));
                //statistic.NewersCountQuarter = employeeNewLst.Count();

                ////новички за год            
                //Newers(new DateTime(dtTemp.Year, 1, 1));
                //statistic.NewersCountYear = employeeNewLst.Count();

                ////Новички
                //statistic.StartNewers = new DateTime(2016, 09, 01);

                //Newers(statistic.StartNewers);
                //statistic.NewersCount = employeeNewLst.Count();

                UpdateContacts();
                misc.ContactsCount = customerLst.Count();
                ClickMethodSortNameContAsc();

                ClickMethod5();

                VacCount();
                //employeeLst.Where(x => x.FullName == "анд");

                //misc.onCount += Filter;


                //employeeLst[0].FullName


                DataUpd();

            }

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


        private void Newers(DateTime startDay)
        {
            //DateTime dateValue;
            employeeNewLst.Clear();            

            foreach (Employee ee in employeeLst)
            {

                //DateTime.TryParse(ee.StartDay, out dateValue);
                if (ee.StartDay >= startDay)
                {
                    employeeNewLst.Add(ee);
                }

            }

            //employeeNewLst.Sort(delegate(Employee em1, Employee em2)
            //{ return em1.StartDay.CompareTo(em1.StartDay); });

            
        }

        //-----------------------------------------------------------
        //Контакты
        //Обновление списка контактов
        public void UpdateContacts()
        {
            customerLstT.Clear();
            customerLst.Clear();
            customerLstT = dbc.CustomerRead();

            foreach (Customer ee in customerLstT)
            {
                customerLst.Add(ee);
            }

            misc.TextChangeCust = "";
        }

        //поисковая строка
        private void ClickMethodCust()
        {
            customerLst.Clear();
            foreach (Customer ee in customerLstT)
            {
                if (ee.FullName.IndexOf(misc.TextChangeCust, StringComparison.OrdinalIgnoreCase) != -1)
                    customerLst.Add(ee);
            }

            misc.SelectedIndexCust = 0;
        }

        //Сброс фильтра
        private void ClickMethod2Cust()
        {
            customerLst.Clear();
            foreach (Customer ee in customerLstT)
            {
                customerLst.Add(ee);
            }    

            misc.TextChangeCust = "";

            ClickMethodSortNameContAsc();
        }

        //загрузка данных о контактах
        private void ClickMethodUpdCust()
        {
            
            String customPath = "";
            customPath = updCont.loadCustom();

            Preparer(customPath);


            String _localPropPath = "Settings.xml";

            XMLcodeContacts xmlCode = new XMLcodeContacts(_localPropPath);
            xmlCode.WriteXml(customPath);
            
                       
        }

        public async void Preparer(String customPath)
        {
            Boolean flag;
            flag = await Task<Boolean>.Factory.StartNew(() =>
            {
                return updCont.Update(customPath);
            });

            UpdateContacts();
            ClickMethodSortNameContAsc(); 
        }

        //Сохранить данные контактов в файл
        private void ClickMethodSaveCust()
        {            
            updCont.saveData(customerLst);
        }

        //Добавить новый контакт
        private void ClickMethodSaveNewCust()
        {
            customerLst.Add(newContact);
            //сохранение в Excel
            updCont.saveData2(newContact.FullName, newContact);
            //сохранение в БД
            dbc.CustomerWritePerson(newContact);
            ClickMethodSortNameContAsc();
            //newContact.Clear();
        }

        //Сортировка по имени по возрастанию
        private void ClickMethodSortNameContAsc()
        {
            ObservableCollection<Customer> customerLstTemp;

            customerLstTemp = customerLst;
            customerLstTemp = new ObservableCollection<Customer>(customerLstTemp.OrderBy(a => a.FullName));
            customerLst.Clear();
            foreach (Customer ee in customerLstTemp)
            {
                customerLst.Add(ee);
            }

            misc.SelectedIndexCust = 0;
        }

        //Сортировка по имени по убыванию
        private void ClickMethodSortNameContDesc()
        {
            ObservableCollection<Customer> customerLstTemp;

            customerLstTemp = customerLst;
            customerLstTemp = new ObservableCollection<Customer>(customerLstTemp.OrderByDescending(a => a.FullName));
            customerLst.Clear();
            foreach (Customer ee in customerLstTemp)
            {
                customerLst.Add(ee);
            }

            misc.SelectedIndexCust = 0;
        }


        //-----------------------------------------------------------
        //Фильтр по подразделениям

        //Сброс фильтра
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


            //видимость стартовой страницы
            misc.StartPageVisible = "Visible";
            misc.FilterPageVisible = "Collapsed";

        }
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

        private void ClickMethodUpdateNewers()
        {
            DateTime dtTemp = DateTime.Now;

            var employeeNewLstT = emplStat.Newers(statistic.StartNewers, dtTemp);

            employeeNewLst.Clear();

            foreach (Employee ee in employeeNewLstT)
            {
                employeeNewLst.Add(ee);
            }
            //Newers(statistic.StartNewers);
            statistic.NewersCount = employeeNewLst.Count();
        }

        private void ClickMethodListSelChangCommand()
        {
            misc.StartPageVisible = "Collapsed";
            misc.FilterPageVisible = "Visible";
        }

        Boolean contactUpd = true;

        private void ClickMethod4()
        {
            misc.Page1State = "Collapsed";
            misc.Page2State = "Visible";
            misc.Page3State = "Collapsed";
            misc.PageStatistic = "Collapsed";

            //Контакты
            if (contactUpd == true)
            {
                String customPath = updCont.GetPath();
                if (customPath == "")
                {
                    Preparer(customPath);
                }
                contactUpd = false;
            }
            
        }

        private void ClickMethod5()
        {
            misc.Page2State = "Collapsed";
            misc.Page1State = "Visible";
            misc.Page3State = "Collapsed";
            misc.PageStatistic = "Collapsed";
        }

        private void ClickMethod6()
        {
            misc.Page2State = "Collapsed";
            misc.Page1State = "Collapsed";
            misc.Page3State = "Visible";
            misc.PageStatistic = "Collapsed";
        }

        private void ClickMethodMenuStatistic()
        {
            misc.Page2State = "Collapsed";
            misc.Page1State = "Collapsed";
            misc.Page3State = "Collapsed";
            misc.PageStatistic = "Visible";
        }

        //-----------------------------------------------------------
        //Обеды
        private void ClickMethodAddPerson()
        {

        }

        

        //Сортировка
        private void ClickMethodSortNameAsc()
        {

            ObservableCollection<Employee> employeeLstTemp;
            
            employeeLstTemp = employeeLst;
            employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderBy(a => a.FullName));
            employeeLst.Clear();
            foreach (Employee ee in employeeLstTemp)
            {
                employeeLst.Add(ee);
            }
            
            misc.SelectedIndex = 0;
        }

        private void ClickMethodSortNameDesc()
        {

            ObservableCollection<Employee> employeeLstTemp;

            employeeLstTemp = employeeLst;
            employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderByDescending(a => a.FullName));
            employeeLst.Clear();
            foreach (Employee ee in employeeLstTemp)
            {
                employeeLst.Add(ee);
            }

            

            misc.SelectedIndex = 0;
        }

        private void ClickMethodSortBirthAsc()
        {

            ObservableCollection<Employee> employeeLstTemp;

            employeeLstTemp = employeeLst;
            employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderBy(a => a.BirthDayShort));
            employeeLst.Clear();
            foreach (Employee ee in employeeLstTemp)
            {
                employeeLst.Add(ee);
            }
            
            misc.SelectedIndex = 0;
        }

        private void ClickMethodSortBirthDesc()
        {

            ObservableCollection<Employee> employeeLstTemp;

            employeeLstTemp = employeeLst;
            employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderByDescending(a => a.BirthDayShort));
            employeeLst.Clear();
            foreach (Employee ee in employeeLstTemp)
            {
                employeeLst.Add(ee);
            }
            
            misc.SelectedIndex = 0;
        }

        private void ClickMethodSortStartAsc()
        {

            ObservableCollection<Employee> employeeLstTemp;

            employeeLstTemp = employeeLst;
            employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderBy(a => a.StartDayShort));
            employeeLst.Clear();
            foreach (Employee ee in employeeLstTemp)
            {
                employeeLst.Add(ee);
            }
            
            misc.SelectedIndex = 0;
        }

        private void ClickMethodSortStartDesc()
        {

            ObservableCollection<Employee> employeeLstTemp;

            employeeLstTemp = employeeLst;
            employeeLstTemp = new ObservableCollection<Employee>(employeeLstTemp.OrderByDescending(a => a.StartDayShort));
            employeeLst.Clear();
            foreach (Employee ee in employeeLstTemp)
            {
                employeeLst.Add(ee);
            }
            
            misc.SelectedIndex = 0;
        }

        //Окно с предстоящими событиями
        private void FutureNewsMethod()
        {
            ViewShower.Show(0, futureNews, false, b => { });
            
        }

        //обновление данных с сайта
        private void DataUpd()
        {
            Boolean flag = false;
            misc.UpdStatus = "Обновление данных";

            

            //flag = await Task<Boolean>.Factory.StartNew(() =>
            //{
            //    return upd.ParseHTML();
            //});

            upd.ParseHTML();
            //if (flag == true) misc.UpdStatus = "";

            t1.Interval = new TimeSpan(0, 0, 0, 5);
            t1.Tick += new EventHandler(timer_Tick1);
            t1.Start();

            
            
        }

        void timer_Tick1(object sender, EventArgs e)
        {

            Boolean flag = upd.flag;
            if (flag == true)
            {
                DataUpdCalc();
            }

        }

        private void DataUpdCalc()
        {
            misc.UpdStatus = "";

            var tempEmpl = dbc.EmployeeRead();

            employeeLst.Clear();

            foreach (Employee ee in tempEmpl)
            {
                employeeLst.Add(ee);
            }
            misc.EmployeeCount = employeeLst.Count() - 10;

            VacCount(); //статусы количество

            misc.SelectedIndex = 0;

            t1.Stop();
        }
    }
}
