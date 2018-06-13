namespace P3.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Model;

    /// <summary>Новости.</summary>
    public class NewsData
    {
        /// <summary>Получить новости.</summary>
        /// <param name="employeeLst">Сотрудники.</param>
        /// <returns>Список.</returns>
        public NewsList GetNews(IEnumerable<Employee> employeeLst)
        {
            var newsList = new NewsList
            {
                News = new List<NewEvent>(),
                FutureNews = new List<NewEvent>()
            };

            foreach (var employee in employeeLst)
            {
                if (employee.BirthDay.Month == DateTime.Now.Month && employee.BirthDay.Day == DateTime.Now.Day)
                {
                    var item = new NewEvent();

                    if (Convert.ToInt32(employee.Age) % 5 == 0)
                    {
                        item.FullName = employee.FullName;
                        item.Prefix = " празднует Юбилей: ";
                        item.YearCount = employee.Age;
                        item.Postfix = " лет";
                    }
                    else
                    {
                        item.FullName = employee.FullName;
                        item.Prefix = " празднует ";
                        item.YearCount = employee.Age;
                        item.Postfix = "-й День рождения";
                    }

                    newsList.News.Add(item);
                }

                if (employee.StartDay.Month == DateTime.Now.Month && employee.StartDay.Day == DateTime.Now.Day)
                {
                    var item = new NewEvent();

                    if (employee.TimeRecord != "менее года")
                    {
                        if (Convert.ToInt32(employee.TimeRecord) % 5 == 0)
                        {
                            // Юбилей
                            item.FullName = employee.FullName;
                            item.Prefix = " празднует Юбилей: ";
                            item.YearCount = Convert.ToInt32(employee.TimeRecord);
                            item.Postfix = " лет работы в компании";
                        }
                        else
                        {
                            item.FullName = employee.FullName;
                            item.Prefix = " празднует ";
                            item.YearCount = Convert.ToInt32(employee.TimeRecord);
                            item.Postfix = "-й год работы в компании";
                        }
                    }

                    newsList.News.Add(item);
                }

                // Предстоящие события
                var dt1 = DateTime.Now;
                var dt2 = employee.BirthDay;
                var diff = dt1.Year - dt2.Year;

                dt2 = employee.BirthDay.AddYears(diff);

                // День рождения
                if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                {
                    var item = new NewEvent
                    {
                        Date = new DateTime(DateTime.Now.Year, employee.BirthDay.Month, employee.BirthDay.Day),
                        FullName = employee.FullName,
                        YearCount = employee.Age + 1
                    };

                    if (Convert.ToInt32(employee.Age + 1) % 5 == 0)
                    {                        
                        item.Prefix = " празднует Юбилей: ";                        
                        item.Postfix = " лет";
                    }
                    else
                    {                        
                        item.Prefix = " празднует ";
                        item.Postfix = "-й День рождения";
                    }

                    newsList.FutureNews.Add(item);
                }

                // Годовщина работы
                var dt3 = DateTime.Now;
                var dt4 = employee.StartDay;
                var diff2 = dt3.Year - dt4.Year;

                dt2 = employee.StartDay.AddYears(diff2);

                if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                {
                    var item = new NewEvent
                    {
                        Date = new DateTime(DateTime.Now.Year, employee.StartDay.Month, employee.StartDay.Day),
                        FullName = employee.FullName
                    };

                    if (employee.TimeRecord == "менее года")
                    {                        
                        item.Prefix = " празднует ";
                        item.YearCount = 1;
                        item.Postfix = "-й год работы в компании";
                    }
                    else if ((Convert.ToInt32(employee.TimeRecord) + 1) % 5 == 0)
                    {
                        // Юбилей
                        item.Prefix = " празднует Юбилей: ";
                        item.YearCount = Convert.ToInt32(employee.TimeRecord) + 1;
                        item.Postfix = " лет работы в компании";
                    }
                    else
                    {
                        item.Prefix = " празднует ";
                        item.YearCount = Convert.ToInt32(employee.TimeRecord) + 1;
                        item.Postfix = "-й год работы в компании";
                    }

                    newsList.FutureNews.Add(item);
                }
            }

            return newsList;
        }
    }
}