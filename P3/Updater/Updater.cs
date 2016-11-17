using P3.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace P3.Updater
{
    class SoftUpdater
    {
        String _remotePropPath = "";
        //String _localPropPath;
        XMLcode xml;

        LogFile logFile = new LogFile();

        public SoftUpdater(XMLcode _xml)
        {
            xml = _xml;            
        }


        public void UpdateSoft()
        {
            try
            {
                _remotePropPath = xml.ReadLocalPropXml();

                List<String> remoteProp = new List<string>();

                String updVer, tempFolderPath = Environment.CurrentDirectory + @"\Temp\";
                String updFolderPath = Environment.CurrentDirectory + @"\Updater\";
                String updPropPath = updFolderPath + @"\UpdaterProp.xml";

                if (_remotePropPath != "")
                {
                    remoteProp = xml.ReadRemotePropXml(_remotePropPath);
                }

                if (remoteProp[0] != Application.ProductVersion && Directory.Exists(remoteProp[1]))
                {
                    updVer = xml.ReadXmlU(updPropPath);

                    if (remoteProp[2] != updVer && Directory.Exists(remoteProp[3]))
                    {
                        if (Directory.Exists(tempFolderPath))
                        {
                            DelDir(tempFolderPath);
                        }

                        Directory.CreateDirectory(tempFolderPath);

                        CopyDir(remoteProp[3], tempFolderPath);

                        DirectoryInfo dir = new DirectoryInfo(updFolderPath);
                        dir.Delete(true);

                        CopyDir(tempFolderPath, updFolderPath);

                        DelDir(tempFolderPath);
                    }
                    //if (MessageBox.Show("Появилась новая версия Phonebook! Обновить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        //Process.Start(updFolderPath + @"\PhonebookUpdater.exe");


                }
                String logText = DateTime.Now.ToString() + "|event|SoftUpdater - UpdateSoft| Обновление завершено";
                logFile.WriteLog(logText);

            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|SoftUpdater - UpdateSoft|" + exception.Message;
                logFile.WriteLog(logText);
            }
        }

        public void CopyDir(string FromDir, string ToDir)
        {
            try
            {
                Directory.CreateDirectory(ToDir);
                foreach (string s1 in Directory.GetFiles(FromDir))
                {
                    string s2 = ToDir + "\\" + Path.GetFileName(s1);
                    File.Copy(s1, s2);
                }

                String logText = DateTime.Now.ToString() + "|event|SoftUpdater - CopyDir| Копирование файлов Updaytera завершено";
                logFile.WriteLog(logText);
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|SoftUpdater - CopyDir|" + exception.Message;
                logFile.WriteLog(logText);
            }
            //foreach (string s in Directory.GetDirectories(FromDir))
            //{
            //    CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            //}
        }

        public void DelDir(String dirPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);

                foreach (FileInfo file in dir.GetFiles()) file.Delete();
                foreach (DirectoryInfo subDirectory in dir.GetDirectories()) subDirectory.Delete(true);

                String logText = DateTime.Now.ToString() + "|event|SoftUpdater - DelDir| Удаление папки Temp завершено";
                logFile.WriteLog(logText);
            }
            catch (Exception exception)
            {
                String logText = DateTime.Now.ToString() + "|fail|SoftUpdater - DelDir|" + exception.Message;
                logFile.WriteLog(logText);
            }
        }
    }
}
