using ProgressBarSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSync
{
    class GridMaker
    {
        private TableLayoutPanel paneltable;
        private List<Label> quickpics;
        private List<String> fileNames;
        private CustomProgressBar progbar;

        public GridMaker(TableLayoutPanel inputtable, CustomProgressBar progbar)
        {
            this.paneltable = inputtable;
            this.quickpics = new List<Label>();
            this.fileNames = new List<String>();
            this.progbar = progbar;
        }

        // Add new lables to display
        public void generateTable(MainDisplay me)
        {
            int cols = this.paneltable.Width / 110;
            int offset = this.paneltable.Width - (110 * cols);
            int spacing = offset / (cols+1);
            this.paneltable.Margin = new System.Windows.Forms.Padding(spacing, 0, spacing, 0);
            this.paneltable.RowCount = (int)Math.Ceiling((double)quickpics.Count/(double)cols);
            this.paneltable.ColumnCount = cols;
            for (int i =0; i < cols; i++)
            {
                this.paneltable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110));
            }
            for (int i = 0; i < this.paneltable.RowCount; i++)
            {
                this.paneltable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140));
            }
            int counter = 0;
            for (int ai = 0; ai < this.paneltable.RowCount; ai++)
            {
                for (int i = 0; i < cols; i++)
                {
                    if (counter < quickpics.Count)
                    {
                        Label toAdd = new Label();
                        toAdd.BackColor = Color.Transparent;
                        toAdd.Parent = quickpics.ElementAt(counter);
                        toAdd.Text = fileNames.ElementAt(counter);
                        toAdd.AutoSize = false;
                        toAdd.TextAlign = ContentAlignment.MiddleCenter;
                        toAdd.Height = 15;
                        toAdd.Location = new Point(0, 95);
                        me.AggiornaContatore(quickpics.ElementAt(counter), i, ai);
                        counter = counter + 1;
                    }
                }
            }

            if (me.syncbutton.InvokeRequired)
            {

                me.syncbutton.BeginInvoke((MethodInvoker)delegate () {
                    me.syncbutton.Enabled = true;
                    me.syncbutton.BackgroundImage = Properties.Resources.sync;
                });
            }
            else
            {
                me.syncbutton.Enabled = true;
               
                me.syncbutton.BackgroundImage = Properties.Resources.sync;

            }
        }  
       
       
        public void clearAll()
        {
            if (paneltable.InvokeRequired)
            {
                paneltable.BeginInvoke((MethodInvoker)delegate () { this.paneltable.Controls.Clear(); });
            }
            else
            {
                this.paneltable.Controls.Clear();
            }
           
            this.quickpics = new List<Label>();
            this.fileNames = new List<String>();
        }

        // Create the lables (file icons) to be added to the display
        private void generateQuickPics(FileInfo file, bool onlyDisplayDownloaded)
        {
            bool genericThummbnail = false;
            String extension = (file.FullName.Substring(1 + file.FullName.LastIndexOf("."))).ToLower();
            if (extension.Equals("bmp") || extension.Equals("gif") || extension.Equals("jpg") || extension.Equals("jpeg") || extension.Equals("png") || extension.Equals("tiff"))
            {
                try
                {
                    Image quickPic = Image.FromFile(file.FullName, true);
                    if (quickPic.Width > quickPic.Height)
                    {
                        double converTer = (double)quickPic.Height / (double)quickPic.Width;
                        Label toAdd = new Label();
                        toAdd.Size = new Size(100, 110);
                        toAdd.Image = ResizeImage(quickPic, 90, (int)(converTer * 90.0));
                        toAdd.ImageAlign = ContentAlignment.TopCenter;
                        quickpics.Add(toAdd);
                        fileNames.Add(file.Name);
                    }
                    else
                    {
                        double converTer = (double)quickPic.Width / (double)quickPic.Height;
                        Label toAdd = new Label();
                        toAdd.Size = new Size(100, 110);
                        toAdd.Image = ResizeImage(quickPic, (int)(converTer * 90.0), 90);
                        toAdd.ImageAlign = ContentAlignment.TopCenter;
                        quickpics.Add(toAdd);
                        fileNames.Add(file.Name);
                    }
                    quickPic.Dispose();
                }
                catch (OutOfMemoryException e)
                {
                    // Image is corrupted; display thumbnail instead of prevoew
                    genericThummbnail = true;
                }
            }
            else // check if file contains readable text. If file is composed of normal (ASCII <128), it is readable text and not encrypted
            {
                bool kepChecking = true;
                if (onlyDisplayDownloaded && !this.fileNames.Contains(file.Name))
                {
              
                    return;
                }
                try
                {
                    String text = System.IO.File.ReadAllText(file.FullName);
                    int failedchars = 0;
                    if (text.Length > 100)
                    {
                        text = text.Substring(0, 100);
                    }
                    foreach (char b in text.ToCharArray())
                    {
                        if (kepChecking)
                        {
                            if (failedchars > 5)
                            {
                                kepChecking = false;
                            }
                            if ((int)b > 127)
                            {
                                failedchars = failedchars + 1;
                            }
                        }
                    }
                    if (kepChecking)
                    {
                        Label toAdd = new Label();
                        toAdd.Padding = new System.Windows.Forms.Padding(8, 10, 20, 25);
                        toAdd.BackgroundImage = Properties.Resources.tfile;
                        toAdd.BackgroundImageLayout = ImageLayout.None;
                        toAdd.MaximumSize = new Size(100, 110);
                        toAdd.AutoSize = true;
                        toAdd.Text = text;
                        quickpics.Add(toAdd);
                        fileNames.Add(file.Name);
                    }
                    else
                    {
                        //encrypted file
                        genericThummbnail = true;
                    }
                    // some files are weird and can throw errors. they definetly aren't text
                } catch (System.IO.IOException)
                {
                    genericThummbnail = true;
                }
            }
            if (genericThummbnail)
            {
                Label toAdd = new Label();
                toAdd.Width = 100;
                toAdd.Padding = new System.Windows.Forms.Padding(10, 5, 20, 25);
                toAdd.Height = 110;
                toAdd.Image = Properties.Resources.nonfile;
                quickpics.Add(toAdd);
                fileNames.Add(file.Name);
            }
                

        }

        // Scan files and call for icons to be generated
        public void generateFromFolder(MainDisplay me, bool onlyDisplayIfDownloaded)
        {
            this.quickpics.Clear();
            this.fileNames.Clear();
            if (me.syncbutton.InvokeRequired)
            {

                me.syncbutton.BeginInvoke((MethodInvoker)delegate () {
                    me.syncbutton.Enabled = false;
                    
                });
            }
            else
            {
                me.syncbutton.Enabled = false;


            }
          
            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.runpath);
            FileInfo[] Files = d.GetFiles();
            Array.Sort(Files, delegate (FileInfo x, FileInfo y) { return x.Name.Substring(0,x.Name.LastIndexOf(".")).CompareTo(y.Name.Substring(0,y.Name.LastIndexOf("."))); });
            Array.Reverse(Files);
    
            foreach (FileInfo file in Files)
            {

                me.AggiornaContatore2(Files.Count());
                generateQuickPics(file, onlyDisplayIfDownloaded);
               
            }
            this.generateTable(me);
        }

       // Resize found image to match grid constriants
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
