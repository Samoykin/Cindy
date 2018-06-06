namespace P3.Utils
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>Лог информация.</summary>
    public class LogInfo
    {
        private const string InfoPath = @"\\elcom.local\files\01-Deps\ДПАСУТП\00_TEMP\Samoykin\log\";

        /// <summary>Сохранить информацию.</summary>
        public void SaveInfo()
        {
            var infoPathFile = InfoPath + "\\log_" + DateTime.Now.ToString("yyy.MM.dd") + ".txt";

            var date = DateTime.Now;
            var name = Environment.UserName;
            var computerName = Environment.MachineName;
            var ip = System.Net.Dns.GetHostByName(Environment.MachineName).AddressList[0].ToString();

            if (Directory.Exists(InfoPath))
            {
                if (!File.Exists(infoPathFile))
                {
                    FileStream fs = File.Create(infoPathFile);
                    fs.Close();
                }

                this.LogInfoWriteFile(infoPathFile, date.ToString(), name, computerName, ip);
            }
        }

        private void LogInfoWriteFile(string infoPathFile, string date, string name, string computerName, string ip)
        {
            var line = date + "|" + name + "|" + computerName + "|" + ip + "|" + Application.ProductVersion;
            File.AppendAllText(infoPathFile, line + Environment.NewLine, System.Text.Encoding.Default);
        }
    }
}