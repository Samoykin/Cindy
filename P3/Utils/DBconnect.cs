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

        private List<string> idVal = new List<string>();
        private List<string> namesVal = new List<string>();
        private List<string> telsVal = new List<string>();
        private List<string> tels2Val = new List<string>();
        private List<string> tels3Val = new List<string>();
        private List<string> emailVal = new List<string>();
        private List<string> divVal = new List<string>();
        private List<string> posVal = new List<string>();
        private List<string> statusVal = new List<string>();
        private List<DateTime> birthDayVal = new List<DateTime>();

        private List<string> vacationVal = new List<string>();
        private List<string> sickVal = new List<string>();
        private List<string> btripVal = new List<string>();
        private List<string> otherVal = new List<string>();

        /// <summary>ID.</summary>
        public List<string> ID
        {
            get { return this.idVal; }
            set { this.idVal = value; }
        }

        /// <summary>Имя.</summary>
        public List<string> Names
        {
            get { return this.namesVal; }
            set { this.namesVal = value; }
        }

        /// <summary>Телефон 1.</summary>
        public List<string> Tels
        {
            get { return this.telsVal; }
            set { this.telsVal = value; }
        }

        /// <summary>Телефон 2.</summary>
        public List<string> Tels2
        {
            get { return this.tels2Val; }
            set { this.tels2Val = value; }
        }

        /// <summary>Телефон 3.</summary>
        public List<string> Tels3
        {
            get { return this.tels3Val; }
            set { this.tels3Val = value; }
        }

        /// <summary>Email.</summary>
        public List<string> Email
        {
            get { return this.emailVal; }
            set { this.emailVal = value; }
        }

        /// <summary>Подразделение.</summary>
        public List<string> Div
        {
            get { return this.divVal; }
            set { this.divVal = value; }
        }

        /// <summary>Должность.</summary>
        public List<string> Pos
        {
            get { return this.posVal; }
            set { this.posVal = value; }
        }

        /// <summary>Статус.</summary>
        public List<string> Status
        {
            get { return this.statusVal; }
            set { this.statusVal = value; }
        }

        /// <summary>День рождения.</summary>
        public List<DateTime> BirthDay
        {
            get { return this.birthDayVal; }
            set { this.birthDayVal = value; }
        }

        /// <summary>Отпуск.</summary>
        public List<string> Vacation
        {
            get { return this.vacationVal; }
            set { this.vacationVal = value; }
        }

        /// <summary>Болезнь.</summary>
        public List<string> Sick
        {
            get { return this.sickVal; }
            set { this.sickVal = value; }
        }

        /// <summary>Командировка.</summary>
        public List<string> BTrip
        {
            get { return this.btripVal; }
            set { this.btripVal = value; }
        }

        /// <summary>Другое.</summary>
        public List<string> Other
        {
            get { return this.otherVal; }
            set { this.otherVal = value; }
        }

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
            var command = "CREATE TABLE employee (id INTEGER PRIMARY KEY UNIQUE, tID VARCHAR, tNames VARCHAR, tTels VARCHAR, tTels2 VARCHAR, tTels3 VARCHAR, tEmail VARCHAR, tDiv VARCHAR, tPos VARCHAR, tStatus VARCHAR, tActStatus VARCHAR, tBirthDay VARCHAR, tStartDay VARCHAR);";
            var connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, Pass));

            var sqlitecommand = new SQLiteCommand(command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>Создать таблицу заказчиков.</summary>
        public void CustomerCreateTable()
        {
            var command = "CREATE TABLE customer (id INTEGER PRIMARY KEY UNIQUE, custNames VARCHAR, custPos VARCHAR, custTels VARCHAR, custTels2 VARCHAR, custEmail VARCHAR, custComp VARCHAR);";
            var connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, Pass));

            var sqlitecommand = new SQLiteCommand(command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>Создать таблицу информации.</summary>
        public void InfoCreateTable()
        {
            var command = "CREATE TABLE info (id INTEGER PRIMARY KEY UNIQUE, date VARCHAR, name VARCHAR, pcName VARCHAR, ip VARCHAR, remoteBD VARCHAR);";
            var connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, Pass));

            var sqlitecommand = new SQLiteCommand(command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }
        
        /// <summary>Создать таблицу статусов.</summary>
        public void StatusCreateTable()
        {
            var command = "CREATE TABLE status (id INTEGER PRIMARY KEY UNIQUE, statusVal VARCHAR);";
            var connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBaseName, Pass));

            var sqlitecommand = new SQLiteCommand(command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>Записать в таблицу сотрудника.</summary>
        /// <param name="id">ID.</param>
        /// <param name="names">Имя.</param>
        /// <param name="tels">Телефон.</param>
        /// <param name="tels2">Телефон2.</param>
        /// <param name="tels3">Телефон3.</param>
        /// <param name="email">Email.</param>
        /// <param name="div">Подразделение.</param>
        /// <param name="pos">Должность.</param>
        public void EmployeeWrite(List<string> id, List<string> names, List<string> tels, List<string> tels2, List<string> tels3, List<string> email, List<string> div, List<string> pos)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));

            connection.Open();
            SQLiteCommand command;

            for (int i = 0; i < this.Names.Count(); i++)
            {
                command = new SQLiteCommand("INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos') VALUES ('" + id[i] + "', '" + names[i] + "', '" + tels[i] + "', '" + tels2[i] + "', '" + tels3[i] + "', '" + email[i] + "', '" + div[i] + "', '" + pos[i] + "');", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();

            File.Copy(DataBase2Name, DataBaseName, true);
        }
        
        /// <summary>Записать в таблицу сотрудника2.</summary>
        /// <param name="employeeLst">Коллекция сотрудников.</param>
        public void EmployeeWrite2(List<Employee> employeeLst) 
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));

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

        /// <summary>Записать в таблицу заказчика.</summary>
        /// <param name="custNames">Имя.</param>
        /// <param name="custPos">Должность.</param>
        /// <param name="custTels">Телефон.</param>
        /// <param name="custTels2">Телефон2.</param>
        /// <param name="custEmail">Email.</param>
        /// <param name="custComp">Компания.</param>
        public void CustomerWrite(List<string> custNames, List<string> custPos, List<string> custTels, List<string> custTels2, List<string> custEmail, List<string> custComp)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
            connection.Open();
            SQLiteCommand command;

            for (int i = 0; i < custNames.Count(); i++)
            {
                command = new SQLiteCommand("INSERT INTO 'customer' ('custNames', 'custPos', 'custTels', 'custTels2', 'custEmail', 'custComp') VALUES ('" + custNames[i] + "', '" + custPos[i] + "', '" + custTels[i] + "', '" + custTels2[i] + "', '" + custEmail[i] + "', '" + custComp[i] + "');", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        /// <summary>Обновить данные о заказчике.</summary>
        /// <param name="contact">Заказчик.</param>
        public void CustomerUpdatePerson(Customer contact)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("UPDATE customer SET custNames = '" + contact.FullName + "', custPos = '" + contact.Position + "', custTels = '" + contact.PhoneMobile + "', custTels2 = '" + contact.PhoneWork + "', custEmail = '" + contact.Email + "', custComp = '" + contact.Company + "' WHERE custNames Like '" + contact.FullName + "';", connection);
                
            command.ExecuteNonQuery();            

            connection.Close();

            File.Copy(DataBase2Name, DataBaseName, true);
        }
        
        /// <summary>Записать контакты в таблицу заказчика.</summary>
        /// <param name="contact">Заказчик.</param>
        public void CustomerWritePerson(Customer contact)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("INSERT INTO 'customer' ('custNames', 'custPos', 'custTels', 'custTels2', 'custEmail', 'custComp') VALUES ('" + contact.FullName + "', '" + contact.Position + "', '" + contact.PhoneMobile + "', '" + contact.PhoneWork + "', '" + contact.Email + "', '" + contact.Company + "');", connection);
            
            command.ExecuteNonQuery();
            connection.Close();

            File.Copy(DataBase2Name, DataBaseName, true);
        }

        /// <summary>Записать в таблицу информации.</summary>
        /// <param name="date">Дата.</param>
        /// <param name="name">Имя.</param>
        /// <param name="computerName">Имя компьютера.</param>
        /// <param name="ip">IP.</param>
        /// <param name="remoteBD">Удаленная БД.</param>
        public void InfoWrite(DateTime date, string name, string computerName, string ip, string remoteBD)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
            connection.Open();
            SQLiteCommand command;

            command = new SQLiteCommand("INSERT INTO 'info' ('date', 'name', 'pcName', 'ip', 'remoteBD') VALUES ('" + date + "', '" + name + "', '" + computerName + "', '" + ip + "', '" + remoteBD + "');", connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
        
        /// <summary>Записать в таблицу статусов.</summary>
        /// <param name="status">Статусы.</param>
        public void StatusWrite(List<string> status)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
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

        /// <summary>Записать в таблицу сотрудников.</summary>
        /// <param name="status">Состояния.</param>
        /// <param name="names">Статусы.</param>
        /// <param name="act">Действия.</param>
        public void EployeeStatusWrite(List<string> status, List<string> names, List<string> act) 
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
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
            
            File.Copy(DataBase2Name, DataBaseName, true);
        }

        /// <summary>Записать в таблицу дни рождения сотрудников.</summary>
        /// <param name="birthDay">Дни рождения.</param>
        /// <param name="startDay">Даты устройства на работу.</param>
        /// <param name="birthDayID">Дни рождения ID.</param>
        public void EployeeBirthDayWrite(List<string> birthDay, List<string> startDay, List<string> birthDayID)
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBase2Name, Pass));
            connection.Open();
            SQLiteCommand command;

            for (int i = 0; i < birthDayID.Count(); i++)
            {
                command = new SQLiteCommand("UPDATE employee SET tBirthDay = '" + birthDay[i] + "' WHERE tID = '" + birthDayID[i] + "';", connection);
                command.ExecuteNonQuery();

                command = new SQLiteCommand("UPDATE employee SET tStartDay = '" + startDay[i] + "' WHERE tID = '" + birthDayID[i] + "';", connection);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        /// <summary>Прочитать из таблицы сотрудников.</summary>
        /// <returns>Сотрудники.</returns>
        public ObservableCollection<Employee> EmployeeRead()
        {
            Employee empl;
            this.EmployeeLst = new ObservableCollection<Employee> { };

            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, Pass));

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

            return this.EmployeeLst;
        }

        /// <summary>Прочитать из таблицы заказчиков.</summary>
        /// <returns>Заказчики.</returns>
        public ObservableCollection<Customer> CustomerRead()
        {
            Customer cust;
            this.CustomerLst = new ObservableCollection<Customer> { };

            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, Pass));
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

            return this.CustomerLst;
        }

        /// <summary>Очистить таблицу.</summary>
        /// <param name="table">Таблица.</param>
        public void ClearTable(string table)
        {
            var command = "DELETE FROM '" + table + "';";

            var connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;Password={1};", DataBase2Name, Pass));

            var sqlitecommand = new SQLiteCommand(command, connection);
            connection.Open();
            sqlitecommand.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>Прочитать ID сотрудников.</summary>
        public void EmployeeReadID()
        {
            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, Pass));

            connection.Open();
            var command = new SQLiteCommand("SELECT tID FROM 'employee';", connection);
            var reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                this.idVal.Add(record["tID"].ToString());
            }

            connection.Close();
        }

        /// <summary>Прочитать статусы сотрудников.</summary>
        /// <returns>Статусы.</returns>
        public List<string> EmployeeReadStatus()
        {
            var status = new List<string>();

            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, Pass));

            connection.Open();
            var command = new SQLiteCommand("SELECT statusVal FROM 'status';", connection);
            var reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                status.Add(record["statusVal"].ToString());
            }

            connection.Close();

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

            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, Pass));

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
                command2 = new SQLiteCommand("INSERT INTO 'employee' ('tID', 'tNames', 'tTels', 'tTels2', 'tTels3', 'tEmail', 'tDiv', 'tPos') VALUES ('" + tempID[i] + "', '" + tempName[i] + "', '" + tempTel[i] + "', '" + tempTel2[i] + "', '" + tempTel3[i] + "', '" + tempEm[i] + "', '" + tempDi[i] + "', '" + tempPo[i] + "');", connection2);
                command2.ExecuteNonQuery();
            }

            connection2.Close();
        }

        /// <summary>Посчитать кол-во статусов.</summary>
        public void EmployeeReadStatusCount()
        {
            var status = new List<string>();

            var connection = new SQLiteConnection(string.Format("Data Source={0};Password={1};", DataBaseName, Pass));

            connection.Open();

            var command = new SQLiteCommand("SELECT tID FROM employee WHERE tStatus LIKE 'в отпуске%' AND tActStatus = '1';", connection);
            var reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                this.Vacation.Add(record["tID"].ToString());
            }

            var command2 = new SQLiteCommand("SELECT tID FROM 'employee' WHERE tStatus LIKE 'в командировке%' AND tActStatus = '1';", connection);
            var reader2 = command2.ExecuteReader();
            foreach (DbDataRecord record in reader2)
            {
                this.BTrip.Add(record["tID"].ToString());
            }

            var command3 = new SQLiteCommand("SELECT tID FROM 'employee' WHERE tStatus LIKE 'на больничном%' AND tActStatus = '1';", connection);
            var reader3 = command3.ExecuteReader();
            foreach (DbDataRecord record in reader3)
            {
                this.Sick.Add(record["tID"].ToString());
            }

            var command4 = new SQLiteCommand("SELECT tID FROM 'employee' WHERE (tStatus LIKE 'на обуч%' OR tStatus LIKE 'не будет%') AND tActStatus = '1';", connection);
            var reader4 = command4.ExecuteReader();
            foreach (DbDataRecord record in reader4)
            {
                this.Other.Add(record["tID"].ToString());
            }

            connection.Close();
        }        
    }
}