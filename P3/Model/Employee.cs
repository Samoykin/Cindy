using P3.Utils;
using P3.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P3.Model
{
    class Employee : INotifyPropertyChanged
    {
        #region Implement INotyfyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Fields

        private string _FullName;
        private string _PhoneWork;
        private string _PhoneMobile;
        private string _PhoneExch;
        private string _Email;
        private string _Division;
        private string _Position;
        private string _ID;
        private string _Status;
        private DateTime _BirthDay;
        private string _BirthDayShort;
        private Int32 _Age; //возраст
        private DateTime _StartDay;
        private string _StartDayShort;
        private String _TimeRecord; //стаж работы
        private string _Image;

        #endregion

        #region Properties


        public string FullName
        {
            get { return _FullName; }
            set
            {
                if (_FullName != value)
                {
                    _FullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }

        public string PhoneWork
        {
            get { return _PhoneWork; }
            set
            {
                if (_PhoneWork != value)
                {
                    _PhoneWork = value;
                    OnPropertyChanged("PhoneWork");
                }
            }
        }

        public string PhoneMobile
        {
            get { return _PhoneMobile; }
            set
            {
                if (_PhoneMobile != value)
                {
                    _PhoneMobile = value;
                    OnPropertyChanged("PhoneMobile");
                }
            }
        }

        public string PhoneExch
        {
            get { return _PhoneExch; }
            set
            {
                if (_PhoneExch != value)
                {
                    _PhoneExch = value;
                    OnPropertyChanged("PhoneExch");
                }
            }
        }

        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Division
        {
            get { return _Division; }
            set
            {
                if (_Division != value)
                {
                    _Division = value;
                    OnPropertyChanged("Division");
                }
            }
        }



        public string Position
        {
            get { return _Position; }
            set
            {
                if (_Position != value)
                {
                    _Position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        public string ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public DateTime BirthDay
        {
            get { return _BirthDay; }
            set
            {
                if (_BirthDay != value)
                {
                    _BirthDay = value;
                    OnPropertyChanged("BirthDay");
                }
            }
        }

        public string BirthDayShort
        {
            get { return _BirthDayShort; }
            set
            {
                if (_BirthDayShort != value)
                {
                    _BirthDayShort = value;
                    OnPropertyChanged("BirthDayShort");
                }
            }
        }

        public DateTime StartDay
        {
            get { return _StartDay; }
            set
            {
                if (_StartDay != value)
                {
                    _StartDay = value;
                    OnPropertyChanged("StartDay");
                }
            }
        }

        public string StartDayShort
        {
            get { return _StartDayShort; }
            set
            {
                if (_StartDayShort != value)
                {
                    _StartDayShort = value;
                    OnPropertyChanged("StartDayShort");
                }
            }
        }

        public Int32 Age
        {
            get { return _Age; }
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        public String TimeRecord
        {
            get { return _TimeRecord; }
            set
            {
                if (_TimeRecord != value)
                {
                    _TimeRecord = value;
                    OnPropertyChanged("TimeRecord");
                }
            }
        }

        public string Image
        {
            get { return _Image; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    OnPropertyChanged("Image");
                }
            }
        }


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

        private void SendMail_Execute(object parameter)
        {
            SendMail sm = new SendMail();
            sm.SendMailOutlook(parameter.ToString());
        }
        #endregion
    }
}
