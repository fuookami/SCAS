#pragma once

#include "GradeInfo.h"

#include <string>
#include <vector>

namespace SCAS
{
	namespace CompCfg
	{
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
			inline void setNoTeamwork(void) { m_beTeamwork = false; m_needEveryPerson = false; m_minPeople = NoTeamwork; m_maxPeople = NoTeamwork; }
			inline void setIsTeamwork(void) { m_beTeamwork = true; m_needEveryPerson = false; m_minPeople = NotNeedEveryPerson; m_maxPeople = NotNeedEveryPerson; }

			inline const bool getNeedEveryPerson(void) const { return m_needEveryPerson; }
			void setNeedEveryOne(const int minPeople, const int maxPeople);
			inline void setNotNeedEveryOne(void) { if (m_beTeamwork) { m_needEveryPerson = false; m_minPeople = NotNeedEveryPerson; m_maxPeople = NotNeedEveryPerson; } }

			inline const int getMinPeople(void) const { return m_minPeople; }
			void setMinPeople(const int minPeople);

			inline const int getMaxPeople(void) const { return m_maxPeople; }
			void setMaxPeople(const int maxPeople);

		private:
			bool m_beTeamwork;

			bool m_needEveryPerson;
			int m_minPeople;
			int m_maxPeople;
		};

		class AthleteValidator
		{
		public:
			static const int NoMaxPerTeam = -1;

			AthleteValidator(void);
			AthleteValidator(const AthleteValidator &ano);
			AthleteValidator(const AthleteValidator &&ano);
			AthleteValidator &operator=(const AthleteValidator &rhs);
			AthleteValidator &operator=(const AthleteValidator &&rhs);
			~AthleteValidator(void);

			inline const std::vector<std::string> &getTypes(void) const { return m_ranks; }

		private:
			std::vector<std::string> m_types;
			std::vector<std::string> m_ranks;
			int m_maxPerTeam;
		};

		class EventTypeInfo
		{
		public:
			enum class eType
			{
				Dual,		// 对抗性运动，比如篮球、足球
				Ranking		// 非对抗性运动（名次制运动），比如田赛、游泳
			};

		private:
			eType m_type;
		};

		class EventInfo
		{
		private:
			std::string m_id;
			std::string m_name;
			int m_scoreRate;

			TeamworkInfo m_teamworkInfo;
			AthleteValidator m_athleteValidator;
			GradeInfo m_gradeInfo;
		};
	};
};
