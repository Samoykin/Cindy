using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P3.Utils
{
    class LogInfo
    {
        String infoPath = @"\\elcom.local\files\01-Deps\ДПАСУТП\00_TEMP\Samoykin\log\";

        public void saveInfo()
        {

            String infoPathFile = infoPath + "\\log_" + DateTime.Now.ToString("yyy.MM.dd") + ".txt";

            DateTime date = DateTime.Now;
            String name = Environment.UserName;
            String pcName = Environment.MachineName;
            String ip = System.Net.Dns.GetHostByName(Environment.MachineName).AddressList[0].ToString();
            String remoteBD = "0";

            if (Directory.Exists(infoPath))
            {
                if (!File.Exists(infoPathFile))
                {
                    FileStream fs = File.Create(infoPathFile);
                    fs.Close();
                }

                logInfoWriteFile(infoPathFile, date.ToString(), name, pcName, ip);
                remoteBD = "1";
            }

            //dbc.InfoWrite(date, name, pcName, ip, remoteBD);
        }


        public void logInfoWriteFile(String infoPathFile, String date, String name, String pcName, String ip)
        {
            String line = date + "|" + name + "|" + pcName + "|" + ip + "|" + Application.ProductVersion;
            File.AppendAllText(infoPathFile, line + Environment.NewLine, System.Text.Encoding.Default);
        }
    }
}
