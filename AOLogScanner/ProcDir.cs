using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace AOLogScanner
{
    class ProcDir
    {
        Logger lg = new Logger();
        SortedList<String, String> FileOrder = new SortedList<String, String>();
        SortedList<String, LineData> JobOrder = new SortedList<String, LineData>();
        string[] AllFiles;
        long LineCounter = 0;      // count the lines, new file will start when needed
        long OutputCounter = 0;    // used for the file name

        public void Reset() {
            FileOrder.Clear();
            JobOrder.Clear();
            LineCounter = 0;
            OutputCounter = 0;
            lg.Reset();
        }

        public void SetDir(String Path) {
            AllFiles = Directory.GetFiles(Path, "p*.*");
        }

        public int GetCount {
            get {
                return AllFiles.Length;
            }
        }
        public void Start(ListBox lb) {
            // close the file if it was opned
            FileData fd;
            lb.Items.Clear();
            lg.Log("\n\nLoading Files Times ranges");
            lg.Log("=======================");
            for (int x = 0; x < Math.Min (2000,AllFiles.Length); x++) {
                fd = new FileData(AllFiles[x], ScanType.RANGE_ONLY);
                lb.Items.Add(fd.GetStart + " to " + fd.GetEnd);
                lg.Log(fd.GetStart + " to " + fd.GetEnd);
                lb.Update();
                // Add for sorting
                FileOrder.Add(fd.GetStart + " to " + fd.GetEnd, AllFiles[x]);
            }
            lb.Items.Clear();
            lg.Log("\nSorted Files");
            lg.Log("=======================");
            foreach (KeyValuePair<string, string> kvp in FileOrder) {
                lb.Items.Add(kvp.Key + " File: " + kvp.Value);
                lg.Log(kvp.Key + " File: " + kvp.Value);
            }

        }
        public void Process(ListBox lb) {

            FileData fd;
            JobOrder.Clear();
            lg.Log("\nProcess Files");
            lg.Log("=======================");

            int x = 0;

            foreach (KeyValuePair<string, string> kvp in FileOrder) {
                lb.SelectedIndex = x;
                x++;
                lb.Update();
                fd = new FileData(kvp.Value, ScanType.PROCESS_FILE);
                SortedJobs(fd);

            }

            lb.SelectedIndex = -1;
            outputData();
        }

        // Add each set of lines to the list for sort
        private void SortedJobs(FileData fd) {
            lg.Log("  " + fd.GetFile + " Job ID Lines :" + fd.GetFileData.Count.ToString());
            for (int x = 0; x < fd.GetFileData.Count; x++) {
                //lg.Log(fd.GetFileData[x].GetRoot);
                JobOrder.Add(fd.GetFileData[x].GetRoot + String.Format(" -- {0:0000000}", LineCounter++), fd.GetFileData[x]);
            }
        }
        public void outputData() {
            StreamWriter sw = null; ;
            StreamWriter job = null; ;

            if (!Directory.Exists(Properties.Settings.Default.OutputLocation + @"\jobs")) {
                Directory .CreateDirectory(Properties.Settings.Default.OutputLocation + @"\jobs");
            }

            lg.Log("\nTotal Job Lines: " + JobOrder.Count.ToString());

            String LastJob = "zzzz";
            lg.Log("Unique Jobs and Start proc");
            foreach (KeyValuePair<string, LineData> kvp in JobOrder) {
                if (kvp.Value.GetRoot != LastJob) {
                    lg.Log(kvp.Value.GetRoot + " " + kvp.Value .ProcName);
                    LastJob = kvp.Value.GetRoot;
                }
            }

            LastJob = "xzz";
//            sw.WriteLine(JobOrder.Count);
            LineCounter = 0;
            foreach (KeyValuePair<string, LineData> kvp in JobOrder) {
                if (kvp.Value.GetRoot != LastJob) {
                    if (LineCounter > 0) {
                        sw.WriteLine("</" + LastJob + ">\n");
                        job.Close();
                    }
                    job = new StreamWriter(String.Format(@"{0}\job-{1}.txt", Properties.Settings.Default.OutputLocation + @"\jobs", FixJobName(kvp.Value.GetRoot)));
                    // start new file on new job
                    // to keep the values together
                    // or is zero so this is the first file
                    if (LineCounter > 5000 || LineCounter == 0) {
                        // the file will be open so close ready for the next
                        if (LineCounter > 0) {
                            sw.Close();
                        }
                        LineCounter = 0;
                        sw = new StreamWriter(String.Format(@"{0}\out{1:000}.xml", Properties.Settings.Default.OutputLocation, OutputCounter));
                        OutputCounter++;

                    }

                    sw.WriteLine("<" + kvp.Value.GetRoot + ">");
                    sw.WriteLine("" + kvp.Value.GetRoot + " " + kvp.Value.ProcName);
                    sw.WriteLine("  " + kvp.Value.StartType + " Started at " + kvp.Value.StartTime);
                    job.WriteLine(kvp.Value.ProcName);
                    job.WriteLine("  " + kvp.Value.StartType + " Started at " + kvp.Value.StartTime);
                    LastJob = kvp.Value.GetRoot;
                }
                else {
                    sw.WriteLine("     " + kvp.Value.ProcName + " " + kvp.Value .StartType);
                    for (int y = 0; y < kvp.Value.ExtraOut.Count; y++) {
                        sw.WriteLine("        " + kvp.Value.ExtraOut[y]);
                        job.WriteLine("        " + kvp.Value.ExtraOut[y]);
                    }
                }
                LineCounter++;
            }

            sw.Close();
            job.Close();
        }
        String FixJobName(String Job) {
            Job = Job.Replace(":", "_");
            return Job;
        }
    }
}
