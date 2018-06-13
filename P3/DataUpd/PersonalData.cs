namespace P3.DataUpd
{
    using System;
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
            var ping = new Ping();

                var pingReply = ping.Send(CheckSite);
                if (pingReply != null && pingReply.Status == IPStatus.Success)
                {
                    for (short i = 0; i < this.idValue.Count; i++)
                    {
                        this.address = "http://ares/Divisions/Lists/Employees/DispForm.aspx?ID=" + this.idValue[i] + "&Source=http%3A%2F%2Fares%2FDivisions%2FLists%2FEmployees%2FPhoneList%2Easpx";

                        this.ExecValues(this.idValue[i]);
                    }

                    this.dbc.ClearTable("employee");
                    this.dbc.EmployeeWrite(this.employeeLst);
                    this.dbc.DatabaseCopy();
                }
        }

        /// <summary>Извлечь значения.</summary>
        /// <param name="id">ID.</param>
        private void ExecValues(string id)
        {
            this.htmlText = this.ghtml.Html(this.address);
            var emplTemp = new Employee();

            if (this.htmlText.IndexOf(@"Фото:", 19, StringComparison.Ordinal) != -1)
            {
                this.text = this.htmlText.Substring(this.htmlText.IndexOf(@"Фото:", StringComparison.Ordinal));
                emplTemp.ID = id;

                // ФИО
                this.text = this.text.Substring(this.text.IndexOf("ФИО", StringComparison.Ordinal));
                var startD = this.text.IndexOf("ms-formbody", 110, StringComparison.Ordinal);
                emplTemp.FullName = this.SubObj(@"ms-formbody", 110, "</td");

                // Подразделение
                this.text = this.text.Substring(this.text.IndexOf("Подразделение", StringComparison.Ordinal));
                startD = this.text.IndexOf("ID", 10, StringComparison.Ordinal);

                var idTemp = this.text.Substring(startD + 3, 3);
                if (idTemp.IndexOf("\"", 0, StringComparison.Ordinal) != -1)
                {
                    idTemp = this.text.Substring(startD + 3, 2);
                }

                if (idTemp.IndexOf("\"", 0, StringComparison.Ordinal) != -1)
                {
                    idTemp = this.text.Substring(startD + 3, 1);
                }

                var divTemp = this.SubObj(idTemp, 160, "</A");
                emplTemp.Division = this.ReplaceStr(divTemp, @"&quot;", string.Empty);

                // День рождения
                this.text = this.text.Substring(this.text.IndexOf("Дата рождения", StringComparison.Ordinal));
                emplTemp.BirthDayShort = this.SubObj("ms-formbody", 70, "</td");

                // Должность
                this.text = this.text.Substring(this.text.IndexOf("Должность", StringComparison.Ordinal));
                emplTemp.Position = this.SubObj("ms-formbody", 70, "</td");

                // Email
                this.text = this.text.Substring(this.text.IndexOf("Email", StringComparison.Ordinal));
                var temp = this.SubObj("mailt", 70, "\">");
                emplTemp.Email = temp.IndexOf("</TH", 0, StringComparison.Ordinal) != -1 ? string.Empty : temp;

                // Внутр тел
                this.text = this.text.Substring(this.text.IndexOf("Внутр. тлф", StringComparison.Ordinal));

                var phoneTemp = this.SubObj("ms-formbody", 70, "</td");
                emplTemp.PhoneWork = this.ReplaceStr(phoneTemp, @"&nbsp;", string.Empty);

                // Сотовый тел
                this.text = this.text.Substring(this.text.IndexOf("Сотовый тлф", StringComparison.Ordinal));
                var phoneMobileTemp = this.SubObj("ms-formbody", 70, "</td");
                emplTemp.PhoneMobile = this.ReplaceStr(phoneMobileTemp, @"&nbsp;", string.Empty);

                // Переадрес тел
                this.text = this.text.Substring(this.text.IndexOf("Переадр.на", StringComparison.Ordinal));
                emplTemp.PhoneExch = this.SubObj("ms-formbody", 70, "</td");

                // Дата прихода в Элком+
                this.text = this.text.Substring(this.text.IndexOf("Дата прихода", StringComparison.Ordinal));
                emplTemp.StartDayShort = this.SubObj("ms-formbody", 70, "</td");

                this.employeeLst.Add(emplTemp);
            }
        }

        private string ReplaceStr(string textRep, string oldValue, string newValue)
        {
            while (textRep.IndexOf(oldValue, 0, StringComparison.Ordinal) != -1)
            {
                textRep = textRep.Replace(oldValue, newValue);
            }           

            return textRep;
        }

        private string SubObj(string sub, int startPos, string endText)
        {
            var startNamePos = this.text.IndexOf(sub, startPos, StringComparison.Ordinal) + sub.Length + 2;
            var endNamePos = this.text.IndexOf(sub == "_self" ? @"</a>" : endText, startNamePos, StringComparison.Ordinal);

            return this.text.Substring(startNamePos, endNamePos - startNamePos);
        }
    }
}