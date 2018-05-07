#pragma once

namespace SSUtils
{
	enum class Endian
	{
		BigEndian,
		LittleEndian
	};

	enum class OperationSystemType
	{
		Windows,
		Linux,
		Unix,
		Android,
		Apple
	};
	
	namespace System
	{
		static const unsigned int CPUIdLength = 8;

#if defined (_WIN32) || defined (_WIN64)
		static const OperationSystemType LocalSystemType = OperationSystemType::Windows;
#elif defined (__linux__)
		static const OperationSystemType LocalSystemType = OperationSystemType::Linux;
#elif defined (__unix__)
		static const OperationSystemType LocalSystemType = OperationSystemType::Unix;
#elif defined (__apple__)
		static const OperationSystemType LocalSystemType = OperationSystemType::Apple;
#else
		static const OperationSystemType LocalSystemType = OperationSystemType::Android;
#endif

		extern const Endian LocalEndian;
	};
};
