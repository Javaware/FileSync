using ProgressBarSample;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace FileSync
{
    partial class MainDisplay : System.Windows.Forms.Form
    {
      
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

       // Establish main GUI componets
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDisplay));
            this.Icon = Properties.Resources.logo;
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.syncbutton = new System.Windows.Forms.Button();
            this.gearButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new CustomProgressBar(Properties.Settings.Default.runpath);
            this.LayoutManager = new GridMaker(tableLayoutPanel1, textBox1);
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
           
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.textBox1);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.syncbutton);
            this.flowLayoutPanel1.Controls.Add(this.gearButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(584, 25);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(30, 6, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Directory: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox1.Location = new System.Drawing.Point(125, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(320, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = global::FileSync.Properties.Settings.Default.runpath;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Location = new System.Drawing.Point(450, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 20);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // syncbutton
            // 
          
            this.syncbutton.BackgroundImage = global::FileSync.Properties.Resources.sync;
            this.syncbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.syncbutton.Location = new System.Drawing.Point(480, 3);
            this.syncbutton.Name = "syncbutton";
            this.syncbutton.Size = new System.Drawing.Size(20, 20);
            this.syncbutton.TabIndex = 1;
            this.syncbutton.UseVisualStyleBackColor = true;
            this.syncbutton.Click += new System.EventHandler(this.sync_Click);
            // 
            // gearButton
            // 
            this.gearButton.BackgroundImage = global::FileSync.Properties.Resources.wrench;
            this.gearButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gearButton.Location = new System.Drawing.Point(500, 3);
            this.gearButton.Name = "gearButton";
            this.gearButton.Size = new System.Drawing.Size(20, 20);
            this.gearButton.TabIndex = 1;
            this.gearButton.UseVisualStyleBackColor = true;
            this.gearButton.Click += new System.EventHandler(this.gear_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = global::FileSync.Properties.Settings.Default.runpath;
            // 
            // tableLayoutPanel1
            // 
         
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 435);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // MainDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "MainDisplay";
            this.Text = "FileSync";
            this.FormClosing +=  (sender, e) => mainForm_FormClosing(sender, e, this);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
         
        }
        // Initiate primary file system scan to display icons
        private void Form1_Shown(Object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Autosync.bar = this.textBox1;
                Autosync.me = this;
                Autosync.layoutmanager = this.LayoutManager;
                Autosync.autoRun();
            }).Start();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                this.LayoutManager.generateFromFolder(this, false);
            }).Start();
            
        }

        //Adds icon to frame
        public  void AggiornaContatore(Label toAdd, int a1, int a2)
        {
            if (this.tableLayoutPanel1.InvokeRequired)
            {
                this.tableLayoutPanel1.BeginInvoke((MethodInvoker)delegate () { this.tableLayoutPanel1.Controls.Add(toAdd, a1, a2); });
            }
            else
            {
                this.tableLayoutPanel1.Controls.Add(toAdd, a1, a2); 
            }
        }

        //Changes Progress bar
        public void AggiornaContatore2(int maximum)
        {
            if (Sync.STOP)
            {
                return;
            }
            if (this.tableLayoutPanel1.InvokeRequired)
            {
                this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.Step = 1; });
                this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.Maximum = maximum; });
                this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.PerformStep(); });
            }
            else
            {
                Debug.WriteLine("Add");
                this.textBox1.UpdateText();
                this.textBox1.Step = 1;
                this.textBox1.Maximum = maximum;
                this.textBox1.PerformStep();
            }

            if (this.textBox1.Value >= this.textBox1.Maximum-1)
            {
                if (this.tableLayoutPanel1.InvokeRequired)
                {

                    this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.Value = 0; });
                    this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.UpdateText(); });
                    this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.Refresh(); });
                    this.textBox1.BeginInvoke((MethodInvoker)delegate () { this.textBox1.UpdateText(); });
                }
                else
                {
                    this.textBox1.Value = 0;
                    this.textBox1.UpdateText();
                    this.textBox1.Refresh();
                    this.textBox1.UpdateText();

                }
            }
        }

       
        //Start settings display
        private void gear_Click(object sender, EventArgs e)
        {
            new SettingsDisplay();
        }

        //Manual sync
        private void sync_Click(object sender, EventArgs e)
        {
            if (!cancel)
            {
                cancel = true;
            this.syncbutton.Refresh();
            this.syncbutton.BackgroundImage = Properties.Resources.abort;
            this.LayoutManager.clearAll();
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Autosync.bar = this.textBox1;
                Autosync.me = this;
                Autosync.layoutmanager = this.LayoutManager;
                Autosync.startSync(null, null);
            }).Start();
        }
            else
            {
                Debug.WriteLine("cancel");
                cancel = false;
                this.syncbutton.Refresh();
                this.syncbutton.Enabled = false;
                this.syncbutton.BackgroundImage = Properties.Resources.sync;
                Sync.STOP = true;
                Sync.finishFile(this.textBox1,this.LayoutManager, this, true);
            }
        }

        public bool cancel = false;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private CustomProgressBar textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button gearButton;
        public System.Windows.Forms.Button syncbutton;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private GridMaker LayoutManager;
    }
}

