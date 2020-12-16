using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LayoutDesigner.Utils
{
    public class Debouncer
    {
        Action lastAction;
        private static readonly Object obj = new Object();
        System.Timers.Timer timer = new System.Timers.Timer();
        internal void Debounce(int delay, Action action)
        {
            lock (obj)
            {
                if (!timer.Enabled)
                {
                    timer.Interval = delay;
                    timer.Elapsed += Timer_Elapsed;
                }
            }
            lastAction = action;
            timer.Stop();
            timer.Start();
            
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Enabled = false;
            timer.Elapsed -= Timer_Elapsed;
            if (lastAction != null) lastAction();
        }
    }
}
