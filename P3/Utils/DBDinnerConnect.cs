using P3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace P3
{
    class DBDinnerConnect
    {
        public String DataBaseName = "Dinners.sqlite";
        public String DataBase2Name = "DBTelsTemp.sqlite";
        //String DataBaseName2 = @"\\elcom.local\files\01-Deps\ДПАСУТП\01_Архив\!Common_ОРДС\Phonebook\DBTels.sqlite";
        //String pass = "Xt,ehfirf3";
        String pass = "";

        public ObservableCollection<Employee> employeeLst { get; set; }
        public ObservableCollection<Customer> customerLst { get; set; }

        public ObservableCollection<DinnerList> dinnerList { get; set; }

        //m_dbConnection.SetPassword("password");
        //m_dbConnection.ChangePassword("aaa");

        List<String> tIDVal = new List<string>();
        List<String> tNamesVal = new List<string>();
        List<String> tTelsVal = new List<string>();
        List<String> tTels2Val = new List<string>();
        List<String> tTels3Val = new List<string>();
        List<String> tEmailVal = new List<string>();
        List<String> tDivVal = new List<string>();
        List<String> tPosVal = new List<string>();
        List<String> tStatusVal = new List<string>();
        List<DateTime> tBirthDayVal = new List<DateTime>();

        public List<String> tID
        {
            get { return tIDVal; }
            set { tIDVal = value; }
        }

        public List<String> tNames
        {
            get { return tNamesVal; }
            set { tNamesVal = value; }
        }

        public List<String> tTels
        {
            get { return tTelsVal; }
            set { tTelsVal = value; }
        }

        public List<String> tTels2
        {
            get { return tTels2Val; }
            set { tTels2Val = value; }
        }

        public List<String> tTels3
        {
            get { return tTels3Val; }
            set { tTels3Val = value; }
        }

        public List<String> tEmail
        {
            get { return tEmailVal; }
            set { tEmailVal = value; }
        }

        public List<String> tDiv
        {
            get { return tDivVal; }
            set { tDivVal = value; }
        }

        public List<String> tPos
        {
            get { return tPosVal; }
            set { tPosVal = value; }
        }

        public List<String> tStatus
        {
            get { return tStatusVal; }
            set { tStatusVal = value; }
        }

        public List<DateTime> tBirthDay
        {
            get { return tBirthDayVal; }
            set { tBirthDayVal = value; }
        }



        List<String> tVacationVal = new List<string>();
        List<String> tSickVal = new List<string>();
        List<String> tBTripVal = new List<string>();
        List<String> tOtherVal = new List<string>();

        public List<String> tVacation
        {
            get { return tVacationVal; }
            set { tVacationVal = value; }
        }

        public List<String> tSick
        {
            get { return tSickVal; }
            set { tSickVal = value; }
        }

        public List<String> tBTrip
        {
            get { return tBTripVal; }
            set { tBTripVal = value; }
        }

        public List<String> tOther
        {
            get { return tOtherVal; }
            set { tOtherVal = value; }
        }

        //public List<String> tVacation { get; set; }
        //public List<String> tSick { get; set; }
        //public List<String> tBTrip { get; set; }

        public void CreateBase()
        {
            if (!File.Exists(DataBaseName))
            {
                SQLiteConnection.CreateFile(DataBaseName);
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=DBTels.sqlite;Version=3;");
                m_dbConnection.SetPassword(pass);

            }

        }

        public void EmployeeCreateTable()
        {
            String Command = "CREATE TABLE employee (id INTEGER PRIMARY KEY UNIQUE, tID VARCHAR, tNames VARCHAR, tTels VARCHAR, tTels2 VARCHAR, tTels3 VARCHAR, tEmail VARCHAR, tDiv VARCHAR, tPos VARCHAR, tStatus VARCHAR, tActStatus VARCHAR, tBirthDay VARCHAR, tStartDay VARCHAR);";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        public void CustomerCreateTable()
        {
            String Command = "CREATE TABLE customer (id INTEGER PRIMARY KEY UNIQUE, custNames VARCHAR, custPos VARCHAR, custTels VARCHAR, custTels2 VARCHAR, custEmail VARCHAR, custComp VARCHAR);";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        public void InfoCreateTable()
        {
            String Command = "CREATE TABLE info (id INTEGER PRIMARY KEY UNIQUE, date VARCHAR, name VARCHAR, pcName VARCHAR, ip VARCHAR, remoteBD VARCHAR);";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        public void StatusCreateTable()
        {
            String Command = "CREATE TABLE status (id INTEGER PRIMARY KEY UNIQUE, statusVal VARCHAR);";
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        public void EmployeeWrite(List<String> tID, List<String> tNames, List<String> tTels, List<String> tTels2, List<String> tTels3, List<String> tEmail, List<String> tDiv, List<String> tPos)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));

            connection.Open();
            SQLiteCommand command;

            for (int i = 0; i < tNames.Count(); i++)
            {
                command = new SQLiteCommand("INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos') VALUES ('" + tID[i] + "', '" + tNames[i] + "', '" + tTels[i] + "', '" + tTels2[i] + "', '" + tTels3[i] + "', '" + tEmail[i] + "', '" + tDiv[i] + "', '" + tPos[i] + "');", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            File.Copy(DataBase2Name, DataBaseName, true);
        }

        public void EmployeeWrite2(List<Employee> employeeLst)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));

            connection.Open();
            SQLiteCommand command;

            for (int i = 0; i < employeeLst.Count(); i++)
            {
                command = new SQLiteCommand("INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos', 'tBirthDay', 'tStartDay') VALUES ('" + employeeLst[i].ID + "', '" + employeeLst[i].FullName + "', '" + employeeLst[i].PhoneWork + "', '" + employeeLst[i].PhoneMobile + "', '" + employeeLst[i].PhoneExch + "', '" + employeeLst[i].Email + "', '" + employeeLst[i].Division + "', '" + employeeLst[i].Position + "', '" + employeeLst[i].BirthDayShort + "', '" + employeeLst[i].StartDayShort + "');", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            File.Copy(DataBase2Name, DataBaseName, true);
        }

        public void CustomerWrite(List<String> custNames, List<String> custPos, List<String> custTels, List<String> custTels2, List<String> custEmail, List<String> custComp)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));
            connection.Open();
            SQLiteCommand command;

            for (int i = 0; i < custNames.Count(); i++)
            {
                command = new SQLiteCommand("INSERT INTO 'customer' ('custNames', 'custPos', 'custTels', 'custTels2', 'custEmail', 'custComp') VALUES ('" + custNames[i] + "', '" + custPos[i] + "', '" + custTels[i] + "', '" + custTels2[i] + "', '" + custEmail[i] + "', '" + custComp[i] + "');", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }




        public void InfoWrite(DateTime date, String name, String pcName, String ip, String remoteBD)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));
            connection.Open();
            SQLiteCommand command;

            command = new SQLiteCommand("INSERT INTO 'info' ('date', 'name', 'pcName', 'ip', 'remoteBD') VALUES ('" + date + "', '" + name + "', '" + pcName + "', '" + ip + "', '" + remoteBD + "');", connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void StatusWrite(List<String> status)
        {

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));
            connection.Open();
            SQLiteCommand command;

            command = new SQLiteCommand("DELETE FROM 'status';", connection);
            command.ExecuteNonQuery();

            for (int i = 0; i < status.Count(); i++)
            {
                command = new SQLiteCommand("INSERT INTO 'status' ('statusVal') VALUES ('" + status[i] + "');", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void EployeeStatusWrite(List<String> status, List<String> names, List<String> act)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));
            connection.Open();
            SQLiteCommand command;

            command = new SQLiteCommand("UPDATE employee SET tStatus = '';", connection);
            command.ExecuteNonQuery();


            for (int i = 0; i < status.Count(); i++)
            {
                command = new SQLiteCommand("UPDATE employee SET tStatus = '" + status[i] + "', tActStatus = '" + act[i] + "' WHERE tNames Like '" + names[i] + "%';", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void EployeeBirthDayWrite(List<String> birthDay, List<String> startDay, List<String> birthDayID)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));
            connection.Open();
            SQLiteCommand command;

            //command = new SQLiteCommand("UPDATE employee SET tBirthDay = '';", connection);
            //command.ExecuteNonQuery();


            for (int i = 0; i < birthDayID.Count(); i++)
            {
                command = new SQLiteCommand("UPDATE employee SET tBirthDay = '" + birthDay[i] + "' WHERE tID = '" + birthDayID[i] + "';", connection);
                command.ExecuteNonQuery();

                command = new SQLiteCommand("UPDATE employee SET tStartDay = '" + startDay[i] + "' WHERE tID = '" + birthDayID[i] + "';", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public ObservableCollection<Employee> EmployeeRead()
        {
            Employee empl;
            employeeLst = new ObservableCollection<Employee> { };

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'employee';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                var format = "dd.MM.yyyy";
                String dtime;
                String dtime2;
                DateTime dateValue;
                
                String tBirthDayTemp = record["tBirthDay"].ToString();
                String tStartDayTemp = record["tStartDay"].ToString();
                
                //День рождения
                if (tBirthDayTemp.Length > 0 && DateTime.TryParse(tBirthDayTemp,out dateValue))
                {
                    dtime = tBirthDayTemp;
                    //DateTime.ParseExact(tBirthDayTemp, format, CultureInfo.CurrentCulture)
                }
                else
                {
                    dtime = "20.12.1994";
                }

                //Дата прихода
                if (tStartDayTemp.Length > 0 && DateTime.TryParse(tStartDayTemp, out dateValue))
                {
                    dtime2 = tStartDayTemp;
                    //DateTime.ParseExact(tStartDayTemp, format, CultureInfo.CurrentCulture)
                }
                else
                {
                    dtime2 = "20.12.1994";
                }

                DateTime tdTemp = DateTime.Now;

                empl = new Employee();

                empl.FullName = record["tNames"].ToString();
                empl.PhoneWork = record["tTels"].ToString();
                empl.PhoneMobile = record["tTels2"].ToString();
                empl.PhoneExch = record["tTels3"].ToString();
                empl.Email = record["tEmail"].ToString();
                empl.Division = record["tDiv"].ToString();
                empl.Position = record["tPos"].ToString();
                empl.ID = record["tID"].ToString();
                empl.Status = record["tStatus"].ToString();
                empl.BirthDay = Convert.ToDateTime(dtime);
                empl.Age = tdTemp.Year - empl.BirthDay.Year;

                String monthTemp = empl.BirthDay.Month.ToString();
                String dayTemp = empl.BirthDay.Day.ToString(); 
                if (empl.BirthDay.Month < 10)
                    monthTemp = "0" + empl.BirthDay.Month.ToString();
                if (empl.BirthDay.Day<10)
                    dayTemp = "0" + empl.BirthDay.Day.ToString();
                empl.BirthDayShort = monthTemp + "." + dayTemp + "." + empl.BirthDay.Year.ToString();

                empl.StartDay = Convert.ToDateTime(dtime2);
                empl.TimeRecord = (tdTemp.Year - empl.StartDay.Year).ToString();
                if (empl.TimeRecord=="0")
                {
                    empl.TimeRecord = "менее года";
                }
                monthTemp = empl.StartDay.Month.ToString();
                dayTemp = empl.StartDay.Day.ToString();
                if (empl.StartDay.Month < 10)
                    monthTemp = "0" + empl.StartDay.Month.ToString();
                if (empl.StartDay.Day < 10)
                    dayTemp = "0" + empl.StartDay.Day.ToString();
                empl.StartDayShort = monthTemp + "." + dayTemp + "." + empl.StartDay.Year.ToString();
                empl.Image = AppDomain.CurrentDomain.BaseDirectory + @"\img\" + empl.ID + ".jpg";

                employeeLst.Add(empl);

            }
            connection.Close();

            return employeeLst;
        }

        public ObservableCollection<Customer> CustomerRead()
        {

            Customer cust;
            customerLst = new ObservableCollection<Customer> { };

            //Телефоны заказчиков
            //dt.Columns.Add("ФИО");
            //dt.Columns.Add("Должность");
            //dt.Columns.Add("Мобильный");
            //dt.Columns.Add("Рабочий");
            //dt.Columns.Add("e-mail");
            //dt.Columns.Add("Организация");


            //dt.Rows.Clear();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'customer';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                cust = new Customer();

                cust.FullName = record["custNames"].ToString();
                cust.PhoneMobile = record["custTels"].ToString();
                cust.PhoneWork = record["custTels2"].ToString();
                cust.Email = record["custEmail"].ToString();
                cust.Position = record["custPos"].ToString();
                cust.Company = record["custComp"].ToString();

                customerLst.Add(cust);
                //dt.Rows.Add(record["custNames"].ToString(), record["custPos"].ToString(), record["custTels"].ToString(), record["custTels2"].ToString(), record["custEmail"].ToString(), record["custComp"].ToString());

            }
            connection.Close();

            return customerLst;
        }

        public void ClearTable(String table)
        {
            String Command = "DELETE FROM '" + table + "';";

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBase2Name, pass));

            SQLiteCommand sqlitecommand = new SQLiteCommand(Command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        public void EmployeeReadID()
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT tID FROM 'employee';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                tIDVal.Add(record["tID"].ToString());
            }
            connection.Close();
        }

        public List<String> EmployeeReadStatus()
        {
            List<String> status = new List<string>();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT statusVal FROM 'status';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                status.Add(record["statusVal"].ToString());
            }
            connection.Close();

            return status;
        }



        public DataTable EmployeeReadBirthDays()
        {
            String prepDate;
            String prepDay;
            String prepMonth;
            DataTable dt = new DataTable();
            dt.Columns.Add("ФИО");
            dt.Columns.Add("Дата");
            //dt.Columns.Add("Дата", typeof(DateTime));

            dt.Rows.Clear();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT tNames, tBirthDay FROM 'employee';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                DateTime dtime2 = DateTime.Today;

                if (record["tBirthDay"].ToString().Length > 0)
                {
                    prepDay = record["tBirthDay"].ToString().Remove(2);
                    prepMonth = record["tBirthDay"].ToString().Substring(3, 2);


                    if (Convert.ToInt32(prepMonth) == dtime2.Month)
                    {

                        if (Convert.ToInt32(prepDay) > dtime2.Day - 3 && Convert.ToInt32(prepDay) < dtime2.Day + 3)
                            dt.Rows.Add(record["tNames"].ToString(), prepDay + "." + prepMonth);
                    }
                }

            }
            connection.Close();

            return dt;
        }




        public void EmployeeCopyTables()
        {
            List<String> tName = new List<string>();
            List<String> tTel = new List<string>();
            List<String> tTel2 = new List<string>();
            List<String> tTel3 = new List<string>();
            List<String> tEm = new List<string>();
            List<String> tDi = new List<string>();
            List<String> tPo = new List<string>();
            List<String> tID = new List<string>();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", "DBTels2.sqlite", pass));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM 'employee';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                tName.Add(record["tNames"].ToString());
                tTel.Add(record["tTels"].ToString());
                tTel2.Add(record["tTels2"].ToString());
                tTel3.Add(record["tTels3"].ToString());
                tEm.Add(record["tEmail"].ToString());
                tDi.Add(record["tDiv"].ToString());
                tPo.Add(record["tPos"].ToString());
                tID.Add(record["tID"].ToString());
            }
            connection.Close();


            SQLiteConnection connection2 = new SQLiteConnection(string.Format("Data Source={0};Password={1};", "DBTels.sqlite", pass));
            connection2.Open();
            SQLiteCommand command2;

            for (int i = 0; i < tName.Count(); i++)
            {
                command2 = new SQLiteCommand("INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos') VALUES ('" + tID[i] + "', '" + tName[i] + "', '" + tTel[i] + "', '" + tTel2[i] + "', '" + tTel3[i] + "', '" + tEm[i] + "', '" + tDi[i] + "', '" + tPo[i] + "');", connection2);
                command2.ExecuteNonQuery();
            }

            connection2.Close();
        }

        public void EmployeeReadStatusCount()
        {
            List<String> status = new List<string>();

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));

            connection.Open();
            //command = new SQLiteCommand("UPDATE employee SET tStatus = '" + status[i] + "' WHERE tNames Like '" + names[i] + "%';", connection);

            SQLiteCommand command = new SQLiteCommand("SELECT tID FROM employee WHERE tStatus LIKE 'в отпуске%' AND tActStatus = '1';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                this.tVacation.Add(record["tID"].ToString());
            }

            SQLiteCommand command2 = new SQLiteCommand("SELECT tID FROM 'employee' WHERE tStatus LIKE 'в командировке%' AND tActStatus = '1';", connection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            foreach (DbDataRecord record in reader2)
            {
                this.tBTrip.Add(record["tID"].ToString());
            }

            SQLiteCommand command3 = new SQLiteCommand("SELECT tID FROM 'employee' WHERE tStatus LIKE 'на больничном%' AND tActStatus = '1';", connection);
            SQLiteDataReader reader3 = command3.ExecuteReader();
            foreach (DbDataRecord record in reader3)
            {
                this.tSick.Add(record["tID"].ToString());
            }

            SQLiteCommand command4 = new SQLiteCommand("SELECT tID FROM 'employee' WHERE (tStatus LIKE 'на обуч%' OR tStatus LIKE 'не будет%') AND tActStatus = '1';", connection);
            SQLiteDataReader reader4 = command4.ExecuteReader();
            foreach (DbDataRecord record in reader4)
            {
                this.tOther.Add(record["tID"].ToString());
            }

            connection.Close();
        }




        public ObservableCollection<DinnerList> DinnerListRead()
        {
            DinnerList dinner;
            dinnerList = new ObservableCollection<DinnerList> { };

            DateTime dt = new DateTime();
            String tableName = "dinner" + DateTime.Today.ToString("yy") + "_" + DateTime.Today.ToString("MM") + "_1";

            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, pass));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM '" + tableName + "';", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                dinner = new DinnerList();

                dinner.ID = record["id"].ToString();
                dinner.Day1 = record["day1"].ToString();
                dinner.Day2 = record["day2"].ToString();
                dinner.Day3 = record["day3"].ToString();

                dinnerList.Add(dinner);

            }
            connection.Close();

            return dinnerList;
        }

        public void DinnerPersonAdd(String id)
        {
            SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, pass));
            connection.Open();
            SQLiteCommand command;

            //command = new SQLiteCommand("INSERT INTO 'dinner16_11' ('idEmpl', 'day1', 'day2', 'day3') VALUES ('" + date + "', '" + name + "', '" + pcName + "', '" + ip + "', '" + remoteBD + "');", connection);
            //command.ExecuteNonQuery();

            connection.Close();
        }





    }


}
