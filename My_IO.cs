using UnityEngine;
using System.Collections;
using System.IO;
using org.in2bits.MyXls;
using System;
using System.Collections.Generic;

public class My_IO : MonoBehaviour
{
    public static bool Flag_Save = false;

    private string path;
    List<TestInfo> listInfos = new List<TestInfo>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ExcelMakerManager.CreateExcelMakerManager();

        if (Run.Flag_Run)
        {
            TestInfo test = new TestInfo
            {
                No = Speed_Gyroscope.No_Data,
                Axis_X_Local = Dll_Import.Dll_Import.dataOutput1[6],
                Axis_Y_Local = Dll_Import.Dll_Import.dataOutput1[7],
                Axis_X_Global = Dll_Import.Dll_Import.dataOutput2[6],
                Axis_Y_Global = Dll_Import.Dll_Import.dataOutput2[7],
                Aangular_Velocity_X_Local = Dll_Import.Dll_Import.dataOutput1[3],
                Aangular_Velocity_Y_Local = Dll_Import.Dll_Import.dataOutput1[4],
                Aangular_Velocity_X_Global = Dll_Import.Dll_Import.dataOutput2[3],
                Aangular_Velocity_Y_Global = Dll_Import.Dll_Import.dataOutput2[4],
                Speed1 = Socket_Client.Speed1,
                Speed2 = Socket_Client.Speed2,
                Speed3 = Socket_Client.Speed3,
                Speed4 = Socket_Client.Speed4,
            };
            Speed_Gyroscope.No_Data++;
            listInfos.Add(test);
        }
    }
    void OnGUI()
    {
        if (Flag_Save)
        {
            PrintExcel();
            listInfos = new List<TestInfo>();
            Speed_Gyroscope.No_Data = 1;
            Flag_Save = false;
        }
    }
    public void PrintExcel()
    {
        if (!Directory.Exists(Application.dataPath + "/Prints"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Prints");
        }
        path = Application.dataPath + "/Prints/Excel_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xls";
        ExcelMakerManager.eInstance.ExcelMaker(path, listInfos);
    }
}