namespace P3.Updater
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using Model;
    using NLog;    
    using Utils;
    using static Model.RemoteSettingsShell;
    using static Model.SettingsShell;
    using static Model.UpdaterSettingsShell;

    /// <summary>Обновление ПО.</summary>
    public class SoftUpdater
    {
        private const string UpdaterPropName = "UpdaterProp.xml";
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string remotePropPath = string.Empty;
        private RootElement settings;
        private string tempFolderPath = Path.Combine(Environment.CurrentDirectory, "Temp");
        private string updFolderPath = Path.Combine(Environment.CurrentDirectory, "Updater");
        private string updPropPath;

        // Конфигурация        
        private RootElementRemoteSettings remoteSettings = new RootElementRemoteSettings();
        private RootElementUpdaterSettings updaterSettings = new RootElementUpdaterSettings();

        /// <summary>Initializes a new instance of the <see cref="SoftUpdater" /> class.</summary
        /// <param name="settings">Параметры.</param>
        public SoftUpdater(RootElement settings)
        {
            this.settings = settings;            
        }

        /// <summary>Обновить.</summary>
        public void UpdateSoft()
        {
            this.remotePropPath = this.settings.SoftUpdate.RemoteSettingsPath;

            // Вычитывание параметров из удаленного xml
            // Инициализация модели настроек
            var settingsXml = new SettingsXml<RootElementRemoteSettings>(this.settings.SoftUpdate.RemoteSettingsPath);
            this.remoteSettings.Phonebook = new Phonebook();
            this.remoteSettings.PhonebookUpd = new PhonebookUpd();

            try
            {
                if (!string.IsNullOrEmpty(this.settings.SoftUpdate.RemoteSettingsPath))
                {
                    this.remoteSettings = settingsXml.ReadXml(this.remoteSettings);
                }

                // Обновление Phonebook Updater
                if (Directory.Exists(this.updFolderPath))
                {
                    this.updPropPath = Path.Combine(this.updFolderPath, UpdaterPropName);

                    // Вычитывание параметров из xml updatera
                    // Инициализация модели настроек
                    var updSettingsXml = new SettingsXml<RootElementUpdaterSettings>(this.updPropPath);
                    this.updaterSettings.Updater = new UpdaterSettingsShell.Updater();

                    this.updaterSettings = updSettingsXml.ReadXml(this.updaterSettings);

                    if (this.remoteSettings.PhonebookUpd.Version != this.updaterSettings.Updater.Version && Directory.Exists(this.remoteSettings.PhonebookUpd.Path))
                    {
                        if (Directory.Exists(this.tempFolderPath))
                        {
                            this.DelDir(this.tempFolderPath);
                        }

                        Directory.CreateDirectory(this.tempFolderPath);
                        this.CopyDir(this.remoteSettings.PhonebookUpd.Path, this.tempFolderPath);
                        this.DelDir(this.updFolderPath);
                        this.CopyDir(this.tempFolderPath, this.updFolderPath);
                        this.DelDir(this.tempFolderPath);

                        this.logger.Info("Обновление Phonebook Updater завершено");
                    }                
                }
                else
                {
                    if (Directory.Exists(this.remoteSettings.PhonebookUpd.Path))
                    {
                        this.CopyDir(this.remoteSettings.PhonebookUpd.Path, this.updFolderPath);
                        this.logger.Info("Обновление Phonebook Updater завершено");
                    }
                    else
                    {
                        this.logger.Warn($"Отсутствует папка {this.remoteSettings.PhonebookUpd.Path}");
                    }                
                } 

                // Обновление Phonebook
                if (this.remoteSettings.Phonebook.Version != Application.ProductVersion && Directory.Exists(this.remoteSettings.Phonebook.Path))
                {
                    if (MessageBox.Show("Появилась новая версия Phonebook! Обновить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Process.Start(Path.Combine(this.updFolderPath, "PhonebookUpdater.exe"));
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
                Directory.CreateDirectory(toDir);
                foreach (var nameFrom in Directory.GetFiles(fromDir))
                {
                    var nameTo = Path.Combine(toDir, Path.GetFileName(nameFrom));
                    File.Copy(nameFrom, nameTo);
                }

                this.logger.Info("Копирование файлов Updaytera завершено");
        }

        private void DelDir(string dirPath)
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
    }
}