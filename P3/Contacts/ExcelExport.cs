namespace P3.Contacts
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    
    using Model;
    using Utils;
    using Excel = Microsoft.Office.Interop.Excel;

    /// <summary>Экспорт данных в Excel.</summary>
    public class ExcelExport
    {
        private LogFile logFile = new LogFile();
        private string filePath;

        // работа с Excel
        private Excel.Application excelApp;
        private Excel.Worksheet workSheetExcel;
        private Excel.Range rangeExcel;

        /// <summary>Initializes a new instance of the <see cref="ExcelExport" /> class.</summary>
        /// <param name="filePath">Путь к файлу.</param>
        public ExcelExport(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>Записать в Excel.</summary>
        /// <param name="custLst">Коллекция контактов.</param>
        public void ExcelWrite(ObservableCollection<Customer> custLst)
        {
            if (File.Exists(this.filePath))
            {
                File.Delete(this.filePath);
            }

            try
            {
                this.excelApp = new Excel.Application();
                this.excelApp.Visible = false;

                this.excelApp.Workbooks.Add();
                this.workSheetExcel = (Excel.Worksheet)this.excelApp.ActiveSheet;

                this.workSheetExcel.Cells[1, 1] = "ФИО";
                this.workSheetExcel.Cells[1, 2] = "Должность";
                this.workSheetExcel.Cells[1, 3] = "Сотовый";
                this.workSheetExcel.Cells[1, 4] = "Рабочий";
                this.workSheetExcel.Cells[1, 5] = "Почта";
                this.workSheetExcel.Cells[1, 6] = "Организация";

                int i = 0;

                foreach (Customer n in custLst)
                {
                    this.rangeExcel = (Excel.Range)this.workSheetExcel.Cells[i + 2, 1];
                    this.rangeExcel.Value = custLst[i].FullName;
                    this.rangeExcel = (Excel.Range)this.workSheetExcel.Cells[i + 2, 2];
                    this.rangeExcel.Value = custLst[i].Position;
                    this.rangeExcel = (Excel.Range)this.workSheetExcel.Cells[i + 2, 3];
                    this.rangeExcel.Value = custLst[i].PhoneMobile;
                    this.rangeExcel = (Excel.Range)this.workSheetExcel.Cells[i + 2, 4];
                    this.rangeExcel.Value = custLst[i].PhoneWork;
                    this.rangeExcel = (Excel.Range)this.workSheetExcel.Cells[i + 2, 5];
                    this.rangeExcel.Value = custLst[i].Email;
                    this.rangeExcel = (Excel.Range)this.workSheetExcel.Cells[i + 2, 6];
                    this.rangeExcel.Value = custLst[i].Company;

                    i++;
                }

                this.workSheetExcel.SaveAs(this.filePath);

                this.excelApp.Quit();
                GC.Collect();

                var logText = DateTime.Now.ToString() + "|event|ExcelExport - excelWrite|Контакты сохранены в Excel " + this.filePath;
                this.logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|ExcelExport - excelWrite|" + ex.Message;
                this.logFile.WriteLog(logText);

                this.workSheetExcel.SaveAs(this.filePath);
                this.excelApp.Quit();
                GC.Collect();
            }
        }
    }
}