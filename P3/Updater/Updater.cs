namespace P3.Updater
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using NLog;

    /// <summary>Обновление ПО.</summary>
    public class SoftUpdater
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string remotePropPath = string.Empty;
        private XMLcode xml;
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

                        this.logger.Info("Обновление Phonebook Updater завершено");
                    }                
                }
                else
                {
                    if (Directory.Exists(this.remoteProp[3]))
                    {
                        this.CopyDir(this.remoteProp[3], this.updFolderPath);
                        this.logger.Info("Обновление Phonebook Updater завершено");
                    }
                    else
                    {
                        this.logger.Warn("Отсутствует папка" + this.remoteProp[3]);
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
                this.logger.Error(ex.Message);
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

                this.logger.Info("Копирование файлов Updaytera завершено");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
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

                this.logger.Info($"Удаление содержимого папки {dirPath} завершено");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
        }
    }
}