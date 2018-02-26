#include "EventInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		TeamworkInfo::TeamworkInfo(void)
			: m_beTeamwork(false), m_needEveryPerson(false), m_minPeople(NoTeamwork), m_maxPeople(NoTeamwork)
		{
		}

		void TeamworkInfo::setNeedEveryOne(const int minPeople, const int maxPeople)
		{
			if (m_beTeamwork 
				&& minPeople >= 0 && minPeople <= maxPeople)
			{
				m_needEveryPerson = true;
				m_minPeople = minPeople;
				m_maxPeople = maxPeople;
			}
		}

		void TeamworkInfo::setMinPeople(const int minPeople)
		{
			if (m_beTeamwork && m_needEveryPerson 
				&& minPeople >= 0 && m_maxPeople >= minPeople)
			{
				m_minPeople = minPeople;
			}
		}

		void TeamworkInfo::setMaxPeople(const int maxPeople)
		{
			if (m_beTeamwork && m_needEveryPerson
				&& maxPeople >= 0 && maxPeople >= m_minPeople)
			{
				m_maxPeople = maxPeople;
			}
		}

		AthleteValidator::AthleteValidator(void)
			: m_types(), m_ranks(), m_maxPerTeam(NoMaxPerTeam)
		{
		}

		const float ScoreInfo::NoScoreRate = 1.0f;

		ScoreInfo::ScoreInfo(void)
			: m_scores(),  m_scoreRate(NoScoreRate), m_breakRecordRateEnabled(false), m_breakRecordRate(NoScoreRate)
		{
		}

		void ScoreInfo::setBreakRecordRateEnable(const bool enabled)
		{
			m_breakRecordRateEnabled = enabled;
			if (!m_breakRecordRateEnabled)
			{
				m_breakRecordRate = NoScoreRate;
			}
		}

		void ScoreInfo::setBreakRecordRate(const float breakRecordScoreRate)
		{
			if (m_breakRecordRateEnabled)
			{
				m_breakRecordRate = breakRecordScoreRate;
			}
		}

		EventInfo::EventInfo(const std::string & id)
			: m_id(id), m_name(), m_type(eType::Ranking), 
			m_gradeInfo(), m_teamworkInfo(), m_athleteValidator()
		{
		}

		EventInfo::EventInfo(std::string && id)
			: m_id(std::move(id)), m_name(), m_type(eType::Ranking),
			m_gradeInfo(), m_teamworkInfo(), m_athleteValidator()
		{
		}
	};
};
