namespace P3.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Model;
    using NLog;
    using static Model.SettingsShell;
    using Excel = Microsoft.Office.Interop.Excel;    

    /// <summary>Обновить контакты.</summary>
    public class UpdateContacts
    {
        private const string DatabasePath = "DBTels.sqlite";
        private const string DatabasePathTemp = "DBTelsTemp.sqlite";
        private Logger logger = LogManager.GetCurrentClassLogger();
        private bool flag = false;

        private Excel.Application excelApp;
        private Excel.Workbook workBookExcel;
        private Excel.Worksheet workSheetExcel;

        private DBconnect dbc = new DBconnect();

        // Конфигурация        
        private RootElement settings;

        /// <summary>Initializes a new instance of the <see cref="UpdateContacts" /> class.</summary>
        /// <param name="settings">Параметры.</param>
        public UpdateContacts(RootElement settings)
        {
            this.settings = settings;
        }

        /// <summary>Получить путь.</summary>
        /// <returns>Путь.</returns>
        public string GetPath()
        {
            var customPath = this.settings.Contacts.FilePath;

            if (!File.Exists(customPath))
            {
                customPath = string.Empty;
            }

            return customPath;
        }

        /// <summary>Обновить.</summary>
        /// <param name="customPath">Путь к файлу.</param>
        /// <returns>Состояние.</returns>
        public bool Update(string customPath)
        {
            if (!string.IsNullOrEmpty(customPath))
            {
                this.flag = this.ReadData(customPath);
                File.Copy(DatabasePathTemp, DatabasePath, true);
            }

            return this.flag;
        }

        /// <summary>Загрузить заказчиков.</summary>
        /// <returns>Заказчики.</returns>
        public string LoadCustom()
        {
            var filename = string.Empty;
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = "c:\\",
                    Filter = "Execl files (*.xlsx)|*.xlsx",
                    FilterIndex = 0,
                    RestoreDirectory = true
                };
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка. {ex.Message}");
                this.logger.Error(ex.Message);
                return string.Empty;
            }

            return filename;
        }
        
        /// <summary>Сохранить.</summary>
        /// <param name="contactName">Имя контакта.</param>
        /// <param name="contact">Контакт.</param>
        /// <returns>Состояние.</returns>
        public bool SaveData2(string contactName, Customer contact)
        {
            var customPath = this.GetPath();
            var flag = false;
            try
            {
                if (customPath != string.Empty)
                {
                    this.excelApp = new Excel.Application
                    {
                        Visible = false
                    };

                    this.workBookExcel = this.excelApp.Workbooks.Open(customPath);
                    this.workSheetExcel = (Excel.Worksheet)this.workBookExcel.Sheets[1];
                    this.excelApp.DisplayAlerts = false;
                    var i = 3;
                    for (; this.workSheetExcel.Cells[i, 1].Text.ToString() != string.Empty; i++)
                    {
                        var name = this.CleanString(this.workSheetExcel.Cells[i, 1].Text.ToString());
                        if (name == contactName)
                        {
                            break;
                        }
                    }

                    this.workSheetExcel.Cells[i, 1] = contact.FullName;
                    this.workSheetExcel.Cells[i, 2] = contact.Position;
                    this.workSheetExcel.Cells[i, 3] = contact.PhoneMobile;
                    this.workSheetExcel.Cells[i, 4] = contact.PhoneWork;
                    this.workSheetExcel.Cells[i, 5] = contact.Email;
                    this.workSheetExcel.Cells[i, 6] = contact.Company;

                    this.workSheetExcel.SaveAs(customPath);
                    this.excelApp.Quit();
                    GC.Collect();

                    flag = true;
                }

                return flag;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка. Невозможно записать данные, возможно файл открыт другим пользователем. {ex.Message}");
                this.logger.Error(ex.Message);
                return flag;
            }
        }

        /// <summary>Сохранить данные.</summary>
        /// <param name="custLst">Коллекция контактов.</param>
        public void SaveData(ObservableCollection<Customer> custLst)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "Export Excel File To";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileName = saveFileDialog.FileName;
                    var excelEx = new ExcelExport(fileName);
                    excelEx.ExcelWrite(custLst);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Файл не создан");
                this.logger.Error(ex.Message);
            }
        }

        /// <summary>Прочитать.</summary>
        /// <param name="customPath">Путь к файлу.</param>
        /// <returns>Состояние.</returns>
        private bool ReadData(string customPath)
        {
            var customers = new List<Customer>();

            try
            {
                this.excelApp = new Excel.Application
                {
                    Visible = false
                };

                this.workBookExcel = this.excelApp.Workbooks.Open(customPath, false);
                this.workSheetExcel = (Excel.Worksheet)this.workBookExcel.Sheets[1];

                for (var i = 2; this.workSheetExcel.Cells[i, 1].Text.ToString() != string.Empty; i++)
                {
                    var cust = new Customer
                    {
                        FullName = this.CleanString(this.workSheetExcel.Cells[i, 1].Text.ToString()),
                        Position = this.CleanString(this.workSheetExcel.Cells[i, 2].Text.ToString()),
                        PhoneMobile = this.CleanString(this.workSheetExcel.Cells[i, 3].Text.ToString()),
                        PhoneWork = this.CleanString(this.workSheetExcel.Cells[i, 4].Text.ToString()),
                        Email = this.CleanString(this.workSheetExcel.Cells[i, 5].Text.ToString()),
                        Company = this.CleanString(this.workSheetExcel.Cells[i, 6].Text.ToString())
                    };

                    customers.Add(cust);
                }

                this.workBookExcel.Close(false, Type.Missing, Type.Missing);
                this.excelApp.Quit();
                GC.Collect();

                // Запись в БД
                this.dbc.ClearTable("customer");
                this.dbc.CustomerWrite(customers);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка. {ex.Message}");
                this.logger.Error(ex.Message);
                return false;
            }
        }

        // Очищение от спецсимволов
        private string CleanString(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var stringBuilder = new StringBuilder(str.Length);
                foreach (var ch in str)
                {
                    if (char.IsControl(ch))
                    {
                        continue;
                    }

                    stringBuilder.Append(ch);
                }

                str = stringBuilder.ToString();
            }

            return str;
        }
    }
}