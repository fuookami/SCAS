#pragma once

#include "GameInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		struct ApplyValidator
		{
		public:
			

		private:
			bool m_enabled;

			bool m_enabledInTeamwork;
			int m_maxApply;
		};

		class TypeInfo
		{
		public:

		private:
			std::vector<std::pair<std::string, std::string>> m_types;
		};

		class RankInfo
		{
		public:



		private:
			bool m_enabled;
			bool m_forced;
			
			std::vector<std::pair<std::string, std::string>> m_ranks;
			std::pair<std::string, std::string> m_defaultRank;
		};

		class PrincipalInfo
		{
		private:
			std::string m_name;
			std::string m_telephone;
			std::string m_email;
		};

		class DateInfo
		{
		private:
			std::vector<DatetimeUtils::Date> m_dates;
		};

		class TeamInfo
		{
		public:
			struct Team
			{
				std::string id;
				std::string shortName;
				std::string name;
			};
		private:
			std::vector<Team> m_teams;
		};

		class CompetitionInfo
		{
		public:
		private:
			const std::string m_id;

			std::string m_name;
			std::string m_subName;

			PrincipalInfo m_principalInfo;
			DateInfo m_dateInfo;
			TypeInfo m_typeInfo;
			RankInfo m_rankInfo;
			ScoreInfo m_publicScoreInfo;
			TeamInfo m_teamInfo;

			std::map<std::string, std::shared_ptr<EventInfo>> m_eventInfos;
			std::map<DatetimeUtils::Date, std::vector<std::shared_ptr<GameInfo>>> m_gameInfos;
		};
	};
};
