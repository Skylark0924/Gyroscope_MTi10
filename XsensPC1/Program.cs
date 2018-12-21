using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft;


namespace XsensPC1
{
    public class Program
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
                Thread thread = new Thread(ToExcel);//创建一个线程
                thread.Start();//开始一个线程
            }


        }
        //static void Print()
        //{
        //    IntPtr pointer = new IntPtr();
        //    float[] mydata = { 0, 0, 0, 0, 0, 0 };
        //    while (true)
        //    {
        //        pointer = mygetdata();
        //        Marshal.Copy(pointer, mydata, 0, 6);
        //        for (int i = 0; i < 6; i++) 
        //        {
        //            Console.Write(mydata[i] + " ");
        //        }
        //        Console.WriteLine();
        //        Thread.Sleep(10);
        //    }           
        //}
        static public void ToExcel()
        {
            int nMax = 9;
            int nMin = 4;
            int rowCount = 1001;//总行数

            const int columnCount = 6;//总列数

            IntPtr pointer = new IntPtr();
            float[] mydata = { 0, 0, 0, 0, 0, 0 };
            //创建Excel对象
            Excel.Application excelApp = new Excel.ApplicationClass();
            //新建工作簿
            Excel.Workbook workBook = excelApp.Workbooks.Add(true);
            //新建工作表
            Excel.Worksheet worksheet = workBook.ActiveSheet as Excel.Worksheet;
            ////设置标题

            //Excel.Range titleRange = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]);//选取单元格

            //titleRange.Merge(true);//合并单元格

            //titleRange.Value2 = strTitle; //设置单元格内文本

            //titleRange.Font.Name = "宋体";//设置字体

            //titleRange.Font.Size = 18;//字体大小

            //titleRange.Font.Bold = false;//加粗显示

            //titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;//水平居中

            //titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//垂直居中

            //titleRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//设置边框

            //titleRange.Borders.Weight = Excel.XlBorderWeight.xlThin;//边框常规粗细

            //设置表头
            string[] strHead = new string[columnCount] { "Acc_x", "Acc_y", "Acc_z", "euler1", "euler2", "euler3" };
            int[] columnWidth = new int[6] { 8, 8, 8, 8, 8, 8 };
            for (int i = 0; i < columnCount; i++)
            {

                //Excel.Range headRange = worksheet.Cells[2, i + 1] as Excel.Range;//获取表头单元格

                Excel.Range headRange = worksheet.Cells[1, i + 1] as Excel.Range;//获取表头单元格,不用标题则从1开始

                headRange.Value2 = strHead[i];//设置单元格文本

                headRange.Font.Name = "宋体";//设置字体

                headRange.Font.Size = 12;//字体大小

                headRange.Font.Bold = false;//加粗显示

                headRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;//水平居中

                headRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//垂直居中

                headRange.ColumnWidth = columnWidth[i];//设置列宽

                //  headRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//设置边框

                // headRange.Borders.Weight = Excel.XlBorderWeight.xlThin;//边框常规粗细

            }
            //设置每列格式
            for (int i = 0; i < columnCount; i++)
            {

                //Excel.Range contentRange = worksheet.get_Range(worksheet.Cells[3, i + 1], worksheet.Cells[rowCount - 1 + 3, i + 1]);

                Excel.Range contentRange = worksheet.get_Range(worksheet.Cells[2, i + 1], worksheet.Cells[rowCount - 1 + 3, i + 1]);//不用标题则从第二行开始

                contentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;//水平居中

                contentRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;//垂直居中

                //contentRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;//设置边框

                // contentRange.Borders.Weight = Excel.XlBorderWeight.xlThin;//边框常规粗细

                contentRange.WrapText = true;//自动换行

                contentRange.NumberFormatLocal = "@";//文本格式

            }
            int k = 2;
            //bool end = true;
            while (k<rowCount)
            {
                pointer = mygetdata();
                Marshal.Copy(pointer, mydata, 0, 6);
                //填充数据
                    excelApp.Cells[k, 1] =  mydata[0];

                    excelApp.Cells[k, 2] = mydata[1];

                    excelApp.Cells[k, 3] = mydata[2];

                    excelApp.Cells[k, 4] = mydata[3];

                    excelApp.Cells[k, 5] = mydata[4];

                    excelApp.Cells[k, 6] = mydata[5];
                k++;
            }
            //设置Excel可见
            excelApp.Visible = true;
        }
    }
}
