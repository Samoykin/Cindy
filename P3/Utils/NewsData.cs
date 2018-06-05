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
        public NewsList GetNews(ObservableCollection<Employee> employeeLst)
        {
            var newsList = new NewsList();
            newsList.News = new List<New>();
            newsList.FutureNews = new List<New>();

            foreach (var e in employeeLst)
            {
                if (e.BirthDay.Month == DateTime.Now.Month && e.BirthDay.Day == DateTime.Now.Day)
                {
                    var item = new New();

                    if (Convert.ToInt32(e.Age) % 5 == 0)
                    {
                        item.FullName = e.FullName;
                        item.Prefix = " празднует Юбилей: ";
                        item.YearCount = e.Age;
                        item.Postfix = " лет";
                    }
                    else
                    {
                        item.FullName = e.FullName;
                        item.Prefix = " празднует ";
                        item.YearCount = e.Age;
                        item.Postfix = "-й День рождения";
                    }

                    newsList.News.Add(item);
                }

                if (e.StartDay.Month == DateTime.Now.Month && e.StartDay.Day == DateTime.Now.Day)
                {
                    var item = new New();

                    if (e.TimeRecord != "менее года")
                    {
                        if (Convert.ToInt32(e.TimeRecord) % 5 == 0)
                        {
                            // юбилей
                            item.FullName = e.FullName;
                            item.Prefix = " празднует Юбилей: ";
                            item.YearCount = Convert.ToInt32(e.TimeRecord);
                            item.Postfix = " лет работы в компании";
                        }
                        else
                        {
                            item.FullName = e.FullName;
                            item.Prefix = " празднует ";
                            item.YearCount = Convert.ToInt32(e.TimeRecord);
                            item.Postfix = "-й год работы в компании";
                        }
                    }

                    newsList.News.Add(item);
                }

                // ----------------------------------------------------------
                // Предстоящие события
                var dt1 = DateTime.Now;
                var dt2 = e.BirthDay;
                int diff = dt1.Year - dt2.Year;

                dt2 = e.BirthDay.AddYears(diff);

                // день рождения
                if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                {
                    var item = new New();

                    item.Date = new DateTime(DateTime.Now.Year, e.BirthDay.Month, e.BirthDay.Day);
                    item.FullName = e.FullName;
                    item.YearCount = e.Age + 1;

                    if (Convert.ToInt32(e.Age + 1) % 5 == 0)
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

                // годовщина работы
                var dt3 = DateTime.Now;
                var dt4 = e.StartDay;
                int diff2 = dt3.Year - dt4.Year;

                dt2 = e.StartDay.AddYears(diff2);

                if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                {
                    var item = new New();
                    item.Date = new DateTime(DateTime.Now.Year, e.StartDay.Month, e.StartDay.Day);
                    item.FullName = e.FullName;

                    if (e.TimeRecord == "менее года")
                    {                        
                        item.Prefix = " празднует ";
                        item.YearCount = 1;
                        item.Postfix = "-й год работы в компании";
                    }
                    else if ((Convert.ToInt32(e.TimeRecord) + 1) % 5 == 0)
                    {
                        // юбилей
                        item.Prefix = " празднует Юбилей: ";
                        item.YearCount = Convert.ToInt32(e.TimeRecord) + 1;
                        item.Postfix = " лет работы в компании";
                    }
                    else
                    {
                        item.Prefix = " празднует ";
                        item.YearCount = Convert.ToInt32(e.TimeRecord) + 1;
                        item.Postfix = "-й год работы в компании";
                    }

                    newsList.FutureNews.Add(item);
                }
            }

            return newsList;
        }
    }
}