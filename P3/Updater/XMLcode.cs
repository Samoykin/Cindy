namespace P3.Updater
{
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    /// <summary>Параметры XML.</summary>
    public class XMLcode
    {
        private string localPropPath;

        /// <summary>Initializes a new instance of the <see cref="XMLcode" /> class.</summary>
        /// <param name="localPropPath">Путь к файлу.</param>
        public XMLcode(string localPropPath)
        {
            this.localPropPath = localPropPath;
        }

        /// <summary>Создать XML.</summary>
        public void CreateXml()
        {
            var textWritter = new XmlTextWriter(this.localPropPath, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("head");
            textWritter.WriteEndElement();
            textWritter.Close();
        }

        /// <summary>Создать узлы XML.</summary>
        public void CreateNodesXml()
        {
            var document = new XmlDocument();
            document.Load(this.localPropPath);

            // Путь до файла с настройками обновления
            XmlNode upd = document.CreateElement("Update");
            document.DocumentElement.AppendChild(upd);

            XmlNode upd1 = document.CreateElement("UpdPath");
            upd1.InnerText = "1";
            upd.AppendChild(upd1);

            document.Save(this.localPropPath);
        }

        /// <summary>Записать в XML.</summary>
        /// <param name="xmlForUpdate">Параметры.</param>
        public void WriteXml(string xmlForUpdate)
        {
            var document = new XmlDocument();
            document.Load(this.localPropPath);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // Путь к PhonebookUpdater
                    if (childnode.Name == "UpdPath")
                    {
                        childnode.InnerText = xmlForUpdate;
                    }
                }
            }

            document.Save(this.localPropPath);
        }

        /// <summary>Прочитать из локального XML.</summary>
        /// <returns>Параметры.</returns>
        public string ReadLocalPropXml()
        {
            var remotePropPath = string.Empty;
            var document = new XmlDocument();
            document.Load(this.localPropPath);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // Путь к PhonebookUpdater
                    if (childnode.Name == "UpdPath")
                    {
                        remotePropPath = childnode.InnerText;
                    }
                }
            }

            return remotePropPath;
        }

        /// <summary>Прочитать из удаленного XML.</summary>
        /// <param name="path">Путь.</param>
        /// <returns>Параметры.</returns>
        public List<string> ReadRemotePropXml(string path)
        {
            var ch = new List<string>();

            var document = new XmlDocument();
            document.Load(path);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // параметры PhonebookUpdater
                    if (childnode.Name == "version")
                    {
                        ch.Add(childnode.InnerText);
                    }

                    if (childnode.Name == "path")
                    {
                        ch.Add(childnode.InnerText);
                    }

                    if (childnode.Name == "versionUpd")
                    {
                        ch.Add(childnode.InnerText);
                    }

                    if (childnode.Name == "pathUpd")
                    {
                        ch.Add(childnode.InnerText);
                    }
                }
            }

            return ch;
        }

        /// <summary>Прочитать из XML 2.</summary>
        /// <param name="path">Путь.</param>
        /// <returns>Параметры.</returns>
        public string ReadXmlU(string path)
        {
            var ch = string.Empty;

            var document = new XmlDocument();
            document.Load(path);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "versionUpd")
                    {
                        ch = childnode.InnerText;
                    }
                }
            }

            return ch;
        }
    }
}