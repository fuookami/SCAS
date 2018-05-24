#include "stdafx.h"
#include "Global.h"

namespace std
{
	string to_string(const SSUtils::int128 & value)
	{
		return value.str();
	}

	SSUtils::int128 stoint128(const string & str)
	{
		return stointx<128>(str);
	}

	string to_string(const SSUtils::uint128 & value)
	{
		return value.str();
	}

	SSUtils::uint128 stouint128(const string & str)
	{
		return stouintx<128>(str);
	}

	string to_string(const SSUtils::int256 & value)
	{
		return value.str();
	}

	SSUtils::int256 stoint256(const string & str)
	{
		return stointx<256>(str);
	}

	string to_string(const SSUtils::uint256 & value)
	{
		return value.str();
	}

	SSUtils::uint256 stouint256(const string & str)
	{
		return stouintx<256>(str);
	}

	string to_string(const SSUtils::int512 & value)
	{
		return value.str();
	}

	SSUtils::int512 stoint512(const string & str)
	{
		return stointx<512>(str);
	}

	string to_string(const SSUtils::uint512 & value)
	{
		return value.str();
	}

	SSUtils::uint512 stouint512(const string & str)
	{
		return stouintx<512>(str);
	}

	string to_string(const SSUtils::int1024 & value)
	{
		return value.str();
	}

	SSUtils::int1024 stoint1024(const string & str)
	{
		return stointx<1024>(str);
	}

	string to_string(const SSUtils::uint1024 & value)
	{
		return value.str();
	}

	SSUtils::uint1024 stouint1024(const string & str)
	{
		return stouintx<1024>(str);
	}

	string to_string(const SSUtils::integer & value)
	{
		return value.str();
	}

	SSUtils::integer stointeger(const string & str)
	{
		return stointx<0>(str);
	}

	string to_string(const SSUtils::rational & value)
	{
		return value.str();
	}

	SSUtils::rational storational(const string & str)
	{
		try
		{
			return SSUtils::rational(str);
		}
		catch (exception &e)
		{
			cerr << e.what() << endl;
			return SSUtils::rational(0);
		}
	}

#define TO_FLOAT_PP(digits, str) \
	try \
	{ \
		return SSUtils::float##digits##(##str##); \
	} \
	catch (exception &e) \
	{ \
		cerr << e.what() << endl; \
		return SSUtils::float##digits##(0); \
	} \

	string to_string(const SSUtils::float32 & value)
	{
		return value.str();
	}

	SSUtils::float32 stofloat32(const string & str)
	{
		TO_FLOAT_PP(32, str);
	}

	string to_string(const SSUtils::float64 & value)
	{
		return value.str();
	}

	SSUtils::float64 stofloat64(const string & str)
	{
		TO_FLOAT_PP(64, str);
	}

	string to_string(const SSUtils::float128 & value)
	{
		return value.str();
	}

	SSUtils::float128 stofloat128(const string & str)
	{
		TO_FLOAT_PP(128, str);
	}

	string to_string(const SSUtils::float256 & value)
	{
		return value.str();
	}

	SSUtils::float256 stofloat256(const string & str)
	{
		TO_FLOAT_PP(256, str);
	}
#undef TO_FLOAT_PP

	string to_string(const SSUtils::dec50 & value)
	{
		return value.str();
	}

	SSUtils::dec50 stodec50(const string & str)
	{
		return stodecimal<50>(str);
	}

	string to_string(const SSUtils::dec100 & value)
	{
		return value.str();
	}

	SSUtils::dec100 stodec100(const string & str)
	{
		return stodecimal<100>(str);
	}

	SSUtils::real storeal(const string & str)
	{
		return stodecimal<std::numeric_limits<SSUtils::real>::digits>(str);
	}
};
