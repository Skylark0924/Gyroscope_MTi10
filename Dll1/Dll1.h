#pragma once
#include "XsensCMT.h"

#ifdef ENCRYPT_EXPORTS  
#define ENCRYPT_EXPORTS __declspec(dllexport)  
#else  
#define ENCRYPT_EXPORTS __declspec(dllimport)  
#endif

#pragma comment( lib, "xsenscmt.lib" )

//extern "C" ENCRYPT_EXPORTS int __stdcall test01(int a, int b, int c);
extern "C" ENCRYPT_EXPORTS signed char __stdcall OpenCOMDevice();
extern "C" ENCRYPT_EXPORTS float* __stdcall mygetdata();
extern "C" ENCRYPT_EXPORTS float* __stdcall mygetCaldata();
extern "C" ENCRYPT_EXPORTS float* __stdcall mygetEulerdata();
extern "C" ENCRYPT_EXPORTS void __stdcall CloseCOMDevice();
void doHardwareScan();
void doMtSettings();
void autoselectmode();
