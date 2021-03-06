// Dll1.cpp : 定义 DLL 应用程序的导出函数。
//
#ifdef _XSENS
#include "xsens_internal.h"
#else
#define KEY "demo1"
#endif

#define HYCOMMONWINAPI_EXPORTS
/*#include <stdio.h>*/		// Needed for printf etc
#include <objbase.h>	// Needed for COM functionality
#include "XsensCMT.h"
//#include <conio.h>		// included for _getch and _kbhit
#include "stdafx.h"
#include "Dll1.h"
#include "math.h"


using namespace std;


// this macro tests for an error and exits the program with a message if there was one
#define EXIT_ON_ERROR(res,comment) if (res != XRV_OK) { /*printf("Error %d occurred in " comment ": %s\n",res,cmtGetResultText(res));*/ exit(1); }


// used to signal that the user initiated the exit, so we do not wait for an extra keypress-
int userQuit = 0;
CmtOutputMode mode;
CmtOutputSettings settings;
unsigned short mtCount = 0;
int screenSensorOffset = 0;
int temperatureOffset = 0;
CmtDeviceId deviceIds[256];
CmtCalData caldata;
CmtEuler euler_data;
XsensResultValue res = XRV_OK;
char serialNumber[24] = "0445F-0F456-486A9-4851A";
long instance = cmtCreateInstance(serialNumber);
unsigned short sdata;
double tdata;


signed char __stdcall OpenCOMDevice()
{
	doHardwareScan();
	autoselectmode();
	doMtSettings();
	return 0;
}

float* __stdcall mygetdata()
{
	float *mydata = new float[6];

	res = cmtGetNextDataBundle(instance);
	res = cmtDataGetSampleCounter(instance, &sdata, deviceIds[0], NULL);
	for (int i = 0; i < mtCount; i++) {
		res = cmtDataGetTemp(instance, &tdata, deviceIds[i], NULL);
		if ((mode & CMT_OUTPUTMODE_CALIB) != 0) {
			res = cmtDataGetCalData(instance, &caldata, deviceIds[i]);
			for (int j = 0; j < 3; j++)
			{

				mydata[j] =/*(float)(round(*/caldata.m_acc.m_data[j]/**100)/100.0)*/;

			}
		}
		res = cmtDataGetOriEuler(instance, &euler_data, deviceIds[i]);
		mydata[3] = /*(float)(round(*/euler_data.m_pitch/* * 100) / 100.0)*/;
		mydata[4] = /*(float)(round(*/euler_data.m_roll/* * 100) / 100.0)*/;
		mydata[5] = /*(float)(round(*/euler_data.m_yaw /** 100) / 100.0)*/;
	}

	return mydata;
}

float* __stdcall mygetCaldata()
{
	float *myCaldata=new float[3];
	
	res = cmtGetNextDataBundle(instance);
	res = cmtDataGetSampleCounter(instance, &sdata, deviceIds[0], NULL);
	for (int i = 0; i < mtCount; i++) {
		res = cmtDataGetTemp(instance, &tdata, deviceIds[i], NULL);
		if ((mode & CMT_OUTPUTMODE_CALIB) != 0) {
			res = cmtDataGetCalData(instance, &caldata, deviceIds[i]);
			for (int j = 0; j < 3; j++)
			{
				
				myCaldata[j] =/*(float)(round(*/caldata.m_acc.m_data[j]/**100)/100.0)*/;
				
			}
		}
	}
	return myCaldata;
}

float* __stdcall mygetEulerdata()
{

	float *myEulerdata = new float[3];

	res = cmtGetNextDataBundle(instance);
	res = cmtDataGetSampleCounter(instance, &sdata, deviceIds[0], NULL);
	for (int i = 0; i < mtCount; i++) {
		res = cmtDataGetTemp(instance, &tdata, deviceIds[i], NULL);		
		res = cmtDataGetOriEuler(instance, &euler_data, deviceIds[i]);
		myEulerdata[0] = /*(float)(round(*/euler_data.m_pitch/* * 100) / 100.0)*/;
		myEulerdata[1] = /*(float)(round(*/euler_data.m_roll/* * 100) / 100.0)*/;
		myEulerdata[2] = /*(float)(round(*/euler_data.m_yaw /** 100) / 100.0)*/;
	}
	return myEulerdata;
}

