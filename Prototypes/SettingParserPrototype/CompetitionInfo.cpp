#include "CompetitionInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		const std::string ApplyValidator::Tag("Apply_Validator");
		const std::string ApplyValidator::Attributes::Enabled("enabled");
		const std::string ApplyValidator::Attributes::EnabledInTeamwork("enabled_in_teamwork");
		const std::string ApplyValidator::Attributes::MaxApply("max_apply");

		const std::string PrincipalInfo::Tag("Principal");
		const std::string PrincipalInfo::NameTag("Name");
		const std::string PrincipalInfo::TelephoneTag("Telephone");
		const std::string PrincipalInfo::EMailTag("EMail");

		const std::string DateInfo::Tag("Dates");
		const std::string DateInfo::DateTag("Date");

		const std::string TypeInfo::Tag("Types");
		const std::string TypeInfo::TypeTag("Type");
		const std::string TypeInfo::Attributes::Id("id");

		const std::string RankInfo::Tag("Ranks");
		const std::string RankInfo::RankTag("Rank");
		const std::string RankInfo::DefaultRankTag("Default_Rank");
		const std::string RankInfo::Attributes::Enabled("enabled");
		const std::string RankInfo::Attributes::Forced("forced");
		const std::string RankInfo::Attributes::Id("id");
		const std::string RankInfo::Attributes::DefaultRankType("type");
		const std::string RankInfo::DefaultRankTypes::Id("id");
		const std::string RankInfo::DefaultRankTypes::Name("name");

		const std::string TeamInfo::Tag("Teams");
		const std::string TeamInfo::TeamTag("Team");
		const std::string TeamInfo::Attributes::Id("id");
		const std::string TeamInfo::Attributes::ShortName("short_name");

		const std::string CompetitionInfo::RootTag("SCAS_CompCfg");
		const std::string CompetitionInfo::NameTag("Name");
		const std::string CompetitionInfo::SubNameTag("Sub_Name");
		const std::string CompetitionInfo::VersionTag("Version");
		const std::string CompetitionInfo::IdentifierTag("Identifier");
		const std::string CompetitionInfo::PublicScoreInfoTag("Public_Score_Info");

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
