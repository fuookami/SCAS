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
			ApplyValidator(const ApplyValidator &ano) = default;
			ApplyValidator(ApplyValidator &&ano) = default;
			ApplyValidator &operator=(const ApplyValidator &rhs) = default;
			ApplyValidator &operator=(ApplyValidator &&rhs) = default;
			~ApplyValidator(void) = default;

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
			using TypeDataType = std::pair<std::string, std::string>;

			TypeInfo(void) = default;
			TypeInfo(const TypeInfo &ano) = default;
			TypeInfo(TypeInfo &&ano) = default;
			TypeInfo &operator=(const TypeInfo &rhs) = default;
			TypeInfo &operator=(TypeInfo &&rhs) = default;
			~TypeInfo(void) = default;

			inline std::vector<TypeDataType> getTypes(void) { return m_types; }
			inline const std::vector<TypeDataType> getTypes(void) const { return m_types; }
			inline void setTypes(const std::vector<TypeDataType> &types) { m_types = types; }
			inline void setTypes(std::vector<TypeDataType> &&types) { m_types = std::move(types); }

		private:
			std::vector<TypeDataType> m_types; // <id, name>
		};

		class RankInfo
		{
		public:
			using RankDataType = std::pair<std::string, std::string>;

			RankInfo(void);
			RankInfo(const RankInfo &ano) = default;
			RankInfo(RankInfo &&ano) = default;
			RankInfo &operator=(const RankInfo &rhs) = default;
			RankInfo &operator=(RankInfo &&rhs) = default;
			~RankInfo(void) = default;

			inline const bool getEnabled(void) const { return m_enabled; }
			inline void setEnabled(void) { if (!m_enabled) { m_enabled = true; m_forced = false; m_ranks.clear(); m_defaultRank = RankDataType(); } }
			inline void setDisabled(void) { if (m_enabled) { m_enabled = false; m_forced = false; m_ranks.clear(); m_defaultRank = RankDataType(); } }

			inline const bool getForced(void) const { return m_forced; }
			inline void setForced(const bool forced) { if (m_enabled) { m_forced = forced; } }

			inline std::vector<RankDataType> &getRanks(void) { return m_ranks; }
			inline const std::vector<RankDataType> &getRanks(void) const { return m_ranks; }
			inline void setRanks(const std::vector<RankDataType> &ranks) { m_ranks = ranks; }
			inline void setRanks(std::vector<RankDataType> &&ranks) { m_ranks = std::move(ranks); }

			inline RankDataType &getDefaultRank(void) { return m_defaultRank; }
			inline const RankDataType &getDefaultRank(void) const { return m_defaultRank; }
			inline void setDefaultRank(const RankDataType &defaultRank) { m_defaultRank = defaultRank; }
			inline void setDefaultRank(RankDataType &&defaultRank) { m_defaultRank = std::move(defaultRank); }

		private:
			bool m_enabled;
			bool m_forced;
			
			std::vector<RankDataType> m_ranks;
			RankDataType m_defaultRank;
		};

		class PrincipalInfo
		{
		public:
			PrincipalInfo(void) = default;
			PrincipalInfo(const PrincipalInfo &ano) = default;
			PrincipalInfo(PrincipalInfo &&ano) = default;
			PrincipalInfo &operator=(const PrincipalInfo &rhs) = default;
			PrincipalInfo &operator=(PrincipalInfo &&rhs) = default;
			~PrincipalInfo(void) = default;

			inline const std::string &getName(void) const { return m_name; }
			inline void setName(const std::string &name) { m_name.assign(name); }
			inline void setName(std::string &&name) { m_name.assign(std::move(name)); }

			inline const std::string &getTelephone(void) const { return m_telephone; }
			inline void setTelephone(const std::string &telephone) { m_telephone.assign(telephone); }
			inline void setTelephone(std::string &&telephone) { m_telephone.assign(std::move(telephone)); }

			inline const std::string &getEmmail(void) const { return m_email; }
			inline void setEmail(const std::string &email) { m_email.assign(email); }
			inline void setEmail(std::string &&email) { m_email.assign(std::move(email)); }
			
		private:
			std::string m_name;
			std::string m_telephone;
			std::string m_email;
		};

		class DateInfo
		{
		public:
			DateInfo(void) = default;
			DateInfo(const DateInfo &ano) = default;
			DateInfo(DateInfo &&ano) = default;
			DateInfo &operator=(const DateInfo &rhs) = default;
			DateInfo &operator=(DateInfo &&rhs) = default;
			~DateInfo(void) = default;

			inline std::vector<DatetimeUtils::Date> &getDates(void) { return m_dates; }
			inline const std::vector<DatetimeUtils::Date> &getDates(void) const { return m_dates; }
			inline void setDates(const std::vector<DatetimeUtils::Date> &dates) { m_dates = dates; }
			inline void setDates(std::vector<DatetimeUtils::Date> &dates) { m_dates = dates; }

		private:
			std::vector<DatetimeUtils::Date> m_dates;
		};

		class TeamInfo
		{
		public:
			struct Team
			{
				Team(void) = default;
				Team(const Team &ano) = default;
				Team(Team &&ano) = default;
				Team &operator=(const Team &rhs) = default;
				Team &operator=(Team &&rhs) = default;
				~Team(void) = default;

				std::string id;
				std::string shortName;
				std::string name;
			};

		public:
			TeamInfo(void) = default;
			TeamInfo(const TeamInfo &ano) = default;
			TeamInfo(TeamInfo &&ano) = default;
			TeamInfo &operator=(const TeamInfo &rhs) = default;
			TeamInfo &operator=(TeamInfo &&rhs) = default;
			~TeamInfo(void) = default;

			inline std::vector<Team> &getTeams(void) { return m_teams; }
			inline const std::vector<Team> &getTeams(void) const { return m_teams; }
			inline void setTeams(const std::vector<Team> &teams) { m_teams = teams; }
			inline void setTeams(std::vector<Team> &&teams) { m_teams = std::move(teams); }

		private:
			std::vector<Team> m_teams;
		};

		class CompetitionInfo
		{
		public:
			CompetitionInfo(const std::string &id = DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())));
			CompetitionInfo(std::string &&id);
			CompetitionInfo(const CompetitionInfo &ano) = default;
			CompetitionInfo(CompetitionInfo &&ano) = default;
			CompetitionInfo &operator=(const CompetitionInfo &rhs) = default;
			CompetitionInfo &operator=(CompetitionInfo &&rhs) = default;
			~CompetitionInfo(void) = default;

			inline const std::string &getId(void) const { return m_id; };
			
			inline const std::string &getName(void) const { return m_name; }
			inline void setName(const std::string &name) { m_name.assign(name); }
			inline void setName(std::string &&name) { m_name.assign(std::move(name)); }
			
			inline const std::string &getSubName(void) const { return m_subName; }
			inline void setSubName(const std::string &subName) { m_subName.assign(subName); }
			inline void setSubName(std::string &&subName) { m_subName.assign(std::move(subName)); }

			inline PrincipalInfo &getPrincipalInfo(void) { return m_principalInfo; }
			inline const PrincipalInfo &getPrincipalInfo(void) const { m_principalInfo; }
			inline void setPrincipalInfo(const PrincipalInfo &principalInfo) { m_principalInfo = principalInfo; }
			inline void setPrincipalInfo(PrincipalInfo &&principalInfo) { m_principalInfo = std::move(principalInfo); }

			inline DateInfo &getDateInfo(void) { return m_dateInfo; }
			inline const DateInfo &getDateInfo(void) const { return m_dateInfo; }
			inline void setDateInfo(const DateInfo &dateInfo) { m_dateInfo = dateInfo; }
			inline void setDateInfo(DateInfo &&dateInfo) { m_dateInfo = std::move(dateInfo); }

			inline TypeInfo &getTypeInfo(void) { return m_typeInfo; }
			inline const TypeInfo &getTypeInfo(void) const { return m_typeInfo; }
			inline void setTypeInfo(const TypeInfo &typeInfo) { m_typeInfo = typeInfo; }
			inline void setTypeInfo(TypeInfo &&typeInfo) { m_typeInfo = std::move(typeInfo); }

			inline RankInfo &getRankInfo(void) { return m_rankInfo; }
			inline const RankInfo &getRankInfo(void) const { return m_rankInfo; }
			inline void setRankInfo(const RankInfo &rankInfo) { m_rankInfo = rankInfo; }
			inline void setRankInfo(RankInfo &&rankInfo) { m_rankInfo = std::move(rankInfo); }

			inline ScoreInfo &getPublicScoreInfo(void) { return m_publicScoreInfo; }
			inline const ScoreInfo &getPublicSocreInfo(void) const { return m_publicScoreInfo; }
			inline void setPublicScoreInfo(const ScoreInfo &publicScoreInfo) { m_publicScoreInfo = publicScoreInfo; }
			inline void setPublicScoreInfo(ScoreInfo &&publicScoreInfo) { m_publicScoreInfo = std::move(publicScoreInfo); }

			inline TeamInfo &getTeamInfo(void) { return m_teamInfo; }
			inline const TeamInfo &getTeamInfo(void) const { return m_teamInfo; }
			inline void setTeamInfo(const TeamInfo &teamInfo) { m_teamInfo = teamInfo; }
			inline void setTeamInfo(TeamInfo &&teamInfo) { m_teamInfo = std::move(teamInfo); }

			inline std::map<std::string, std::shared_ptr<EventInfo>> &getEventInfos(void) { return m_eventInfos; }
			inline const std::map<std::string, std::shared_ptr<EventInfo>> &getEventInfos(void) const { return m_eventInfos; }

			inline std::map<DatetimeUtils::Date, std::vector<std::shared_ptr<GameInfo>>> &getGameInfos(void) { m_gameInfos; }
			inline const std::map<DatetimeUtils::Date, std::vector<std::shared_ptr<GameInfo>>> &getGameInfos(void) const { return m_gameInfos; }

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
