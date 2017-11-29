using FileSync.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSync
{
    public partial class MainDisplay : Form
    {
        public MainDisplay()
        {
           InitializeComponent();
           
        }

        private void mainForm_FormClosing(object sender, System.ComponentModel.CancelEventArgs e, MainDisplay me)
        {
            if (me.cancel == true)
            {
                if (MessageBox.Show("A sync is in progress. Are you sure you would like to exit?", "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    e.Cancel = false;
                    this.Activate();
                }
                else
                {
                    e.Cancel = true;
                    this.Activate();
                }

            }
            else
            {
                e.Cancel = false;
                this.Activate();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
                    }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            String directory = folderBrowserDialog1.SelectedPath;
            if (result == DialogResult.OK )
            {
                textBox1.Text = directory;
                Properties.Settings.Default.runpath = directory;
                Properties.Settings.Default.Save();
            }
        }

    }
}