void __stdcall CloseCOMDevice()
{
		cmtClose(instance);
		cmtDestroyInstance(instance);
}

//int main(int argc, char* argv[])
//{
//	(void)argc; (void)argv;
//	doHardwareScan();
//	autoselectmode();
//	doMtSettings();
//}



//int main(int argc, char* argv[])
//{
//	(void)argc; (void)argv;	// Make the compiler stop complaining about unused parameters
//
//	XsensResultValue res = XRV_OK;
//	short screenSkipFactor = 10;
//	short screenSkipFactorCnt = screenSkipFactor;
//
//	// Set exit function
//	atexit(exitFunc);
//
//	// lets create the Xsens CMT instance to handle the sensor(s)
//	/*printf("Creating an XsensCMT instance\n");*/
//
//	///// *********  Enter your own serial code here... ie. "12345-12345-12345-12345"
//	///// This number is just here for demo purposes
//	char serialNumber[24] = KEY;
//	//if (strcmp(serialNumber, "demo1") == 0)
//	//	printf("Warning: Using \"demo1\" as a serial code will limit CMT functionality to 1000 calls. Enter your own serial code for unlimited CMT functionality.\n");
//
//	instance = cmtCreateInstance(serialNumber);
//	//if (instance != -1)
//	//	printf("CMT instance created\n\n");
//	//else {
//	//	printf("Creation of CMT instance failed, probably because of an invalid serial number\n");
//	//	exit(1);
//	//}
//
//	// Perform hardware scan
//	doHardwareScan();
//
//	// Give user a (short) chance to see hardware scan results
//	Sleep(2000);
//
//	//clear screen present & get the user output mode selection.
//	clrscr();
//	/*getUserInputs();*/
//	autoselectmode();
//
//	// Set device to user input settings
//	doMtSettings();
//
//	// Wait for first data item(s) to arrive. In production code, you would use a callback function instead (see cmtSetCallbackFunction)
//	Sleep(20);
//
//	//get the placement offsets, clear the screen and write the fixed headers.
//	calcScreenOffset();
//	clrscr();
//	writeHeaders();
//
//	// vars for sample counter & temp.
//	unsigned short sdata;
//	double tdata;
//
//	//structs to hold data.
//	CmtCalData caldata;
//	CmtQuat qat_data;
//	CmtEuler euler_data;
//	CmtMatrix matrix_data;
//
//	while (!userQuit && res == XRV_OK)
//	{
//		//get the bundle of data
//		res = cmtGetNextDataBundle(instance);
//		Sleep(10);
//
//		//get sample count, goto position & display.
//		res = cmtDataGetSampleCounter(instance, &sdata, deviceIds[0], NULL);
//		//gotoxy(0, 0);
//		//printf("Sample Counter %05hu\n", sdata);
//
//		if (screenSkipFactorCnt++ == screenSkipFactor) {
//			screenSkipFactorCnt = 0;
//
//			for (unsigned int i = 0; i < mtCount; i++) {
//				// Output Temperature
//				//if ((mode & CMT_OUTPUTMODE_TEMP) != 0) {
//				//	gotoxy(0, 4 + i * screenSensorOffset);
//				//	res = cmtDataGetTemp(instance, &tdata, deviceIds[i], NULL);
//				//	printf("%6.2f", tdata);
//				//}
//
//				gotoxy(0, 5 + temperatureOffset + i * screenSensorOffset);	// Output Calibrated data
//				if ((mode & CMT_OUTPUTMODE_CALIB) != 0) {
//
//					res = cmtDataGetCalData(instance, &caldata, deviceIds[i]);
//					//printf("%6.2f\t%6.2f\t%6.2f", caldata.m_acc.m_data[0], caldata.m_acc.m_data[1], caldata.m_acc.m_data[2]);
//
//					//gotoxy(0, 7 + temperatureOffset + i * screenSensorOffset);
//					//printf("%6.2f\t%6.2f\t%6.2f", caldata.m_gyr.m_data[0], caldata.m_gyr.m_data[1], caldata.m_gyr.m_data[2]);
//
//					//gotoxy(0, 9 + temperatureOffset + i * screenSensorOffset);
//					//printf("%6.2f\t%6.2f\t%6.2f", caldata.m_mag.m_data[0], caldata.m_mag.m_data[1], caldata.m_mag.m_data[2]);
//					//gotoxy(0, 13 + temperatureOffset + i * screenSensorOffset);
//				}
//
//				if ((mode & CMT_OUTPUTMODE_ORIENT) != 0) {
//					switch (settings & CMT_OUTPUTSETTINGS_ORIENTMODE_MASK) {
//					case CMT_OUTPUTSETTINGS_ORIENTMODE_QUATERNION:
//						// Output: quaternion
//						res = cmtDataGetOriQuat(instance, &qat_data, deviceIds[i]);
//						printf("%6.3f\t%6.3f\t%6.3f\t%6.3f\n", qat_data.m_data[0], qat_data.m_data[1], qat_data.m_data[2], qat_data.m_data[3]);
//						break;
//
//					case CMT_OUTPUTSETTINGS_ORIENTMODE_EULER:
//						// Output: Euler
//						res = cmtDataGetOriEuler(instance, &euler_data, deviceIds[i]);
//						//printf("%6.1f\t%6.1f\t%6.1f\n", euler_data.m_roll, euler_data.m_pitch, euler_data.m_yaw);
//						break;
//
//					case CMT_OUTPUTSETTINGS_ORIENTMODE_MATRIX:
//						// Output: Cosine Matrix
//						res = cmtDataGetOriMatrix(instance, &matrix_data, deviceIds[i], NULL);
//						printf("%6.3f\t%6.3f\t%6.3f\n", matrix_data.m_data[0][0], matrix_data.m_data[0][1], matrix_data.m_data[0][2]);
//						printf("%6.3f\t%6.3f\t%6.3f\n", matrix_data.m_data[1][0], matrix_data.m_data[1][1], matrix_data.m_data[1][2]);
//						printf("%6.3f\t%6.3f\t%6.3f\n", matrix_data.m_data[2][0], matrix_data.m_data[2][1], matrix_data.m_data[2][2]);
//						break;
//					default:
//						;
//					}
//				}
//			}
//		}
//
//		if (_kbhit())
//			userQuit = 1;
//	}
//
//	clrscr();
//	cmtClose(instance);
//
//	return 0;
//}

