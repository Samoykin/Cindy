using P3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Utils
{
    class NewsData
    {
        public NewsList GetNews(ObservableCollection<Employee> employeeLst)
        {
            //List<Division> news = new List<Division>();
            //List<Division> futureNews = new List<Division>();
            NewsList nList = new NewsList();
            nList.news = new List<New>();
            nList.futureNews = new List<New>();
            //List<New> news2 = new List<New>();
            //List<New> futureNews2 = new List<New>();

            foreach (Employee e in employeeLst)
            {
                if (e.BirthDay.Month == DateTime.Now.Month && e.BirthDay.Day == DateTime.Now.Day)
                {
                    New item = new New();

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
                    nList.news.Add(item);
                }

                if (e.StartDay.Month == DateTime.Now.Month && e.StartDay.Day == DateTime.Now.Day)
                {
                    New item = new New();

                    if (e.TimeRecord != "менее года")
                    {
                        if (Convert.ToInt32(e.TimeRecord) % 5 == 0)
                        {
                            //юбилей
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

                    nList.news.Add(item);

                }

                //----------------------------------------------------------
                //Предстоящие события
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = e.BirthDay;
                Int32 diff = dt1.Year - dt2.Year;

                dt2 = e.BirthDay.AddYears(diff);

                //день рождения
                if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                {
                    New item = new New();

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

                    nList.futureNews.Add(item);
                }

                //годовщина работы
                DateTime dt3 = DateTime.Now;
                DateTime dt4 = e.StartDay;
                Int32 diff2 = dt3.Year - dt4.Year;

                dt2 = e.StartDay.AddYears(diff2);

                if (dt2 > dt1 && dt2 < dt1.AddDays(7))
                {
                    New item = new New();
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
                        //юбилей
                        item.Prefix = " празднует Юбилей: ";
                        item.YearCount = Convert.ToInt32(e.TimeRecord) +1;
                        item.Postfix = " лет работы в компании";
                    }
                    else
                    {
                        item.Prefix = " празднует ";
                        item.YearCount = Convert.ToInt32(e.TimeRecord) + 1;
                        item.Postfix = "-й год работы в компании";
                    }

                    nList.futureNews.Add(item);
                }


            }

            return nList;
        }
    }
}
