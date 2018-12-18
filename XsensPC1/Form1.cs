
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;


namespace XsensPC1
{
    public partial class Form1 : Form
    {
        [DllImport("../../../Release/Dll1.dll", EntryPoint = "test01", CharSet = CharSet.Ansi)]
        public static extern int test01(int a, int b, int c);

        [DllImport("../../../Release/Dll1.dll", EntryPoint = "mygetCaldata", CharSet = CharSet.Ansi)]
        public static extern IntPtr mygetCaldata();

        [DllImport("../../../Release/Dll1.dll", EntryPoint = "mygetEulerdata", CharSet = CharSet.Ansi)]
        public static extern IntPtr mygetEulerdata();

        //[DllImport("../../../Release/Dll1.dll", EntryPoint = "test02", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        //public extern static int test02(int a, int b);

        SerialPort s = new SerialPort();    //实例化一个串口对象，在前端控件中可以直接拖过来，但最好是在后端代码中写代码，这样复制到其他地方不会出错。s是一个串口的句柄  
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;   //防止跨线程访问出错，好多地方会用到              
            button1.Text = "打开串口";
            int[] item = { 9600, 115200 };    //定义一个Item数组，遍历item中每一个变量a，增加到comboBox2的列表中              
            foreach (int a in item)
            {
                comboBox2.Items.Add(a.ToString());
            }

