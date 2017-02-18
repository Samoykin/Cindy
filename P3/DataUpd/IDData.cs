using P3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P3.DataUpd
{
    class IDData
    {
        DBconnect dbc = new DBconnect();
        GetHTML ghtml = new GetHTML();
        String address = "http://ares/Divisions/Lists/Employees/PhoneList.aspx";        
        String htmlText;
        String text;
        List<String> tID = new List<String>();
        public Boolean flag = false;

        public async void ParseHTML()
        {

            await Task.Factory.StartNew(() =>
            {
                ConnCheck();
            });
            //ConnCheck();

            //return flag;            
        }

        public void ConnCheck()
        {
            Ping q = new Ping();

            try
            {
                PingReply an = q.Send("ares.elcom.local");
                if (an.Status == IPStatus.Success)
                {
                    htmlText = ghtml.html(address);

                    if (htmlText.IndexOf(@"Кол-во значений", 19) != -1)
                    {

                        text = htmlText.Substring(htmlText.IndexOf(@"Кол-во значений"));
                        text = text.Remove(text.LastIndexOf(@"<!-- FooterBanner closes the TD") - 39);

                        execValues();
                    }

                }
                else
                {
                    flag = false;
                }

            }
            catch { }
        }


        public void execValues()
        {
            tID.Clear();

            Int32 countWorcers;
            Int32 i = 0;

            String id;

            Int32 startPos = text.IndexOf(@"ID", 0);

            countWorcers = Int32.Parse(text.Substring(18, text.IndexOf(@"</B>", 19) - 18));

            while (i != countWorcers)
            {
                id = text.Substring(startPos + 3, 3);
                if (id.IndexOf("\"", 0) != -1)
                {
                    id = text.Substring(startPos + 3, 2);
                }

                tID.Add(id);

                startPos = text.IndexOf(@"A HREF", startPos);
                startPos = text.IndexOf(@"ID", startPos+64);

                i++;
            }

            PersonalData pd = new PersonalData(tID);
            pd.ParseHTML();

            Status stEmpl2 = new Status(); // статусы
            stEmpl2.ParseHTML();

            dPic dlP = new dPic(); //фотки
            dlP.tID = tID;
            dlP.ParseHTML();

            flag = true;

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


        
    }
}
