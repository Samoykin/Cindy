namespace P3.Model
{
    using System.ComponentModel;

    /// <summary>Настройки.</summary>
    public class Misc : INotifyPropertyChanged
    {
        #region Fields

        private string textChange; 
        private string textChangeCust; 
        private int selectedIndex; 
        private int selectedIndexCust;
        private string selectedDiv;
        private int vacation;
        private int btrip;
        private int sick;
        private string page1State;
        private string page2State;
        private string page3State; 
        private string pageStatistic; 
        private string dinnerSelectedPeople;
        private int employeeCount; 
        private int contactsCount;
        private string saveStatus; 
        private string updStatus; 
        private int middleAge;
        private int middleTimeRecord; 
        private string startPageVisible; 
        private string filterPageVisible; 
        private string accessDenied; 
        private string accessDeniedMess;

        #endregion

        /// <summary>Делегат.</summary>
        public delegate void MethodContainer();

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Поиск по имени сотрудника.</summary>
        public string TextChange
        {
            get
            {
                return this.textChange;
            }

            set
            {
                if (this.textChange != value)
                {
                    this.textChange = value;
                    this.OnPropertyChanged("TextChange");
                }
            }
        }

        /// <summary>Поиск по имени заказчика.</summary>
        public string TextChangeCust
        {
            get
            {
                return this.textChangeCust;
            }

            set
            {
                if (this.textChangeCust != value)
                {
                    this.textChangeCust = value;
                    this.OnPropertyChanged("TextChangeCust");
                }
            }
        }

        /// <summary>Индекс в ListView списка сотрудников.</summary>
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                if (this.selectedIndex != value)
                {
                    this.selectedIndex = value;
                    this.OnPropertyChanged("SelectedIndex");
                }
            }
        }

        /// <summary>Индекс в ListView списка заказчиков.</summary>
        public int SelectedIndexCust
        {
            get
            {
                return this.selectedIndexCust;
            }

            set
            {
                if (this.selectedIndexCust != value)
                {
                    this.selectedIndexCust = value;
                    this.OnPropertyChanged("SelectedIndexCust");
                }
            }
        }

        /// <summary>Выбранное значение в ComboBox по подразделениям.</summary>
        public string SelectedDiv
        {
            get
            {
                return this.selectedDiv;
            }

            set
            {
                if (this.selectedDiv != value)
                {
                    this.selectedDiv = value;
                    this.OnPropertyChanged("SelectedDiv");
                }
            }
        }

        /// <summary>Кол-во сотрудников в отпуске.</summary>
        public int Vacation
        {
            get
            {
                return this.vacation;
            }

            set
            {
                if (this.vacation != value)
                {
                    this.vacation = value;
                    this.OnPropertyChanged("Vacation");
                }
            }
        }

        /// <summary>Кол-во сотрудников в командировке.</summary>
        public int BTrip
        {
            get
            {
                return this.btrip;
            }

            set
            {
                if (this.btrip != value)
                {
                    this.btrip = value;
                    this.OnPropertyChanged("BTrip");
                }
            }
        }

        /// <summary>Кол-во сотрудников на больничном.</summary>
        public int Sick
        {
            get
            {
                return this.sick;
            }

            set
            {
                if (this.sick != value)
                {
                    this.sick = value;
                    this.OnPropertyChanged("Sick");
                }
            }
        }

        /// <summary>Состояние страницы 1.</summary>
        public string Page1State
        {
            get
            {
                return this.page1State;
            }

            set
            {
                if (this.page1State != value)
                {
                    this.page1State = value;
                    this.OnPropertyChanged("Page1State");
                }
            }
        }

        /// <summary>Состояние страницы 2.</summary>
        public string Page2State
        {
            get
            {
                return this.page2State;
            }

            set
            {
                if (this.page2State != value)
                {
                    this.page2State = value;
                    this.OnPropertyChanged("Page2State");
                }
            }
        }

        /// <summary>Состояние страницы 3.</summary>
        public string Page3State
        {
            get
            {
                return this.page3State;
            }

            set
            {
                if (this.page3State != value)
                {
                    this.page3State = value;
                    this.OnPropertyChanged("Page3State");
                }
            }
        }

        /// <summary>Состояние страницы статистики.</summary>
        public string PageStatistic
        {
            get
            {
                return this.pageStatistic;
            }

            set
            {
                if (this.pageStatistic != value)
                {
                    this.pageStatistic = value;
                    this.OnPropertyChanged("PageStatistic");
                }
            }
        }

        /// <summary>Выбранный сотрудник для добавления в обеды.</summary>
        public string DinnerSelectedPeople
        {
            get
            {
                return this.dinnerSelectedPeople;
            }

            set
            {
                if (this.dinnerSelectedPeople != value)
                {
                    this.dinnerSelectedPeople = value;
                    this.OnPropertyChanged("DinnerSelectedPeople");
                }
            }
        }

        /// <summary>Количество сотрудников.</summary>
        public int EmployeeCount
        {
            get
            {
                return this.employeeCount;
            }

            set
            {
                if (this.employeeCount != value)
                {
                    this.employeeCount = value;
                    this.OnPropertyChanged("EmployeeCount");
                }
            }
        }

        /// <summary>Количество контактов.</summary>
        public int ContactsCount
        {
            get
            {
                return this.contactsCount;
            }

            set
            {
                if (this.contactsCount != value)
                {
                    this.contactsCount = value;
                    this.OnPropertyChanged("ContactsCount");
                }
            }
        }

        /// <summary>Состояние сохранения.</summary>
        public string SaveStatus
        {
            get
            {
                return this.saveStatus;
            }

            set
            {
                if (this.saveStatus != value)
                {
                    this.saveStatus = value;
                    this.OnPropertyChanged("SaveStatus");
                }
            }
        }

        /// <summary>Состояние обновления данных с сайта.</summary>
        public string UpdStatus
        {
            get
            {
                return this.updStatus;
            }

            set
            {
                if (this.updStatus != value)
                {
                    this.updStatus = value;
                    this.OnPropertyChanged("UpdStatus");
                }
            }
        }

        /// <summary>Средний возраст.</summary>
        public int MiddleAge
        {
            get
            {
                return this.middleAge;
            }

            set
            {
                if (this.middleAge != value)
                {
                    this.middleAge = value;
                    this.OnPropertyChanged("MiddleAge");
                }
            }
        }

        /// <summary>Средний стаж.</summary>
        public int MiddleTimeRecord
        {
            get
            {
                return this.middleTimeRecord;
            }

            set
            {
                if (this.middleTimeRecord != value)
                {
                    this.middleTimeRecord = value;
                    this.OnPropertyChanged("MiddleTimeRecord");
                }
            }
        }

        /// <summary>Видимость стартовой страницы.</summary>
        public string StartPageVisible
        {
            get
            {
                return this.startPageVisible;
            }

            set
            {
                if (this.startPageVisible != value)
                {
                    this.startPageVisible = value;
                    this.OnPropertyChanged("StartPageVisible");
                }
            }
        }

        /// <summary>Видимость страницы с данными.</summary>
        public string FilterPageVisible
        {
            get
            {
                return this.filterPageVisible;
            }

            set
            {
                if (this.filterPageVisible != value)
                {
                    this.filterPageVisible = value;
                    this.OnPropertyChanged("FilterPageVisible");
                }
            }
        }

        /// <summary>Доступ закрыт если не в домене.</summary>
        public string AccessDenied
        {
            get
            {
                return this.accessDenied;
            }

            set
            {
                if (this.accessDenied != value)
                {
                    this.accessDenied = value;
                    this.OnPropertyChanged("AccessDenied");
                }
            }
        }

        /// <summary>Доступ закрыт если не в домене сообщение.</summary>
        public string AccessDeniedMess
        {
            get
            {
                return this.accessDeniedMess;
            }

            set
            {
                if (this.accessDeniedMess != value)
                {
                    this.accessDeniedMess = value;
                    this.OnPropertyChanged("AccessDeniedMess");
                }
            }
        }

        #endregion

        #region Implement INotyfyPropertyChanged members

        /// <summary>Изменения свойства.</summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}