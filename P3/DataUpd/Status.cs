namespace P3.DataUpd
{
    using System;
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;
    using Utils;

    /// <summary>Состояние.</summary>
    public class Status
    {
        private const string Address = "http://ares/Lists/List2/AllItems.aspx";
        private DBconnect dbc = new DBconnect();
        private GetHTML ghtml = new GetHTML();        
        private string htmlText;
        private string text;
        private int countStatus;
        private List<string> status = new List<string>();
        private List<string> statusName = new List<string>();
        private List<string> statusState = new List<string>();
        private List<string> statusDate = new List<string>();
        private List<string> statusAct = new List<string>();

        /// <summary>Распарсить HTML.</summary>
        public void ParseHTML()
        {
            short i = 0;
            var ping = new Ping();

            var pingReply = ping.Send("ares.elcom.local");
            if (pingReply != null && pingReply.Status == IPStatus.Success)
            {
                this.htmlText = this.ghtml.Html(Address);

                if (this.htmlText.IndexOf(@"Кол-во значений", 0, StringComparison.Ordinal) != -1)
                {
                    this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Кол-во значений", StringComparison.Ordinal));
                    var startPos = this.text.IndexOf(@"_self", 0, StringComparison.Ordinal);

                    this.countStatus = int.Parse(this.text.Substring(18, this.text.IndexOf(@"</B>", 19, StringComparison.Ordinal) - 18));

                    while (i != this.countStatus)
                    {
                        this.status.Add(this.SubObj("_self", startPos));

                        startPos += 5;
                        startPos = this.text.IndexOf(@"_self", startPos, StringComparison.Ordinal);
                        i++;
                    }

                    this.StatusParse();
                }
            }
        }

        private string SubObj(string sub, int startPos)
        {
            var startNamePos = this.text.IndexOf(sub, startPos, StringComparison.Ordinal) + sub.Length + 2;
            var endNamePos = this.text.IndexOf(sub == "_self" ? @"</a>" : @"</TD>", startNamePos, StringComparison.Ordinal);

            return this.text.Substring(startNamePos, endNamePos - startNamePos);
        }

        private void StatusParse()
        {
            var pattern = @"(\w{2,}\s\w[.]\s|\w{2,}\s\w[.]\w[.])";
            var regex = new Regex(pattern);

            var pattern2 = @"(на больн\D+|в отпуске\D+|в команд\D+|не будет\D+|на обучен\D+|отсутствует\D+)";
            var regex2 = new Regex(pattern2);

            var pattern3 = @"(\d+\W\d+\D+\d+\W\d+[-]\d+\W\d+|\d+\W\d+[-]\d+\W\d+|\d+\W\d+)";
            var regex3 = new Regex(pattern3);

            var pattern4 = @"( \(и.о. \D+\))";
            var regex4 = new Regex(pattern4);

            var k = 0;

            foreach (var str in this.status)
            {
                var match2 = regex2.Match(this.status[k]);
                var match3 = regex3.Match(this.status[k]);
                var match4 = regex4.Match(this.status[k]);

                foreach (Match match in regex.Matches(this.status[k]))
                {
                    var st = match.Value.Remove(match.Value.LastIndexOf('.'));
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
            this.dbc.DatabaseCopy();
            this.dbc.StatusWrite(this.status);
        }

        private string ActStatus(string date)
        {
            var act = string.Empty;
            DateTime d1;

            switch (date.Length)
            {
                case 5:
                    d1 = DateTime.ParseExact(date, "dd.MM", null);

                    act = d1 <= DateTime.Today ? "1" : "0";

                    break;

                case 11:
                    var stD = date.Substring(0, 5);
                    var endD = date.Substring(6, 5);

                    d1 = DateTime.ParseExact(stD, "dd.MM", null);
                    var d2 = DateTime.ParseExact(endD, "dd.MM", null);

                    act = (d1 <= DateTime.Today && d2 >= DateTime.Today) ? "1" : "0";

                    break;
            }

            return act;
        }
    }
}