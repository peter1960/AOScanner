using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AOLogScanner
{
    class Logger
    {
        StreamWriter lg;
        public Logger() {
        }
        public void Log(String cLine) {
            lg = new StreamWriter(Properties.Settings.Default.OutputLocation + @"\Log.txt",true);
            lg.WriteLine(cLine);
            lg.Close();
        }
        public void Reset() {
            lg = new StreamWriter(Properties.Settings.Default.OutputLocation + @"\Log.txt");
            lg.Close();
        }
    }

}
