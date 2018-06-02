namespace P3.Model
{
    using System.ComponentModel;
    using System.Windows.Input;

    using Utils;
    using ViewModel;

    /// <summary>Заказчик.</summary>
    public class Customer : INotifyPropertyChanged
    {
        #region Fields

        private string fullName;
        private string phoneWork;
        private string phoneMobile;
        private string email;
        private string position;
        private string company;
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

        /// <summary>Компания.</summary>
        public string Company
        {
            get
            {
                return this.company;
            }

            set
            {
                if (this.company != value)
                {
                    this.company = value;
                    this.OnPropertyChanged("Company");
                }
            }
        }

        #endregion

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

        private void SendMail_Execute(object parameter)
        {
            var sm = new SendMail();
            sm.SendMailOutlook(parameter.ToString());
        }
    }
}