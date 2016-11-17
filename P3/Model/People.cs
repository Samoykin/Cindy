using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Model
{
    class People : INotifyPropertyChanged
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

        private string _Name;
        private string _Phone;
        private string _Age;
        private string _Descr;

        #endregion

        #region Properties


        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Phone
        {
            get { return _Phone; }
            set
            {
                if (_Phone != value)
                {
                    _Phone = value;
                    OnPropertyChanged("Phone");
                }
            }
        }

        public string Age
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

        public string Descr
        {
            get { return _Descr; }
            set
            {
                if (_Descr != value)
                {
                    _Descr = value;
                    OnPropertyChanged("Descr");
                }
            }
        }


        #endregion
    }
}
