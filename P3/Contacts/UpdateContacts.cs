using P3.Model;
using P3.Updater;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace P3.Contacts
{
    class UpdateContacts
    {
        //String customPath = @"\\elcom.local\files\01-Deps\ДПАСУТП\05_Контакты\Контакты заказчиков.xlsx";
        String path = @"DBTels.sqlite";
        String pathTemp = @"DBTelsTemp.sqlite";
        Boolean flag = false;

        private Excel.Application ExcelApp;
        private Excel.Workbook WorkBookExcel;
        private Excel.Workbook WorkBookExcel2;
        private Excel.Worksheet WorkSheetExcel;
        private Excel.Worksheet WorkSheetExcel2;
        private Excel.Range RangeExcel;
        string customPathTxt = @"customTel.txt";

        public getData getData = new getData();

        DBconnect dbc = new DBconnect();

        public String GetPath()
        {
            String customPath = "";
            String _localPropPath = "Settings.xml";

            XMLcodeContacts xmlCode = new XMLcodeContacts(_localPropPath);
            customPath = xmlCode.ReadLocalPropXml();

            if (!File.Exists(customPath))
            {
                customPath = "";
            }
            return customPath;
        }


        public Boolean Update(String customPath)
        {
            if (customPath != "")
            {
                flag = readData(customPath);
                File.Copy(pathTemp, path, true);
            }

            return flag;
        }

        public String loadCustom()
        {
            String filename = "";
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;
                //openFileDialog.CreatePrompt = true;
                //openFileDialog.Title = "Export Excel File To";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = openFileDialog.FileName;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка." + exception);
                return "";
            }
            return filename;
        }


        public Boolean readData(String customPath)
        {
            String name;
            String position;
            String tel;
            String workTel;
            String email;
            String company;

            getData.name.Clear();
            getData.position.Clear();
            getData.tel.Clear();
            getData.workTel.Clear();

            try
            {

                ExcelApp = new Excel.Application();
                ExcelApp.Visible = false;
                WorkBookExcel = ExcelApp.Workbooks.Open(customPath, false); //открываем книгу
                WorkSheetExcel = (Excel.Worksheet)WorkBookExcel.Sheets[1]; //Получаем ссылку на лист 1

                //excelcells = excelworksheet.get_Range("D215", Type.Missing); //Выбираем ячейку для вывода A1
                // WorkSheetExcel.Cells[i, 1].Text.ToString() != ""

                for (int i = 2; WorkSheetExcel.Cells[i, 1].Text.ToString() != ""; i++)
                {
                    name = CleanString(WorkSheetExcel.Cells[i, 1].Text.ToString());
                    position = CleanString(WorkSheetExcel.Cells[i, 2].Text.ToString());
                    tel = CleanString(WorkSheetExcel.Cells[i, 3].Text.ToString());
                    workTel = CleanString(WorkSheetExcel.Cells[i, 4].Text.ToString());
                    email = CleanString(WorkSheetExcel.Cells[i, 5].Text.ToString());
                    company = CleanString(WorkSheetExcel.Cells[i, 6].Text.ToString());

                    getData.name.Add(name);
                    getData.position.Add(position);
                    getData.tel.Add(tel);
                    getData.workTel.Add(workTel);
                    getData.email.Add(email);
                    getData.company.Add(company);
                }


                WorkBookExcel.Close(false, Type.Missing, Type.Missing); //закрыл не сохраняя
                ExcelApp.Quit();
                GC.Collect();

                //Запись в БД
                dbc.ClearTable("customer");
                dbc.CustomerWrite(getData.name, getData.position, getData.tel, getData.workTel, getData.email, getData.company);

                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка." + exception);
                return false;
            }


        }

        public Boolean saveData2(String contactName, Customer contact)
        {
            String customPath = GetPath();
            String name;
            Boolean flag = false;
            try
            {
                if (customPath != "")
                {
                    ExcelApp = new Excel.Application();
                    ExcelApp.Visible = false;
                    WorkBookExcel = ExcelApp.Workbooks.Open(customPath);
                    WorkSheetExcel = (Excel.Worksheet)WorkBookExcel.Sheets[1];
                    ExcelApp.DisplayAlerts = false;
                    int i = 3;
                    for (; WorkSheetExcel.Cells[i, 1].Text.ToString() != ""; i++)
                    {
                        name = CleanString(WorkSheetExcel.Cells[i, 1].Text.ToString());
                        if (name == contactName)
                            break;
                    }

                    WorkSheetExcel.Cells[i, 1] = contact.FullName;
                    WorkSheetExcel.Cells[i, 2] = contact.Position;
                    WorkSheetExcel.Cells[i, 3] = contact.PhoneMobile;
                    WorkSheetExcel.Cells[i, 4] = contact.PhoneWork;
                    WorkSheetExcel.Cells[i, 5] = contact.Email;
                    WorkSheetExcel.Cells[i, 6] = contact.Company;

                    WorkSheetExcel.SaveAs(customPath);
                    ExcelApp.Quit();
                    GC.Collect();

                    flag = true;
                }

                return flag;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка. Невозможно записать данные, возможно файл открыт другим пользователем." + exception);
                return flag;
            }

        }

        //очищение от спецсимволов
        static public string CleanString(string s)
        {
            if (s != null && s.Length > 0)
            {
                StringBuilder sb = new StringBuilder(s.Length);
                foreach (char c in s)
                {
                    if (Char.IsControl(c) == true)
                        continue;
                    sb.Append(c);
                }
                s = sb.ToString();
            }
            return s;
        }


        public void saveData(ObservableCollection<Customer> custLst)
        {
            Excel.Application ExcelApp;
            Excel.Workbook WorkBookExcel;
            Excel.Worksheet WorkSheetExcel;
            Excel.Range RangeExcel;
            String fileName = "";

            try
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";

                saveFileDialog.FilterIndex = 0;

                saveFileDialog.RestoreDirectory = true;

                saveFileDialog.CreatePrompt = true;

                saveFileDialog.Title = "Export Excel File To";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog.FileName;

                }
                else goto link1;
                //if (fileName.Length > 1)
                //{
                //    ExcelApp = new Excel.Application();
                //    ExcelApp.Application.Workbooks.Add(Type.Missing);


                //    ExcelApp.ActiveWorkbook.SaveAs(fileName);
                //    ExcelApp.ActiveWorkbook.Saved = true;
                //    ExcelApp.Quit();
                //}
                ExcelExport exEx = new ExcelExport(fileName);
                exEx.excelWrite(custLst);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Файл не создан");

            }
        link1: ;

        }

        
    }

    public class getData
    {
        public List<string> name = new List<string>();
        public List<string> position = new List<string>();
        public List<string> tel = new List<string>();
        public List<string> workTel = new List<string>();
        public List<string> email = new List<string>();
        public List<string> company = new List<string>();
    }
}
