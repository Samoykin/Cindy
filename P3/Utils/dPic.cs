using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P3.Utils
{
    class dPic
    {
        DBconnect dbc = new DBconnect();
        GetHTML ghtml = new GetHTML();
        String address = "";
        String htmlText;
        String text;
        List<String> tIDVal = new List<string>();

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
                        if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "//img//" + idTemp + ".jpg"))
                        {
                            address = "http://ares/Divisions/Lists/Employees/DispForm.aspx?ID=" + idTemp + "&Source=http%3A%2F%2Fares%2FDivisions%2FLists%2FEmployees%2FPhoneList%2Easpx";

                            GetPic();
                        }
                    }


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
                    Int32 endP = text.IndexOf("\" ALT", 0);
                    String subObj = text.Substring(startP, endP - startP);

                    savePic(subObj);


                }


            }



        }

        private void savePic(string source)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;

            wc.DownloadFile(source, AppDomain.CurrentDomain.BaseDirectory + "//img//" + idTemp + ".jpg");
        }


    }
}
