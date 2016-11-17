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
        String XmlForUpdate = "";
        XMLcode x1 = new XMLcode();

        public void UpdateSoft()
        {
            String[] cht = new String[26];
            cht = x1.ReadXml();
            XmlForUpdate = cht[26];


            List<String> ch = new List<string>();

            String updVer, pathToTemp = Environment.CurrentDirectory + @"\Temp\";
            String pathToUpd = Environment.CurrentDirectory + @"\Updater\";
            String updPath = pathToUpd + @"\UpdProp.xml";

            if (XmlForUpdate != "")
            {
                ch = x1.ReadXmlForUpdate(XmlForUpdate);
            }

            if (ch[0] != Application.ProductVersion && Directory.Exists(ch[1]))
            {
                updVer = x1.ReadXmlU(updPath);

                if (ch[2] != updVer && Directory.Exists(ch[3]))
                {
                    if (Directory.Exists(pathToTemp))
                    {
                        delFromDir(pathToTemp);
                    }

                    Directory.CreateDirectory(pathToTemp);

                    CopyDir(ch[3], pathToTemp);

                    DirectoryInfo dir = new DirectoryInfo(pathToUpd);
                    dir.Delete(true);

                    CopyDir(pathToTemp, pathToUpd);

                    delFromDir(pathToTemp);
                }
                if (MessageBox.Show("Появилась новая версия Phonebook! Обновить?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    Process.Start(pathToUpd + @"\PhonebookUpdater.exe");
            }
        }

        static void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }

        static void delFromDir(String dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);

            foreach (FileInfo file in dir.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in dir.GetDirectories()) subDirectory.Delete(true);

        }
    }
}