            comboBox2.SelectedItem = comboBox2.Items[1];    //默认为列表第二个变量  
           
        }
        private void Form1_Load(object sender, EventArgs e)   //窗体事件要先配置端口信息。          
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            comboBox1.SelectedItem = comboBox1.Items[0];
            Array.Sort(ports);
        }
        private void ShowMessageBySeconds1(int seconds, string showMessage)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(showMessage);
                }));
            }
            else
            {
                richTextBox1.AppendText(showMessage);
            }
            //Thread.Sleep(0 * seconds);
        }

        private void ShowMessageBySeconds2(int seconds, string showMessage)
        {
            if (richTextBox2.InvokeRequired)
            {
                richTextBox2.Invoke(new Action(() =>
                {
                    richTextBox2.AppendText(showMessage);
                }));
            }
            else
            {
                richTextBox2.AppendText(showMessage);
            }
            //Thread.Sleep(0 * seconds);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1.Text = "关闭串口";
            //    try
            //    {
            //        if (!s.IsOpen)
            //        {
            //            s.PortName = comboBox1.SelectedItem.ToString();
            //            s.BaudRate = Convert.ToInt32(comboBox2.SelectedItem.ToString());
            //            s.Open();
            //            s.DataReceived += s_DataReceived;
            //            button1.Text = "关闭串口";
            //            //MessageBox.Show("串口已打开");                  
            //        }
            //        else
            //        {
            //            s.Close();
            //            s.DataReceived -= s_DataReceived;
            //            button1.Text = "打开串口";
            //        }
            //    }
            //    catch (Exception ee)
            //    {
            //        MessageBox.Show(ee.ToString());
            //    }

            //double[] myCaldata;
            //myCaldata = new double[3];
            //myCaldata = mygetCaldata();
            //for (int i = 0; i < 3; i++)
            //{
            //    richTextBox1.AppendText(myCaldata[i].ToString() + "\n");
            //}
            //richTextBox1.Clear();
            //richTextBox2.Clear();
            //for (int j = 0; j < 10; j++)
            //{


            /////加速度与Euler角的连续输出

            //for (int j = 0; j < 50; j++)
            //{
            //    IntPtr pointer = new IntPtr();
            //    pointer = mygetCaldata();
            //    double[] myCaldata = { 0, 0, 0 };
            //    Marshal.Copy(pointer, myCaldata, 0, 3);
            //    IntPtr pointer1 = new IntPtr();
            //    pointer1 = mygetEulerdata();
            //    double[] myEulerdata = { 0, 0, 0 };
            //    Marshal.Copy(pointer1, myEulerdata, 0, 3);
            //    for (int i = 0; i < 3; i++)
            //    {
            //        richTextBox1.AppendText(myCaldata[i].ToString() + "\n");
            //        richTextBox2.AppendText(myEulerdata[i].ToString() + "\n");
            //    }
            //}



            /////加速度与Euler角的连续单独输出
            richTextBox1.Select();
            richTextBox2.Select();
            IntPtr pointer1 = new IntPtr();
            IntPtr pointer2 = new IntPtr();
            double[] myCaldata = { 0, 0, 0 };
            double[] myEulerdata = { 0, 0, 0 };
            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                for (int i = 1; i < 20; i++)
                {
                    //IntPtr pointer1 = new IntPtr();
                    pointer1 = mygetCaldata();
                    //double[] myCaldata = { 0, 0, 0 };
                    Marshal.Copy(pointer1, myCaldata, 0, 3);
                    //IntPtr pointer2 = new IntPtr();
                    pointer2 = mygetEulerdata();
                    //double[] myEulerdata = { 0, 0, 0 };
                    Marshal.Copy(pointer2, myEulerdata, 0, 3);
                    for (int j = 0; j < 3; j++)
                    {
                        ShowMessageBySeconds1(1, myCaldata[j].ToString() + Environment.NewLine);
                        ShowMessageBySeconds2(1, myEulerdata[j].ToString() + Environment.NewLine);
                    }
                    System.Threading.Thread.Sleep(100);
                    richTextBox1.Clear();
                    richTextBox2.Clear();
                }
            });
            th.IsBackground = true;
            th.Start();

            ///////加速度的连续独立输出
            //richTextBox1.Select();

            //System.Threading.Thread th1 = new System.Threading.Thread(() =>
            //{
            //    for (int i = 1; i < 1000; i++)
            //    {
            //        IntPtr pointer = new IntPtr();
            //        pointer = mygetCaldata();
            //        double[] myCaldata = { 0, 0, 0 };
            //        Marshal.Copy(pointer, myCaldata, 0, 3);
            //        for (int j = 0; j < 3; j++)
            //        {
            //            ShowMessageBySeconds1(1, myCaldata[j].ToString() + Environment.NewLine);

            //        }
            //        System.Threading.Thread.Sleep(500);
            //        richTextBox1.Clear();
            //    }
            //});
            //th1.IsBackground = true;
            //th1.Start();

            //////Euler角的连续独立输出
            //richTextBox2.Select();

            //System.Threading.Thread th2 = new System.Threading.Thread(() =>
            //{
            //    for (int i = 1; i < 1000; i++)
            //    {
            //        IntPtr pointer = new IntPtr();
            //        pointer = mygetEulerdata();
            //        double[] myEulerdata = { 0, 0, 0 };
            //        Marshal.Copy(pointer, myEulerdata, 0, 3);
            //        for (int j = 0; j < 3; j++)
            //        {
            //            ShowMessageBySeconds2(1, myEulerdata[j].ToString() + Environment.NewLine);

            //        }
            //        System.Threading.Thread.Sleep(500);
            //        richTextBox2.Clear();
            //    }
            //});
            //th2.IsBackground = true;
            //th2.Start();

            //richTextBox1.Clear();
            //IntPtr pointer = mygetCaldata();
            //double[] myCaldata = new double[3];
            //Marshal.Copy(pointer, myCaldata, 0, 3);
            //ShowMessageBySeconds(1, myCaldata[2].ToString() + Environment.NewLine);
            //    for (int j = 0; j < 100; j++)
            //    {
            //        IntPtr pointer1 = new IntPtr();
            //        pointer1 = mygetEulerdata();
            //        double[] myEulerdata = { 0, 0, 0 };
            //        Marshal.Copy(pointer1, myEulerdata, 0, 3);
            //        for (int i = 0; i < 3; i++)
            //        {
            //            richTextBox2.AppendText(myEulerdata[i].ToString() + "\n");
            //        }

            //        //System.Threading.Thread.Sleep(100);
            //        //richTextBox1.Clear();
            //    }


        }
        //void s_DataReceived(object sender, SerialDataReceivedEventArgs e)
        ////数据接收事件，读到数据的长度赋值给count，如果是8位（节点内部编程规定好的），就申请一个byte类型的buff数组，s句柄来读数据          
        //{
        //    //double r1 = mygetCaldata();
        //    int r2 = test02(5, 2);

        //    richTextBox1.Text = r2.ToString();


        //}




        //    int count = s.BytesToRead;
        //    string str = null;
        //    //if (count == 124)
        //    //{
        //        byte[] buff = new byte[count];
        //        s.Read(buff, 0, count);
        //        s.DiscardInBuffer();
        //        for (int i = 0; i < buff.Length; i++) //窗体显示                          
        //        {
        //            str += buff[i].ToString("X2");  //16进制显示                          
        //        }
        //        richTextBox1.Text += str + "\r\n";
        //        foreach (byte item in buff)
        //        //读取Buff中存的数据，转换成显示的十六进制数                  
        //        {
        //            str += item.ToString("X2") + " ";
        //        }
        //        richTextBox1.Text = System.DateTime.Now.ToString() + ": " + str + "\n" + richTextBox1.Text;      //这是跨线程访问richtextbox,原程序和DataReceived事件是两个不同的线程同时在执行                  

        //private void button3_Click(object sender, EventArgs e)   //每次发一个字节     
        //{
        //    string[] sendbuff = richTextBox2.Text.Split();  //分割输入的字符串，判断有多少个字节需要发送          
        //    Debug.WriteLine("发送字节数：" + sendbuff.Length);
        //    foreach (string item in sendbuff)
        //    {
        //        int count = 1;
        //        byte[] buff = new byte[count];
        //        buff[0] = byte.Parse(item, System.Globalization.NumberStyles.HexNumber);//格式化字符串为十六进制数值                
        //        s.Write(buff, 0, count);
        //    }
        //}
        //private void button2_Click(object sender, EventArgs e)//刷新右边的数值        
        //{
        //    int count = 1;
        //    byte[] buff = new byte[count];
        //    buff[0] = byte.Parse("04", System.Globalization.NumberStyles.HexNumber);//这里只显示04节点的信息          
        //    s.Write(buff, 0, count);
        //}
    }
}
