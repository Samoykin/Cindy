using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace P3.Updater
{
    class XMLcode
    {
        private string _localPropPath;
        //private string pathForUpd = @"d:\Temp\prop.xml";

        public XMLcode(String localPropPath)
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
            XmlNode upd = document.CreateElement("Update");
            document.DocumentElement.AppendChild(upd);

            XmlNode upd1 = document.CreateElement("UpdPath");
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
                    if (childnode.Name == "UpdPath")
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
                    if (childnode.Name == "UpdPath")
                        _remotePropPath = childnode.InnerText;
                }
            }
            return _remotePropPath;
        }

        public List<String> ReadRemotePropXml(String path)
        {
            List<String> ch = new List<string>();

            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //параметры PhonebookUpdater
                    if (childnode.Name == "version")
                        ch.Add(childnode.InnerText);
                    if (childnode.Name == "path")
                        ch.Add(childnode.InnerText);
                    if (childnode.Name == "versionUpd")
                        ch.Add(childnode.InnerText);
                    if (childnode.Name == "pathUpd")
                        ch.Add(childnode.InnerText);
                }
            }
            return ch;
        }

        public String ReadXmlU(string path)
        {
            String ch = "";

            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "versionUpd")
                        ch = childnode.InnerText;
                }
            }

            return ch;
        }
    }
}
