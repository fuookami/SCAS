#include "CompetitionInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		ApplyValidator::ApplyValidator(void)
			: m_enabled(false), m_enabledInTeamwork(false), m_maxApply(NoMaxApply)
		{
		}

		RankInfo::RankInfo(void)
			: m_enabled(false), m_forced(false), m_ranks(), m_defaultRank()
		{
		}

		CompetitionInfo::CompetitionInfo(const std::string & id)
			: m_id(id), m_name(), m_subName(), 
			m_principalInfo(), m_dateInfo(), m_typeInfo(), m_rankInfo(), m_publicScoreInfo(), m_teamInfo(), 
			m_eventInfos(), m_gameInfos()
		{
		}

		CompetitionInfo::CompetitionInfo(std::string && id)
			: m_id(id), m_name(), m_subName(),
			m_principalInfo(), m_dateInfo(), m_typeInfo(), m_rankInfo(), m_publicScoreInfo(), m_teamInfo(),
			m_eventInfos(), m_gameInfos()
		{
		}
	};
};
