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

        List<String> remoteProp = new List<string>();

        String updVer, tempFolderPath = Environment.CurrentDirectory + @"\Temp\";
        String updFolderPath = Environment.CurrentDirectory + @"\Updater\";
        String updPropPath;

        public SoftUpdater(XMLcode _xml)
        {
            xml = _xml;
            
        }


        public void UpdateSoft()
        {
            _remotePropPath = xml.ReadLocalPropXml();

            try
            {
                if (_remotePropPath != "")
                {
                    remoteProp = xml.ReadRemotePropXml(_remotePropPath);
                }
                //обновление Phonebook Updater
                if (Directory.Exists(updFolderPath))
                {
                    updPropPath = updFolderPath + @"\UpdaterProp.xml";
                    updVer = xml.ReadXmlU(updPropPath);

                    if (remoteProp[2] != updVer && Directory.Exists(remoteProp[3]))
                    {
                        if (Directory.Exists(tempFolderPath))
                        {
                            DelDir(tempFolderPath);
                        }

                        Directory.CreateDirectory(tempFolderPath);

                        CopyDir(remoteProp[3], tempFolderPath);

                        DelDir(updFolderPath);

                        CopyDir(tempFolderPath, updFolderPath);

                        DelDir(tempFolderPath);

                        String logText2 = DateTime.Now.ToString() + "|event|SoftUpdater - UpdateSoft| Обновление Phonebook Updater завершено";
                        logFile.WriteLog(logText2);
                    }

                
                }
                else
                {
                    if (Directory.Exists(remoteProp[3]))
                    {
                        CopyDir(remoteProp[3], updFolderPath);

                        String logText2 = DateTime.Now.ToString() + "|event|SoftUpdater - UpdateSoft| Обновление Phonebook Updater завершено";
                        logFile.WriteLog(logText2);
                    }
                    else
                    {
                        String logText = DateTime.Now.ToString() + "|warning|SoftUpdater - UpdateSoft| Отсутствует папка" + remoteProp[3];
                        logFile.WriteLog(logText);
                    }
                
                }

                


                //обновление Phonebook
                if (remoteProp[0] != Application.ProductVersion && Directory.Exists(remoteProp[1]))
                {

                    if (MessageBox.Show("Появилась новая версия Phonebook! Обновить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        Process.Start(updFolderPath + @"\PhonebookUpdater.exe");


                }
               

            }
            catch (Exception ex)
            {
                String logText = DateTime.Now.ToString() + "|fail|SoftUpdater - UpdateSoft|" + ex.Message;
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
            catch (Exception ex)
            {
                String logText = DateTime.Now.ToString() + "|fail|SoftUpdater - CopyDir|" + ex.Message;
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

                String logText = DateTime.Now.ToString() + "|event|SoftUpdater - DelDir| Удаление содержимого папки " + dirPath + " завершено";
                logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                String logText = DateTime.Now.ToString() + "|fail|SoftUpdater - DelDir|" + ex.Message;
                logFile.WriteLog(logText);
            }
        }
    }
}
