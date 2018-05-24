#include "stdafx.h"
#include "_pri_encryption_global.h"

#ifdef _DEBUG
#pragma comment(lib, "cryptlibd.lib")
#else
#pragma comment(lib, "cryptlib.lib")
#endif

namespace SSUtils
{
	namespace Encryption
	{
		CryptoPP::RandomPool globalRNG;
	};
};
