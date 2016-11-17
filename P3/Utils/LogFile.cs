using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3.Utils
{
    class LogFile
    {
        String path = @"log.txt";

        public void WriteLog(String str)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(str);
            }

        }
    }
}
