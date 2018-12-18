using UnityEngine;
using System.Collections;
using org.in2bits.MyXls;
using System.Collections.Generic;
using System;
public class TestInfo
{
    public int No;
    public float Axis_X_Local;
    
    public float Axis_X_Global;
    public float Axis_Y_Local;
    public float Axis_Y_Global;
    public float Aangular_Velocity_X_Local;
    public float Aangular_Velocity_Y_Local;
    public float Aangular_Velocity_X_Global;
    public float Aangular_Velocity_Y_Global;
    public int Speed1;
    public int Speed2;
    public int Speed3;
    public int Speed4;
};
public class ExcelMakerManager
{

    public static ExcelMakerManager eInstance;
    public static ExcelMakerManager CreateExcelMakerManager()
    {
        if (eInstance == null)
        {
            eInstance = new ExcelMakerManager();
        }
        return eInstance;
    }
    //链表为 物体信息 .
    public void ExcelMaker(string name, List<TestInfo> listInfo)
    {
        XlsDocument xls = new XlsDocument();//新建一个xls文档
        xls.FileName = name; //设定文件名

        //Add some metadata (visible from Excel under File -> Properties)
        xls.SummaryInformation.Author = "Harry"; //填加xls文件作者信息
        xls.SummaryInformation.Subject = "test";//填加文件主题信息

        string sheetName = "Sheet0";
        Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);//填加名为"chc 实例"的sheet页
        Cells cells = sheet.Cells;//Cells实例是sheet页中单元格（cell）集合

        int rowNum = listInfo.Count;
        int rowMin = 1;
        int row = 0;

        for (int x = 0; x < rowNum + 1; x++)
        {
            if (x == 0)
            {
                cells.Add(1, 1, "序号");
                cells.Add(1, 2, "平台X轴偏角");
                cells.Add(1, 3, "平台Y轴偏角");
                cells.Add(1, 4, "底板X轴偏角");
                cells.Add(1, 5, "平底板Y轴偏角");
                cells.Add(1, 6, "平台X轴角速度");
                cells.Add(1, 7, "平台Y轴角速度");
                cells.Add(1, 8, "底板X轴角速度");
                cells.Add(1, 9, "底板Y轴角速度");
                cells.Add(1, 10, "一号电机速度");
                cells.Add(1, 11, "二号电机速度");
                cells.Add(1, 12, "三号电机速度");
                cells.Add(1, 13, "四号电机速度");
            }
            else
            {
                cells.Add(rowMin + x, 1, listInfo[row].No);
                cells.Add(rowMin + x, 2, listInfo[row].Axis_X_Local);
                cells.Add(rowMin + x, 3, listInfo[row].Axis_Y_Local);
                cells.Add(rowMin + x, 4, listInfo[row].Axis_X_Global);
                cells.Add(rowMin + x, 5, listInfo[row].Axis_Y_Global);
                cells.Add(rowMin + x, 6, listInfo[row].Aangular_Velocity_X_Local);
                cells.Add(rowMin + x, 7, listInfo[row].Aangular_Velocity_Y_Local);
                cells.Add(rowMin + x, 8, listInfo[row].Aangular_Velocity_X_Global);
                cells.Add(rowMin + x, 9, listInfo[row].Aangular_Velocity_Y_Global);
                cells.Add(rowMin + x, 10, listInfo[row].Speed1);
                cells.Add(rowMin + x, 11, listInfo[row].Speed2);
                cells.Add(rowMin + x, 12, listInfo[row].Speed3);
                cells.Add(rowMin + x, 13, listInfo[row].Speed4);
                row++;
            }
        }
        xls.Save();
    }
}