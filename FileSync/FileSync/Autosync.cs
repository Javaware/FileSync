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
           
            Thread.Sleep(Properties.Settings.Default.autorun);
            startSync();
        }

        
        public static void startSync()
        {

            me.cancel = true;
            me.syncbutton.Refresh();
            me.syncbutton.BackgroundImage = Properties.Resources.abort;
            me.LayoutManager.clearAll();

            layoutmanager.clearAll();
            Sync.sync(Autosync.bar, layoutmanager, me);
            me.cancel = false;
            me.syncbutton.Refresh();
            me.syncbutton.BackgroundImage = Properties.Resources.sync;
            autoRun();

            
        }

    }
}