//////////////////////////////////////////////////////////////////////////
// doHardwareScan
//
// Checks available COM ports and scans for MotionTrackers
void doHardwareScan()
{
	XsensResultValue res;
	CmtPortInfo portInfo[256];
	unsigned long portCount = 0;

	/*printf("Scanning for connected Xsens devices...");*/
	res = cmtScanPorts(portInfo, &portCount, 0);
	//EXIT_ON_ERROR(res, "cmtScanPorts");
	/*printf("done\n");*/

	if (portCount == 0) {
		/*printf("No MotionTrackers found\n\n");*/
		//exit(0);
	}

	//for (int i = 0; i < (int)portCount; i++) {
	//	printf("Using COM port %d at %d baud\n\n",
	//		(long)portInfo[i].m_portNr, portInfo[i].m_baudrate);
	//}

	//printf("Opening ports...");
	//open the port which the device is connected to and connect at the device's baudrate.
	for (int p = 0; p < (int)portCount; p++) {
		res = cmtOpenPort(instance, portInfo[p].m_portNr, portInfo[p].m_baudrate);
	//	EXIT_ON_ERROR(res, "cmtOpenPort");
	}
	//printf("done\n\n");

	//get the Mt sensor count.
	//printf("Retrieving MotionTracker count (excluding attached Xbus Master(s))\n");
	res = cmtGetMtCount(instance, &mtCount);
	//EXIT_ON_ERROR(res, "cmtGetMtCount");
	//printf("MotionTracker count: %i\n\n", mtCount);

	// retrieve the device IDs 
	//printf("Retrieving MotionTrackers device ID(s)\n");
	for (unsigned int j = 0; j < mtCount; j++) {
		res = cmtGetMtDeviceId(instance, &deviceIds[j], j);
		//EXIT_ON_ERROR(res, "cmtGetDeviceId");
	//	printf("Device ID at index %i: %08x\n", j, (long)deviceIds[j]);
	}

	// make sure that we get the freshest data
	//printf("\nSetting queue mode so that we always get the latest data\n\n");
	res = cmtSetQueueMode(instance, CMT_QM_LAST);
	//EXIT_ON_ERROR(res, "cmtSetQueueMode");
}

