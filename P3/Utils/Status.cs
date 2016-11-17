using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P3.Utils
{
    class Status
    {
        DBconnect dbc = new DBconnect();
        GetHTML ghtml = new GetHTML();
        String address = "http://ares/Lists/List2/AllItems.aspx";
        String htmlText;
        String text;
        Int32 countStatus;
        List<String> status = new List<string>();
        List<String> stName = new List<string>();
        List<String> stState = new List<string>();
        List<String> stDate = new List<string>();
        List<String> stID = new List<string>();
        List<String> stAct = new List<string>();


        //public Status(WebBrowser webBrowser1)
        //{
        //    // TODO: Complete member initialization
        //    this.webBrowser1 = webBrowser1;
        //}



        public void ParseHTML()
        {
            Int16 i = 0;
            Int32 startPos = 0;

            Ping q = new Ping();

            try
            {
                PingReply an = q.Send("ares.elcom.local");
                if (an.Status == IPStatus.Success)
                {
                    htmlText = ghtml.html(address);

                    if (htmlText.IndexOf(@"Кол-во значений", 0) != -1)
                    {
                        text = htmlText.Substring(htmlText.IndexOf(@"Кол-во значений"));
                        startPos = text.IndexOf(@"_self", 0);

                        countStatus = int.Parse(text.Substring(18, text.IndexOf(@"</B>", 19) - 18));

                        while (i != countStatus)
                        {
                            status.Add(subObj("_self", startPos));

                            startPos += 5;
                            startPos = text.IndexOf(@"_self", startPos);
                            i++;
                        }

                        StatusParse();
                    }

                }
            }
            catch { }


        }

        public String subObj(String sub, Int32 startPos)
        {
            Int32 len = sub.Length + 2;
            string sub2;

            if (sub == "_self")
            { sub2 = @"</a>"; }
            else
            { sub2 = @"</TD>"; }

            Int32 startNamePos = text.IndexOf(sub, startPos) + len;


            Int32 endNamePos = text.IndexOf(sub2, startNamePos);

            String subObj = text.Substring(startNamePos, endNamePos - startNamePos);

            return subObj;
        }

        public void StatusParse()
        {
            String st = "";
            //status1 = dbc.EmployeeReadStatus();

            String pattern = @"(\w{2,}\s\w[.]\s|\w{2,}\s\w[.]\w[.])";
            Regex regex = new Regex(pattern);

            String pattern2 = @"(на больн\D+|в отпуске\D+|в команд\D+|не будет\D+|на обучен\D+)";
            Regex regex2 = new Regex(pattern2);

            String pattern3 = @"(\d+\W\d+\D+\d+\W\d+[-]\d+\W\d+|\d+\W\d+[-]\d+\W\d+|\d+\W\d+)";
            Regex regex3 = new Regex(pattern3);

            String pattern4 = @"( \(и.о. \D+\))";
            Regex regex4 = new Regex(pattern4);

            int k = 0;

            foreach (String str in status)
            {
                Match match2 = regex2.Match(status[k]);
                Match match3 = regex3.Match(status[k]);
                Match match4 = regex4.Match(status[k]);

                foreach (Match match in regex.Matches(status[k]))
                {
                    st = match.Value.Remove(match.Value.LastIndexOf('.'));
                    if (st.LastIndexOf('.') != -1)
                        st = st.Remove(st.LastIndexOf('.'));

                    stName.Add(st);
                    stState.Add(match2.Value + match3.Value + match4.Value);
                    stDate.Add(match3.Value);
                    stAct.Add(ActStatus(match3.Value));
                }
                k++;
            }
            dbc.EployeeStatusWrite(stState, stName, stAct);
            dbc.StatusWrite(status);
        }

        public String ActStatus(String date)
        {
            String act = "";
            String stD = "";
            String endD = "";
            DateTime d1;
            DateTime d2;

            String pattern3 = @"(\d+\W\d+)";
            Regex regex3 = new Regex(pattern3);

            Match match3 = regex3.Match(date);


            switch (date.Length)
            {
                case 5:
                    d1 = DateTime.ParseExact(date, "dd.MM", null);

                    if (d1 <= DateTime.Today)
                        act = "1";
                    else
                        act = "0";

                    break;

                case 11:
                    stD = date.Substring(0, 5);
                    endD = date.Substring(6, 5);

                    d1 = DateTime.ParseExact(stD, "dd.MM", null);
                    d2 = DateTime.ParseExact(endD, "dd.MM", null);

                    if (d1 <= DateTime.Today && d2 >= DateTime.Today)
                        act = "1";
                    else
                        act = "0";


                    break;
            }
            return act;

        }
    }
}
