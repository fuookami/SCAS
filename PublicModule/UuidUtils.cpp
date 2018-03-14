#include "UuidUtils.h"

#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp>

#ifdef _WIN32
#include <objbase.h>
#else
#include <uuid/uuid.h>
#endif

#include "RandomUtils.h"

namespace UUIDUtil
{
	std::array<byte, UUIDLength> generateUUIDV1(void)
	{
		std::array<byte, UUIDLength> ret;
		GUID uuid;

#ifdef _WIN32
		CoCreateGuid(&uuid);
#else
		uuid_generate(reinterpret_cast<byte *>(&guid));
#endif
		std::copy(reinterpret_cast<const byte *>(&uuid), reinterpret_cast<const byte *>(&uuid) + UUIDLength, ret.begin());

		return ret;
	}

	std::array<byte, UUIDLength> generateUUIDV4(void)
	{
		using namespace boost::uuids;

		static std::mt19937_64 randomGen(RandomUtils::generateNewRandomGenerator_64());
		static basic_random_generator<std::mt19937_64> uuidGen(randomGen);

		uuid u(uuidGen());
		std::array<byte, UUIDLength> ret;
		std::copy(u.begin(), u.end(), ret.begin());

		return ret;
	}
};
