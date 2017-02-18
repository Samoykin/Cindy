using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace P3.Updater
{
    class XMLcodeContacts
    {
        private string _localPropPath;

        public XMLcodeContacts(String localPropPath)
        {
            _localPropPath = localPropPath;
        }

        public void CreateXml()
        {
            XmlTextWriter textWritter = new XmlTextWriter(_localPropPath, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("head");
            textWritter.WriteEndElement();
            textWritter.Close();
        }

        public void CreateNodesXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(_localPropPath);

            //Путь до файла с настройками обновления
            XmlNode upd = document.CreateElement("Contacts");
            document.DocumentElement.AppendChild(upd);

            XmlNode upd1 = document.CreateElement("Path");
            upd1.InnerText = "1";
            upd.AppendChild(upd1);

            document.Save(_localPropPath);

        }

        public void WriteXml(String XmlForUpdate)
        {

            XmlDocument document = new XmlDocument();
            document.Load(_localPropPath);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //Путь к PhonebookUpdater
                    if (childnode.Name == "Path")
                        childnode.InnerText = XmlForUpdate;
                }
            }
            document.Save(_localPropPath);
        }

        public String ReadLocalPropXml()
        {
            String _remotePropPath = "";
            XmlDocument document = new XmlDocument();
            document.Load(_localPropPath);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //Путь к PhonebookUpdater
                    if (childnode.Name == "Path")
                        _remotePropPath = childnode.InnerText;
                }
            }
            return _remotePropPath;
        }

        

    
    }
}
