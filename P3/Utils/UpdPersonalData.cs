namespace P3.Utils
{
    using System.Collections.Generic;
    using System.Net.NetworkInformation;

    /// <summary>Обновление персональных данных.</summary>
    public class UpdPersonalData
    {
        private DBconnect dbc = new DBconnect();
        private GetHTML ghtml = new GetHTML();
        private string address = "http://ares/Divisions/Lists/Employees/PhoneList.aspx";
        private string htmlText;
        private string text;

        private List<string> tempID = new List<string>();
        private List<string> tempNames = new List<string>();
        private List<string> tempTels = new List<string>();
        private List<string> tempTels2 = new List<string>();
        private List<string> tempTels3 = new List<string>();
        private List<string> tempEmail = new List<string>();
        private List<string> tempDiv = new List<string>();
        private List<string> tempPos = new List<string>();
        private List<string> tempStatus = new List<string>();

        /// <summary>Распарсить HTML.</summary>
        public void ParseHTML()
        {
            Ping q = new Ping();

            try
            {
                this.htmlText = this.ghtml.Html(this.address);

                if (this.htmlText.IndexOf(@"Кол-во значений", 19) != -1)
                {
                    this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Кол-во значений"));
                    this.text = this.text.Remove(this.text.LastIndexOf(@"<!-- FooterBanner closes the TD") - 39);
                    this.ExecValues();
                }
            }
            catch
            {
            }
        }

        private void ExecValues()
        {
            this.tempID.Clear();
            this.tempNames.Clear();
            this.tempTels.Clear();
            this.tempTels2.Clear();
            this.tempTels3.Clear();
            this.tempEmail.Clear();
            this.tempDiv.Clear();
            this.tempPos.Clear();

            int countWorcers;
            int i = 0;

            string id;
            string name1;
            string t1;
            string t2;
            string t3;
            string email1;
            string division1;
            string position1;

            int startPos = this.text.IndexOf(@"ID", 0);

            countWorcers = int.Parse(this.text.Substring(18, this.text.IndexOf(@"</B>", 19) - 18));

            while (i != countWorcers)
            {
                id = this.text.Substring(startPos + 3, 3);
                if (id.IndexOf("\"", 0) != -1)
                {
                    id = this.text.Substring(startPos + 3, 2);
                }

                this.tempID.Add(id);

                // Имя
                name1 = this.SubObj(@"_self", startPos);
                this.tempNames.Add(name1);

                // Внутренний телефон
                startPos = this.text.IndexOf(@"ms-vb2", startPos);
                t1 = this.SubObj(@"ms-vb2", startPos);
                startPos += 5;

                // Мобильный телефон
                startPos = this.text.IndexOf(@"ms-vb2", startPos);
                t2 = this.SubObj(@"ms-vb2", startPos);
                startPos += 5;

                // Переадресация на сотовый
                startPos = this.text.IndexOf(@"ms-vb2", startPos);
                t3 = this.SubObj(@"ms-vb2", startPos);
                startPos += 5;

                // e-mail
                startPos = this.text.IndexOf(@"ms-vb2", startPos);
                startPos += 8;
                int checkEnd = this.text.IndexOf(@"</TD><TD", startPos);
                if (checkEnd - startPos > 5)
                {
                    startPos = this.text.IndexOf(@">", startPos) + 1;
                    email1 = this.text.Substring(startPos, this.text.IndexOf(@"</a></TD>", startPos) - startPos);
                }
                else
                {
                    email1 = string.Empty;
                }

                startPos += 5;
                this.tempEmail.Add(email1);

                // Подразделение
                startPos = this.text.IndexOf(@"ms-vb2", startPos);
                startPos += 8;
                startPos = this.text.IndexOf(@">", startPos) + 1;
                division1 = this.text.Substring(startPos, this.text.IndexOf(@"</A></TD>", startPos) - startPos);
                startPos += 5;
                this.tempDiv.Add(division1);

                // Должность
                startPos = this.text.IndexOf(@"ms-vb2", startPos);
                position1 = this.SubObj(@"ms-vb2", startPos);
                this.tempPos.Add(position1);
                startPos += 5;

                startPos = this.text.IndexOf(@"ID", startPos);
                t1 = this.SpaceReplase(t1);
                this.tempTels.Add(t1);
                t2 = this.SpaceReplase(t2);
                this.tempTels2.Add(t2);
                t3 = this.SpaceReplase(t3);
                this.tempTels3.Add(t3);

                i++;
            }

            try
            {
                this.dbc.ClearTable("employee");
            }
            catch
            {
            }

            this.dbc.EmployeeWrite(this.tempID, this.tempNames, this.tempTels, this.tempTels2, this.tempTels3, this.tempEmail, this.tempDiv, this.tempPos);
        }

        private string SpaceReplase(string str)
        {
            if (str.IndexOf(@"&nbsp;", 0) != -1)
            {
                while (str.IndexOf(@"&nbsp;", 0) != -1)
                {
                    str = str.Replace(@"&nbsp;", string.Empty);
                }
            }

            return str;
        }

        private string SubObj(string sub, int startPos)
        {
            int len = sub.Length + 2;
            string sub2;

            if (sub == "_self")
            {
                sub2 = @"</a>";
            }
            else
            {
                sub2 = @"</TD>";
            }

            int startNamePos = this.text.IndexOf(sub, startPos) + len;
            int endNamePos = this.text.IndexOf(sub2, startNamePos);
            string subObj = this.text.Substring(startNamePos, endNamePos - startNamePos);

            return subObj;
        }
    }
}