namespace P3.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;

    /// <summary>Состояние.</summary>
    public class Status
    {
        private DBconnect dbc = new DBconnect();
        private GetHTML ghtml = new GetHTML();
        private string address = "http://ares/Lists/List2/AllItems.aspx";
        private string htmlText;
        private string text;
        private int countStatus;
        private List<string> status = new List<string>();
        private List<string> statusName = new List<string>();
        private List<string> statusState = new List<string>();
        private List<string> statusDate = new List<string>();
        private List<string> statusID = new List<string>();
        private List<string> statusAct = new List<string>();

        /// <summary>Распарсить HTML.</summary>
        public void ParseHTML()
        {
            short i = 0;
            var startPos = 0;

            var ping = new Ping();

            try
            {
                var pingReply = ping.Send("ares.elcom.local");
                if (pingReply.Status == IPStatus.Success)
                {
                    this.htmlText = this.ghtml.Html(this.address);

                    if (this.htmlText.IndexOf(@"Кол-во значений", 0) != -1)
                    {
                        this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Кол-во значений"));
                        startPos = this.text.IndexOf(@"_self", 0);

                        this.countStatus = int.Parse(this.text.Substring(18, this.text.IndexOf(@"</B>", 19) - 18));

                        while (i != this.countStatus)
                        {
                            this.status.Add(this.SubObj("_self", startPos));

                            startPos += 5;
                            startPos = this.text.IndexOf(@"_self", startPos);
                            i++;
                        }

                        this.StatusParse();
                    }
                }
            }
            catch
            {
            }
        }

        private string SubObj(string sub, int startPos)
        {
            var len = sub.Length + 2;
            string sub2;

            if (sub == "_self")
            {
                sub2 = @"</a>";
            }
            else
            {
                sub2 = @"</TD>";
            }

            var startNamePos = this.text.IndexOf(sub, startPos) + len;
            var endNamePos = this.text.IndexOf(sub2, startNamePos);
            var subObj = this.text.Substring(startNamePos, endNamePos - startNamePos);

            return subObj;
        }

        private void StatusParse()
        {
            var st = string.Empty;

            var pattern = @"(\w{2,}\s\w[.]\s|\w{2,}\s\w[.]\w[.])";
            var regex = new Regex(pattern);

            var pattern2 = @"(на больн\D+|в отпуске\D+|в команд\D+|не будет\D+|на обучен\D+|отсутствует\D+)";
            var regex2 = new Regex(pattern2);

            var pattern3 = @"(\d+\W\d+\D+\d+\W\d+[-]\d+\W\d+|\d+\W\d+[-]\d+\W\d+|\d+\W\d+)";
            var regex3 = new Regex(pattern3);

            var pattern4 = @"( \(и.о. \D+\))";
            var regex4 = new Regex(pattern4);

            int k = 0;

            foreach (string str in this.status)
            {
                var match2 = regex2.Match(this.status[k]);
                var match3 = regex3.Match(this.status[k]);
                var match4 = regex4.Match(this.status[k]);

                foreach (Match match in regex.Matches(this.status[k]))
                {
                    st = match.Value.Remove(match.Value.LastIndexOf('.'));
                    if (st.LastIndexOf('.') != -1)
                    {
                        st = st.Remove(st.LastIndexOf('.'));
                    }

                    this.statusName.Add(st);
                    this.statusState.Add(match2.Value + match3.Value + match4.Value);
                    this.statusDate.Add(match3.Value);
                    this.statusAct.Add(this.ActStatus(match3.Value));
                }

                k++;
            }

            this.dbc.EployeeStatusWrite(this.statusState, this.statusName, this.statusAct);
            this.dbc.StatusWrite(this.status);
        }

        private string ActStatus(string date)
        {
            var act = string.Empty;
            var stD = string.Empty;
            var endD = string.Empty;
            DateTime d1;
            DateTime d2;

            var pattern3 = @"(\d+\W\d+)";
            var regex3 = new Regex(pattern3);

            var match3 = regex3.Match(date);

            switch (date.Length)
            {
                case 5:
                    d1 = DateTime.ParseExact(date, "dd.MM", null);

                    if (d1 <= DateTime.Today)
                    {
                        act = "1";
                    }
                    else
                    {
                        act = "0";
                    }

                    break;

                case 11:
                    stD = date.Substring(0, 5);
                    endD = date.Substring(6, 5);

                    d1 = DateTime.ParseExact(stD, "dd.MM", null);
                    d2 = DateTime.ParseExact(endD, "dd.MM", null);

                    if (d1 <= DateTime.Today && d2 >= DateTime.Today)
                    {
                        act = "1";
                    }
                    else
                    {
                        act = "0";
                    }

                    break;
            }

            return act;
        }
    }
}