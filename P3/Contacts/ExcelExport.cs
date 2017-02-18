using P3.Model;
using P3.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace P3.Contacts
{
    class ExcelExport
    {
        private LogFile logFile = new LogFile();
        String _filePath;

        //работа с Excel
        private Excel.Application ExcelApp;
        private Excel.Workbook WorkBookExcel;
        private Excel.Worksheet WorkSheetExcel;
        private Excel.Range RangeExcel;

        public ExcelExport(String filePath)
        {
            _filePath = filePath;

        }

        public void excelWrite(ObservableCollection<Customer> custLst)
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            try
            {
                ExcelApp = new Excel.Application();
                ExcelApp.Visible = false;
                //WorkBookExcel = ExcelApp.Workbooks.Open(_filePath, false); 
                ExcelApp.Workbooks.Add();
                WorkSheetExcel = (Excel.Worksheet)ExcelApp.ActiveSheet;

                WorkSheetExcel.Cells[1, 1] = "ФИО";
                WorkSheetExcel.Cells[1, 2] = "Должность";
                WorkSheetExcel.Cells[1, 3] = "Сотовый";
                WorkSheetExcel.Cells[1, 4] = "Рабочий";
                WorkSheetExcel.Cells[1, 5] = "Почта";
                WorkSheetExcel.Cells[1, 6] = "Организация";

                int i = 0;

                foreach (Customer n in custLst)
                {
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 1];
                    RangeExcel.Value = custLst[i].FullName;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 2];
                    RangeExcel.Value = custLst[i].Position;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 3];
                    RangeExcel.Value = custLst[i].PhoneMobile;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 4];
                    RangeExcel.Value = custLst[i].PhoneWork;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 5];
                    RangeExcel.Value = custLst[i].Email;
                    RangeExcel = (Excel.Range)WorkSheetExcel.Cells[i + 2, 6];
                    RangeExcel.Value = custLst[i].Company;

                    i++;
                }

                WorkSheetExcel.SaveAs(_filePath);
                //WorkBookExcel.Close(true, Type.Missing, Type.Missing);
                ExcelApp.Quit();
                GC.Collect();

                String logText = DateTime.Now.ToString() + "|event|ExcelExport - excelWrite|Контакты сохранены в Excel " + _filePath;
                logFile.WriteLog(logText);
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|ExcelExport - excelWrite|" + exception.Message;
                logFile.WriteLog(logText);

                WorkSheetExcel.SaveAs(_filePath);
                ExcelApp.Quit();
                GC.Collect();

            }


        }
    }
}
