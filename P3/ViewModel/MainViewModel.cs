﻿namespace P3.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Contacts;
    using DataUpd;
    using Model;
    using NLog;
    using Updater;
    using Utils;
    using static Model.Shell;

    /// <summary>Главная ViewModel.</summary>
    public class MainViewModel 
    {
        private const string DatabasePath = "DBTels.sqlite";
        private const string DatabasePathTemp = "DBTelsTemp.sqlite";
        private const string SettingsPath = "Settings.xml";

        private DBconnect dbc = new DBconnect();
        private UpdateContacts updCont; 
        private Logger logger = LogManager.GetCurrentClassLogger();
        private LogInfo logInfo = new LogInfo();
        private System.Windows.Forms.Timer timerForUpd = new System.Windows.Forms.Timer();
        private DispatcherTimer timer1 = new DispatcherTimer();
        private IDData upd = new IDData();
        private SettingsXml<RootElement> settingsXml;

        // Конфигурация        
        private RootElement settings = new RootElement();

        private bool contactUpd = true;
        private EmplStatistic emplStat;
        private ICommand sendMail;
        private ICommand saveData;

        /// <summary>Initializes a new instance of the <see cref="MainViewModel" /> class.</summary>
        public MainViewModel()
        {
            this.logInfo.SaveInfo(); // Сохранение информации о запустившем программу
            this.logger.Info("Запуск приложения Phonebook 3 Cindy");
            
            try
            {
                // Вычитывание параметров из XML
                // Инициализация модели настроек
                this.settingsXml = new SettingsXml<RootElement>(SettingsPath);
                this.settings.SoftUpdate = new SoftUpdate();
                this.settings.Contacts = new Contacts();

                if (!File.Exists(SettingsPath))
                {
                    this.settings = this.SetDefaultValue(this.settings); // Значения по умолчанию
                    this.settingsXml.WriteXml(this.settings);
                }
                else
                {
                    this.settings = this.settingsXml.ReadXml(this.settings);
                }

                this.updCont = new UpdateContacts(this.settings);
                this.Misc = new Misc { };

                // (Environment.UserDomainName) == "ELCOM"      
                if (true) 
                {                 
                    var upd = new SoftUpdater(this.settings);
                    upd.UpdateSoft();                

                    if (!File.Exists(DatabasePath))
                    {
                        this.dbc.CreateBase();
                        this.dbc.EmployeeCreateTable();
                        this.dbc.CustomerCreateTable();
                        this.dbc.InfoCreateTable();
                        this.dbc.StatusCreateTable();
                    }

                    File.Copy(DatabasePath, DatabasePathTemp, true);

                    this.EmployeeLst = new ObservableCollection<Employee> { };
                    this.EmployeeLstT = new ObservableCollection<Employee> { };
                    this.EmployeeNewLst = new ObservableCollection<Employee> { };

                    this.CustomerLst = new ObservableCollection<Customer> { };
                    this.CustomerLstT = new ObservableCollection<Customer> { };

                    this.NewContact = new Customer { };

                    this.News = new ObservableCollection<Division> { };  // новости
                    this.FutureNews = new ObservableCollection<Division> { };  // предстоящие события
                    this.News2 = new ObservableCollection<NewEvent> { };
                    this.FutureNews2 = new ObservableCollection<NewEvent> { };  // предстоящие события

                    this.Statistic = new Statistic { };

                    this.Divisions = new ObservableCollection<Division> { };
                    this.SelectedDiv = new Employee { };

                    this.DataUpdCommand = new Command(arg => this.DataUpd()); // обновление данных с сайта
                    this.ClickCommand = new Command(arg => this.ClickMethod());
                    this.ClickCommandCust = new Command(arg => this.ClickMethodCust());
                    this.ClickCommand2 = new Command(arg => this.ClickMethod2());
                    this.ClickCommand3 = new Command(arg => this.ClickMethod3());
                    this.UpdateNewersCommand = new Command(arg => this.ClickMethodUpdateNewers());
                    this.ListSelChangCommand = new Command(arg => this.ClickMethodListSelChangCommand());

                    // Контакты
                    this.ClickCommand2Cust = new Command(arg => this.ClickMethod2Cust());
                    this.UpdCustCommand = new Command(arg => this.ClickMethodUpdCust());
                    this.SaveCustCommand = new Command(arg => this.ClickMethodSaveCust());
                    this.SaveNewCust = new Command(arg => this.ClickMethodSaveNewCust());
                    this.SortNameContAscCommand = new Command(arg => this.ClickMethodSortNameContAsc());
                    this.SortNameContDescCommand = new Command(arg => this.ClickMethodSortNameContDesc());

                    // Меню
                    this.ClickCommand4 = new Command(arg => this.ClickMethod4());
                    this.ClickCommand5 = new Command(arg => this.ClickMethod5());
                    this.MenuStatistic = new Command(arg => this.ClickMethodMenuStatistic());

                    // Сортировка
                    this.SortNameAscCommand = new Command(arg => this.ClickMethodSortNameAsc());
                    this.SortNameDescCommand = new Command(arg => this.ClickMethodSortNameDesc());
                    this.SortBirthAscCommand = new Command(arg => this.ClickMethodSortBirthAsc());
                    this.SortBirthDescCommand = new Command(arg => this.ClickMethodSortBirthDesc());
                    this.SortStartAscCommand = new Command(arg => this.ClickMethodSortStartAsc());
                    this.SortStartDescCommand = new Command(arg => this.ClickMethodSortStartDesc());

                    var tempEmpl = this.dbc.EmployeeRead();
                    this.EmployeeLst = tempEmpl;

                    // Видимость стартовой страницы
                    this.Misc.StartPageVisible = "Visible";
                    this.Misc.FilterPageVisible = "Collapsed";

                    // -----------------------------------------------------
                    // Новости
                    var newsData = new NewsData();

                    var newsList = new NewsList();
                    var newsT = new List<NewEvent>();
                    var futureNewsT = new List<NewEvent>();
                    newsList = newsData.GetNews(this.EmployeeLst);

                    newsT = newsList.News;
                    futureNewsT = newsList.FutureNews;

                    if (newsT != null && newsT.Count != 0)
                    {
                        foreach (var n in newsT.OrderBy(a => a.Date))
                        {
                            this.News2.Add(n);
                        }
                    }
                    else
                    {
                        var newT = new NewEvent();
                        newT.Prefix = "На данный момент количество новостей ";
                        newT.Postfix = " штук";
                        this.News2.Add(newT);
                    }

                    if (futureNewsT != null)
                    {
                        foreach (NewEvent n in futureNewsT.OrderBy(a => a.Date))
                        {
                            this.FutureNews2.Add(n);
                        }
                    }

                    this.Misc.EmployeeCount = this.EmployeeLst.Count() - 10; // Количество сотрудников кроме Корпорат номера сотр Логистика, Офис в Иркутсе, Офис в Красноярске, Офис в Москве, Офис в США, Офис в ТВЗ, Офис в Томске, Охранник, Столовая

                    int age = 0;
                    int timeRec = 0;

                    foreach (Employee em in this.EmployeeLst)
                    {
                        age += em.Age;

                        int timeRecT = 0;
                        if (em.TimeRecord == "менее года")
                        {
                            timeRecT = 0;
                        }
                        else
                        {
                            timeRecT = Convert.ToInt32(em.TimeRecord);
                        }

                        timeRec += timeRecT;
                    }

                    this.Misc.MiddleAge = age / this.Misc.EmployeeCount;
                    this.Misc.MiddleTimeRecord = timeRec / this.Misc.EmployeeCount;

                    // ---подразделения---
                    var divList = new List<string>();
                    Division temp;

                    foreach (Employee ee in this.EmployeeLst)
                    {
                        this.EmployeeLstT.Add(ee);
                        divList.Add(ee.Division);
                    }

                    divList.Sort();

                    var result = (from m in divList select m).Distinct().ToList();

                    temp = new Division();
                    temp.Value = "1. Все";
                    this.Divisions.Add(temp);

                    foreach (var s in result)
                    {
                        temp = new Division();
                        temp.Value = s;
                        this.Divisions.Add(temp);
                    }

                    this.SelectedDiv.Division = "1. Все";

                    // ----------------------
                    this.emplStat = new EmplStatistic(this.EmployeeLst, this.Statistic);
                    this.Statistic = this.emplStat.CalcCount();

                    var employeeNewLstT = this.emplStat.Newers(this.Statistic.StartNewers, DateTime.Now);

                    this.EmployeeNewLst.Clear();

                    foreach (var ee in employeeNewLstT)
                    {
                        this.EmployeeNewLst.Add(ee);
                    }

                    this.UpdateContacts();
                    this.Misc.ContactsCount = this.CustomerLst.Count();
                    this.ClickMethodSortNameContAsc();

                    this.ClickMethod5();
                    this.VacCount();
                    this.DataUpd();
                }
                else
                {
                    this.Misc.AccessDenied = "Collapsed";
                    this.Misc.AccessDeniedMess = "Visible";
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }

        #region Properties

        /// <summary>Параметры.</summary>
        public Misc Misc { get; set; }

        /// <summary>Новости 2.</summary>
        public ObservableCollection<NewEvent> News2 { get; set; }

        /// <summary>Анонсы событий 2.</summary>
        public ObservableCollection<NewEvent> FutureNews2 { get; set; }

        /// <summary>Новости.</summary>
        public ObservableCollection<Division> News { get; set; }

        /// <summary>Анонсы событий.</summary>
        public ObservableCollection<Division> FutureNews { get; set; } 

        /// <summary>Статистика.</summary>
        public Statistic Statistic { get; set; }

        /// <summary>Новый контакт.</summary>
        public Customer NewContact { get; set; }

        /// <summary>Подразделения.</summary>
        public ObservableCollection<Division> Divisions { get; set; }

        /// <summary>Сотрудники.</summary>
        public ObservableCollection<Employee> EmployeeLst { get; set; }

        /// <summary>Сотрудники темп.</summary>
        public ObservableCollection<Employee> EmployeeLstT { get; set; }

        /// <summary>Сотрудники темп2.</summary>
        public ObservableCollection<Employee> EmployeeNewLst { get; set; }

        /// <summary>Заказчики.</summary>
        public ObservableCollection<Customer> CustomerLst { get; set; }

        /// <summary>Заказчики темп.</summary>
        public ObservableCollection<Customer> CustomerLstT { get; set; }

        /// <summary>Выбранное подразделение.</summary>
        public Employee SelectedDiv { get; set; }
        
        #endregion

        #region Commands

        /// <summary>Обновить данные.</summary>
        public ICommand DataUpdCommand { get; set; }

        /// <summary>О программе.</summary>
        public ICommand ClickCommand { get; set; }

        /// <summary>Заказчики.</summary>
        public ICommand ClickCommandCust { get; set; }

        /// <summary>Тестовая2.</summary>
        public ICommand ClickCommand2 { get; set; }

        /// <summary>Тестовая3.</summary>
        public ICommand ClickCommand3 { get; set; }

        /// <summary>Добавить контакт.</summary>
        public ICommand ClickCommandAddPerson { get; set; }

        /// <summary>Обновить новости.</summary>
        public ICommand UpdateNewersCommand { get; set; }

        /// <summary>Тестовая4.</summary>
        public ICommand ListSelChangCommand { get; set; }

        /// <summary>Сбросить фильтр.</summary> // Контакты
        public ICommand ClickCommand2Cust { get; set; }

        /// <summary>Обновить контакты.</summary>
        public ICommand UpdCustCommand { get; set; }

        /// <summary>Сохранить контакт.</summary>
        public ICommand SaveCustCommand { get; set; }

        /// <summary>Сохранить нового заказчика.</summary>
        public ICommand SaveNewCust { get; set; }

        /// <summary>Сортировка по имени по возрастанию.</summary>
        public ICommand SortNameContAscCommand { get; set; }

        /// <summary>Сортировка по имени по убыванию.</summary>
        public ICommand SortNameContDescCommand { get; set; }

        /// <summary>Контакты.</summary> // Меню
        public ICommand ClickCommand4 { get; set; }

        /// <summary>Главная.</summary>
        public ICommand ClickCommand5 { get; set; }

        /// <summary>Статистика.</summary>
        public ICommand MenuStatistic { get; set; }
        
        /// <summary>Сортировка по имени по возрастанию.</summary> // Сортировка сотрудников
        public ICommand SortNameAscCommand { get; set; }

        /// <summary>Сортировка по имени по убыванию.</summary>
        public ICommand SortNameDescCommand { get; set; }

        /// <summary>Сортировка по дню рождения по возрастанию.</summary>
        public ICommand SortBirthAscCommand { get; set; }

        /// <summary>Сортировка по дню рождения по убыванию.</summary>
        public ICommand SortBirthDescCommand { get; set; }

        /// <summary>Сортировка по дате прихода по возрастанию.</summary>
        public ICommand SortStartAscCommand { get; set; }

        /// <summary>Сортировка по дате прихода по убыванию.</summary>
        public ICommand SortStartDescCommand { get; set; }

        /// <summary>Отправить E-mail.</summary>
        public ICommand SendMail
        {
            get
            {
                if (this.sendMail == null)
                {
                    this.sendMail = new RelayCommand<object>(this.SendMail_Execute);
                }

                return this.sendMail;
            }
        }

        /// <summary>Сохранить.</summary>
        public ICommand SaveData
        {
            get
            {
                if (this.saveData == null)
                {
                    this.saveData = new RelayCommand<object>(this.SaveData_Execute);
                }

                return this.saveData;
            }
        }

        private void SaveData_Execute(object parameter)
        {
            string name = parameter.ToString();
            Customer chageContact = new Customer();
            bool flag = false;

            foreach (Customer c in this.CustomerLst)
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

            // сохранение в Excel
            flag = this.updCont.SaveData2(parameter.ToString(), chageContact);

            // сохранение в БД
            this.dbc.CustomerUpdatePerson(chageContact);

            if (flag == true)
            {
                this.Misc.SaveStatus = "Данные контакта обновлены";
            }
            else
            {
                this.Misc.SaveStatus = "Данные не обновлены";
            }

            this.timerForUpd.Interval = 2000;
            this.timerForUpd.Tick += new EventHandler(this.Timer_Tick);
            this.timerForUpd.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Misc.SaveStatus = string.Empty;
            this.timerForUpd.Stop();
        }

        #endregion

        private void SendMail_Execute(object parameter)
        {
            var sm = new SendMail();
            sm.SendMailOutlook(parameter.ToString());
        }

        private void VacCount()
        {
            this.Misc = this.dbc.EmployeeReadStatusCount(this.Misc);
        }

        // Фильтр по имени
        private void ClickMethod()
        {
           this.EmployeeLst.Clear();
            foreach (Employee ee in this.EmployeeLstT)
            {
                if (ee.FullName.IndexOf(this.Misc.TextChange, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.EmployeeLst.Add(ee);
                }
            }

            this.Misc.SelectedIndex = 0;
        }
        
        private void Newers(DateTime startDay)
        {
            this.EmployeeNewLst.Clear();            

            foreach (Employee ee in this.EmployeeLst)
            {
                if (ee.StartDay >= startDay)
                {
                    this.EmployeeNewLst.Add(ee);
                }
            }
        }

        // -----------------------------------------------------------
        // Контакты
        // Обновление списка контактов
        private void UpdateContacts()
        {
            this.CustomerLstT.Clear();
            this.CustomerLst.Clear();
            this.CustomerLstT = this.dbc.CustomerRead();

            foreach (Customer ee in this.CustomerLstT)
            {
                this.CustomerLst.Add(ee);
            }

            this.Misc.TextChangeCust = string.Empty;
        }

        // поисковая строка
        private void ClickMethodCust()
        {
            this.CustomerLst.Clear();
            foreach (Customer ee in this.CustomerLstT)
            {
                if (ee.FullName.IndexOf(this.Misc.TextChangeCust, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.CustomerLst.Add(ee);
                }
            }

            this.Misc.SelectedIndexCust = 0;
        }

        // Сброс фильтра
        private void ClickMethod2Cust()
        {
            this.CustomerLst.Clear();
            foreach (var ee in this.CustomerLstT)
            {
                this.CustomerLst.Add(ee);
            }    

            this.Misc.TextChangeCust = string.Empty;

            this.ClickMethodSortNameContAsc();
        }

        // Загрузка данных о контактах
        private void ClickMethodUpdCust()
        {
            var customPath = string.Empty;
            customPath = this.updCont.LoadCustom();

            this.Preparer(customPath);

            this.settings.Contacts.FilePath = customPath;
            this.settingsXml.WriteXml(this.settings);            
        }
        
        private async void Preparer(string customPath)
        {
            bool flag;
            flag = await Task<bool>.Factory.StartNew(() =>
            {
                return this.updCont.Update(customPath);
            });

            this.UpdateContacts();
            this.ClickMethodSortNameContAsc(); 
        }

        // Сохранить данные контактов в файл
        private void ClickMethodSaveCust()
        {            
            this.updCont.SaveData(this.CustomerLst);
        }

        // Добавить новый контакт
        private void ClickMethodSaveNewCust()
        {
            this.CustomerLst.Add(this.NewContact);            
            this.updCont.SaveData2(this.NewContact.FullName, this.NewContact); // сохранение в Excel            
            this.dbc.CustomerWritePerson(this.NewContact); // сохранение в БД
            this.ClickMethodSortNameContAsc();
        }

        // Сортировка по имени по возрастанию
        private void ClickMethodSortNameContAsc()
        {
            ObservableCollection<Customer> customerLstTemp;

            customerLstTemp = this.CustomerLst;
            customerLstTemp = new ObservableCollection<Customer>(customerLstTemp.OrderBy(a => a.FullName));
            this.CustomerLst.Clear();
            foreach (Customer ee in customerLstTemp)
            {
                this.CustomerLst.Add(ee);
            }

            this.Misc.SelectedIndexCust = 0;
        }

        // Сортировка по имени по убыванию
        private void ClickMethodSortNameContDesc()
        {
            ObservableCollection<Customer> customerLstTemp;

            customerLstTemp = this.CustomerLst;
            customerLstTemp = new ObservableCollection<Customer>(customerLstTemp.OrderByDescending(a => a.FullName));
            this.CustomerLst.Clear();
            foreach (Customer ee in customerLstTemp)
            {
                this.CustomerLst.Add(ee);
            }

            this.Misc.SelectedIndexCust = 0;
        }

        // -----------------------------------------------------------
        // Фильтр по подразделениям
        // Сброс фильтра
        private void ClickMethod2()
        {
            this.EmployeeLst.Clear();
            foreach (Employee ee in this.EmployeeLstT)
            {
                this.EmployeeLst.Add(ee);
            }

            this.Misc.SelectedIndex = 0;

            this.Misc.TextChange = string.Empty;
            this.SelectedDiv.Division = "1. Все";

            // видимость стартовой страницы
            this.Misc.StartPageVisible = "Visible";
            this.Misc.FilterPageVisible = "Collapsed";
        }

        private void ClickMethod3()
        {
            this.EmployeeLst.Clear();
            if (this.SelectedDiv.Division == "1. Все")
            {
                foreach (Employee ee in this.EmployeeLstT)
                {
                    this.EmployeeLst.Add(ee);
                }
            }
            else
            {
                foreach (Employee ee in this.EmployeeLstT)
                {
                    if (ee.Division.IndexOf(this.SelectedDiv.Division, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        this.EmployeeLst.Add(ee);
                    }
                }
            }            

            this.Misc.SelectedIndex = 0;
        }        

        private void Filter()
        {
            this.EmployeeLst.Clear();            
        }

        private void ClickMethodUpdateNewers()
        {
            var employeeNewLstT = this.emplStat.Newers(this.Statistic.StartNewers, DateTime.Now);

            this.EmployeeNewLst.Clear();

            foreach (var ee in employeeNewLstT)
            {
                this.EmployeeNewLst.Add(ee);
            }

            this.Statistic.NewersCount = this.EmployeeNewLst.Count();
        }

        private void ClickMethodListSelChangCommand()
        {
            this.Misc.StartPageVisible = "Collapsed";
            this.Misc.FilterPageVisible = "Visible";
        }               

        private void ClickMethod4()
        {
            this.Misc.Page1State = "Collapsed";
            this.Misc.Page2State = "Visible";
            this.Misc.Page3State = "Collapsed";
            this.Misc.PageStatistic = "Collapsed";

            // Контакты
            if (this.contactUpd == true)
            {
                string customPath = this.updCont.GetPath();
                if (customPath == string.Empty)
                {
                    this.Preparer(customPath);
                }

                this.contactUpd = false;
            }
        }

        private void ClickMethod5()
        {
            this.Misc.Page2State = "Collapsed";
            this.Misc.Page1State = "Visible";
            this.Misc.Page3State = "Collapsed";
            this.Misc.PageStatistic = "Collapsed";

            // видимость стартовой страницы
            this.Misc.StartPageVisible = "Visible";
            this.Misc.FilterPageVisible = "Collapsed";
        }

        private void ClickMethod6()
        {
            this.Misc.Page2State = "Collapsed";
            this.Misc.Page1State = "Collapsed";
            this.Misc.Page3State = "Visible";
            this.Misc.PageStatistic = "Collapsed";
        }

        private void ClickMethodMenuStatistic()
        {
            this.Misc.Page2State = "Collapsed";
            this.Misc.Page1State = "Collapsed";
            this.Misc.Page3State = "Collapsed";
            this.Misc.PageStatistic = "Visible";
        }

        // Сортировка
        private void ClickMethodSortNameAsc()
        {
            ObservableCollection<Employee> sortedEmployees;

            sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderBy(a => a.FullName));
            this.EmployeeLst.Clear();
            foreach (Employee ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void ClickMethodSortNameDesc()
        {
            ObservableCollection<Employee> sortedEmployees;

            sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderByDescending(a => a.FullName));
            this.EmployeeLst.Clear();
            foreach (Employee ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }            

            this.Misc.SelectedIndex = 0;
        }

        private void ClickMethodSortBirthAsc()
        {
            ObservableCollection<Employee> sortedEmployees;

            sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderBy(a => a.BirthDayShort));
            this.EmployeeLst.Clear();
            foreach (Employee ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void ClickMethodSortBirthDesc()
        {
            ObservableCollection<Employee> sortedEmployees;

            sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderByDescending(a => a.BirthDayShort));
            this.EmployeeLst.Clear();
            foreach (Employee ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void ClickMethodSortStartAsc()
        {
            ObservableCollection<Employee> sortedEmployees;

            sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderBy(a => a.StartDayShort));
            this.EmployeeLst.Clear();
            foreach (Employee ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void ClickMethodSortStartDesc()
        {
            ObservableCollection<Employee> sortedEmployees;

            sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderByDescending(a => a.StartDayShort));
            this.EmployeeLst.Clear();
            foreach (Employee ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        // обновление данных с сайта
        private void DataUpd()
        {
            this.Misc.UpdStatus = "Обновление данных";            
            this.upd.ParseHTML();

            this.timer1.Interval = new TimeSpan(0, 0, 0, 5);
            this.timer1.Tick += new EventHandler(this.Timer_Tick1);
            this.timer1.Start();  
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            var flag = this.upd.Flag;
            if (flag == true)
            {
                this.DataUpdCalc();
            }
        }

        private void DataUpdCalc()
        {
            this.Misc.UpdStatus = string.Empty;

            var tempEmpl = this.dbc.EmployeeRead();

            this.EmployeeLst.Clear();

            foreach (var ee in tempEmpl)
            {
                this.EmployeeLst.Add(ee);
            }

            this.Misc.EmployeeCount = this.EmployeeLst.Count() - 10;

            this.VacCount(); // статусы количество

            this.Misc.SelectedIndex = 0;
            this.timer1.Stop();
        }

        private RootElement SetDefaultValue(RootElement set)
        {
            set.SoftUpdate.UpdPath = @"d:\Temp\RemoteProp.xml";
            set.Contacts.FilePath = @"\\elcom.local\files\01-Deps\ДПАСУТП\05_Контакты\Контакты заказчиков.xlsx";            

            return set;
        }
    }
}