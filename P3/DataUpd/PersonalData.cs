namespace P3.DataUpd
{
    using System.Collections.Generic;
    using System.Net.NetworkInformation;

    using Model;
    using Utils;

    /// <summary>Данные сотрудников.</summary>
    public class PersonalData
    {
        private const string CheckSite = "ares.elcom.local";
        private DBconnect dbc = new DBconnect();
        private GetHTML ghtml = new GetHTML();
        private string address = string.Empty;
        private string htmlText;
        private string text;
        private List<Employee> employeeLst = new List<Employee>();
        private List<string> idValue = new List<string>();

        /// <summary>Initializes a new instance of the <see cref="PersonalData" /> class.</summary>
        /// <param name="idValue">Коллекция Id.</param>
        public PersonalData(List<string> idValue)
        {
            this.idValue = idValue;
        }

        /// <summary>Распарсить HTML.</summary>
        public void ParseHTML()
        {
            var q = new Ping();

            try
            {
                var an = q.Send(CheckSite);
                if (an.Status == IPStatus.Success)
                {
                    for (short i = 0; i < this.idValue.Count; i++)
                    {
                        this.address = "http://ares/Divisions/Lists/Employees/DispForm.aspx?ID=" + this.idValue[i] + "&Source=http%3A%2F%2Fares%2FDivisions%2FLists%2FEmployees%2FPhoneList%2Easpx";

                        this.ExecValues(this.idValue[i]);
                    }

                    try
                    {
                        this.dbc.ClearTable("employee");
                    }
                    catch
                    {
                    }

                    this.dbc.EmployeeWrite2(this.employeeLst);
                }
            }
            catch
            {
            }
        }

        /// <summary>Извлечь значения.</summary>
        /// <param name="id">ID.</param>
        private void ExecValues(string id)
        {
            this.htmlText = this.ghtml.Html(this.address);
            var emplTemp = new Employee();

            if (this.htmlText.IndexOf(@"Фото:", 19) != -1)
            {
                this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Фото:"));

                    emplTemp.ID = id;

                // ФИО
                this.text = this.text.Substring(this.text.IndexOf("ФИО"));
                var startD = this.text.IndexOf("ms-formbody", 110);

                    emplTemp.FullName = this.SubObj(@"ms-formbody", 110, "</td");

                // Подразделение
                this.text = this.text.Substring(this.text.IndexOf("Подразделение"));
                    startD = this.text.IndexOf("ID", 10);

                    string idTemp;
                    idTemp = this.text.Substring(startD + 3, 3);
                    if (idTemp.IndexOf("\"", 0) != -1)
                    {
                        idTemp = this.text.Substring(startD + 3, 2);
                    }

                    if (idTemp.IndexOf("\"", 0) != -1)
                    {
                        idTemp = this.text.Substring(startD + 3, 1);
                    }

                var divTemp = this.SubObj(idTemp, 160, "</A");
                    emplTemp.Division = this.ReplaceStr(divTemp, @"&quot;", string.Empty);

                // День рождения
                this.text = this.text.Substring(this.text.IndexOf("Дата рождения"));
                    emplTemp.BirthDayShort = this.SubObj("ms-formbody", 70, "</td");

                // Должность
                this.text = this.text.Substring(this.text.IndexOf("Должность"));
                    emplTemp.Position = this.SubObj("ms-formbody", 70, "</td");

                // Email
                this.text = this.text.Substring(this.text.IndexOf("Email"));
                    var temp = this.SubObj("mailt", 70, "\">");
                    if (temp.IndexOf("</TH", 0) != -1)
                    {
                        emplTemp.Email = string.Empty;
                    }
                    else
                    {
                        emplTemp.Email = temp;
                    }

                // Внутр тел
                this.text = this.text.Substring(this.text.IndexOf("Внутр. тлф"));

                    var phoneTemp = this.SubObj("ms-formbody", 70, "</td");
                    emplTemp.PhoneWork = this.ReplaceStr(phoneTemp, @"&nbsp;", string.Empty);

                // Сотовый тел
                this.text = this.text.Substring(this.text.IndexOf("Сотовый тлф"));
                var phoneMobileTemp = this.SubObj("ms-formbody", 70, "</td");
                    emplTemp.PhoneMobile = this.ReplaceStr(phoneMobileTemp, @"&nbsp;", string.Empty);

                // Переадрес тел
                this.text = this.text.Substring(this.text.IndexOf("Переадр.на"));
                    emplTemp.PhoneExch = this.SubObj("ms-formbody", 70, "</td");

                // Дата прихода в Элком+
                this.text = this.text.Substring(this.text.IndexOf("Дата прихода"));

                    emplTemp.StartDayShort = this.SubObj("ms-formbody", 70, "</td");

                this.employeeLst.Add(emplTemp);
            }
        }

        private string ReplaceStr(string text, string oldValue, string newValue)
        {
            while (text.IndexOf(oldValue, 0) != -1)
            {
                text = text.Replace(oldValue, newValue);
            }           

            return text;
        }

        private string SubObj(string sub, int startPos, string endText)
        {
            var len = sub.Length + 2;
            string sub2;

            if (sub == "_self")
            {
                sub2 = @"</a>";
            }
            else
            {
                sub2 = endText;
            }

            int startNamePos = this.text.IndexOf(sub, startPos) + len;

            int endNamePos = this.text.IndexOf(sub2, startNamePos);

            return this.text.Substring(startNamePos, endNamePos - startNamePos);
        }
    }
}