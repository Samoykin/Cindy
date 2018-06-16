namespace P3.Model
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using Utils;
    using ViewModel;

    /// <summary>Сотрудник.</summary>
    public sealed class Employee : INotifyPropertyChanged
    { 
        #region Fields

        private string fullName;
        private string phoneWork;
        private string phoneMobile;
        private string phoneExch;
        private string email;
        private string division;
        private string position;
        private string id;
        private string status;
        private DateTime birthDay;
        private string birthDayShort;
        private int age;
        private DateTime startDay;
        private string startDayShort;
        private string timeRecord;
        private string image;
        private ICommand sendMail;

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Полное имя.</summary>
        public string FullName
        {
            get
            {
                return this.fullName;
            }

            set
            {
                if (this.fullName != value)
                {
                    this.fullName = value;
                    this.OnPropertyChanged("FullName");
                }
            }
        }

        /// <summary>Рабочий телефон.</summary>
        public string PhoneWork
        {
            get
            {
                return this.phoneWork;
            }

            set
            {
                if (this.phoneWork != value)
                {
                    this.phoneWork = value;
                    this.OnPropertyChanged("PhoneWork");
                }
            }
        }

        /// <summary>Мобильный телефон.</summary>
        public string PhoneMobile
        {
            get
            {
                return this.phoneMobile;
            }

            set
            {
                if (this.phoneMobile != value)
                {
                    this.phoneMobile = value;
                    this.OnPropertyChanged("PhoneMobile");
                }
            }
        }

        /// <summary>Добавочный телефон.</summary>
        public string PhoneExch
        {
            get
            {
                return this.phoneExch;
            }

            set
            {
                if (this.phoneExch != value)
                {
                    this.phoneExch = value;
                    this.OnPropertyChanged("PhoneExch");
                }
            }
        }

        /// <summary>Email.</summary>
        public string Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.OnPropertyChanged("Email");
                }
            }
        }

        /// <summary>Подразделение.</summary>
        public string Division
        {
            get
            {
                return this.division;
            }

            set
            {
                if (this.division != value)
                {
                    this.division = value;
                    this.OnPropertyChanged("Division");
                }
            }
        }

        /// <summary>Должность.</summary>
        public string Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.OnPropertyChanged("Position");
                }
            }
        }

        /// <summary>ID.</summary>
        public string ID
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.OnPropertyChanged("ID");
                }
            }
        }

        /// <summary>Состояние.</summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>День рождения.</summary>
        public DateTime BirthDay
        {
            get
            {
                return this.birthDay;
            }

            set
            {
                if (this.birthDay != value)
                {
                    this.birthDay = value;
                    this.OnPropertyChanged("BirthDay");
                }
            }
        }

        /// <summary>День рождения коротко.</summary>
        public string BirthDayShort
        {
            get
            {
                return this.birthDayShort;
            }

            set
            {
                if (this.birthDayShort != value)
                {
                    this.birthDayShort = value;
                    this.OnPropertyChanged("BirthDayShort");
                }
            }
        }

        /// <summary>Дата принятия на работу.</summary>
        public DateTime StartDay
        {
            get
            {
                return this.startDay;
            }

            set
            {
                if (this.startDay != value)
                {
                    this.startDay = value;
                    this.OnPropertyChanged("StartDay");
                }
            }
        }

        /// <summary>Дата принятия на работу коротко.</summary>
        public string StartDayShort
        {
            get
            {
                return this.startDayShort;
            }

            set
            {
                if (this.startDayShort != value)
                {
                    this.startDayShort = value;
                    this.OnPropertyChanged("StartDayShort");
                }
            }
        }

        /// <summary>Возраст.</summary>
        public int Age
        {
            get
            {
                return this.age;
            }

            set
            {
                if (this.age != value)
                {
                    this.age = value;
                    this.OnPropertyChanged("Age");
                }
            }
        }

        /// <summary>Стаж.</summary>
        public string TimeRecord
        {
            get
            {
                return this.timeRecord;
            }

            set
            {
                if (this.timeRecord != value)
                {
                    this.timeRecord = value;
                    this.OnPropertyChanged("TimeRecord");
                }
            }
        }

        /// <summary>Фотография.</summary>
        public string Image
        {
            get
            {
                return this.image;
            }

            set
            {
                if (this.image != value)
                {
                    this.image = value;
                    this.OnPropertyChanged("Image");
                }
            }
        }
        
        /// <summary>Отправить письмо.</summary>
        public ICommand SendMailCmd => this.sendMail ?? (this.sendMail = new RelayCommand<object>(SendMail));

        #endregion

        private static void SendMail(object parameter)
        {
            var sm = new SendMail();
            sm.SendMailOutlook(parameter.ToString());
        }

        #region Implement INotyfyPropertyChanged members

        /// <summary>Изменения свойства.</summary>
        /// <param name="propertyName">Имя свойства.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}