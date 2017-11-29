using ProgressBarSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FileSync
{
    class Autosync
    {
        //Runs the sync method autmatically at an interval specified in settings

        public static CustomProgressBar bar;
        public static GridMaker layoutmanager;
        public static MainDisplay me;


        public static void autoRun()
        {
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(startSync);
            myTimer.Interval = Properties.Settings.Default.autorun; 
            myTimer.Start();
        }

        
        public static void startSync(object source, ElapsedEventArgs e)
        {
         
            layoutmanager.clearAll();
            Sync.sync(Autosync.bar, layoutmanager, me);
        }

    }
}
