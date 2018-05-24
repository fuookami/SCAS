#include "stdafx.h"
#include "_pri_math_global.h"

namespace SSUtils
{
	namespace Math
	{
		const dec50 &_DefaultPrecision(void)
		{
			static const dec50 ret(pow(dec50(10), DefaultPrecisionDigits));
			return ret;
		}
	};
};

