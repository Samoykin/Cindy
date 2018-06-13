namespace P3.Model
{
    using System.ComponentModel;

    /// <summary>Человек.</summary>
    public class People : INotifyPropertyChanged
    {
        #region Fields

        private string name;
        private string phone;
        private string age;
        private string descr;

        #endregion

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        /// <summary>Имя.</summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>Телефон.</summary>
        public string Phone
        {
            get
            {
                return this.phone;
            }

            set
            {
                if (this.phone != value)
                {
                    this.phone = value;
                    this.OnPropertyChanged("Phone");
                }
            }
        }

        /// <summary>Возраст.</summary>
        public string Age
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

        /// <summary>Описание.</summary>
        public string Descr
        {
            get
            {
                return this.descr;
            }

            set
            {
                if (this.descr != value)
                {
                    this.descr = value;
                    this.OnPropertyChanged("Descr");
                }
            }
        }

        #endregion

        #region Implement INotyfyPropertyChanged members
        
        /// <summary>Изменения свойства.</summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}