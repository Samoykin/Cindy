namespace P3.DataUpd
{
    using System;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;
    using NLog;
    using Utils;

    /// <summary>Парсер.</summary>
    public class DataUpdater
    {
        private const string Address = @"http://ares/Divisions/Lists/Employees/PhoneList.aspx";
        private const string CheckSite = "ares.elcom.local";
        private GetHTML ghtml = new GetHTML();        
        private string htmlText;
        private string text;
        private List<string> tID = new List<string>();
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>Флаг.</summary>
        public bool Flag { get; set; }

        /// <summary>Распарсить HTML.</summary>
        /// <param name="logger">Логгер.</param>
        public async void ParseHTML(Logger logger)
        {
            this.logger = logger;
            await Task.Factory.StartNew(this.ConnCheck);   
        }

        /// <summary>Проверить связь.</summary>
        private void ConnCheck()
        {
            var ping = new Ping();

            try
            {
                var pingReply = ping.Send(CheckSite);
                if (pingReply != null && pingReply.Status == IPStatus.Success)
                {
                    this.htmlText = this.ghtml.Html(Address);

                    if (this.htmlText.IndexOf("Кол-во значений", 19, StringComparison.Ordinal) != -1)
                    {
                        this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Кол-во значений", StringComparison.Ordinal));
                        this.text = this.text.Remove(this.text.LastIndexOf(@"<!-- FooterBanner closes the TD", StringComparison.Ordinal) - 39);

                        this.ExecValues();
                    }
                }
                else
                {
                    this.Flag = false;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }

        /// <summary>Извлечь значения.</summary>
        private void ExecValues()
        {
            this.tID.Clear();

            var i = 0;
            var startPos = this.text.IndexOf(@"ID", 0, StringComparison.Ordinal);
            var countWorcers = int.Parse(this.text.Substring(18, this.text.IndexOf(@"</B>", 19, StringComparison.Ordinal) - 18));

            while (i != countWorcers)
            {
                var id = this.text.Substring(startPos + 3, 3);
                if (id.IndexOf("\"", 0, StringComparison.Ordinal) != -1)
                {
                    id = this.text.Substring(startPos + 3, 2);
                }

                this.tID.Add(id);

                startPos = this.text.IndexOf(@"A HREF", startPos, StringComparison.Ordinal);
                startPos = this.text.IndexOf(@"ID", startPos + 64, StringComparison.Ordinal);

                i++;
            }

            var personalData = new PersonalData(this.tID);
            personalData.ParseHTML();

            var statusEmpl2 = new Status(); // Статусы
            statusEmpl2.ParseHTML();

            var dlP = new Picture // Фотки
            {
                TempID = this.tID
            }; 
            
            dlP.ParseHTML(this.logger);

            this.Flag = true;
        }              
    }
}