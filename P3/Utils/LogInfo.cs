namespace P3.Utils
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>Лог информация.</summary>
    public class LogInfo
    {
        private string infoPath = @"\\elcom.local\files\01-Deps\ДПАСУТП\00_TEMP\Samoykin\log\";

        /// <summary>Сохранить информацию.</summary>
        public void SaveInfo()
        {
            var infoPathFile = this.infoPath + "\\log_" + DateTime.Now.ToString("yyy.MM.dd") + ".txt";

            var date = DateTime.Now;
            var name = Environment.UserName;
            var computerName = Environment.MachineName;
            var ip = System.Net.Dns.GetHostByName(Environment.MachineName).AddressList[0].ToString();
            string remoteBD = "0";

            if (Directory.Exists(this.infoPath))
            {
                if (!File.Exists(infoPathFile))
                {
                    FileStream fs = File.Create(infoPathFile);
                    fs.Close();
                }

                this.LogInfoWriteFile(infoPathFile, date.ToString(), name, computerName, ip);
                remoteBD = "1";
            }
        }

        private void LogInfoWriteFile(string infoPathFile, string date, string name, string computerName, string ip)
        {
            var line = date + "|" + name + "|" + computerName + "|" + ip + "|" + Application.ProductVersion;
            File.AppendAllText(infoPathFile, line + Environment.NewLine, System.Text.Encoding.Default);
        }
    }
}