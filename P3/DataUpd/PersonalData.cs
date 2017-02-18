using P3.Model;
using P3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace P3.DataUpd
{
    class PersonalData
    {
        DBconnect dbc = new DBconnect();
        GetHTML ghtml = new GetHTML();
        String address = "";
        String htmlText;
        String text;
        List<Employee> employeeLst = new List<Employee>();

        List<String> _tIDVal = new List<string>();
        //List<String> tNames = new List<string>();
        //List<String> tTels = new List<string>();
        //List<String> tTels2 = new List<string>();
        //List<String> tTels3 = new List<string>();
        //List<String> tEmail = new List<string>();
        //List<String> tDiv = new List<string>();
        //List<String> tPos = new List<string>();
        List<String> tBirthDay = new List<string>();
        List<String> tBirthID = new List<string>();
        List<String> tStartDay = new List<string>();
        String idTemp;

        //public List<String> tID
        //{
        //    get { return tIDVal; }
        //    set { tIDVal = value; }
        //}

        public PersonalData(List<String> tIDVal)
        {
            _tIDVal = tIDVal;
        }

        public void ParseHTML()
        {

            Ping q = new Ping();

            try
            {
                PingReply an = q.Send("ares.elcom.local");
                if (an.Status == IPStatus.Success)
                {

                    for (Int16 i = 0; i < _tIDVal.Count; i++)
                    {

                        //idTemp = _tIDVal[i];

                        address = "http://ares/Divisions/Lists/Employees/DispForm.aspx?ID=" + _tIDVal[i] + "&Source=http%3A%2F%2Fares%2FDivisions%2FLists%2FEmployees%2FPhoneList%2Easpx";

                        execValues(_tIDVal[i]);

                    }

                    try
                    {
                        dbc.ClearTable("employee");
                    }
                    catch { }

                    dbc.EmployeeWrite2(employeeLst);

                    //dbc.EployeeBirthDayWrite(tBirthDay, tStartDay, tBirthID);

                }

            }
            catch { }

        }


        private void execValues(String id)
        {
            htmlText = ghtml.html(address);
            Employee emplTemp = new Employee();

            if (htmlText.IndexOf(@"Фото:", 19) != -1)
            {







                text = htmlText.Substring(htmlText.IndexOf(@"Фото:"));

                //Int32 startP = text.IndexOf("SRC", 0);
                //if (startP < 300 && startP != -1)
                //{
                    //startP += 5;
                    emplTemp.ID = id;
                    //ФИО
                    text = text.Substring(text.IndexOf("ФИО"));
                    Int32 startD = text.IndexOf("ms-formbody", 110);

                    emplTemp.FullName = subObj(@"ms-formbody", 110, "</td");

                    //Подразделение
                    text = text.Substring(text.IndexOf("Подразделение"));
                    startD = text.IndexOf("ID", 10);
                    //startD = text.IndexOf(@">", 10);
                    String idTemp;
                    idTemp = text.Substring(startD + 3, 3);
                    if (idTemp.IndexOf("\"", 0) != -1)
                    {
                        idTemp = text.Substring(startD + 3, 2);
                    }
                    if (idTemp.IndexOf("\"", 0) != -1)
                    {
                        idTemp = text.Substring(startD + 3, 1);
                    }
                    String divTemp = subObj(idTemp, 160, "</A");
                    emplTemp.Division = ReplaceStr(divTemp, @"&quot;", "");

                    //День рождения
                    text = text.Substring(text.IndexOf("Дата рождения"));
                    //startD = text.IndexOf("ms-formbody", 70);
                    emplTemp.BirthDayShort = subObj("ms-formbody", 70, "</td");
                    //tBirthDay.Add(text.Substring(startD + 13, 10));

                    //Должность
                    text = text.Substring(text.IndexOf("Должность"));
                    emplTemp.Position = subObj("ms-formbody", 70, "</td");

                    //Email
                    text = text.Substring(text.IndexOf("Email"));
                    String temp = subObj("mailt", 70, "\">");
                    if (temp.IndexOf("</TH", 0) != -1)
                    {
                        emplTemp.Email = "";
                    }
                    else
                    {
                        emplTemp.Email = temp;
                    }

                    //Внутр тел
                    text = text.Substring(text.IndexOf("Внутр. тлф"));

                    String phoneTemp = subObj("ms-formbody", 70, "</td");
                    emplTemp.PhoneWork = ReplaceStr(phoneTemp, @"&nbsp;", "");


                    //emplTemp.PhoneWork = subObj("ms-formbody", 70, "</td");

                    //Сотовый тел
                    text = text.Substring(text.IndexOf("Сотовый тлф"));
                    String phoneMobileTemp = subObj("ms-formbody", 70, "</td");
                    emplTemp.PhoneMobile = ReplaceStr(phoneMobileTemp, @"&nbsp;", "");

                    //Переадрес тел
                    text = text.Substring(text.IndexOf("Переадр.на"));
                    emplTemp.PhoneExch = subObj("ms-formbody", 70, "</td");
                    
                    //Дата прихода в Элком+
                    text = text.Substring(text.IndexOf("Дата прихода"));
                    //startD = text.IndexOf("ms-formbody", 70);
                    //tStartDay.Add(text.Substring(startD + 13, 10));
                    emplTemp.StartDayShort = subObj("ms-formbody", 70, "</td");


                    employeeLst.Add(emplTemp);
                    //tBirthID.Add(idTemp);



                //}


            }



        }

        public String ReplaceStr(String text, String oldValue, String newValue)
        {
            while (text.IndexOf(oldValue, 0) != -1)
            {
                text = text.Replace(oldValue, newValue);
            }           

            return text;

        }

        public String subObj(String sub, Int32 startPos, String endText)
        {
            Int32 len = sub.Length + 2;
            String sub2;

            if (sub == "_self")
            { sub2 = @"</a>"; }
            else
            { sub2 = endText; }

            //@"</td>"

            Int32 startNamePos = text.IndexOf(sub, startPos) + len;


            Int32 endNamePos = text.IndexOf(sub2, startNamePos);

            String subObj = text.Substring(startNamePos, endNamePos - startNamePos);

            return subObj;
        }


    }
}
