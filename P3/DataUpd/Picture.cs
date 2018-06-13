namespace P3.DataUpd
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.NetworkInformation;
    using NLog;
    using Utils;

    /// <summary>Изображение.</summary>
    public class Picture
    {
        private GetHTML ghtml = new GetHTML();
        private string address = string.Empty;
        private string htmlText;
        private string text;
        private List<string> tempID = new List<string>();
        private string idTemp;

        /// <summary>ID.</summary>
        public List<string> TempID
        {
            get { return this.tempID; }
            set { this.tempID = value; }
        }

        /// <summary>Распарсить.</summary>
        /// <param name="logger">Логгер.</param>
        public void ParseHTML(Logger logger)
        {
            var ping = new Ping();

            try
            {
                var pingReply = ping.Send("ares.elcom.local");
                if (pingReply != null && pingReply.Status == IPStatus.Success)
                {
                    for (short i = 0; i < this.tempID.Count; i++)
                    {
                        this.idTemp = this.tempID[i];
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "//img//" + this.idTemp + ".jpg"))
                        {
                            this.address = "http://ares/Divisions/Lists/Employees/DispForm.aspx?ID=" + this.idTemp + "&Source=http%3A%2F%2Fares%2FDivisions%2FLists%2FEmployees%2FPhoneList%2Easpx";
                            this.GetPic();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void GetPic()
        {
            this.htmlText = this.ghtml.Html(this.address);

            if (this.htmlText.IndexOf(@"Фото:", 19, StringComparison.Ordinal) != -1)
            {
                this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Фото:", StringComparison.Ordinal));

                var startP = this.text.IndexOf("SRC", 0, StringComparison.Ordinal);
                if (startP < 300 && startP != -1)
                {
                    startP += 5;
                    var endP = this.text.IndexOf("\" ALT", 0, StringComparison.Ordinal);
                    var subObj = this.text.Substring(startP, endP - startP);

                    this.SavePic(subObj);
                }
            }
        }

        private void SavePic(string source)
        {
            var wc = new WebClient
            {
                Credentials = CredentialCache.DefaultCredentials
            };

            wc.DownloadFile(source, $"{AppDomain.CurrentDomain.BaseDirectory}/img/{this.idTemp}.jpg");
        }
    }
}