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
        private string pathToXml = "prop.xml";
        private string pathForUpd = @"d:\Temp\prop.xml";

        public void CreateXml()
        {
            XmlTextWriter textWritter = new XmlTextWriter(pathToXml, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("head");
            textWritter.WriteEndElement();
            textWritter.Close();
        }

        public void CreateNodesXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(pathToXml);

            //Видимость колонок сотрудников
            XmlNode emplColVisible = document.CreateElement("emplColVisible");
            document.DocumentElement.AppendChild(emplColVisible); // указываем родителя
            //XmlAttribute attribute = document.CreateAttribute("el"); // создаём атрибут
            //attribute.Value = "checkBox"; // устанавливаем значение атрибута
            //element.Attributes.Append(attribute); // добавляем атрибут

            XmlNode t1 = document.CreateElement("tTels"); // даём имя
            t1.InnerText = "1"; // и значение
            emplColVisible.AppendChild(t1); // и указываем кому принадлежит

            XmlNode t2 = document.CreateElement("tTels2");
            t2.InnerText = "1";
            emplColVisible.AppendChild(t2);

            XmlNode t3 = document.CreateElement("tTels3");
            t3.InnerText = "1";
            emplColVisible.AppendChild(t3);

            XmlNode t4 = document.CreateElement("tEmail");
            t4.InnerText = "1";
            emplColVisible.AppendChild(t4);

            XmlNode t5 = document.CreateElement("tDiv");
            t5.InnerText = "1";
            emplColVisible.AppendChild(t5);

            XmlNode t6 = document.CreateElement("tPos");
            t6.InnerText = "1";
            emplColVisible.AppendChild(t6);

            XmlNode t7 = document.CreateElement("tFoto");
            t7.InnerText = "1";
            emplColVisible.AppendChild(t7);

            XmlNode t8 = document.CreateElement("tBirthDay");
            t8.InnerText = "1";
            emplColVisible.AppendChild(t8);

            XmlNode t9 = document.CreateElement("tStartDay");
            t9.InnerText = "1";
            emplColVisible.AppendChild(t9);

            //Видимость колонок заказчиков
            XmlNode custColVisible = document.CreateElement("custColVisible");
            document.DocumentElement.AppendChild(custColVisible);

            XmlNode c1 = document.CreateElement("custPos");
            c1.InnerText = "1";
            custColVisible.AppendChild(c1);

            XmlNode c2 = document.CreateElement("custTels");
            c2.InnerText = "1";
            custColVisible.AppendChild(c2);

            XmlNode c3 = document.CreateElement("custTels2");
            c3.InnerText = "1";
            custColVisible.AppendChild(c3);

            XmlNode c4 = document.CreateElement("custEmail");
            c4.InnerText = "1";
            custColVisible.AppendChild(c4);

            XmlNode c5 = document.CreateElement("custComp");
            c5.InnerText = "1";
            custColVisible.AppendChild(c5);

            //Ширина колонок сотрудников
            XmlNode emplColWidth = document.CreateElement("emplColWidth");
            document.DocumentElement.AppendChild(emplColWidth); // указываем родителя

            XmlNode t1W = document.CreateElement("tNamesW"); // даём имя
            t1W.InnerText = "1"; // и значение
            emplColWidth.AppendChild(t1W); // и указываем кому принадлежит

            XmlNode t2W = document.CreateElement("tTelsW");
            t2W.InnerText = "1";
            emplColWidth.AppendChild(t2W);

            XmlNode t3W = document.CreateElement("tTels2W");
            t3W.InnerText = "1";
            emplColWidth.AppendChild(t3W);

            XmlNode t4W = document.CreateElement("tTels3W");
            t4W.InnerText = "1";
            emplColWidth.AppendChild(t4W);

            XmlNode t5W = document.CreateElement("tEmailW");
            t5W.InnerText = "1";
            emplColWidth.AppendChild(t5W);

            XmlNode t6W = document.CreateElement("tDivW");
            t6W.InnerText = "1";
            emplColWidth.AppendChild(t6W);

            XmlNode t7W = document.CreateElement("tPosW");
            t7W.InnerText = "1";
            emplColWidth.AppendChild(t7W);

            XmlNode t8W = document.CreateElement("tFotoW");
            t8W.InnerText = "1";
            emplColWidth.AppendChild(t8W);

            XmlNode t9W = document.CreateElement("tBirthDayW");
            t9W.InnerText = "1";
            emplColWidth.AppendChild(t9W);

            XmlNode t10W = document.CreateElement("tStartDayW");
            t10W.InnerText = "1";
            emplColWidth.AppendChild(t10W);

            //Ширина колонок заказчиков
            XmlNode custColWidth = document.CreateElement("custColWidth");
            document.DocumentElement.AppendChild(custColWidth);

            XmlNode c1W = document.CreateElement("custNamesW");
            c1W.InnerText = "1";
            custColWidth.AppendChild(c1W);

            XmlNode c2W = document.CreateElement("custPosW");
            c2W.InnerText = "1";
            custColWidth.AppendChild(c2W);

            XmlNode c3W = document.CreateElement("custTelsW");
            c3W.InnerText = "1";
            custColWidth.AppendChild(c3W);

            XmlNode c4W = document.CreateElement("custTels2W");
            c4W.InnerText = "1";
            custColWidth.AppendChild(c4W);

            XmlNode c5W = document.CreateElement("custEmailW");
            c5W.InnerText = "1";
            custColWidth.AppendChild(c5W);

            XmlNode c6W = document.CreateElement("custCompW");
            c6W.InnerText = "1";
            custColWidth.AppendChild(c6W);

            XmlNode upd = document.CreateElement("Update");
            document.DocumentElement.AppendChild(upd);

            XmlNode upd1 = document.CreateElement("UpdPath");
            upd1.InnerText = "1";
            upd.AppendChild(upd1);

            document.Save(pathToXml);

        }

        public void WriteXml(String[] emplChBox, String[] emplWidth, String[] custChBox, String[] custWidth, String XmlForUpdate)
        {

            XmlDocument document = new XmlDocument();
            document.Load(pathToXml);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //видимость колонок сотрудников
                    if (childnode.Name == "tTels")
                        childnode.InnerText = emplChBox[0];
                    if (childnode.Name == "tTels2")
                        childnode.InnerText = emplChBox[1];
                    if (childnode.Name == "tTels3")
                        childnode.InnerText = emplChBox[2];
                    if (childnode.Name == "tEmail")
                        childnode.InnerText = emplChBox[3];
                    if (childnode.Name == "tDiv")
                        childnode.InnerText = emplChBox[4];
                    if (childnode.Name == "tPos")
                        childnode.InnerText = emplChBox[5];
                    if (childnode.Name == "tFoto")
                        childnode.InnerText = emplChBox[6];
                    if (childnode.Name == "tBirthDay")
                        childnode.InnerText = emplChBox[7];
                    if (childnode.Name == "tStartDay")
                        childnode.InnerText = emplChBox[8];

                    //ширина колонок сотрудников
                    if (childnode.Name == "tNamesW")
                        childnode.InnerText = emplWidth[0];
                    if (childnode.Name == "tTelsW")
                        childnode.InnerText = emplWidth[1];
                    if (childnode.Name == "tTels2W")
                        childnode.InnerText = emplWidth[2];
                    if (childnode.Name == "tTels3W")
                        childnode.InnerText = emplWidth[3];
                    if (childnode.Name == "tEmailW")
                        childnode.InnerText = emplWidth[4];
                    if (childnode.Name == "tDivW")
                        childnode.InnerText = emplWidth[5];
                    if (childnode.Name == "tPosW")
                        childnode.InnerText = emplWidth[6];
                    if (childnode.Name == "tFotoW")
                        childnode.InnerText = emplWidth[7];
                    if (childnode.Name == "tBirthDayW")
                        childnode.InnerText = emplWidth[8];
                    if (childnode.Name == "tStartDayW")
                        childnode.InnerText = emplWidth[9];

                    //видимость колонок заказчиков
                    if (childnode.Name == "custPos")
                        childnode.InnerText = custChBox[0];
                    if (childnode.Name == "custTels")
                        childnode.InnerText = custChBox[1];
                    if (childnode.Name == "custTels2")
                        childnode.InnerText = custChBox[2];
                    if (childnode.Name == "custEmail")
                        childnode.InnerText = custChBox[3];
                    if (childnode.Name == "custComp")
                        childnode.InnerText = custChBox[4];

                    //ширина колонок заказчиков
                    if (childnode.Name == "custNamesW")
                        childnode.InnerText = custWidth[0];
                    if (childnode.Name == "custPosW")
                        childnode.InnerText = custWidth[1];
                    if (childnode.Name == "custTelsW")
                        childnode.InnerText = custWidth[2];
                    if (childnode.Name == "custTels2W")
                        childnode.InnerText = custWidth[3];
                    if (childnode.Name == "custEmailW")
                        childnode.InnerText = custWidth[4];
                    if (childnode.Name == "custCompW")
                        childnode.InnerText = custWidth[5];

                    //Путь к PhonebookUpdater
                    if (childnode.Name == "UpdPath")
                        childnode.InnerText = XmlForUpdate;
                }
            }
            document.Save(pathToXml);
        }

        public String[] ReadXml()
        {
            String[] ch = new String[31];
            //List<String> ch = new List<string>();
            XmlDocument document = new XmlDocument();
            document.Load(pathToXml);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //видимость колонок сотрудников
                    if (childnode.Name == "tTels")
                        ch[0] = childnode.InnerText;
                    if (childnode.Name == "tTels2")
                        ch[1] = childnode.InnerText;
                    if (childnode.Name == "tTels3")
                        ch[2] = childnode.InnerText;
                    if (childnode.Name == "tEmail")
                        ch[3] = childnode.InnerText;
                    if (childnode.Name == "tDiv")
                        ch[4] = childnode.InnerText;
                    if (childnode.Name == "tPos")
                        ch[5] = childnode.InnerText;
                    if (childnode.Name == "tFoto")
                        ch[6] = childnode.InnerText;
                    if (childnode.Name == "tBirthDay")
                        ch[27] = childnode.InnerText;
                    if (childnode.Name == "tStartDay")
                        ch[28] = childnode.InnerText;

                    //ширина колонок сотрудников
                    if (childnode.Name == "tNamesW")
                        ch[7] = childnode.InnerText;
                    if (childnode.Name == "tTelsW")
                        ch[8] = childnode.InnerText;
                    if (childnode.Name == "tTels2W")
                        ch[9] = childnode.InnerText;
                    if (childnode.Name == "tTels3W")
                        ch[10] = childnode.InnerText;
                    if (childnode.Name == "tEmailW")
                        ch[11] = childnode.InnerText;
                    if (childnode.Name == "tDivW")
                        ch[12] = childnode.InnerText;
                    if (childnode.Name == "tPosW")
                        ch[13] = childnode.InnerText;
                    if (childnode.Name == "tFotoW")
                        ch[14] = childnode.InnerText;
                    if (childnode.Name == "tBirthDayW")
                        ch[29] = childnode.InnerText;
                    if (childnode.Name == "tStartDayW")
                        ch[30] = childnode.InnerText;

                    //видимость колонок заказчиков
                    if (childnode.Name == "custPos")
                        ch[15] = childnode.InnerText;
                    if (childnode.Name == "custTels")
                        ch[16] = childnode.InnerText;
                    if (childnode.Name == "custTels2")
                        ch[17] = childnode.InnerText;
                    if (childnode.Name == "custEmail")
                        ch[18] = childnode.InnerText;
                    if (childnode.Name == "custComp")
                        ch[19] = childnode.InnerText;

                    //ширина колонок заказчиков
                    if (childnode.Name == "custNamesW")
                        ch[20] = childnode.InnerText;
                    if (childnode.Name == "custPosW")
                        ch[21] = childnode.InnerText;
                    if (childnode.Name == "custTelsW")
                        ch[22] = childnode.InnerText;
                    if (childnode.Name == "custTels2W")
                        ch[23] = childnode.InnerText;
                    if (childnode.Name == "custEmailW")
                        ch[24] = childnode.InnerText;
                    if (childnode.Name == "custCompW")
                        ch[25] = childnode.InnerText;

                    //Путь к PhonebookUpdater
                    if (childnode.Name == "UpdPath")
                        ch[26] = childnode.InnerText;
                }
            }
            return ch;
        }

        public List<String> ReadXmlForUpdate(string path)
        {
            //String[] ch = new String[27];
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
