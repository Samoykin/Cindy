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

        private String _TextChange; //поле поиска по имени сотрудника
        private String _TextChangeCust; //поле поиска по имени заказчика
        private Int32 _SelectedIndex; //индекс в ListView списка сотрудников
        private Int32 _SelectedIndexCust; //индекс в ListView списка заказчиков
        private String _SelectedDiv; //выбранное значение в ComboBox по подразделениям
        private Int32 _Vacation; //кол-во сотрудников в отпуске
        private Int32 _BTrip; //кол-во сотрудников в командировке
        private Int32 _Sick; //кол-во сотрудников на больничном
        private String _Page1State; //
        private String _Page2State; //
        private String _Page3State; //
        private String _PageStatistic; //
        private String _DinnerSelectedPeople; //Выбранный сотрудник для добавления
        private Int32 _EmployeeCount; //количество сотрудников
        private Int32 _ContactsCount; //количество контактов
        private String _SaveStatus; //количество контактов
        private String _UpdStatus; //состояние обновления данных с сайта
        private Int32 _MiddleAge; //средний возраст
        private Int32 _MiddleTimeRecord; //средний стаж
        private string _startPageVisible; //видимость стартовой страницы
        private string _filterPageVisible; //видимость страницы с данными


        public String TextChange
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

        public String TextChangeCust
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

        public Int32 SelectedIndex
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

        public Int32 SelectedIndexCust
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

        public String SelectedDiv
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

        public Int32 Vacation
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

        public Int32 BTrip
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

        public Int32 Sick
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

        public String Page1State
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

        public String Page2State
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

        public String Page3State
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

        public String PageStatistic
        {
            get { return _PageStatistic; }
            set
            {
                if (_PageStatistic != value)
                {
                    _PageStatistic = value;
                    OnPropertyChanged("PageStatistic");
                    //onCount();
                }
            }
        }

        public String DinnerSelectedPeople
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

        public Int32 EmployeeCount
        {
            get { return _EmployeeCount; }
            set
            {
                if (_EmployeeCount != value)
                {
                    _EmployeeCount = value;
                    OnPropertyChanged("EmployeeCount");
                }
            }
        }

        public Int32 ContactsCount
        {
            get { return _ContactsCount; }
            set
            {
                if (_ContactsCount != value)
                {
                    _ContactsCount = value;
                    OnPropertyChanged("ContactsCount");
                }
            }
        }

        public String SaveStatus
        {
            get { return _SaveStatus; }
            set
            {
                if (_SaveStatus != value)
                {
                    _SaveStatus = value;
                    OnPropertyChanged("SaveStatus");
                    //onCount();
                }
            }
        }

        public String UpdStatus
        {
            get { return _UpdStatus; }
            set
            {
                if (_UpdStatus != value)
                {
                    _UpdStatus = value;
                    OnPropertyChanged("UpdStatus");
                    //onCount();
                }
            }
        }
        public Int32 MiddleAge
        {
            get { return _MiddleAge; }
            set
            {
                if (_MiddleAge != value)
                {
                    _MiddleAge = value;
                    OnPropertyChanged("MiddleAge");
                }
            }
        }

        public Int32 MiddleTimeRecord
        {
            get { return _MiddleTimeRecord; }
            set
            {
                if (_MiddleTimeRecord != value)
                {
                    _MiddleTimeRecord = value;
                    OnPropertyChanged("MiddleTimeRecord");
                }
            }
        }
        public String StartPageVisible
        {
            get { return _startPageVisible; }
            set
            {
                if (_startPageVisible != value)
                {
                    _startPageVisible = value;
                    OnPropertyChanged("StartPageVisible");
                    //onCount();
                }
            }
        }
        public String FilterPageVisible
        {
            get { return _filterPageVisible; }
            set
            {
                if (_filterPageVisible != value)
                {
                    _filterPageVisible = value;
                    OnPropertyChanged("FilterPageVisible");
                    //onCount();
                }
            }
        }

    }
}
