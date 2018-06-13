namespace P3.Utils
{
    using System;
    using NLog;

    using Outlook2 = Microsoft.Office.Interop.Outlook;

    /// <summary>Отправка писем.</summary>
    public class SendMail
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>Отправить письмо через Outlook.</summary>
        /// <param name="mail">Письмо.</param>
        public void SendMailOutlook(string mail)
        {
            try
            {
                var outlookApp = new Outlook2.Application();
                var mailItem =
                    outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem) as
                        Microsoft.Office.Interop.Outlook.MailItem;
                if (mailItem != null)
                {
                    mailItem.To = mail;
                    mailItem.Importance = Outlook2.OlImportance.olImportanceLow;
                    mailItem.Display();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }
    }
}