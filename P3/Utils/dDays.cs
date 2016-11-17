using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P3.Utils
{
    class dDays
    {
        DBconnect dbc = new DBconnect();
        GetHTML ghtml = new GetHTML();
        String address = "";
        String htmlText;
        String text;
        List<String> tIDVal = new List<string>();
        List<String> tBirthDay = new List<string>();
        List<String> tBirthID = new List<string>();
        List<String> tStartDay = new List<string>();
        String idTemp;

        public List<String> tID
        {
            get { return tIDVal; }
            set { tIDVal = value; }
        }

        public void ParseHTML()
        {

            Ping q = new Ping();

            try
            {
                PingReply an = q.Send("ares.elcom.local");
                if (an.Status == IPStatus.Success)
                {

                    for (Int16 i = 0; i < tIDVal.Count; i++)
                    {

                        idTemp = tIDVal[i];

                        address = "http://ares/Divisions/Lists/Employees/DispForm.aspx?ID=" + idTemp + "&Source=http%3A%2F%2Fares%2FDivisions%2FLists%2FEmployees%2FPhoneList%2Easpx";

                        GetPic();

                    }
                    dbc.EployeeBirthDayWrite(tBirthDay, tStartDay, tBirthID);

                }

            }
            catch { }

        }


        private void GetPic()
        {
            htmlText = ghtml.html(address);

            if (htmlText.IndexOf(@"Фото:", 19) != -1)
            {

                text = htmlText.Substring(htmlText.IndexOf(@"Фото:"));

                Int32 startP = text.IndexOf("SRC", 0);
                if (startP < 300 && startP != -1)
                {
                    startP += 5;

                    //День рождения
                    text = text.Substring(text.IndexOf("Дата рождения"));
                    Int32 startD = text.IndexOf("ms-formbody", 70);
                    tBirthDay.Add(text.Substring(startD + 13, 10));

                    //Дата прихода в Элком+
                    text = text.Substring(text.IndexOf("Дата прихода"));
                    startD = text.IndexOf("ms-formbody", 70);
                    tStartDay.Add(text.Substring(startD + 13, 10));

                    tBirthID.Add(idTemp);



                }


            }



        }
    }
}
