namespace P3.Contacts
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    using Model;
    using Updater;
    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>Обновить контакты.</summary>
    public class UpdateContacts
    {
        private string path = @"DBTels.sqlite";
        private string pathTemp = @"DBTelsTemp.sqlite";
        private bool flag = false;

        private Excel.Application excelApp;
        private Excel.Workbook workBookExcel;
        private Excel.Worksheet workSheetExcel;

        private GetData getData = new GetData();
        private DBconnect dbc = new DBconnect();

        /// <summary>Получить путь.</summary>
        /// <returns>Путь.</returns>
        public string GetPath()
        {
            var customPath = string.Empty;
            var localPropPath = "Settings.xml";

            var xmlCode = new XMLcodeContacts(localPropPath);
            customPath = xmlCode.ReadLocalPropXml();

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
            if (customPath != string.Empty)
            {
                this.flag = this.ReadData(customPath);
                File.Copy(this.pathTemp, this.path, true);
            }

            return this.flag;
        }

        /// <summary>Загрузить заказчиков.</summary>
        /// <returns>Заказчики.</returns>
        public string LoadCustom()
        {
            string filename = string.Empty;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка." + ex);
                return string.Empty;
            }

            return filename;
        }

        /// <summary>Прочитать.</summary>
        /// <param name="customPath">Путь к файлу.</param>
        /// <returns>Состояние.</returns>
        public bool ReadData(string customPath)
        {
            string name;
            string position;
            string tel;
            string workTel;
            string email;
            string company;

            this.getData.Name.Clear();
            this.getData.Position.Clear();
            this.getData.Tel.Clear();
            this.getData.WorkTel.Clear();

            try
            {
                this.excelApp = new Excel.Application();
                this.excelApp.Visible = false;
                this.workBookExcel = this.excelApp.Workbooks.Open(customPath, false); // открываем книгу
                this.workSheetExcel = (Excel.Worksheet)this.workBookExcel.Sheets[1]; // Получаем ссылку на лист 1

                for (int i = 2; this.workSheetExcel.Cells[i, 1].Text.ToString() != string.Empty; i++)
                {
                    name = this.CleanString(this.workSheetExcel.Cells[i, 1].Text.ToString());
                    position = this.CleanString(this.workSheetExcel.Cells[i, 2].Text.ToString());
                    tel = this.CleanString(this.workSheetExcel.Cells[i, 3].Text.ToString());
                    workTel = this.CleanString(this.workSheetExcel.Cells[i, 4].Text.ToString());
                    email = this.CleanString(this.workSheetExcel.Cells[i, 5].Text.ToString());
                    company = this.CleanString(this.workSheetExcel.Cells[i, 6].Text.ToString());

                    this.getData.Name.Add(name);
                    this.getData.Position.Add(position);
                    this.getData.Tel.Add(tel);
                    this.getData.WorkTel.Add(workTel);
                    this.getData.Email.Add(email);
                    this.getData.Company.Add(company);
                }

                this.workBookExcel.Close(false, Type.Missing, Type.Missing); // закрыл не сохраняя
                this.excelApp.Quit();
                GC.Collect();

                // Запись в БД
                this.dbc.ClearTable("customer");
                this.dbc.CustomerWrite(this.getData.Name, this.getData.Position, this.getData.Tel, this.getData.WorkTel, this.getData.Email, this.getData.Company);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка." + ex);
                return false;
            }            
        }

        /// <summary>Сохранить.</summary>
        /// <param name="contactName">Имя контакта.</param>
        /// <param name="contact">Контакт.</param>
        /// <returns>Состояние.</returns>
        public bool SaveData2(string contactName, Customer contact)
        {
            string customPath = this.GetPath();
            string name;
            bool flag = false;
            try
            {
                if (customPath != string.Empty)
                {
                    this.excelApp = new Excel.Application();
                    this.excelApp.Visible = false;
                    this.workBookExcel = this.excelApp.Workbooks.Open(customPath);
                    this.workSheetExcel = (Excel.Worksheet)this.workBookExcel.Sheets[1];
                    this.excelApp.DisplayAlerts = false;
                    int i = 3;
                    for (; this.workSheetExcel.Cells[i, 1].Text.ToString() != string.Empty; i++)
                    {
                        name = this.CleanString(this.workSheetExcel.Cells[i, 1].Text.ToString());
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
                MessageBox.Show("Ошибка. Невозможно записать данные, возможно файл открыт другим пользователем." + ex);
                return flag;
            }
        }

        /// <summary>Сохранить данные.</summary>
        /// <param name="custLst">Коллекция контактов.</param>
        public void SaveData(ObservableCollection<Customer> custLst)
        {
            var fileName = string.Empty;

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
                    fileName = saveFileDialog.FileName;
                }
                else
                {
                    goto link1;
                }
                
                var excelEx = new ExcelExport(fileName);
                excelEx.ExcelWrite(custLst);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Файл не создан");
            }

        link1: ;
        }

        // очищение от спецсимволов
        private string CleanString(string s)
        {
            if (s != null && s.Length > 0)
            {
                var sb = new StringBuilder(s.Length);
                foreach (char c in s)
                {
                    if (char.IsControl(c) == true)
                    {
                        continue;
                    }

                    sb.Append(c);
                }

                s = sb.ToString();
            }

            return s;
        }
    }
}