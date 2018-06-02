namespace P3.Updater
{
    using System.Text;
    using System.Xml;

    /// <summary>Параметры XML контактов.</summary>
    public class XMLcodeContacts
    {
        private string localPropPath;

        /// <summary>Initializes a new instance of the <see cref="XMLcodeContacts" /> class.</summary>
        /// <param name="localPropPath">Путь к файлу.</param>
        public XMLcodeContacts(string localPropPath)
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
            XmlNode upd = document.CreateElement("Contacts");
            document.DocumentElement.AppendChild(upd);

            XmlNode upd1 = document.CreateElement("Path");
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
                    if (childnode.Name == "Path")
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
            string remotePropPath = string.Empty;
            var document = new XmlDocument();
            document.Load(this.localPropPath);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // Путь к PhonebookUpdater
                    if (childnode.Name == "Path")
                    {
                        remotePropPath = childnode.InnerText;
                    }
                }
            }

            return remotePropPath;
        }
    }
}