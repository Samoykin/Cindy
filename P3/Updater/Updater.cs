namespace P3.Updater
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    using Utils;

    /// <summary>Обновление ПО.</summary>
    public class SoftUpdater
    {
        private string remotePropPath = string.Empty;
        private XMLcode xml;
        private LogFile logFile = new LogFile();
        private List<string> remoteProp = new List<string>();
        private string updVer, tempFolderPath = Environment.CurrentDirectory + @"\Temp\";
        private string updFolderPath = Environment.CurrentDirectory + @"\Updater\";
        private string updPropPath;

        /// <summary>Initializes a new instance of the <see cref="SoftUpdater" /> class.</summary
        /// <param name="xml">XML.</param>
        public SoftUpdater(XMLcode xml)
        {
            this.xml = xml;            
        }

        /// <summary>Обновить.</summary>
        public void UpdateSoft()
        {
            this.remotePropPath = this.xml.ReadLocalPropXml();

            try
            {
                if (this.remotePropPath != string.Empty)
                {
                    this.remoteProp = this.xml.ReadRemotePropXml(this.remotePropPath);
                }

                // обновление Phonebook Updater
                if (Directory.Exists(this.updFolderPath))
                {
                    this.updPropPath = this.updFolderPath + @"\UpdaterProp.xml";
                    this.updVer = this.xml.ReadXmlU(this.updPropPath);

                    if (this.remoteProp[2] != this.updVer && Directory.Exists(this.remoteProp[3]))
                    {
                        if (Directory.Exists(this.tempFolderPath))
                        {
                            this.DelDir(this.tempFolderPath);
                        }

                        Directory.CreateDirectory(this.tempFolderPath);
                        this.CopyDir(this.remoteProp[3], this.tempFolderPath);
                        this.DelDir(this.updFolderPath);
                        this.CopyDir(this.tempFolderPath, this.updFolderPath);
                        this.DelDir(this.tempFolderPath);

                        var logText2 = DateTime.Now.ToString() + "|event|SoftUpdater - UpdateSoft| Обновление Phonebook Updater завершено";
                        this.logFile.WriteLog(logText2);
                    }                
                }
                else
                {
                    if (Directory.Exists(this.remoteProp[3]))
                    {
                        this.CopyDir(this.remoteProp[3], this.updFolderPath);

                        var logText2 = DateTime.Now.ToString() + "|event|SoftUpdater - UpdateSoft| Обновление Phonebook Updater завершено";
                        this.logFile.WriteLog(logText2);
                    }
                    else
                    {
                        var logText = DateTime.Now.ToString() + "|warning|SoftUpdater - UpdateSoft| Отсутствует папка" + this.remoteProp[3];
                        this.logFile.WriteLog(logText);
                    }                
                } 

                // обновление Phonebook
                if (this.remoteProp[0] != Application.ProductVersion && Directory.Exists(this.remoteProp[1]))
                {
                    if (MessageBox.Show("Появилась новая версия Phonebook! Обновить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Process.Start(this.updFolderPath + @"\PhonebookUpdater.exe");
                    }
                }
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|SoftUpdater - UpdateSoft|" + ex.Message;
                this.logFile.WriteLog(logText);
            }
        }       

        private void CopyDir(string fromDir, string toDir)
        {
            try
            {
                Directory.CreateDirectory(toDir);
                foreach (var s1 in Directory.GetFiles(fromDir))
                {
                    var s2 = toDir + "\\" + Path.GetFileName(s1);
                    File.Copy(s1, s2);
                }

                var logText = DateTime.Now.ToString() + "|event|SoftUpdater - CopyDir| Копирование файлов Updaytera завершено";
                this.logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|SoftUpdater - CopyDir|" + ex.Message;
                this.logFile.WriteLog(logText);
            }
        }

        private void DelDir(string dirPath)
        {
            try
            {
                var dir = new DirectoryInfo(dirPath);

                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }

                foreach (var subDirectory in dir.GetDirectories())
                {
                    subDirectory.Delete(true);
                }

                var logText = DateTime.Now.ToString() + "|event|SoftUpdater - DelDir| Удаление содержимого папки " + dirPath + " завершено";
                this.logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|SoftUpdater - DelDir|" + ex.Message;
                this.logFile.WriteLog(logText);
            }
        }
    }
}