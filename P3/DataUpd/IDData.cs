namespace P3.DataUpd
{    
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;

    using Utils;

    /// <summary>Парсер.</summary>
    public class IDData
    {
        private const string Address = @"http://ares/Divisions/Lists/Employees/PhoneList.aspx";
        private const string CheckSite = "ares.elcom.local";
        private DBconnect dbc = new DBconnect();
        private GetHTML ghtml = new GetHTML();        
        private string htmlText;
        private string text;
        private List<string> tID = new List<string>();

        /// <summary>Флаг.</summary>
        public bool Flag { get; set; }

        /// <summary>Распарсить HTML.</summary>
        public async void ParseHTML()
        {
            await Task.Factory.StartNew(() =>
            {
                ConnCheck();
            });   
        }

        /// <summary>Проверить связь.</summary>
        public void ConnCheck()
        {
            var q = new Ping();

            try
            {
                var an = q.Send(CheckSite);
                if (an.Status == IPStatus.Success)
                {
                    this.htmlText = this.ghtml.Html(Address);

                    if (this.htmlText.IndexOf("Кол-во значений", 19) != -1)
                    {
                        this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Кол-во значений"));
                        this.text = this.text.Remove(this.text.LastIndexOf(@"<!-- FooterBanner closes the TD") - 39);

                        this.ExecValues();
                    }
                }
                else
                {
                    this.Flag = false;
                }
            }
            catch
            {
            }
        }

        /// <summary>Извлечь значения.</summary>
        public void ExecValues()
        {
            this.tID.Clear();

            int countWorcers;
            var i = 0;

            string id;

            var startPos = this.text.IndexOf(@"ID", 0);

            countWorcers = int.Parse(this.text.Substring(18, this.text.IndexOf(@"</B>", 19) - 18));

            while (i != countWorcers)
            {
                id = this.text.Substring(startPos + 3, 3);
                if (id.IndexOf("\"", 0) != -1)
                {
                    id = this.text.Substring(startPos + 3, 2);
                }

                this.tID.Add(id);

                startPos = this.text.IndexOf(@"A HREF", startPos);
                startPos = this.text.IndexOf(@"ID", startPos + 64);

                i++;
            }

            var personalData = new PersonalData(this.tID);
            personalData.ParseHTML();

            var statusEmpl2 = new Status(); // статусы
            statusEmpl2.ParseHTML();

            var dlP = new Picture(); // фотки
            dlP.TempID = this.tID;
            dlP.ParseHTML();

            this.Flag = true;
        }              
    }
}