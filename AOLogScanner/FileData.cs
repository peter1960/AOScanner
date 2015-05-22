using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace AOLogScanner
{
    enum ScanType
    {
        RANGE_ONLY,
        PROCESS_FILE
    };
    class FileData
    {

        String StartTime;
        String EndTime;
        bool Start = true;
        String cFileName;
        // first item will be 0;
        int LineDataCounter = -1;
        List<LineData> MainLine = new List<LineData>();

        public FileData(String FileToOpen, ScanType type) {
            try {
                cFileName = FileToOpen;
                int nStart;
                int nEnd;
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(FileToOpen);

                //Read the first line of text
                String line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null) {
                    nStart = line.IndexOf("[Current Time=");
                    if (nStart >= 0) {
                        if (Start) {
                            Start = false;
                            StartTime = line.Substring(0, nStart);
                        }
                        else {
                            EndTime = line.Substring(0, nStart);
                        }
                        String proc = ExtractTokens(line, "[Process Name=:", "]");
                        if (type == ScanType.PROCESS_FILE) {
                            String root = ExtractTokens(line, "[Root Job Id=", "]");
                            MainLine.Add(new LineData(root));
                            LineDataCounter++;
                            MainLine[LineDataCounter].ProcName = proc;
                            MainLine[LineDataCounter].StartTime = StartTime;
                        }
                    }
                    else {
                        // when full processing then only enter here after
                        // the first start time has been seen or the iten is not in the
                        // array yet
                        if (type == ScanType.PROCESS_FILE && !Start) {
                            if (line.StartsWith("The process started. ")) {
                                MainLine[LineDataCounter].StartType = line.Substring(21);
                            }
                            else if (line.StartsWith("The process Terminated ")) {
                                MainLine[LineDataCounter].EndType = line.Substring(21);
                            }
                            else {
                                MainLine[LineDataCounter].ExtraIn = line;
                            }
                        }

                    }
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e) {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        private String ExtractTokens(string cLine, String key, String end) {
            int nStart = cLine.IndexOf(key);
            int nEnd = cLine.IndexOf(end, nStart);
            return cLine.Substring(nStart + key.Length, nEnd - (nStart + key.Length));

        }
        public String GetStart {
            get {
                return StartTime;
            }
        }
        public String GetEnd {
            get {
                return EndTime;
            }
        }
        public String GetFile {
            get {
                return cFileName;
            }
        }
        public List<LineData> GetFileData {
            get {
                return MainLine;
            }
        }
    }
}
