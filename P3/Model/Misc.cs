using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Model
{
    class Misc : INotifyPropertyChanged
    {
        public delegate void MethodContainer();
        public event MethodContainer onCount;


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

        private string _TextChange; //поле поиска по имени сотрудника
        private string _TextChangeCust; //поле поиска по имени заказчика
        private int _SelectedIndex; //индекс в ListView списка сотрудников
        private int _SelectedIndexCust; //индекс в ListView списка заказчиков
        private string _SelectedDiv; //выбранное значение в ComboBox по подразделениям
        private int _Vacation; //кол-во сотрудников в отпуске
        private int _BTrip; //кол-во сотрудников в командировке
        private int _Sick; //кол-во сотрудников на больничном
        private string _Page1State; //
        private string _Page2State; //
        private string _Page3State; //
        private string _DinnerSelectedPeople; //Выбранный сотрудник для добавления

        public string TextChange
        {
            get { return _TextChange; }
            set
            {
                if (_TextChange != value)
                {
                    _TextChange = value;
                    OnPropertyChanged("TextChange");
                    //onCount();
                }
            }
        }

        public string TextChangeCust
        {
            get { return _TextChangeCust; }
            set
            {
                if (_TextChangeCust != value)
                {
                    _TextChangeCust = value;
                    OnPropertyChanged("TextChangeCust");
                    //onCount();
                }
            }
        }

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    OnPropertyChanged("SelectedIndex");
                }
            }
        }

        public int SelectedIndexCust
        {
            get { return _SelectedIndexCust; }
            set
            {
                if (_SelectedIndexCust != value)
                {
                    _SelectedIndexCust = value;
                    OnPropertyChanged("SelectedIndexCust");
                }
            }
        }

        public string SelectedDiv
        {
            get { return _SelectedDiv; }
            set
            {
                if (_SelectedDiv != value)
                {
                    _SelectedDiv = value;
                    OnPropertyChanged("SelectedDiv");
                }
            }
        }

        public int Vacation
        {
            get { return _Vacation; }
            set
            {
                if (_Vacation != value)
                {
                    _Vacation = value;
                    OnPropertyChanged("Vacation");
                }
            }
        }

        public int BTrip
        {
            get { return _BTrip; }
            set
            {
                if (_BTrip != value)
                {
                    _BTrip = value;
                    OnPropertyChanged("BTrip");
                }
            }
        }

        public int Sick
        {
            get { return _Sick; }
            set
            {
                if (_Sick != value)
                {
                    _Sick = value;
                    OnPropertyChanged("Sick");
                }
            }
        }

        public string Page1State
        {
            get { return _Page1State; }
            set
            {
                if (_Page1State != value)
                {
                    _Page1State = value;
                    OnPropertyChanged("Page1State");
                    //onCount();
                }
            }
        }

        public string Page2State
        {
            get { return _Page2State; }
            set
            {
                if (_Page2State != value)
                {
                    _Page2State = value;
                    OnPropertyChanged("Page2State");
                    //onCount();
                }
            }
        }

        public string Page3State
        {
            get { return _Page3State; }
            set
            {
                if (_Page3State != value)
                {
                    _Page3State = value;
                    OnPropertyChanged("Page3State");
                    //onCount();
                }
            }
        }

        public string DinnerSelectedPeople
        {
            get { return _DinnerSelectedPeople; }
            set
            {
                if (_DinnerSelectedPeople != value)
                {
                    _DinnerSelectedPeople = value;
                    OnPropertyChanged("DinnerSelectedPeople");
                    //onCount();
                }
            }
        }



    }
}
