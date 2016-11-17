using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Model
{
    class Customer : INotifyPropertyChanged
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
        private string _Email;
        private string _Position;
        private string _Company;
        

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

        public string Company
        {
            get { return _Company; }
            set
            {
                if (_Company != value)
                {
                    _Company = value;
                    OnPropertyChanged("Company");
                }
            }
        }

 



        #endregion
    }
}