//////////////////////////////////////////////////////////////////////////
 //getUserInputs

 //Request user for output data
//void getUserInputs()
//{
//	do{
//		printf("Select desired output:\n");
//		printf("1 - Calibrated data\n");
//		printf("2 - Orientation data\n");
//		printf("3 - Both Calibrated and Orientation data\n");
//		printf("4 - Temperature and Calibrated data\n");
//		printf("5 - Temperature and Orientation data\n");
//		printf("6 - Temperature, Calibrated and Orientation data\n");
//		printf("Enter your choice: ");
//		scanf_s("%d", &mode);
//		// flush stdin
//		while (getchar() != '\n') continue;
//
//		if (mode < 1 || mode > 6) {
//			printf("\n\nPlease enter a valid output mode\n");
//		}
//	}while(mode < 1 || mode > 6);
//	clrscr();
//
//	switch(mode)
//	{
//	case 1:
//		mode = CMT_OUTPUTMODE_CALIB;
//		break;
//	case 2:
//		mode = CMT_OUTPUTMODE_ORIENT;
//		break;
//	case 3:
//		mode = CMT_OUTPUTMODE_CALIB | CMT_OUTPUTMODE_ORIENT;
//		break;
//	case 4:
//		mode = CMT_OUTPUTMODE_TEMP | CMT_OUTPUTMODE_CALIB;
//		break;
//	case 5:
//		mode = CMT_OUTPUTMODE_TEMP | CMT_OUTPUTMODE_ORIENT;
//		break;
//	case 6:
//		mode = CMT_OUTPUTMODE_TEMP | CMT_OUTPUTMODE_CALIB | CMT_OUTPUTMODE_ORIENT;
//		break;
//	}
//
//	if ((mode & CMT_OUTPUTMODE_ORIENT) != 0) {
//		do{
//			printf("Select desired output format\n");
//			printf("1 - Quaternions\n");
//			printf("2 - Euler angles\n");
//			printf("3 - Matrix\n");
//			printf("Enter your choice: ");
//			scanf_s("%d", &settings);
//			// flush stdin
//			while (getchar() != '\n') continue;
//
//			if (settings < 1  || settings > 3) {
//				printf("\n\nPlease enter a valid choice\n");
//			}
//		}while(settings < 1 || settings > 3);
//
//		// Update outputSettings to match data specs of SetOutputSettings
//		switch(settings) {
//		case 1:
//			settings = CMT_OUTPUTSETTINGS_ORIENTMODE_QUATERNION;
//			break;
//		case 2:
//			settings = CMT_OUTPUTSETTINGS_ORIENTMODE_EULER;
//			break;
//		case 3:
//			settings = CMT_OUTPUTSETTINGS_ORIENTMODE_MATRIX;
//			break;
//		}
//	}
//	else{
//		settings = 0;
//	}
//	settings |= CMT_OUTPUTSETTINGS_TIMESTAMP_SAMPLECNT;
//}

