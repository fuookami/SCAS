#pragma once

#include <vector>
#include "DateTimeUtils.h"

namespace SCAS
{
	namespace CompCfg
	{
		class GroupInfo
		{
		public:
			static const int NoGroup = -1;

			GroupInfo(void);
			GroupInfo(const GroupInfo &ano);
			GroupInfo(const GroupInfo &&ano);
			GroupInfo &operator=(const GroupInfo &rhs);
			GroupInfo &operator=(const GroupInfo &&rhs);
			~GroupInfo(void);

			inline const bool getEnabled(void) const { return m_enabled; }
			inline const int getPeoplePerGroup(void) const { return m_peoplePerGroup; }

			inline void setDisabled(void) { m_enabled = false; m_peoplePerGroup = NoGroup; }
			inline void setEnabled(const int peoplePerGroup) { m_enabled = true; m_peoplePerGroup = peoplePerGroup; }

		private:
			bool m_enabled;
			int m_peoplePerGroup;
		};

		class TeamworkInfo
		{
		public:
			static const int NoTeamwork = -1;
			static const int NotNeedEveryPerson = -1;

			TeamworkInfo(void);
			TeamworkInfo(const TeamworkInfo &ano);
			TeamworkInfo(const TeamworkInfo &&ano);
			TeamworkInfo &operator=(const TeamworkInfo &rhs);
			TeamworkInfo &operator=(const TeamworkInfo &&rhs);
			~TeamworkInfo(void);

			inline const bool getIsTeamwork(void) const { return m_beTeamwork; }
			inline const bool getNeedEveryPerson(void) const { return m_needEveryPerson; }
			inline const int getMinPeople(void) const { return m_minPeople; }
			inline const int getMaxPeople(void) const { return m_maxPeople; }

			inline void setNoTeamwork(void) { m_beTeamwork = false; m_needEveryPerson = false; m_minPeople = NoTeamwork; m_maxPeople = NoTeamwork; }
			inline void setIsTeamwork(void) { m_beTeamwork = true; m_needEveryPerson = false; m_minPeople = NotNeedEveryPerson; m_maxPeople = NotNeedEveryPerson; }
			void setNeedEveryOne(const int minPeople, const int maxPeople);
			inline void setNotNeedEveryOne(void) { if (m_beTeamwork) { m_needEveryPerson = false; m_minPeople = NotNeedEveryPerson; m_maxPeople = NotNeedEveryPerson; } }
			void setMinPeople(const int minPeople);
			void setMaxPeople(const int maxPeople);

		private:
			bool m_beTeamwork;

			bool m_needEveryPerson;
			int m_minPeople;
			int m_maxPeople;
		};

		class AthleteValidator
		{
		private:
			std::vector<std::string> m_types;
			std::vector<std::string> m_ranks;
			int m_maxPerTeam;
		};

		class GameInfo
		{
		private:
			std::string m_id;
			std::string m_name;
			int m_scoreRate;
			
			DateTimeUtils::TimeDuration m_planIntervalTime;
			DateTimeUtils::TimeDuration m_planTimePerGroup;

			GroupInfo m_groupInfo;
			TeamworkInfo m_teamworkInfo;
			AthleteValidator m_athleteValidator;
		};
	};
};
