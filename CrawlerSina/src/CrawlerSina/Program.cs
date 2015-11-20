#define i_debug

using NetDimension.Weibo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;

namespace CrawlerSina
{
    class Program
    {
        private static System.Timers.Timer timer;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
#if i_debug
            clsMain obj = new clsMain();
            Thread t = new Thread(obj.Run);
            t.Start();
            //判断子线程是否结束
            while (t.IsAlive)
            {
                Thread.Sleep(1000);
            }
            Console.WriteLine("抓取结束......");
            Console.ReadLine();
#else
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            while (true)
            {
                Thread.Sleep(10 * 60 * 1000);
            }
#endif
        }
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dateTime = e.SignalTime;
            Thread.CurrentThread.IsBackground = false;
            //抓取任务每天xx点xx分开始执行......
            int hour = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["startTimeHour"]);
            int minute = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["startTimeMinute"]);
            if (dateTime.Hour == hour && dateTime.Minute == minute)
            {
                timer.Stop();

                Console.WriteLine(dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "抓取开始......");
                clsMain obj = new clsMain();
                Thread t = new Thread(obj.Run);
                t.Start();
                //判断子线程是否结束
                while (t.IsAlive)
                {
                    Thread.Sleep(1000);
                }
                Console.WriteLine(dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "抓取结束......");

                timer.Start();
            }
        }
    }
}
