namespace P3.Utils
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;    
    using System.Linq;
    using Model;

    /// <summary>Получение HTML.</summary>
    public class NewsManager
    {
        private IEnumerable<Employee> employeeLst;

        /// <summary>Initializes a new instance of the <see cref="NewsManager" /> class.</summary>
        /// <param name="employeeLst">Адрес.</param>
        public NewsManager(IEnumerable<Employee> employeeLst)
        {
            this.employeeLst = employeeLst;
        }

        /// <summary>HTML.</summary>
        /// <returns>Строка.</returns>
        public ObservableCollection<NewEvent> GetNews()
        {
            var news = new ObservableCollection<NewEvent>();

            var newsData = new NewsData();
            var newsList = new NewsList();
            var newsT = new List<NewEvent>();

            newsList = newsData.GetNews(this.employeeLst);

            newsT = newsList.News;

            if (newsT != null && newsT.Count != 0)
            {
                foreach (var n in newsT.OrderBy(a => a.Date))
                {
                    news.Add(n);
                }
            }
            else
            {
                var newT = new NewEvent();
                newT.Prefix = "На данный момент количество новостей ";
                newT.Postfix = " штук";
                news.Add(newT);
            }
            
            return news;
        }

        /// <summary>HTMLss.</summary>
        /// <returns>Строка.</returns>
        public ObservableCollection<NewEvent> GetFutureNews()
        {
            var futureNews = new ObservableCollection<NewEvent>();

            var newsData = new NewsData();

            var newsList = new NewsList();
            var futureNewsT = new List<NewEvent>();
            newsList = newsData.GetNews(this.employeeLst);

            futureNewsT = newsList.FutureNews;

            if (futureNewsT != null)
            {
                foreach (NewEvent n in futureNewsT.OrderBy(a => a.Date))
                {
                    futureNews.Add(n);
                }
            }

            return futureNews;
        }
    }
}