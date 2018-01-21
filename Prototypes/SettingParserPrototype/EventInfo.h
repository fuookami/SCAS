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

		public:
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
			static const uint32 NoMaxPerTeam = 0;

		public:
			AthleteValidator(void);
			AthleteValidator(const AthleteValidator &ano);
			AthleteValidator(const AthleteValidator &&ano);
			AthleteValidator &operator=(const AthleteValidator &rhs);
			AthleteValidator &operator=(const AthleteValidator &&rhs);
			~AthleteValidator(void);

			inline std::vector<std::string> &getTypes(void) { return m_types; }
			inline const std::vector<std::string> &getTypes(void) const { return m_types; }
			inline void setTypes(const std::vector<std::string> &types) { m_types = types; }
			inline void setTypes(const std::vector<std::string> &&types) { m_types = std::move(types); }

			inline std::vector<std::string> &getRanks(void) { return m_ranks; }
			inline const std::vector<std::string> &getRanks(void) const { return m_ranks; }
			inline void setRanks(const std::vector<std::string> &ranks) { m_ranks = ranks; }
			inline void setRanks(const std::vector<std::string> &&ranks) { m_ranks = std::move(ranks); }

			inline int getMaxPerTeam(void) const { return m_maxPerTeam; }
			inline void setMaxPerTeam(const uint32 maxPerTeam) { m_maxPerTeam = maxPerTeam; }
			inline void removeMaxPerTeam(void) { m_maxPerTeam = NoMaxPerTeam; }

		private:
			std::vector<std::string> m_types;
			std::vector<std::string> m_ranks;
			uint32 m_maxPerTeam;
		};

		class ScoreInfo
		{
		public:
			static const float NoScoreRate;

		public:
			ScoreInfo(void);
			ScoreInfo(const ScoreInfo &ano);
			ScoreInfo(const ScoreInfo &&ano);
			ScoreInfo &operator=(const ScoreInfo &rhs);
			ScoreInfo &operator=(const ScoreInfo &&rhs);
			~ScoreInfo(void);
			
			inline std::vector<int> &getScores(void) { return m_scores; }
			inline const std::vector<int> &getScores(void) const { return m_scores; }
			inline void setScores(const std::vector<int> &scores) { m_scores = scores; }
			inline void setScores(const std::vector<int> &&scores) { m_scores = std::move(scores); }

			inline const float getScoreRate(void) const { return m_scoreRate; }
			inline void setScoreRate(const float scoreRate) { m_scoreRate = scoreRate; }

			inline const bool getBreakRecordRateEnable(void) const { return m_breakRecordRateEnabled; }
			void setBreakRecordRateEnable(const bool enabled);

			inline const float getBreakRecordRate(void) { return m_breakRecordRate; }
			void setBreakRecordRate(const float breakRecordScoreRate);

		private:
			std::vector<int> m_scores;

			float m_scoreRate;
			
			bool m_breakRecordRateEnabled;
			float m_breakRecordRate;
		};

		class EventInfo
		{
		public:
			enum class eType
			{
				Dual,		// 对抗性运动，比如篮球、足球
				Ranking		// 非对抗性运动（名次制运动），比如田赛、游泳
			};

		public:
			EventInfo(const std::string &id = "");
			EventInfo(const std::string &&id);
			EventInfo(const EventInfo &ano);
			EventInfo(const EventInfo &&ano);
			EventInfo &operator=(const EventInfo &rhs) = delete;
			EventInfo &operator=(const EventInfo &&rhs) = delete;
			~EventInfo(void);

			inline const std::string &getId(void) const { return m_id; }

			inline const std::string &getName(void) const { return m_name; }
			inline void setName(const std::string &name) { m_name.assign(name); }
			inline void setName(const std::string &&name) { m_name.assign(std::move(name)); }

			inline const eType getType(void) const { return m_type; }
			inline void setType(const eType type) { m_type = type; }

			inline GradeInfo &getGradeInfo(void) { return m_gradeInfo; }
			inline const GradeInfo &getGradeInfo(void) const { return m_gradeInfo; }
			inline void setGradeInfo(const GradeInfo &gradeInfo) { m_gradeInfo = gradeInfo; }
			inline void setGradeInfo(const GradeInfo &&gradeInfo) { m_gradeInfo = std::move(gradeInfo); }

			inline TeamworkInfo &getTeamworkInfo(void) { return m_teamworkInfo; }
			inline const TeamworkInfo &getTeamworkInfo(void) const { return m_teamworkInfo; }
			inline void setTeamworkInfo(const TeamworkInfo &teamworkInfo) { m_teamworkInfo = teamworkInfo; }
			inline void setTeamworkInfo(const TeamworkInfo &&teamworkInfo) { m_teamworkInfo = std::move(teamworkInfo); }

			inline AthleteValidator &getAthleteValidator(void) { return m_athleteValidator; }
			inline const AthleteValidator &getAthleteValidator(void) const { return m_athleteValidator; }
			inline void setAthleteValidator(const AthleteValidator &athleteValidator) { m_athleteValidator = athleteValidator; }
			inline void setAthleteValidator(const AthleteValidator &&athleteValidator) { m_athleteValidator = std::move(athleteValidator); }

		private:
			const std::string m_id;
			std::string m_name;
			eType m_type;

			GradeInfo m_gradeInfo;
			TeamworkInfo m_teamworkInfo;
			AthleteValidator m_athleteValidator;
		};
	};
};
