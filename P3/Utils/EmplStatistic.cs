namespace P3.Utils
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Model;

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
        }

        /// <summary>Количество новичков.</summary>
        /// <returns>Статистика.</returns>
        public Statistic CalcCount()
        {
            var date = DateTime.Now;

            // Новички за месяц 
            this.Newers(new DateTime(date.Year, date.Month, 1), date);
            this.statistic.NewersCountMonth = this.employeeNewLst.Count();

            // Новички за 1 квартал
            this.Newers(new DateTime(date.Year, 1, 1), new DateTime(date.Year, 3, 31));
            this.statistic.NewersCountQuarter1 = this.employeeNewLst.Count();

            // Новички за 2 квартал
            this.Newers(new DateTime(date.Year, 4, 1), new DateTime(date.Year, 6, 30));
            this.statistic.NewersCountQuarter2 = this.employeeNewLst.Count();

            // Новички за 3 квартал
            this.Newers(new DateTime(date.Year, 7, 1), new DateTime(date.Year, 9, 30));
            this.statistic.NewersCountQuarter3 = this.employeeNewLst.Count();

            // Новички за 4 квартал
            this.Newers(new DateTime(date.Year, 10, 1), new DateTime(date.Year, 12, 31));
            this.statistic.NewersCountQuarter4 = this.employeeNewLst.Count();

            // Новички за год            
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

            foreach (var empl in this.employeeLst)
            {
                if (empl.StartDay >= startDay && empl.StartDay <= endDay)
                {
                    this.employeeNewLst.Add(empl);
                }
            }

            return this.employeeNewLst;
        }
    }
}