void autoselectmode()
{
	mode = CMT_OUTPUTMODE_CALIB | CMT_OUTPUTMODE_ORIENT;
	settings = CMT_OUTPUTSETTINGS_ORIENTMODE_EULER;
	settings |= CMT_OUTPUTSETTINGS_TIMESTAMP_SAMPLECNT;
}

//////////////////////////////////////////////////////////////////////////
// doMTSettings
//
// Set user settings in MTi/MTx
// Assumes initialized global MTComm class
void doMtSettings()
{
	XsensResultValue res;

	// set sensor to config sate
	res = cmtGotoConfig(instance);
	//EXIT_ON_ERROR(res, "cmtGotoConfig");

	unsigned short sampleFreq;
	res = cmtGetSampleFrequency(instance, &sampleFreq, deviceIds[0]);
	printf("%d", sampleFreq);
	// set the device output mode for the device(s)
	/*printf("Configuring your mode selection");*/
	sampleFreq = 50;
	for (int i = 0; i < mtCount; i++) {
		res = cmtSetDeviceMode(instance, mode, settings, sampleFreq, deviceIds[i]);
		//EXIT_ON_ERROR(res, "setDeviceMode");
	}

	// start receiving data
	res = cmtGotoMeasurement(instance);
	//EXIT_ON_ERROR(res, "cmtGotoMeasurement");
}

//////////////////////////////////////////////////////////////////////////
// writeHeaders
//
// Write appropriate headers to screen
//void writeHeaders()
//{
//	for (unsigned int i = 0; i < mtCount; i++) {
//		gotoxy(0, 2 + i * screenSensorOffset);
//		printf("MotionTracker %d\n", i + 1);
//
//		if ((mode & CMT_OUTPUTMODE_TEMP) != 0) {
//			temperatureOffset = 3;
//			gotoxy(0, 3 + i * screenSensorOffset);
//			printf("Temperature");
//			gotoxy(7, 4 + i * screenSensorOffset);
//			printf("degrees celcius");
//			gotoxy(0, 6 + i * screenSensorOffset);
//		}
//
//		if ((mode & CMT_OUTPUTMODE_CALIB) != 0) {
//			gotoxy(0, 3 + temperatureOffset + i * screenSensorOffset);
//			printf("Calibrated sensor data");
//			gotoxy(0, 4 + temperatureOffset + i * screenSensorOffset);
//			printf(" Acc X\t Acc Y\t Acc Z");
//			gotoxy(23, 5 + temperatureOffset + i * screenSensorOffset);
//			printf("(m/s^2)");
//			gotoxy(0, 6 + temperatureOffset + i * screenSensorOffset);
//			printf(" Gyr X\t Gyr Y\t Gyr Z");
//			gotoxy(23, 7 + temperatureOffset + i * screenSensorOffset);
//			printf("(rad/s)");
//			gotoxy(0, 8 + temperatureOffset + i * screenSensorOffset);
//			printf(" Mag X\t Mag Y\t Mag Z");
//			gotoxy(23, 9 + temperatureOffset + i * screenSensorOffset);
//			printf("(a.u.)");
//			gotoxy(0, 11 + temperatureOffset + i * screenSensorOffset);
//		}
//
//		if ((mode & CMT_OUTPUTMODE_ORIENT) != 0) {
//			printf("Orientation data\n");
//			switch (settings & CMT_OUTPUTSETTINGS_ORIENTMODE_MASK) {
//			case CMT_OUTPUTSETTINGS_ORIENTMODE_QUATERNION:
//				printf("    q0\t    q1\t    q2\t    q3\n");
//				break;
//			case CMT_OUTPUTSETTINGS_ORIENTMODE_EULER:
//				printf("  Roll\t Pitch\t   Yaw\n");
//				printf("                       degrees\n");
//				break;
//			case CMT_OUTPUTSETTINGS_ORIENTMODE_MATRIX:
//				printf(" Matrix\n");
//				break;
//			default:
//				;
//			}
//		}
//	}
//}

