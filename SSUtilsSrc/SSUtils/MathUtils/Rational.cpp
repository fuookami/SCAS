#include "stdafx.h"
#include "Rational.h"

namespace SSUtils
{
	namespace Math
	{
		integer round(const rational &value)
		{
			return static_cast<integer>(boost::math::round(value.convert_to<dec50>()));
		}
		integer floor(const rational &value)
		{
			return static_cast<integer>(value.convert_to<dec50>());
		}
		integer ceil(const rational &value)
		{
			return floor(value) + 1;
		}
	};
};
