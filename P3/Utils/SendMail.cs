namespace P3.Utils
{
    using Outlook2 = Microsoft.Office.Interop.Outlook;

    /// <summary>Отправка писем.</summary>
    public class SendMail
    {
        /// <summary>Отправить письмо через Outlook.</summary>
        /// <param name="mail">Письмо.</param>
        public void SendMailOutlook(string mail)
        {
            object missingValue = System.Reflection.Missing.Value;
            var outlookApp = new Outlook2.Application();
            var mailItem = outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as Microsoft.Office.Interop.Outlook.MailItem;
            mailItem.To = mail;
            mailItem.Importance = Outlook2.OlImportance.olImportanceLow;
            mailItem.Display();
        }
    }
}