#pragma once

#include <iostream>
#include <vector>
#include <cstdint>
#include <boost/multiprecision/cpp_int.hpp>
#include <boost/multiprecision/cpp_bin_float.hpp>
#include <boost/multiprecision/cpp_dec_float.hpp>

namespace SSUtils
{
	using wchar = wchar_t;
	using byte = unsigned char;
	using Block = std::vector<byte>;

	using int8 = std::int8_t;
	using uint8 = std::uint8_t;
	using int16 = std::int16_t;
	using uint16 = std::uint16_t;
	using int32 = std::int32_t;
	using uint32 = std::uint32_t;
	using int64 = std::int64_t;
	using uint64 = std::uint64_t;
	using int128 = boost::multiprecision::int128_t;
	using uint128 = boost::multiprecision::uint128_t;
	using int256 = boost::multiprecision::int256_t;
	using uint256 = boost::multiprecision::uint256_t;
	using int512 = boost::multiprecision::int512_t;
	using uint512 = boost::multiprecision::uint512_t;
	using int1024 = boost::multiprecision::int1024_t;
	using uint1024 = boost::multiprecision::uint1024_t;
	template<uint32 bits>
	using intx = boost::multiprecision::number<boost::multiprecision::cpp_int_backend<bits, bits>>;
	template<uint32 bits, typename T = std::enable_if_t<bits != 0>>
	using uintx = boost::multiprecision::number<boost::multiprecision::cpp_int_backend<bits, bits, boost::multiprecision::cpp_integer_type::unsigned_magnitude>>;
	using integer = boost::multiprecision::cpp_int;
	using rational = boost::multiprecision::cpp_rational;

	using float32 = boost::multiprecision::cpp_bin_float_single;
	using float64 = boost::multiprecision::cpp_bin_float_double;
	using float128 = boost::multiprecision::cpp_bin_float_double_extended;
	using float256 = boost::multiprecision::cpp_bin_float_quad;

	using dec50 = boost::multiprecision::cpp_dec_float_50;
	using dec100 = boost::multiprecision::cpp_dec_float_100;
	template<uint32 Digits, typename T = std::enable_if_t<Digits != 0>>
	using decimal = boost::multiprecision::number<boost::multiprecision::cpp_dec_float<Digits>>;
	using real = dec100;
};

namespace std
{
	string to_string(const SSUtils::int128 &value);
	SSUtils::int128 stoint128(const string &str);

	string to_string(const SSUtils::uint128 &value);
	SSUtils::uint128 stouint128(const string &str);

	string to_string(const SSUtils::int256 &value);
	SSUtils::int256 stoint256(const string &str);

	string to_string(const SSUtils::uint256 &value);
	SSUtils::uint256 stouint256(const string &str);

	string to_string(const SSUtils::int512 &value);
	SSUtils::int512 stoint512(const string &str);

	string to_string(const SSUtils::uint512 &value);
	SSUtils::uint512 stouint512(const string &str);

	string to_string(const SSUtils::int1024 &value);
	SSUtils::int1024 stoint1024(const string &str);

	string to_string(const SSUtils::uint1024 &value);
	SSUtils::uint1024 stouint1024(const string &str);

	template<SSUtils::uint32 bits>
	string to_string(const SSUtils::intx<bits> &value)
	{
		return value.str();
	}
	template<SSUtils::uint32 bits>
	SSUtils::intx<bits> stointx(const string &str)
	{
		try
		{
			return SSUtils::intx<bits>(str);
		}
		catch (exception &e)
		{
			cerr << e.what() << endl;
			return SSUtils::intx<bits>(0);
		}
	}

	template<SSUtils::uint32 bits>
	string to_string(const SSUtils::uintx<bits> &value)
	{
		return value.str();
	}
	template<SSUtils::uint32 bits>
	SSUtils::uintx<bits> stouintx(const string &str)
	{
		try
		{
			return SSUtils::uintx<bits>(str);
		}
		catch (exception &e)
		{
			cerr << e.what() << endl;
			return SSUtils::uintx<bits>(0);
		}
	}

	string to_string(const SSUtils::integer &value);
	SSUtils::integer stointeger(const string &str);

	string to_string(const SSUtils::rational &value);
	SSUtils::rational storational(const string &str);

	string to_string(const SSUtils::float32 &value);
	SSUtils::float32 stofloat32(const string &str);

	string to_string(const SSUtils::float64 &value);
	SSUtils::float64 stofloat64(const string &str);

	string to_string(const SSUtils::float128 &value);
	SSUtils::float128 stofloat128(const string &str);

	string to_string(const SSUtils::float256 &value);
	SSUtils::float256 stofloat256(const string &str);

	string to_string(const SSUtils::dec50 &value);
	SSUtils::dec50 stodec50(const string &str);

	string to_string(const SSUtils::dec100 &value);
	SSUtils::dec100 stodec100(const string &str);
	SSUtils::real storeal(const string &str);

	template<SSUtils::uint32 Digits>
	string to_string(const SSUtils::decimal<Digits> &value)
	{ 
		return value.str(); 
	}
	template<SSUtils::uint32 Digits>
	SSUtils::decimal<Digits> stodecimal(const string &str) 
	{ 
		try
		{
			return SSUtils::decimal<Digits>(str);
		}
		catch (exception &e)
		{
			cerr << e.what() << endl;
			return SSUtils::decimal<Digits>(0);
		}
	}
};
