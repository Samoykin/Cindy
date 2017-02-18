using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook2 = Microsoft.Office.Interop.Outlook;

namespace P3.Utils
{
    class SendMail
    {
        public void SendMailOutlook(String mail)
        {
            object missingValue = System.Reflection.Missing.Value;
            var oApp = new Outlook2.Application();
            var mailItem = oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Microsoft.Office.Interop.Outlook.MailItem;
            mailItem.To = mail;
            //mailItem.Subject = "Тема";
            //mailItem.HTMLBody = "<h1>Заголовок</h1><p>Текст</p>";
            //mailItem.Attachments.Add("C:\\test.xlsx");
            mailItem.Importance = Outlook2.OlImportance.olImportanceLow;
            mailItem.Display();
        }
    }
}
