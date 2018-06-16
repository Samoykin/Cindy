namespace P3.Model
{
    using System;
    using System.ComponentModel;

    /// <summary>Статистика.</summary>
    public sealed class Statistic : INotifyPropertyChanged
    {
        #region Fields

        private DateTime startNewers;
        private int newersCount;
        private int newersCountMonth;
        private int newersCountQuarter1;
        private int newersCountQuarter2;
        private int newersCountQuarter3;
        private int newersCountQuarter4;
        private int newersCountYear;

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Дата выборки новиньких.</summary>
        public DateTime StartNewers
        {
            get
            {
                return this.startNewers;
            }

            set
            {
                if (this.startNewers != value)
                {
                    this.startNewers = value;
                    this.OnPropertyChanged("StartNewers");
                }
            }
        }

        /// <summary>Количество новичков.</summary>
        public int NewersCount
        {
            get
            {
                return this.newersCount;
            }

            set
            {
                if (this.newersCount != value)
                {
                    this.newersCount = value;
                    this.OnPropertyChanged("NewersCount");
                }
            }
        }

        /// <summary>Количество новичков за месяц.</summary>
        public int NewersCountMonth
        {
            get
            {
                return this.newersCountMonth;
            }

            set
            {
                if (this.newersCountMonth != value)
                {
                    this.newersCountMonth = value;
                    this.OnPropertyChanged("NewersCountMonth");
                }
            }
        }

        /// <summary>Количество новичков за 1 квартал.</summary>
        public int NewersCountQuarter1
        {
            get
            {
                return this.newersCountQuarter1;
            }

            set
            {
                if (this.newersCountQuarter1 != value)
                {
                    this.newersCountQuarter1 = value;
                    this.OnPropertyChanged("NewersCountQuarter1");
                }
            }
        }

        /// <summary>Количество новичков за 2 квартал.</summary>
        public int NewersCountQuarter2
        {
            get
            {
                return this.newersCountQuarter2;
            }

            set
            {
                if (this.newersCountQuarter2 != value)
                {
                    this.newersCountQuarter2 = value;
                    this.OnPropertyChanged("NewersCountQuarter2");
                }
            }
        }

        /// <summary>Количество новичков за 3 квартал.</summary>
        public int NewersCountQuarter3
        {
            get
            {
                return this.newersCountQuarter3;
            }

            set
            {
                if (this.newersCountQuarter3 != value)
                {
                    this.newersCountQuarter3 = value;
                    this.OnPropertyChanged("NewersCountQuarter3");
                }
            }
        }

        /// <summary>Количество новичков за 4 квартал.</summary>
        public int NewersCountQuarter4
        {
            get
            {
                return this.newersCountQuarter4;
            }

            set
            {
                if (this.newersCountQuarter4 != value)
                {
                    this.newersCountQuarter4 = value;
                    this.OnPropertyChanged("NewersCountQuarter4");
                }
            }
        }

        /// <summary>Количество новичков за год.</summary>
        public int NewersCountYear
        {
            get
            {
                return this.newersCountYear;
            }

            set
            {
                if (this.newersCountYear != value)
                {
                    this.newersCountYear = value;
                    this.OnPropertyChanged("NewersCountYear");
                }
            }
        }

        #endregion

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