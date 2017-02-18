using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Model
{
    class Statistic : INotifyPropertyChanged
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


        private DateTime _StartNewers; //Дата выборки новиньких
        private Int32 _NewersCount; //Количество новичков
        private Int32 _NewersCountMonth; //Количество новичков за месяц
        private Int32 _NewersCountQuarter1; //Количество новичков за 1 квартал
        private Int32 _NewersCountQuarter2; //Количество новичков за 2 квартал
        private Int32 _NewersCountQuarter3; //Количество новичков за 3 квартал
        private Int32 _NewersCountQuarter4; //Количество новичков за 4 квартал
        private Int32 _NewersCountYear; //Количество новичков за год

        public DateTime StartNewers
        {
            get { return _StartNewers; }
            set
            {
                if (_StartNewers != value)
                {
                    _StartNewers = value;
                    OnPropertyChanged("StartNewers");
                    //onCount();
                }
            }
        }

        public Int32 NewersCount
        {
            get { return _NewersCount; }
            set
            {
                if (_NewersCount != value)
                {
                    _NewersCount = value;
                    OnPropertyChanged("NewersCount");
                }
            }
        }

        public Int32 NewersCountMonth
        {
            get { return _NewersCountMonth; }
            set
            {
                if (_NewersCountMonth != value)
                {
                    _NewersCountMonth = value;
                    OnPropertyChanged("NewersCountMonth");
                }
            }
        }

        public Int32 NewersCountQuarter1
        {
            get { return _NewersCountQuarter1; }
            set
            {
                if (_NewersCountQuarter1 != value)
                {
                    _NewersCountQuarter1 = value;
                    OnPropertyChanged("NewersCountQuarter1");
                }
            }
        }

        public Int32 NewersCountQuarter2
        {
            get { return _NewersCountQuarter2; }
            set
            {
                if (_NewersCountQuarter2 != value)
                {
                    _NewersCountQuarter2 = value;
                    OnPropertyChanged("NewersCountQuarter2");
                }
            }
        }

        public Int32 NewersCountQuarter3
        {
            get { return _NewersCountQuarter3; }
            set
            {
                if (_NewersCountQuarter3 != value)
                {
                    _NewersCountQuarter3 = value;
                    OnPropertyChanged("NewersCountQuarter3");
                }
            }
        }

        public Int32 NewersCountQuarter4
        {
            get { return _NewersCountQuarter4; }
            set
            {
                if (_NewersCountQuarter4 != value)
                {
                    _NewersCountQuarter4 = value;
                    OnPropertyChanged("NewersCountQuarter4");
                }
            }
        }

        public Int32 NewersCountYear
        {
            get { return _NewersCountYear; }
            set
            {
                if (_NewersCountYear != value)
                {
                    _NewersCountYear = value;
                    OnPropertyChanged("NewersCountYear");
                }
            }
        }
    }
}
