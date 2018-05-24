#pragma once

#include <cryptopp/randpool.h>

namespace SSUtils
{
	namespace Encryption
	{
		extern CryptoPP::RandomPool globalRNG;
	};
};
