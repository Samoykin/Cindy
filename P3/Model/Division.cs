namespace P3.Model
{
    using System.ComponentModel;

    /// <summary>Подразделение.</summary>
    public class Division : INotifyPropertyChanged
    {
        private string divValue;

        /// <summary>Событие изменения свойства.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Значение.</summary>
        public string Value
        {
            get
            {
                return this.divValue;
            }

            set
            {
                if (this.divValue != value)
                {
                    this.divValue = value;
                    this.OnPropertyChanged("Value");
                }
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
    }
}