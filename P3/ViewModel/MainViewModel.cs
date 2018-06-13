namespace P3.ViewModel
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
    using static Model.SettingsShell;

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
        private DataUpdater upd = new DataUpdater();
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
                this.settings.Contacts = new SettingsShell.Contacts();

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

                    this.News = new ObservableCollection<NewEvent> { };
                    this.FutureNews = new ObservableCollection<NewEvent> { };

                    this.Statistic = new Statistic { };

                    this.Divisions = new ObservableCollection<Division> { };
                    this.SelectedDiv = new Employee { };

                    // Команды
                    // Меню                    
                    this.MenuMainCmd = new Command(arg => this.MenuMain());
                    this.MenuContactsCmd = new Command(arg => this.MenuContacts());
                    this.MenuStatisticCmd = new Command(arg => this.MenuStatistic());

                    // Сотрудники
                    this.SearchEmployeeCmd = new Command(arg => this.SearchEmployee());
                    this.ClearFilterEmployeeCmd = new Command(arg => this.ClearFilterEmployee());
                    this.SelectDivEmployeeCmd = new Command(arg => this.SelectDivEmployee());                    
                    this.SelectEmployeeCmd = new Command(arg => this.SelectEmployee());

                    // Контакты
                    this.SearchContactCmd = new Command(arg => this.SearchContact());
                    this.ClearFilterContactCmd = new Command(arg => this.ClearFilterContact());
                    this.DownloadContactCmd = new Command(arg => this.DownloadContact());
                    this.SaveContactCmd = new Command(arg => this.SaveContact());
                    this.AddNewContactCmd = new Command(arg => this.AddNewContact());
                    this.SortNameAscContactCmd = new Command(arg => this.SortNameAscContact());
                    this.SortNameDescContactCmd = new Command(arg => this.SortNameDescContact());

                    // Статистика
                    this.UpdateNewersStatCmd = new Command(arg => this.UpdateNewersStat());
                    
                    // Сортировка сотрудников
                    this.SortNameAscCmd = new Command(arg => this.SortNameAsc());
                    this.SortNameDescCmd = new Command(arg => this.SortNameDesc());
                    this.SortBirthAscCmd = new Command(arg => this.SortBirthAsc());
                    this.SortBirthDescCmd = new Command(arg => this.SortBirthDesc());
                    this.SortStartAscCmd = new Command(arg => this.SortStartAsc());
                    this.SortStartDescCmd = new Command(arg => this.SortStartDesc());

                    // Сервисные
                    this.DataUpdCmd = new Command(arg => this.DataUpd());

                    var tempEmpl = this.dbc.EmployeeRead();
                    this.EmployeeLst = tempEmpl;

                    // Видимость стартовой страницы
                    this.Misc.StartPageVisible = "Visible";
                    this.Misc.FilterPageVisible = "Collapsed";

                    // -----------------------------------------------------
                    // Новости
                    var newsManag = new NewsManager(this.EmployeeLst);
                    this.News = newsManag.GetNews();
                    this.FutureNews = newsManag.GetFutureNews();

                    this.Misc.EmployeeCount = this.EmployeeLst.Count();

                    var age = 0;
                    var timeRec = 0;

                    foreach (var em in this.EmployeeLst)
                    {
                        age += em.Age;
                        var timeRecT = em.TimeRecord == "менее года" ? 0 : Convert.ToInt32(em.TimeRecord);
                        timeRec += timeRecT;
                    }

                    this.Misc.MiddleAge = age / this.Misc.EmployeeCount;
                    this.Misc.MiddleTimeRecord = timeRec / this.Misc.EmployeeCount;

                    // Подразделения
                    var divList = new List<string>();

                    foreach (var ee in this.EmployeeLst)
                    {
                        this.EmployeeLstT.Add(ee);
                        divList.Add(ee.Division);
                    }

                    divList.Sort();

                    var result = (from m in divList select m).Distinct().ToList();

                    var division = new Division
                    {
                        Value = "1. Все"
                    };

                    this.Divisions.Add(division);

                    foreach (var s in result)
                    {
                        division = new Division
                        {
                            Value = s
                        };

                        this.Divisions.Add(division);
                    }

                    this.SelectedDiv.Division = "1. Все";

                    this.emplStat = new EmplStatistic(this.EmployeeLst, this.Statistic);
                    this.Statistic = this.emplStat.CalcCount();

                    var employeeNewLstT = this.emplStat.Newers(this.Statistic.StartNewers, DateTime.Now);

                    this.EmployeeNewLst.Clear();

                    foreach (var ee in employeeNewLstT)
                    {
                        this.EmployeeNewLst.Add(ee);
                    }

                    this.SortNameAsc();
                    this.UpdateContacts();
                    this.Misc.ContactsCount = this.CustomerLst.Count();
                    this.SortNameAscContact();

                    this.MenuMain();
                    this.Misc = this.dbc.EmployeeReadStatusCount(this.Misc);
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

        /// <summary>Новости.</summary>
        public ObservableCollection<NewEvent> News { get; set; }

        /// <summary>Анонсы событий.</summary>
        public ObservableCollection<NewEvent> FutureNews { get; set; }

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

        /// <summary>Главная.</summary>
        public ICommand MenuMainCmd { get; set; }

        /// <summary>Контакты.</summary> // Меню
        public ICommand MenuContactsCmd { get; set; }

        /// <summary>Статистика.</summary>
        public ICommand MenuStatisticCmd { get; set; }

        /// <summary>Искать сотрудника.</summary>
        public ICommand SearchEmployeeCmd { get; set; }

        /// <summary>Сбросить фильтр сотрудников.</summary>
        public ICommand ClearFilterEmployeeCmd { get; set; }

        /// <summary>Выбрать подразделение.</summary>
        public ICommand SelectDivEmployeeCmd { get; set; }

        /// <summary>Выбрать сотрудника.</summary>
        public ICommand SelectEmployeeCmd { get; set; }

        /// <summary>Искать контакт.</summary>
        public ICommand SearchContactCmd { get; set; }

        /// <summary>Сбросить фильтр контактов.</summary>
        public ICommand ClearFilterContactCmd { get; set; }

        /// <summary>Загрузить контакты.</summary>
        public ICommand DownloadContactCmd { get; set; }

        /// <summary>Сохранить контакты в файл.</summary>
        public ICommand SaveContactCmd { get; set; }

        /// <summary>Добавить новый контакт.</summary>
        public ICommand AddNewContactCmd { get; set; }

        /// <summary>Сортировка по имени по возрастанию.</summary>
        public ICommand SortNameAscContactCmd { get; set; }

        /// <summary>Сортировка по имени по убыванию.</summary>
        public ICommand SortNameDescContactCmd { get; set; }

        /// <summary>Добавить контакт.</summary>
        public ICommand ClickCommandAddPerson { get; set; }

        /// <summary>Обновить новичков.</summary>
        public ICommand UpdateNewersStatCmd { get; set; }

        /// <summary>Сортировка по имени по возрастанию.</summary>
        public ICommand SortNameAscCmd { get; set; }

        /// <summary>Сортировка по имени по убыванию.</summary>
        public ICommand SortNameDescCmd { get; set; }

        /// <summary>Сортировка по дню рождения по возрастанию.</summary>
        public ICommand SortBirthAscCmd { get; set; }

        /// <summary>Сортировка по дню рождения по убыванию.</summary>
        public ICommand SortBirthDescCmd { get; set; }

        /// <summary>Сортировка по дате прихода по возрастанию.</summary>
        public ICommand SortStartAscCmd { get; set; }

        /// <summary>Сортировка по дате прихода по убыванию.</summary>
        public ICommand SortStartDescCmd { get; set; }

        /// <summary>Обновить данные с сайта.</summary>
        public ICommand DataUpdCmd { get; set; }

        /// <summary>Отправить E-mail.</summary>
        public ICommand SendMail
        {
            get
            {
                if (this.sendMail == null)
                {
                    this.sendMail = new RelayCommand<object>(this.SendMailExecute);
                }

                return this.sendMail;
            }
        }

        /// <summary>Сохранить контакт.</summary>
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

        #endregion

        // Сохранение контакта
        private void SaveData_Execute(object parameter)
        {
            var name = parameter.ToString();
            var chageContact = new Customer();

            foreach (var c in this.CustomerLst)
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

            // Сохранение в Excel
            var flag = this.updCont.SaveData2(parameter.ToString(), chageContact);

            // Сохранение в БД
            this.dbc.CustomerUpdatePerson(chageContact);
            this.dbc.DatabaseCopy();

            this.Misc.SaveStatus = flag ? "Данные контакта обновлены" : "Данные не обновлены";

            this.timerForUpd.Interval = 2000;
            this.timerForUpd.Tick += this.TimerTick;
            this.timerForUpd.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            this.Misc.SaveStatus = string.Empty;
            this.timerForUpd.Stop();
        }

        private void SendMailExecute(object parameter)
        {
            var sm = new SendMail();
            sm.SendMailOutlook(parameter.ToString());
        }

        // Поиск сотрудника
        private void SearchEmployee()
        {
           this.EmployeeLst.Clear();
            foreach (var ee in this.EmployeeLstT)
            {
                if (ee.FullName.IndexOf(this.Misc.TextChange, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.EmployeeLst.Add(ee);
                }
            }

            this.Misc.SelectedIndex = 0;
        }
        
        // -----------------------------------------------------------
        // Сотрудники
        // Сброс фильтра сотрудников
        private void ClearFilterEmployee()
        {
            this.EmployeeLst.Clear();
            foreach (var ee in this.EmployeeLstT)
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

        // Выбор подразделения
        private void SelectDivEmployee()
        {
            this.EmployeeLst.Clear();
            if (this.SelectedDiv.Division == "1. Все")
            {
                foreach (var ee in this.EmployeeLstT)
                {
                    this.EmployeeLst.Add(ee);
                }
            }
            else
            {
                foreach (var ee in this.EmployeeLstT)
                {
                    if (ee.Division.IndexOf(this.SelectedDiv.Division, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        this.EmployeeLst.Add(ee);
                    }
                }
            }

            this.Misc.SelectedIndex = 0;
        }

        // Выбор сотрудника
        private void SelectEmployee()
        {
            this.Misc.StartPageVisible = "Collapsed";
            this.Misc.FilterPageVisible = "Visible";
        }

        // Сброс фильтра
        private void ClearFilterContact()
        {
            this.CustomerLst.Clear();
            foreach (var ee in this.CustomerLstT)
            {
                this.CustomerLst.Add(ee);
            }    

            this.Misc.TextChangeCust = string.Empty;

            this.SortNameAscContact();
        }

        // -----------------------------------------------------------
        // Контакты
        // Обновление списка контактов
        private void UpdateContacts()
        {
            this.CustomerLstT.Clear();
            this.CustomerLst.Clear();
            this.CustomerLstT = this.dbc.CustomerRead();

            foreach (var ee in this.CustomerLstT)
            {
                this.CustomerLst.Add(ee);
            }

            this.Misc.TextChangeCust = string.Empty;
        }

        // Поиск контакта
        private void SearchContact()
        {
            this.CustomerLst.Clear();
            foreach (var ee in this.CustomerLstT)
            {
                if (ee.FullName.IndexOf(this.Misc.TextChangeCust, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.CustomerLst.Add(ee);
                }
            }

            this.Misc.SelectedIndexCust = 0;
        }

        // Загрузка контактов из файла
        private void DownloadContact()
        {
            var customPath = this.updCont.LoadCustom();

            this.Preparer(customPath);

            this.settings.Contacts.FilePath = customPath;
            this.settingsXml.WriteXml(this.settings);            
        }
        
        // Сохранение контактов в файл
        private void SaveContact()
        {            
            this.updCont.SaveData(this.CustomerLst);
        }

        // Добавление нового контакта
        private void AddNewContact()
        {
            this.CustomerLst.Add(this.NewContact);            
            this.updCont.SaveData2(this.NewContact.FullName, this.NewContact); // Сохранение в Excel            
            this.dbc.CustomerWritePerson(this.NewContact); // Сохранение в БД
            this.dbc.DatabaseCopy();
            this.SortNameAscContact();
        }

        // Сортировка по имени по возрастанию
        private void SortNameAscContact()
        {
            var customerLstTemp = this.CustomerLst;
            customerLstTemp = new ObservableCollection<Customer>(customerLstTemp.OrderBy(a => a.FullName));
            this.CustomerLst.Clear();
            foreach (var ee in customerLstTemp)
            {
                this.CustomerLst.Add(ee);
            }

            this.Misc.SelectedIndexCust = 0;
        }

        // Сортировка по имени по убыванию
        private void SortNameDescContact()
        {
            var customerLstTemp = this.CustomerLst;
            customerLstTemp = new ObservableCollection<Customer>(customerLstTemp.OrderByDescending(a => a.FullName));
            this.CustomerLst.Clear();
            foreach (var ee in customerLstTemp)
            {
                this.CustomerLst.Add(ee);
            }

            this.Misc.SelectedIndexCust = 0;
        }

        // -----------------------------------------------------------
        // Обновить список новеньких
        private void UpdateNewersStat()
        {
            var employeeNewLstT = this.emplStat.Newers(this.Statistic.StartNewers, DateTime.Now);

            this.EmployeeNewLst.Clear();

            foreach (var ee in employeeNewLstT)
            {
                this.EmployeeNewLst.Add(ee);
            }

            this.Statistic.NewersCount = this.EmployeeNewLst.Count();
        }

        // Главная страница
        private void MenuMain()
        {
            this.Misc.Page2State = "Collapsed";
            this.Misc.Page1State = "Visible";
            this.Misc.Page3State = "Collapsed";
            this.Misc.PageStatistic = "Collapsed";

            // видимость стартовой страницы
            this.Misc.StartPageVisible = "Visible";
            this.Misc.FilterPageVisible = "Collapsed";
        }

        // Страница контактов
        private void MenuContacts()
        {
            this.Misc.Page1State = "Collapsed";
            this.Misc.Page2State = "Visible";
            this.Misc.Page3State = "Collapsed";
            this.Misc.PageStatistic = "Collapsed";

            // Контакты
            if (this.contactUpd)
            {
                var customPath = this.updCont.GetPath();
                if (customPath == string.Empty)
                {
                    this.Preparer(customPath);
                }

                this.contactUpd = false;
            }
        }

        // Страница статистики
        private void MenuStatistic()
        {
            this.Misc.Page2State = "Collapsed";
            this.Misc.Page1State = "Collapsed";
            this.Misc.Page3State = "Collapsed";
            this.Misc.PageStatistic = "Visible";
        }

        // Сортировка
        private void SortNameAsc()
        {
            var sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderBy(a => a.FullName));
            this.EmployeeLst.Clear();
            foreach (var ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void SortNameDesc()
        {
            var sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderByDescending(a => a.FullName));
            this.EmployeeLst.Clear();
            foreach (var ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }            

            this.Misc.SelectedIndex = 0;
        }

        private void SortBirthAsc()
        {
            var sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderBy(a => a.BirthDayShort));
            this.EmployeeLst.Clear();
            foreach (var ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void SortBirthDesc()
        {
            var sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderByDescending(a => a.BirthDayShort));
            this.EmployeeLst.Clear();
            foreach (var ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void SortStartAsc()
        {
            var sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderBy(a => a.StartDayShort));
            this.EmployeeLst.Clear();
            foreach (var ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private void SortStartDesc()
        {
            var sortedEmployees = this.EmployeeLst;
            sortedEmployees = new ObservableCollection<Employee>(sortedEmployees.OrderByDescending(a => a.StartDayShort));
            this.EmployeeLst.Clear();
            foreach (var ee in sortedEmployees)
            {
                this.EmployeeLst.Add(ee);
            }
            
            this.Misc.SelectedIndex = 0;
        }

        private async void Preparer(string customPath)
        {
            await Task<bool>.Factory.StartNew(() => this.updCont.Update(customPath));

            this.UpdateContacts();
            this.SortNameAscContact();
        }

        // Обновление данных с сайта
        private void DataUpd()
        {
            this.Misc.UpdStatus = "Обновление данных";            
            this.upd.ParseHTML(this.logger);

            this.timer1.Interval = new TimeSpan(0, 0, 0, 5);
            this.timer1.Tick += this.Timer_Tick1;
            this.timer1.Start();  
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            var flag = this.upd.Flag;
            if (flag)
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

            this.Misc = this.dbc.EmployeeReadStatusCount(this.Misc);

            this.Misc.SelectedIndex = 0;
            this.timer1.Stop();
        }

        private RootElement SetDefaultValue(RootElement set)
        {
            set.SoftUpdate.RemoteSettingsPath = @"d:\Temp\RemoteProp.xml";
            set.Contacts.FilePath = @"C:\Контакты.xlsx";            

            return set;
        }
    }
}