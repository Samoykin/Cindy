using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Model
{
    class DinnerList : INotifyPropertyChanged
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
        private string _ID;
        private string _Day1;
        private string _Day2;
        private string _Day3;
        //private string _Day4;
        #endregion

        #region Properties
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
        public string Day1
        {
            get { return _Day1; }
            set
            {
                if (_Day1 != value)
                {
                    _Day1 = value;
                    OnPropertyChanged("Day1");
                }
            }
        }

        public string Day2
        {
            get { return _Day2; }
            set
            {
                if (_Day2 != value)
                {
                    _Day2 = value;
                    OnPropertyChanged("Day2");
                }
            }
        }

        public string Day3
        {
            get { return _Day3; }
            set
            {
                if (_Day3 != value)
                {
                    _Day3 = value;
                    OnPropertyChanged("Day3");
                }
            }
        }

        #endregion
    }
}
