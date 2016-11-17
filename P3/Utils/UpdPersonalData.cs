using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P3.Utils
{
    class UpdPersonalData
    {
        DBconnect dbc = new DBconnect();
        GetHTML ghtml = new GetHTML();
        String address = "http://ares/Divisions/Lists/Employees/PhoneList.aspx";
        //String address = "http://test2.ru/index.html";
        String htmlText;
        String text;


        List<String> tID = new List<string>();
        List<String> tNames = new List<string>();
        List<String> tTels = new List<string>();
        List<String> tTels2 = new List<string>();
        List<String> tTels3 = new List<string>();
        List<String> tEmail = new List<string>();
        List<String> tDiv = new List<string>();
        List<String> tPos = new List<string>();
        List<String> tStatus = new List<string>();



        public void ParseHTML()
        {
            //Int16 i = 0;
            //Int32 startPos = 0;

            Ping q = new Ping();

            try
            {
                //PingReply an = q.Send("ares.elcom.local");
                //if (an.Status == IPStatus.Success)
                //{
                htmlText = ghtml.html(address);




                if (htmlText.IndexOf(@"Кол-во значений", 19) != -1)
                {

                    text = htmlText.Substring(htmlText.IndexOf(@"Кол-во значений"));
                    text = text.Remove(text.LastIndexOf(@"<!-- FooterBanner closes the TD") - 39);



                    execValues();



                }

                //}

            }
            catch { }

        }


        public void execValues()
        {
            tID.Clear();
            tNames.Clear();
            tTels.Clear();
            tTels2.Clear();
            tTels3.Clear();
            tEmail.Clear();
            tDiv.Clear();
            tPos.Clear();

            Int32 countWorcers;
            Int32 i = 0;

            String id;
            String name1;
            String t1;
            String t2;
            String t3;
            String email1;
            String division1;
            String position1;

            //int startPos = text.IndexOf(@"_self", 0);
            Int32 startPos = text.IndexOf(@"ID", 0);

            countWorcers = Int32.Parse(text.Substring(18, text.IndexOf(@"</B>", 19) - 18));

            while (i != countWorcers)
            {
                //id
                id = text.Substring(startPos + 3, 3);
                if (id.IndexOf("\"", 0) != -1)
                {
                    id = text.Substring(startPos + 3, 2);
                }

                tID.Add(id);

                //Имя
                name1 = subObj(@"_self", startPos);
                tNames.Add(name1);

                //Внутренний телефон
                startPos = text.IndexOf(@"ms-vb2", startPos);
                t1 = subObj(@"ms-vb2", startPos);
                startPos += 5;

                //Мобильный телефон
                startPos = text.IndexOf(@"ms-vb2", startPos);
                t2 = subObj(@"ms-vb2", startPos);
                startPos += 5;

                //Переадресация на сотовый
                startPos = text.IndexOf(@"ms-vb2", startPos);
                t3 = subObj(@"ms-vb2", startPos);
                startPos += 5;

                //e-mail
                startPos = text.IndexOf(@"ms-vb2", startPos);
                startPos += 8;
                Int32 checkEnd = text.IndexOf(@"</TD><TD", startPos);
                if (checkEnd - startPos > 5)
                {
                    startPos = text.IndexOf(@">", startPos) + 1;
                    email1 = text.Substring(startPos, text.IndexOf(@"</a></TD>", startPos) - startPos);
                }
                else
                { email1 = ""; }
                startPos += 5;
                tEmail.Add(email1);

                //Подразделение
                startPos = text.IndexOf(@"ms-vb2", startPos);
                startPos += 8;
                startPos = text.IndexOf(@">", startPos) + 1;
                division1 = text.Substring(startPos, text.IndexOf(@"</A></TD>", startPos) - startPos);
                startPos += 5;
                tDiv.Add(division1);

                //Должность
                startPos = text.IndexOf(@"ms-vb2", startPos);
                position1 = subObj(@"ms-vb2", startPos);
                tPos.Add(position1);
                startPos += 5;

                startPos = text.IndexOf(@"ID", startPos);
                t1 = spaceReplase(t1);
                tTels.Add(t1);
                t2 = spaceReplase(t2);
                tTels2.Add(t2);
                t3 = spaceReplase(t3);
                tTels3.Add(t3);

                i++;
            }

            try
            {
                dbc.ClearTable("employee");
            }
            catch { }
            dbc.EmployeeWrite(tID, tNames, tTels, tTels2, tTels3, tEmail, tDiv, tPos);


        }

        public String spaceReplase(String str)
        {
            if (str.IndexOf(@"&nbsp;", 0) != -1)
            {
                while (str.IndexOf(@"&nbsp;", 0) != -1)
                { str = str.Replace(@"&nbsp;", ""); }
            }
            return str;
        }


        public String subObj(String sub, Int32 startPos)
        {
            Int32 len = sub.Length + 2;
            String sub2;

            if (sub == "_self")
            { sub2 = @"</a>"; }
            else
            { sub2 = @"</TD>"; }

            Int32 startNamePos = text.IndexOf(sub, startPos) + len;


            Int32 endNamePos = text.IndexOf(sub2, startNamePos);

            String subObj = text.Substring(startNamePos, endNamePos - startNamePos);

            return subObj;
        }
    }
}
