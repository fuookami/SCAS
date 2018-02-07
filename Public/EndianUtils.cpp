#include "EndianUtils.h"

namespace EndianUtils
{
	const Endian getLocalEndian(void)
	{
		static Endian localEndian(Endian::Unknown);
		if (localEndian == Endian::Unknown)
		{
			union
			{
				int number;
				char s;
			} LocalEndianChecker;

			LocalEndianChecker.number = 0x010000002;
			localEndian = (LocalEndianChecker.s == 0x01) ? Endian::BigEndian : Endian::LittleEndian;
		}

		return localEndian;
	}
};
