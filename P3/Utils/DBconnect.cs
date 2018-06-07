namespace P3
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.Common;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using Model;

    /// <summary>Подключение к БД.</summary>
    public class DBconnect
    {
        private const string DataBaseName = "DBTels.sqlite";
        private const string DataBase2Name = "DBTelsTemp.sqlite";
        private const string Pass = "Xt,ehfirf3";

        private ObservableCollection<Employee> EmployeeLst { get; set; }

        private ObservableCollection<Customer> CustomerLst { get; set; }

        /// <summary>Создать БД.</summary>
        public void CreateBase()
        {
            if (!File.Exists(DataBaseName))
            {
                SQLiteConnection.CreateFile(DataBaseName);
                SQLiteConnection connection = new SQLiteConnection("Data Source=DBTels.sqlite;Version=3;");
                connection.SetPassword(Pass);
            }
        }

        /// <summary>Создать таблицу сотрудников.</summary>
        public void EmployeeCreateTable()
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                var command = "CREATE TABLE employee (id INTEGER PRIMARY KEY UNIQUE, tID VARCHAR, tNames VARCHAR, tTels VARCHAR, tTels2 VARCHAR, tTels3 VARCHAR, tEmail VARCHAR, tDiv VARCHAR, tPos VARCHAR, tStatus VARCHAR, tActStatus VARCHAR, tBirthDay VARCHAR, tStartDay VARCHAR);";
                var sqlitecommand = new SQLiteCommand(command, connection);
                connection.Open();
                sqlitecommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>Создать таблицу заказчиков.</summary>
        public void CustomerCreateTable()
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                var command = "CREATE TABLE customer (id INTEGER PRIMARY KEY UNIQUE, custNames VARCHAR, custPos VARCHAR, custTels VARCHAR, custTels2 VARCHAR, custEmail VARCHAR, custComp VARCHAR);";
                var sqlitecommand = new SQLiteCommand(command, connection);
                connection.Open();
                sqlitecommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>Создать таблицу информации.</summary>
        public void InfoCreateTable()
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                var command = "CREATE TABLE info (id INTEGER PRIMARY KEY UNIQUE, date VARCHAR, name VARCHAR, pcName VARCHAR, ip VARCHAR, remoteBD VARCHAR);";
                var sqlitecommand = new SQLiteCommand(command, connection);
                connection.Open();
                sqlitecommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        
        /// <summary>Создать таблицу статусов.</summary>
        public void StatusCreateTable()
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                var command = "CREATE TABLE status (id INTEGER PRIMARY KEY UNIQUE, statusVal VARCHAR);";
                var sqlitecommand = new SQLiteCommand(command, connection);
                connection.Open();
                sqlitecommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        
        /// <summary>Записать в таблицу сотрудника.</summary>
        /// <param name="employeeLst">Коллекция сотрудников.</param>
        public void EmployeeWrite(List<Employee> employeeLst) 
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command;

                for (int i = 0; i < employeeLst.Count(); i++)
                {
                    command = new SQLiteCommand($"INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos', 'tBirthDay', 'tStartDay') VALUES ('{employeeLst[i].ID}', '{employeeLst[i].FullName}', '{employeeLst[i].PhoneWork}', '{employeeLst[i].PhoneMobile}', '{employeeLst[i].PhoneExch}', '{employeeLst[i].Email}', '{employeeLst[i].Division}', '{employeeLst[i].Position}', '{employeeLst[i].BirthDayShort}', '{employeeLst[i].StartDayShort}');", connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();

                File.Copy(DataBase2Name, DataBaseName, true);
            }
        }

        /// <summary>Записать в таблицу заказчика.</summary>
        /// <param name="customers">Заказчики.</param>
        public void CustomerWrite(List<Customer> customers)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command;

                for (int i = 0; i < customers.Count(); i++)
                {
                    command = new SQLiteCommand($"INSERT INTO 'customer' ('custNames', 'custPos', 'custTels', 'custTels2', 'custEmail', 'custComp') VALUES ('{customers[i].FullName}', '{customers[i].Position}', '{customers[i].PhoneMobile}', '{customers[i].PhoneWork}', '{customers[i].Email}', '{customers[i].Company}');", connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        /// <summary>Обновить данные о заказчике.</summary>
        /// <param name="contact">Заказчик.</param>
        public void CustomerUpdatePerson(Customer contact)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"UPDATE customer SET custNames = '{contact.FullName}', custPos = '{contact.Position}', custTels = '{contact.PhoneMobile}', custTels2 = '{contact.PhoneWork}', custEmail = '{contact.Email}', custComp = '{contact.Company}' WHERE custNames Like '{contact.FullName}';", connection);

                command.ExecuteNonQuery();

                connection.Close();

                File.Copy(DataBase2Name, DataBaseName, true);
            }
        }
        
        /// <summary>Записать контакты в таблицу заказчика.</summary>
        /// <param name="contact">Заказчик.</param>
        public void CustomerWritePerson(Customer contact)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"INSERT INTO 'customer' ('custNames', 'custPos', 'custTels', 'custTels2', 'custEmail', 'custComp') VALUES ('{contact.FullName}', '{contact.Position}', '{contact.PhoneMobile}', '{contact.PhoneWork}', '{contact.Email}', '{contact.Company}');", connection);

                command.ExecuteNonQuery();
                connection.Close();

                File.Copy(DataBase2Name, DataBaseName, true);
            }
        }

        /// <summary>Записать в таблицу информации.</summary>
        /// <param name="date">Дата.</param>
        /// <param name="name">Имя.</param>
        /// <param name="computerName">Имя компьютера.</param>
        /// <param name="ip">IP.</param>
        /// <param name="remoteBD">Удаленная БД.</param>
        public void InfoWrite(DateTime date, string name, string computerName, string ip, string remoteBD)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command;

                command = new SQLiteCommand($"INSERT INTO 'info' ('date', 'name', 'pcName', 'ip', 'remoteBD') VALUES ('{date}', '{name}', '{computerName}', '{ip}', '{remoteBD}');", connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        
        /// <summary>Записать в таблицу статусов.</summary>
        /// <param name="status">Статусы.</param>
        public void StatusWrite(List<string> status)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command;

                command = new SQLiteCommand("DELETE FROM 'status';", connection);
                command.ExecuteNonQuery();

                for (int i = 0; i < status.Count(); i++)
                {
                    command = new SQLiteCommand($"INSERT INTO 'status' ('statusVal') VALUES ('{status[i]}');", connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        /// <summary>Записать в таблицу статусы сотрудников.</summary>
        /// <param name="status">Состояния.</param>
        /// <param name="names">Статусы.</param>
        /// <param name="act">Действия.</param>
        public void EployeeStatusWrite(List<string> status, List<string> names, List<string> act) 
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command;

                command = new SQLiteCommand("UPDATE employee SET tStatus = '';", connection);
                command.ExecuteNonQuery();

                for (int i = 0; i < status.Count(); i++)
                {
                    command = new SQLiteCommand($"UPDATE employee SET tStatus = '{status[i]}', tActStatus = '{act[i]}' WHERE tNames Like '{names[i]}%';", connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();

                File.Copy(DataBase2Name, DataBaseName, true);
            }
        }

        /// <summary>Записать в таблицу дни рождения сотрудников.</summary>
        /// <param name="birthDay">Дни рождения.</param>
        /// <param name="startDay">Даты устройства на работу.</param>
        /// <param name="birthDayID">Дни рождения ID.</param>
        public void EployeeBirthDayWrite(List<string> birthDay, List<string> startDay, List<string> birthDayID)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                connection.Open();
                SQLiteCommand command;

                for (int i = 0; i < birthDayID.Count(); i++)
                {
                    command = new SQLiteCommand($"UPDATE employee SET tBirthDay = '{birthDay[i]}' WHERE tID = '{birthDayID[i]}';", connection);
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand($"UPDATE employee SET tStartDay = '{startDay[i]}' WHERE tID = '{birthDayID[i]}';", connection);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        /// <summary>Прочитать из таблицы сотрудников.</summary>
        /// <returns>Сотрудники.</returns>
        public ObservableCollection<Employee> EmployeeRead()
        {
            Employee empl;
            this.EmployeeLst = new ObservableCollection<Employee> { };

            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM 'employee';", connection);
                var reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                {
                    string dtime;
                    string dtime2;
                    DateTime dateValue;

                    var birthDayTemp = record["tBirthDay"].ToString();
                    var startDayTemp = record["tStartDay"].ToString();

                    // День рождения
                    if (birthDayTemp.Length > 0 && DateTime.TryParse(birthDayTemp, out dateValue))
                    {
                        dtime = birthDayTemp;
                    }
                    else
                    {
                        dtime = "21.12.1994";
                    }

                    // Дата прихода
                    if (startDayTemp.Length > 0 && DateTime.TryParse(startDayTemp, out dateValue))
                    {
                        dtime2 = startDayTemp;
                    }
                    else
                    {
                        dtime2 = "21.12.1994";
                    }

                    var dateTemp = DateTime.Now;

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

                    // -------------------------------
                    // Возраст
                    empl.Age = dateTemp.Year - empl.BirthDay.Year;
                    if (dateTemp.Month < empl.BirthDay.Month)
                    {
                        empl.Age = empl.Age - 1;
                    }
                    else if (dateTemp.Month == empl.BirthDay.Month && dateTemp.Day < empl.BirthDay.Day)
                    {
                        empl.Age = empl.Age - 1;
                    }

                    // -------------------------------
                    // День рождения
                    var monthTemp = empl.BirthDay.Month.ToString();
                    var dayTemp = empl.BirthDay.Day.ToString();
                    if (empl.BirthDay.Month < 10)
                    {
                        monthTemp = "0" + empl.BirthDay.Month.ToString();
                    }

                    if (empl.BirthDay.Day < 10)
                    {
                        dayTemp = "0" + empl.BirthDay.Day.ToString();
                    }

                    empl.BirthDayShort = monthTemp + "." + dayTemp + "." + empl.BirthDay.Year.ToString();

                    // -------------------------------
                    // Стаж
                    empl.StartDay = Convert.ToDateTime(dtime2);

                    var timeRecordTmp = dateTemp.Year - empl.StartDay.Year;

                    if (timeRecordTmp == 0)
                    {
                        empl.TimeRecord = "менее года";
                    }
                    else if (dateTemp.Month < empl.StartDay.Month)
                    {
                        empl.TimeRecord = (timeRecordTmp - 1).ToString();
                    }
                    else if (dateTemp.Month == empl.StartDay.Month && dateTemp.Day < empl.StartDay.Day)
                    {
                        empl.TimeRecord = (timeRecordTmp - 1).ToString();
                    }
                    else
                    {
                        empl.TimeRecord = timeRecordTmp.ToString();
                    }

                    if (empl.TimeRecord == "0")
                    {
                        empl.TimeRecord = "менее года";
                    }

                    // -------------------------------
                    // Дата начала работы 
                    monthTemp = empl.StartDay.Month.ToString();
                    dayTemp = empl.StartDay.Day.ToString();
                    if (empl.StartDay.Month < 10)
                    {
                        monthTemp = "0" + empl.StartDay.Month.ToString();
                    }

                    if (empl.StartDay.Day < 10)
                    {
                        dayTemp = "0" + empl.StartDay.Day.ToString();
                    }

                    empl.StartDayShort = monthTemp + "." + dayTemp + "." + empl.StartDay.Year.ToString();

                    // -------------------------------
                    // фото
                    empl.Image = AppDomain.CurrentDomain.BaseDirectory + @"\img\" + empl.ID + ".jpg";

                    this.EmployeeLst.Add(empl);
                }

                connection.Close();
            }

            return this.EmployeeLst;
        }

        /// <summary>Прочитать из таблицы заказчиков.</summary>
        /// <returns>Заказчики.</returns>
        public ObservableCollection<Customer> CustomerRead()
        {
            Customer cust;
            this.CustomerLst = new ObservableCollection<Customer> { };

            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM 'customer';", connection);
                var reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                {
                    cust = new Customer();

                    cust.FullName = record["custNames"].ToString();
                    cust.PhoneMobile = record["custTels"].ToString();
                    cust.PhoneWork = record["custTels2"].ToString();
                    cust.Email = record["custEmail"].ToString();
                    cust.Position = record["custPos"].ToString();
                    cust.Company = record["custComp"].ToString();

                    this.CustomerLst.Add(cust);
                }

                connection.Close();
            }

            return this.CustomerLst;
        }

        /// <summary>Очистить таблицу.</summary>
        /// <param name="table">Таблица.</param>
        public void ClearTable(string table)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBase2Name)))
            {
                var command = "DELETE FROM '" + table + "';";

                var sqlitecommand = new SQLiteCommand(command, connection);
                connection.Open();
                sqlitecommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>Прочитать статусы сотрудников.</summary>
        /// <returns>Статусы.</returns>
        public List<string> EmployeeReadStatus()
        {
            var status = new List<string>();

            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT statusVal FROM 'status';", connection);
                var reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                {
                    status.Add(record["statusVal"].ToString());
                }

                connection.Close();
            }

            return status;
        }

        /// <summary>Прочитать дни рождения сотрудников.</summary>
        /// <returns>Дни рождения.</returns>
        public DataTable EmployeeReadBirthDays()
        {
            string prepDay;
            string prepMonth;
            var dt = new DataTable();
            dt.Columns.Add("ФИО");
            dt.Columns.Add("Дата");

            dt.Rows.Clear();

            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT tNames, tBirthDay FROM 'employee';", connection);
                var reader = command.ExecuteReader();
                foreach (DbDataRecord record in reader)
                {
                    var dtime2 = DateTime.Today;

                    if (record["tBirthDay"].ToString().Length > 0)
                    {
                        prepDay = record["tBirthDay"].ToString().Remove(2);
                        prepMonth = record["tBirthDay"].ToString().Substring(3, 2);

                        if (Convert.ToInt32(prepMonth) == dtime2.Month)
                        {
                            if (Convert.ToInt32(prepDay) > dtime2.Day - 3 && Convert.ToInt32(prepDay) < dtime2.Day + 3)
                            {
                                dt.Rows.Add(record["tNames"].ToString(), prepDay + "." + prepMonth);
                            }
                        }
                    }
                }

                connection.Close();
            }

            return dt;
        }

        /// <summary>Скопировать таблицу сотрудников.</summary>
        public void EmployeeCopyTables()
        {
            var tempName = new List<string>();
            var tempTel = new List<string>();
            var tempTel2 = new List<string>();
            var tempTel3 = new List<string>();
            var tempEm = new List<string>();
            var tempDi = new List<string>();
            var tempPo = new List<string>();
            var tempID = new List<string>();

            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", "DBTels2.sqlite", Pass));
            connection.Open();
            var command = new SQLiteCommand("SELECT * FROM 'employee';", connection);
            var reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                tempName.Add(record["tNames"].ToString());
                tempTel.Add(record["tTels"].ToString());
                tempTel2.Add(record["tTels2"].ToString());
                tempTel3.Add(record["tTels3"].ToString());
                tempEm.Add(record["tEmail"].ToString());
                tempDi.Add(record["tDiv"].ToString());
                tempPo.Add(record["tPos"].ToString());
                tempID.Add(record["tID"].ToString());
            }

            connection.Close();

            var connection2 = new SQLiteConnection(string.Format("Data Source={0};Password={1};", "DBTels.sqlite", Pass));
            connection2.Open();
            SQLiteCommand command2;

            for (int i = 0; i < tempName.Count(); i++)
            {
                command2 = new SQLiteCommand($"INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos') VALUES ('{tempID[i]}', '{tempName[i]}', '{tempTel[i]}', '{tempTel2[i]}', '{tempTel3[i]}', '{tempEm[i]}', '{tempDi[i]}', '{tempPo[i]}');", connection2);
                command2.ExecuteNonQuery();
            }

            connection2.Close();
        }

        /// <summary>Посчитать кол-во статусов.</summary>
        /// <param name="misc">Настройки.</param>
        /// <returns>Настройки с количеством состояний.</returns>
        public Misc EmployeeReadStatusCount(Misc misc)
        {
            using (var connection = new SQLiteConnection(this.Connstring(DataBaseName)))
            {
                connection.Open();

                var command = new SQLiteCommand("SELECT COUNT(*) FROM employee WHERE tStatus LIKE 'в отпуске%' AND tActStatus = '1';", connection);

                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                    {
                        misc.Vacation = r.GetInt32(0);
                    }
                }

                var command2 = new SQLiteCommand("SELECT COUNT(*) FROM 'employee' WHERE tStatus LIKE 'в командировке%' AND tActStatus = '1';", connection);

                using (IDataReader r = command2.ExecuteReader())
                {
                    if (r.Read())
                    {
                        misc.BTrip = r.GetInt32(0);
                    }
                }

                var command3 = new SQLiteCommand("SELECT COUNT(*) FROM 'employee' WHERE tStatus LIKE 'на больничном%' AND tActStatus = '1';", connection);

                using (IDataReader r = command3.ExecuteReader())
                {
                    if (r.Read())
                    {
                        misc.Sick = r.GetInt32(0);
                    }
                }

                connection.Close();
            }

            return misc;
        }

        private string Connstring(string dataBaseName)
        {
            return string.Format("Data Source={0};Version=3;Password={1};", dataBaseName, Pass);
        }
    }
}