#include "GameInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		GroupInfo::GroupInfo(void)
			: m_enabled(false), m_peoplePerGroup(NoGroup)
		{
		}

		GameTypeInfo::GameTypeInfo(const std::string & eventInfoId, const std::shared_ptr<EventInfo>& refEventInfo)
			: m_eventInfoId(eventInfoId), ref_eventInfo(refEventInfo), 
			m_type(eType::Group), m_pattern(ePattern::Ranking), 
			m_orderInEvent(NoPos), m_orderInType(NoPos)
		{
		}

		GameInfo::GameInfo(const std::string &eventInfoId, const std::string & id)
			: m_id(id), m_name(), m_athleteNumber(NoAthleteNumber), m_orderInDay(NoPos), 
			m_planIntervalTime(), m_planTimePerGroup(), 
			m_gameTypeInfo(eventInfoId), m_groupInfo()
		{
		}

		GameInfo::GameInfo(const std::string &eventInfoId, std::string && id)
			: m_id(id), m_name(), m_athleteNumber(NoAthleteNumber), m_orderInDay(NoPos),
			m_planIntervalTime(), m_planTimePerGroup(),
			m_gameTypeInfo(eventInfoId), m_groupInfo()
		{
		}
	};
};
