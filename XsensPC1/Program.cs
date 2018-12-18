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

        [DllImport("Dll1", EntryPoint = "CloseCOMDevice", CharSet = CharSet.Ansi)]
        public static extern void CloseCOMDevice();

        public static byte cResult = 1;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            cResult = OpenCOMDevice();
            if (cResult == 0)
            {
                Thread thread = new Thread(Print);//创建一个线程
                thread.Start();//开始一个线程
            }


        }
        static void Print()
        {
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
