namespace P3.Utils
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using P3.Model;

    /// <summary>Статистика по сотрудникам.</summary>
    public class EmplStatistic
    {
        private Statistic statistic = new Statistic();
        private ObservableCollection<Employee> employeeLst = new ObservableCollection<Employee>();
        private ObservableCollection<Employee> employeeNewLst = new ObservableCollection<Employee>();

        /// <summary>Initializes a new instance of the <see cref="EmplStatistic" /> class.</summary>
        /// <param name="employeeLst">Сотрудники.</param>
        /// <param name="statistic">Статистика.</param>
        public EmplStatistic(ObservableCollection<Employee> employeeLst, Statistic statistic)
        {
            this.employeeLst = employeeLst;
            this.statistic = statistic;
            this.CalcCount();
        }

        /// <summary>Количество новичков.</summary>
        /// <returns>Статистика.</returns>
        public Statistic CalcCount()
        {
            var date = DateTime.Now;

            // новички за месяц 
            this.Newers(new DateTime(date.Year, date.Month, 1), date);
            this.statistic.NewersCountMonth = this.employeeNewLst.Count();

            // новички за 1 квартал         DateTime.Now.AddDays(-92)   
            this.Newers(new DateTime(date.Year, 1, 1), new DateTime(date.Year, 3, 31));
            this.statistic.NewersCountQuarter1 = this.employeeNewLst.Count();

            // новички за 2 квартал
            this.Newers(new DateTime(date.Year, 4, 1), new DateTime(date.Year, 6, 30));
            this.statistic.NewersCountQuarter2 = this.employeeNewLst.Count();

            // новички за 3 квартал
            this.Newers(new DateTime(date.Year, 7, 1), new DateTime(date.Year, 9, 30));
            this.statistic.NewersCountQuarter3 = this.employeeNewLst.Count();

            // новички за 4 квартал
            this.Newers(new DateTime(date.Year, 10, 1), new DateTime(date.Year, 12, 31));
            this.statistic.NewersCountQuarter4 = this.employeeNewLst.Count();

            // новички за год            
            this.Newers(new DateTime(date.Year, 1, 1), date);
            this.statistic.NewersCountYear = this.employeeNewLst.Count();

            // Новички
            this.statistic.StartNewers = DateTime.Now.AddDays(-60);

            this.Newers(this.statistic.StartNewers, date);
            this.statistic.NewersCount = this.employeeNewLst.Count();

            return this.statistic;
        }

        /// <summary>Получить новичков.</summary>
        /// <param name="startDay">Дата начала выборки.</param>
        /// <param name="endDay">Дата конца выборки.</param>
        /// <returns>Новички.</returns>
        public ObservableCollection<Employee> Newers(DateTime startDay, DateTime endDay)
        {
            this.employeeNewLst.Clear();

            foreach (Employee ee in this.employeeLst)
            {
                if (ee.StartDay >= startDay && ee.StartDay <= endDay)
                {
                    this.employeeNewLst.Add(ee);
                }
            }

            return this.employeeNewLst;
        }
    }
}