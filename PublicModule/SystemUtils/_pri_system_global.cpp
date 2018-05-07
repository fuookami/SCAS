#include "_pri_system_global.h"

namespace SSUtils
{
	namespace System
	{
		const Endian LocalEndian = []()
		{
			union
			{
				int number;
				char s;
			} LocalEndianChecker;

			LocalEndianChecker.number = 0x010000002;
			return (LocalEndianChecker.s == 0x01) ? Endian::BigEndian : Endian::LittleEndian;
		}();
	};
};