//////////////////////////////////////////////////////////////////////////
// calcScreenOffset
//
// Calculates offset for screen data with multiple sensors.
//void calcScreenOffset()
//{
//	// 1 line for "Sensor ..."
//	screenSensorOffset += 1;
//	if ((mode & CMT_OUTPUTMODE_TEMP) != 0)
//		screenSensorOffset += 3;
//	if ((mode & CMT_OUTPUTMODE_CALIB) != 0)
//		screenSensorOffset += 8;
//	if ((mode & CMT_OUTPUTMODE_ORIENT) != 0) {
//		switch (settings & CMT_OUTPUTSETTINGS_ORIENTMODE_MASK) {
//		case CMT_OUTPUTSETTINGS_ORIENTMODE_QUATERNION:
//			screenSensorOffset += 4;
//			break;
//		case CMT_OUTPUTSETTINGS_ORIENTMODE_EULER:
//			screenSensorOffset += 4;
//			break;
//		case CMT_OUTPUTSETTINGS_ORIENTMODE_MATRIX:
//			screenSensorOffset += 6;
//			break;
//		default:
//			;
//		}
//	}
//}

//////////////////////////////////////////////////////////////////////////
// clrscr
//
// Clear console screen
//void clrscr()
//{
//#ifdef WIN32
//	CONSOLE_SCREEN_BUFFER_INFO csbi;
//	HANDLE hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
//	COORD coord = { 0, 0 };
//	DWORD count;
//
//	GetConsoleScreenBufferInfo(hStdOut, &csbi);
//	FillConsoleOutputCharacter(hStdOut, ' ', csbi.dwSize.X * csbi.dwSize.Y, coord, &count);
//	SetConsoleCursorPosition(hStdOut, coord);
//#else
//	int i;
//
//	for (i = 0; i < 100; i++)
//		// Insert new lines to create a blank screen
//		putchar('\n');
//	gotoxy(0, 0);
//#endif
//}

//////////////////////////////////////////////////////////////////////////
// gotoxy
//
// Sets the cursor position at the specified console position
//
// Input
//	 x	: New horizontal cursor position
//   y	: New vertical cursor position
//void gotoxy(int x, int y)
//{
//#ifdef WIN32
//	COORD coord;
//	coord.X = x;
//	coord.Y = y;
//	SetConsoleCursorPosition(GetStdHandle(STD_OUTPUT_HANDLE), coord);
//#else
//	char essq[100];		// String variable to hold the escape sequence
//	char xstr[100];		// Strings to hold the x and y coordinates
//	char ystr[100];		// Escape sequences must be built with characters
//
//	/*
//	** Convert the screen coordinates to strings
//	*/
//	sprintf(xstr, "%d", x);
//	sprintf(ystr, "%d", y);
//
//	/*
//	** Build the escape sequence (vertical move)
//	*/
//	essq[0] = '\0';
//	strcat(essq, "\033[");
//	strcat(essq, ystr);
//
//	/*
//	** Described in man terminfo as vpa=\E[%p1%dd
//	** Vertical position absolute
//	*/
//	strcat(essq, "d");
//
//	/*
//	** Horizontal move
//	** Horizontal position absolute
//	*/
//	strcat(essq, "\033[");
//	strcat(essq, xstr);
//	// Described in man terminfo as hpa=\E[%p1%dG
//	strcat(essq, "G");
//
//	/*
//	** Execute the escape sequence
//	** This will move the cursor to x, y
//	*/
//	printf("%s", essq);
//#endif
//}

//////////////////////////////////////////////////////////////////////////
// exitFunc
//
// Closes cmt nicely
//void exitFunc(void)
//{
//	// if we have a valid instance, we should get rid of it at the end of the program
//	if (instance != -1) {
//		// Close any open COM ports
//		cmtClose(instance);
//		cmtDestroyInstance(instance);
//	}
//
//	// get rid of keystrokes before we post our message
//	while (_kbhit()) _getch();
//
//	// wait for a keypress
//	if (!userQuit)
//	{
//		//printf("Press a key to exit\n");
//		//_getch();
//	}
//}
