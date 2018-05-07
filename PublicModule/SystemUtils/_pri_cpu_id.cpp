#include "_pri_cpu_id.h"
#include <numeric>

#ifndef __ANDROID__
#if defined(_WIN32) || defined(_WIN64) || defined(__linux__) || defined(__unix__) || defined(__apple__)
#define PC_PLATFORM
#endif
#endif // __ANDROID__

#if defined(PC_PLATFORM)
#if defined(__GNUC__)
#include <cpuid.h>
#include <sysconf.h>
// __GNUC__
#elif defined(_MSC_VER)
#if _MSC_VER > 1400
#include <Intrin.h>
#include <windows.h>
#endif // _MSC_VER > 1400
#else
#error "Only supports MSVC or GCC."
#endif // _MSC_VER
#endif // PC_PLATFORM

namespace SSUtils
{
	namespace System
	{
		std::array<byte, CPUIdLength> getCPUId(void)
		{
			std::array<byte, CPUIdLength> cpuId;
#ifdef PC_PLATFORM
			uint32 dwBuf[4];
			_getCPUId(dwBuf, 1); // 获取cpu系列号

			cpuId[0] = 0xFF & (dwBuf[3] >> 24); // 高字节
			cpuId[1] = 0xFF & (dwBuf[3] >> 16);
			cpuId[2] = 0xFF & (dwBuf[3] >> 8);
			cpuId[3] = 0xFF & (dwBuf[3]); // 低字节

			cpuId[4] = 0xFF & (dwBuf[0] >> 24); // 高字节
			cpuId[5] = 0xFF & (dwBuf[0] >> 16);
			cpuId[6] = 0xFF & (dwBuf[0] >> 8);
			cpuId[7] = 0xFF & (dwBuf[0]); // 低字节
#endif
			return cpuId;
		}

		uint32 getCPUCoreNumber(void)
		{
			uint32 num(1);
#if defined(PC_PLATFORM)
#if defined(__GNUC__)
			count = sysconf(_SC_NPROCESSORS_CONF);
#elif defined(_MSC_VER)
			SYSTEM_INFO si;
			GetSystemInfo(&si);
			num = si.dwNumberOfProcessors;
#endif
#endif
			return num;
		}

		void _getCPUId(uint32  CPUInfo[4], uint32 infoType)
		{
#if defined(PC_PLATFORM)
#if defined(__GNUC__)    // GCC
			__cpuid(infoType, CPUInfo[0], CPUInfo[1], CPUInfo[2], CPUInfo[3]);
#elif defined(_MSC_VER)    // MSVC
#if _MSC_VER >= 1400    // VC2005才支持__cpuid
			__cpuid((int*)(void*)CPUInfo, (int)infoType);
#else
			_getCPUIdEx(CPUInfo, infoType, 0);
#endif
#endif    // #if defined(__GNUC__)
#endif
		}

		void _getCPUIdEx(uint32 CPUInfo[4], uint32 infoType, uint32 ecxValue)
		{
#if defined(PC_PLATFORM)
#if defined(__GNUC__)    // GCC
			__cpuid_count(infoType, ecxValue, CPUInfo[0], CPUInfo[1], CPUInfo[2], CPUInfo[3]);
#elif defined(_MSC_VER)    // MSVC
#if defined(_WIN64) || _MSC_VER>=1600    // 64位下不支持内联汇编. 1600: VS2010, 据说VC2008 SP1之后才支持__cpuidex.
			__cpuidex((int32*)(void*)CPUInfo, (int32)infoType, (int32)ecxValue);
#else
			if (0 == CPUInfo)
				return;
			_asm {
				// load. 读取参数到寄存器.
				mov edi, CPUInfo;    // 准备用edi寻址CPUInfo
				mov eax, infoType;
				mov ecx, ecxValue;
				// CPUID
				cpuid;
				// save. 将寄存器保存到CPUInfo
				mov[edi], eax;
				mov[edi + 4], ebx;
				mov[edi + 8], ecx;
				mov[edi + 12], edx;
			}
#endif
#endif    // #if defined(__GNUC__)
#endif
		}
	};
};
