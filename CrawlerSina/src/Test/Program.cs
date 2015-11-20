using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Test
{
    // 定义委托
    public delegate void NumberChangedEventHandler(int count);

    // 定义事件发布者
    public class Publishser
    {
        private int count;
        //public NumberChangedEventHandler NumberChanged; // 声明委托变量
        public event NumberChangedEventHandler NumberChanged; // 声明一个事件
        public void DoSomething()
        {
            // 在这里完成一些工作 ...
            if (NumberChanged != null)
            { // 触发事件
                count++;
                NumberChanged(count);
            }
        }
    }

    // 定义事件订阅者
    public class Subscriber
    {
        public void OnNumberChanged(int count)
        {
            Console.WriteLine("Subscriber notified: count = {0}", count);
        }
    }

    // 热水器
    public class Heater
    {
        private int temperature;
        public string type = "RealFire 001"; // 添加型号作为演示
        public string area = "China Xian"; // 添加产地作为演示
        //声明委托
        public delegate void BoiledEventHandler(Object sender, BoiledEventArgs e);
        //声明事件
        public event BoiledEventHandler Boiled; 
        // 定义BoiledEventArgs 类，传递给Observer 所感兴趣的信息
        public class BoiledEventArgs : EventArgs
        {
            public readonly int temperature;
            public BoiledEventArgs(int temperature)
            {
                this.temperature = temperature;
            }
        }
        // 可以供继承自 Heater 的类重写，以便继承类拒绝其他对象对它的监视
        protected virtual void OnBoiled(BoiledEventArgs e)
        {
            if (Boiled != null)
            { // 如果有对象注册
                Boiled(this, e); // 调用所有注册对象的方法
            }
        }
        // 烧水。
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;
                if (temperature > 95)
                {
                    //建立BoiledEventArgs 对象。
                    BoiledEventArgs e = new BoiledEventArgs(temperature);
                    OnBoiled(e); // 调用 OnBolied 方法
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
    // 警报器
    public class Alarm
    {
        public void MakeAlert(Object sender, Heater.BoiledEventArgs e)
        {
            Heater heater = (Heater)sender; //这里是不是很熟悉呢？
            //访问 sender 中的公共字段
            Console.WriteLine("Alarm：{0} - {1}: ", heater.area, heater.type);
            Console.WriteLine("Alarm: 嘀嘀嘀，水已经 {0} 度了：", e.temperature);
            Console.WriteLine();
        }
    }
    // 显示器
    public class Display
    {
        public static void ShowMsg(Object sender, Heater.BoiledEventArgs e)
        { //静态方法
            Heater heater = (Heater)sender;
            Console.WriteLine("Display：{0} - {1}: ", heater.area, heater.type);
            Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", e.temperature);
            Console.WriteLine();
        }
    }
    class Program
    {
        //创建日志记录组件实例
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            /*Publishser pub = new Publishser();
            Subscriber sub = new Subscriber();
            pub.NumberChanged += new NumberChangedEventHandler(sub.OnNumberChanged);
            pub.DoSomething(); // 应该通过DoSomething()来触发事件
            //pub.NumberChanged(100); // 但可以被这样直接调用，对委托变量的不恰当使用*/

            /*Heater heater = new Heater();
            Alarm alarm = new Alarm();
            //heater.Boiled += alarm.MakeAlert; //注册方法
            //heater.Boiled += (new Alarm()).MakeAlert; //给匿名对象注册方法
            heater.Boiled += new Heater.BoiledEventHandler(alarm.MakeAlert); //也可以这么注册
            heater.Boiled += Display.ShowMsg; //注册静态方法
            heater.BoilWater(); //烧水，会自动调用注册过对象的方法*/

            //log.Info("haha");

            /*
            StreamWriter sw = new StreamWriter(@"D:\Sina_Blog\new_crwedInfo.txt", true);
            
            string sql = "SELECT uid FROM sinauserid WHERE ischeck='-1'";
            DataTable dt = db_MySQL.DBUnit.getInstance().query(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                sw.WriteLine(dt.Rows[i][0].ToString());
            }
            sw.Flush();
            sw.Close();*/
            /*HTTPUnit.TopList all = new HTTPUnit.TopList();
            List<string> list = all.getTopHotUserList();
            foreach (string str in list)
            {
                log.Info(str);
            }*/

            string sql = "12";
            char[] c={'1','2'};
            String str = new String(c);
            Console.WriteLine(sql.Equals(str));



            Console.ReadLine();
        }
    }
}
