#pragma once

#include "GameInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		struct ApplyValidator
		{
		public:
			static const int NoMaxApply = 0;

		public:
			ApplyValidator(void);
			ApplyValidator(const ApplyValidator &ano);
			ApplyValidator(const ApplyValidator &&ano);
			ApplyValidator &operator=(const ApplyValidator &rhs);
			ApplyValidator &operator=(const ApplyValidator &&rhs);
			~ApplyValidator(void);

			inline const bool getEnabled(void) const { return m_enabled; }
			inline void setEnabled(void) { if (!m_enabled) { m_enabled = true; m_enabledInTeamwork = false; m_maxApply = NoMaxApply; } }
			inline void setDisabled(void) { if (m_enabled) { m_enabled = false; m_enabledInTeamwork = false; m_maxApply = NoMaxApply; } }

			inline const bool getEnabledInTeamwork(void) const { return m_enabledInTeamwork; }
			inline void setEnabledInTeamwork(const bool enabled) { if (m_enabled) { m_enabledInTeamwork = enabled; } }

			inline const int getMaxApply(void) const { return m_maxApply; }
			inline void setMaxApply(const int maxApply) { if (m_enabled && maxApply > NoMaxApply) { m_maxApply = maxApply; } }

		private:
			bool m_enabled;

			bool m_enabledInTeamwork;
			int m_maxApply;
		};

		class TypeInfo
		{
		public:
			TypeInfo(void);
			TypeInfo(const TypeInfo &ano);
			TypeInfo(const TypeInfo &&ano);
			TypeInfo &operator=(const TypeInfo &rhs);
			TypeInfo &operator=(const TypeInfo &&rhs);
			~TypeInfo(void);

			inline std::vector<std::pair<std::string, std::string>> getTypes(void) { return m_types; }
			inline const std::vector<std::pair<std::string, std::string>> getTypes(void) const { return m_types; }
			inline void setTypes(const std::vector<std::pair<std::string, std::string>> &types) { m_types = types; }
			inline void setTypes(const std::vector<std::pair<std::string, std::string>> &&types) { m_types = std::move(types); }

		private:
			std::vector<std::pair<std::string, std::string>> m_types; // <id, name>
		};

		class RankInfo
		{
		public:
			RankInfo(void);
			RankInfo(const RankInfo &ano);
			RankInfo(const RankInfo &&ano);
			RankInfo &operator=(const RankInfo &rhs);
			RankInfo &operator=(const RankInfo &&rhs);
			~RankInfo(void);

			inline const bool getEnabled(void) const { return m_enabled; }
			inline void setEnabled(void) { if (!m_enabled) { m_enabled = true; m_forced = false; m_ranks.clear(); m_defaultRank = std::pair<std::string, std::string>(); } }
			inline void setDisabled(void) { if (m_enabled) { m_enabled = false; m_forced = false; m_ranks.clear(); m_defaultRank = std::pair<std::string, std::string>(); } }

			inline const bool getForced(void) const { return m_forced; }
			inline void setForced(const bool forced) { if (m_enabled) { m_forced = forced; } }

			inline std::vector<std::pair<std::string, std::string>> &getRanks(void) { return m_ranks; }
			inline const std::vector<std::pair<std::string, std::string>> &getRanks(void) const { return m_ranks; }
			inline void setRanks()

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
