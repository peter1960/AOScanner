using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOLogScanner
{
    class LineData
    {
        private String root;
        private String proc;
        private String start;
        private String end;
        private String startTime;
        private String endTime;
        private List<String> data;
        public LineData(String RootJob) {
            root = RootJob;
            data = new List<String>();
        }


        public String GetRoot {
            get {
                return root;
            }
        }

        public String ProcName {
            get {
                return proc;
            }
            set {
                proc = value;
            }
        }
        public String StartType {
            get {
                return start;
            }
            set {
                start = value;
            }
        }
        public String EndType {
            get {
                return end;
            }
            set {
                end = value;
            }
        }
        public String StartTime {
            get {
                return startTime;
            }
            set {
                startTime = value;
            }
        }
        public String EndTime {
            get {
                return endTime;
            }
            set {
                endTime = value;
            }
        }
        public String ExtraIn {
            set {
                data.Add(value);
            }
        }
        public List<String> ExtraOut {
            get {
                return data;
            }
        }
    }
}
