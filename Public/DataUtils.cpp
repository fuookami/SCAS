#include "DataUtils.h"
#include <sstream>
#include <iomanip>

namespace DataUtils
{
	const std::string toHexString(const Data & data)
	{
		std::ostringstream sout;
		for (const byte b : data)
		{
			sout << std::setfill('0') << std::setw(2) << std::hex << static_cast<uint32>(b) << ' ';
		}

		return sout.str();
	}
};
