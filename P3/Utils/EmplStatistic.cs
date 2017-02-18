using P3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Utils
{
    class EmplStatistic
    {
        Statistic _statistic = new Statistic();
        ObservableCollection<Employee> _employeeLst = new ObservableCollection<Employee>();
        ObservableCollection<Employee> _employeeNewLst = new ObservableCollection<Employee>();

        public EmplStatistic(ObservableCollection<Employee> employeeLst, Statistic statistic)
        {
            _employeeLst = employeeLst;
            _statistic = statistic;
            CalcCount();
        }

        public Statistic CalcCount()
        {
            DateTime dtTemp = DateTime.Now;
            //новички за месяц 
            Newers(new DateTime(dtTemp.Year, dtTemp.Month, 1), dtTemp);
            _statistic.NewersCountMonth = _employeeNewLst.Count();

            //новички за 1 квартал         DateTime.Now.AddDays(-92)   
            Newers(new DateTime(dtTemp.Year, 1, 1), new DateTime(dtTemp.Year, 3, 31));
            _statistic.NewersCountQuarter1 = _employeeNewLst.Count();

            //новички за 2 квартал
            Newers(new DateTime(dtTemp.Year, 4, 1), new DateTime(dtTemp.Year, 6, 30));
            _statistic.NewersCountQuarter2 = _employeeNewLst.Count();

            //новички за 3 квартал
            Newers(new DateTime(dtTemp.Year, 7, 1), new DateTime(dtTemp.Year, 9, 30));
            _statistic.NewersCountQuarter3 = _employeeNewLst.Count();

            //новички за 4 квартал
            Newers(new DateTime(dtTemp.Year, 10, 1), new DateTime(dtTemp.Year, 12, 31));
            _statistic.NewersCountQuarter4 = _employeeNewLst.Count();

            //новички за год            
            Newers(new DateTime(dtTemp.Year, 1, 1), dtTemp);
            _statistic.NewersCountYear = _employeeNewLst.Count();

            //Новички
            _statistic.StartNewers = DateTime.Now.AddDays(-60);
                //new DateTime(2016, 09, 01);

            Newers(_statistic.StartNewers, dtTemp);
            _statistic.NewersCount = _employeeNewLst.Count();

            return _statistic;
        }

        //public Statistic DynamicCalcNewers()
        //{
            

        //    return _statistic;
        //}

        public ObservableCollection<Employee> Newers(DateTime startDay, DateTime endDay)
        {
            _employeeNewLst.Clear();

            foreach (Employee ee in _employeeLst)
            {
                if (ee.StartDay >= startDay && ee.StartDay <= endDay)
                {
                    _employeeNewLst.Add(ee);
                }

            }

            return _employeeNewLst;
        }



    }
}
