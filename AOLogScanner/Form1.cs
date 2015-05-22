using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOLogScanner
{
    public partial class Form1 : Form
    {
        ProcDir theDir = new ProcDir();
        public Form1() {
            InitializeComponent();
            textSource.Text = Properties.Settings.Default.SourceLocation;
            textOutput.Text = Properties.Settings.Default.OutputLocation;

        }

        private void button1_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            theDir.Reset();
            theDir.SetDir(textSource.Text);
            textStatus.Text = theDir.GetCount.ToString();
            theDir.Start(listResults);
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            button2.Enabled = false;
            theDir.Process(listResults);
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void buttonSource_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textSource.Text;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK) {
                textSource.Text = fbd.SelectedPath;
                Properties.Settings.Default.SourceLocation = textSource.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonOutput_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = textOutput.Text;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK) {
                textOutput.Text = fbd.SelectedPath;
                Properties.Settings.Default.OutputLocation = textOutput.Text;
                Properties.Settings.Default.Save();

            }
        }
    }
}
