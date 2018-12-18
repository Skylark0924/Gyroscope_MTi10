using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


namespace XsensPC1
{
    static class Program
    {

        [DllImport("Dll1", EntryPoint = "OpenCOMDevice", CharSet = CharSet.Ansi)]
        public static extern Byte OpenCOMDevice();

        [DllImport("Dll1", EntryPoint = "mygetdata", CharSet = CharSet.Ansi)]
        public static extern IntPtr mygetdata();

        [DllImport("Dll1", EntryPoint = "mygetCaldata", CharSet = CharSet.Ansi)]
        public static extern IntPtr mygetCaldata();

        [DllImport("Dll1", EntryPoint = "mygetEulerdata", CharSet = CharSet.Ansi)]
        public static extern IntPtr mygetEulerdata();

        [DllImport("Dll1.dll", EntryPoint = "CloseCOMDevice", CharSet = CharSet.Ansi)]
        public static extern void CloseCOMDevice();

        public static byte cResult = 1;

        //[DllImport(@"../../../Release/Dll1.dll", EntryPoint = "test01", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        //extern static int test01(int a, int b, int c);

        //[DllImport(@"../../../Release/Dll1.dll", EntryPoint = "test02", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        //extern static int test02(int a, int b);
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //int r1 = test01(1, 2, 3);
            //int r2 = test02(5, 2);
            //Console.WriteLine("test01结果：" + r1.ToString());
            //Console.WriteLine("test02结果：" + r2.ToString());
            //Console.Read();
            //IntPtr pCaldata = Marshal.AllocHGlobal(128);
            //Marshal.WriteByte(pCaldata, 0);
            //mygetCaldata(pCaldata);
            //string strRes = Marshal.PtrToStringAnsi(pCaldata);
            //Console.WriteLine(strRes.ToString());

            //Marshal.FreeHGlobal(pCaldata);

            //IntPtr pRet = mygetCaldata();
            //string strRet = Marshal.PtrToStringAnsi(pRet);
            //Console.WriteLine("返回值：");
            //Console.WriteLine(strRet);

            //double[] ab = mygetCaldata();
            //IntPtr inper = mygetCaldata();
            //Console.WriteLine("返回值：");
            //Console.WriteLine(inper);
            //for (int j=0;j<10000; j++)
            //{
            //    IntPtr pointer = mygetCaldata();
            //    double[] myCaldata = new double[3];
            //    Marshal.Copy(pointer, myCaldata, 0, 3);
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Console.WriteLine(myCaldata[i] + "\n");
            //    }

            //}
           
            cResult = OpenCOMDevice();
            if (cResult == 0)
            {
                Thread thread = new Thread(Print);//创建一个线程
                thread.Start();//开始一个线程
            }


        }
        static void Print()
        {
            //IntPtr pointer1 = new IntPtr();
            //IntPtr pointer2 = new IntPtr();
            //float[] myCaldata = { 0, 0, 0 };
            //float[] myEulerdata = { 0, 0, 0 };
            //while (true) {
            //    pointer1 = mygetCaldata();
            //    Marshal.Copy(pointer1, myCaldata, 0, 3);
            //    pointer2 = mygetEulerdata();
            //    Marshal.Copy(pointer2, myEulerdata, 0, 3);
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Console.Write(myCaldata[i]+"  ");
            //    }
            //    for (int i = 0; i < 3; i++)
            //    {
            //        Console.Write(myEulerdata[i]+"  ");
            //    }
            //    Console.WriteLine();
            //    Thread.Sleep(10);
            //}

            IntPtr pointer = new IntPtr();
            float[] mydata = { 0, 0, 0, 0, 0, 0 };
            while (true)
            {
                pointer = mygetdata();
                Marshal.Copy(pointer, mydata, 0, 6);
                for (int i = 0; i < 6; i++) 
                {
                    Console.Write(mydata[i] + " ");
                }
                Console.WriteLine();
                Thread.Sleep(10);
            }
             
        }
   
    }
}
