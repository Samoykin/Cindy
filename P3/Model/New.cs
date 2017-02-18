using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Model
{
    class New : INotifyPropertyChanged
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

        private DateTime _date;
        private string _fullName;
        private int _yearCount;
        private string _prefix;
        private string _postfix;
        #endregion

        #region Properties
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }

        public int YearCount
        {
            get { return _yearCount; }
            set
            {
                if (_yearCount != value)
                {
                    _yearCount = value;
                    OnPropertyChanged("YearCount");
                }
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                if (_prefix != value)
                {
                    _prefix = value;
                    OnPropertyChanged("Prefix");
                }
            }
        }

        public string Postfix
        {
            get { return _postfix; }
            set
            {
                if (_postfix != value)
                {
                    _postfix = value;
                    OnPropertyChanged("Postfix");
                }
            }
        }

        #endregion
    }
}
