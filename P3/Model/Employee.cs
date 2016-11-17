using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string _BirthDay;
        private string _StartDay;
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

        public string BirthDay
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

        public string StartDay
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




        #endregion
    }
}
