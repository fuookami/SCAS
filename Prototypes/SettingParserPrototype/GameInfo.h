#pragma once

#include <vector>

namespace SCAS
{
	namespace CompCfg
	{
		struct GroupInfo
		{
			bool enabled;
			int peoplePerGroup;
		};

		struct TeamworkInfo
		{
			bool beTeamwork;

			bool needEveryPerson;
			int minPeople;
			int maxPeople;
		};

		struct AthleteValidator
		{
			std::vector<int> types;
			std::vector<int> ranks;
			int maxPerTeam;
		};

		struct GameInfo
		{

		};
	};
};
