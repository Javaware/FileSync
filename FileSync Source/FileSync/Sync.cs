using AisUriProviderApi;
using ProgressBarSample;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace FileSync
{

  
    class Sync
    {
        public static bool STOP = false;
        public static bool isSyncing = false;
        // Delete all other files, split up downloads into new threads and start them
        public static void sync(CustomProgressBar progbar, GridMaker displayController, MainDisplay me)
        {
            if (STOP || isSyncing)
                return;

            isSyncing = true;
            try
            {
                DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.runpath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (IOException )
            {
                Thread.CurrentThread.Abort();
            }
            AisUriProvider api = new AisUriProvider();
            List<Uri> filesFromServer = api.Get().ToList();
            if (progbar.InvokeRequired)
            {
                progbar.BeginInvoke((MethodInvoker)delegate () { progbar.Maximum = filesFromServer.Count(); });
            }
            else
            {
                progbar.Maximum = filesFromServer.Count();
            }
            for (int i = 0; i < filesFromServer.Count; i = i + (filesFromServer.Count / Properties.Settings.Default.n))
            {
                int lowerBound = i;
                int upperBound = i + (filesFromServer.Count / Properties.Settings.Default.n);
                
                  
                    Thread t3 = new Thread(() => downloadPattern(filesFromServer, lowerBound, upperBound,progbar, displayController,me));
                    t3.Start();
                }
                
              
            }

        // Fired when sync ends... either when complete or canceled
        public static void finishFile(CustomProgressBar bar, GridMaker displayController, MainDisplay me, bool onlyDisplayDownloaded)
        {

            Debug.WriteLine("done syncing!");
            me.cancel = false;
            System.Threading.Thread.Sleep(1000);
            if (bar.InvokeRequired)
            {

                bar.BeginInvoke((MethodInvoker)delegate () { bar.Value = 0; });
                bar.BeginInvoke((MethodInvoker)delegate () { bar.UpdateText(); });
                bar.BeginInvoke((MethodInvoker)delegate () { bar.Refresh(); });
                bar.BeginInvoke((MethodInvoker)delegate () { bar.UpdateText(); });
            }
            else
            {
                bar.Value = 0;
                bar.UpdateText();
                bar.Refresh();
                bar.UpdateText();

            }
            displayController.generateFromFolder(me, onlyDisplayDownloaded);
            isSyncing = false;
            
        }

       
        // Used to download multiple files in list. Useful for threading (IE thread1 should download items 1-3, thread2 should download items 4-6, etc.)
        private static void downloadPattern(List<Uri> filesFromServer, int startIndex, int endIndex, CustomProgressBar bar, GridMaker displayController, MainDisplay me)
        {
            if (STOP)
            {
                return;
            }
            for (int i = startIndex; i < endIndex; i = i + 1)
            {

                if (i < filesFromServer.Count)
                {
                    String file = filesFromServer.ElementAt(i).ToString();
                    String fileExtension = file.Substring(file.LastIndexOf("/") + 1);
                 
                    try
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(filesFromServer.ElementAt(i));
                        req.Timeout = 120000;
                        req.ReadWriteTimeout = 120000;
                        var w = (HttpWebResponse)req.GetResponse();

                        using (Stream filer = File.OpenWrite(Properties.Settings.Default.runpath + "\\" + fileExtension))
                        {
                            w.GetResponseStream().CopyTo(filer);
                        }
                    }
                    catch (System.Net.WebException e)
                    {
                        MessageBox.Show("Please check your internet connection. Heres what we know:\n" + e.Message, "An error was incurred",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Debug.WriteLine("file");
                        if (STOP)
                        {
                            FileInfo tfile = new FileInfo(Properties.Settings.Default.runpath + "\\" + fileExtension);
                            Debug.WriteLine(tfile.Exists);
                            tfile.Delete();
                            return;
                        }
                        if (bar.InvokeRequired)
                        {
                            bar.BeginInvoke((MethodInvoker)delegate () { bar.Step = 1; });
                            bar.BeginInvoke((MethodInvoker)delegate () { bar.PerformStep(); });
                        }
                        else
                        {
                            bar.BeginInvoke((MethodInvoker)delegate () { bar.Step = 1; });
                            bar.BeginInvoke((MethodInvoker)delegate () { bar.PerformStep(); });
                        }
                        if (bar.Value+1 >= bar.Maximum )
                        {
                            finishFile(bar, displayController, me, false);
                        }
                    
                  
                    
                }
            }
        }
        
    }
}
