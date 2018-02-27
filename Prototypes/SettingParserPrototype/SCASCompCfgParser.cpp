#include "SCASCompCfgParser.h"
#include "XMLUtils.h"
#include "StringUtils.h"

namespace SCAS
{
	namespace CompCfg
	{
		const bool saveToXML(const std::shared_ptr<CompetitionInfo> info, const std::string & outUrl)
		{
			XMLUtils::XMLNode root(CompetitionInfo::RootTag);

			XMLUtils::XMLNode nameNode(CompetitionInfo::NameTag);
			nameNode.setContent(info->getName());
			root.addChild(std::move(nameNode));

			XMLUtils::XMLNode subNameNode(CompetitionInfo::SubNameTag);
			subNameNode.setContent(info->getSubName());
			root.addChild(std::move(subNameNode));

			XMLUtils::XMLNode versionNode(CompetitionInfo::VersionTag);
			versionNode.setContent(info->getVersion());
			root.addChild(std::move(versionNode));

			XMLUtils::XMLNode identifierNode(CompetitionInfo::IdentifierTag);
			identifierNode.setContent(info->getIdentifier());
			root.addChild(std::move(identifierNode));

			{
				XMLUtils::XMLNode applyValidatorNode(ApplyValidator::Tag);
				const auto &applyValidator(info->getApplyValidator());
				applyValidatorNode.addAttr(std::make_pair(ApplyValidator::Attributes::Enabled, StringUtils::to_string(applyValidator.getEnabled())));
				if (applyValidator.getEnabled())
				{
					applyValidatorNode.addAttr(std::make_pair(ApplyValidator::Attributes::EnabledInTeamwork, StringUtils::to_string(applyValidator.getEnabledInTeamwork())));
					applyValidatorNode.addAttr(std::make_pair(ApplyValidator::Attributes::MaxApply, std::to_string(applyValidator.getMaxApply())));
				}
				root.addChild(std::move(applyValidatorNode));
			}

			{
				XMLUtils::XMLNode principalInfoNode(PrincipalInfo::Tag);
				const auto &principalInfo(info->getPrincipalInfo());

				XMLUtils::XMLNode nameNode(PrincipalInfo::NameTag);
				nameNode.setContent(principalInfo.getName());
				principalInfoNode.addChild(std::move(nameNode));

				XMLUtils::XMLNode telephoneNode(PrincipalInfo::TelephoneTag);
				telephoneNode.setContent(principalInfo.getTelephone());
				principalInfoNode.addChild(std::move(telephoneNode));

				XMLUtils::XMLNode emailNode(PrincipalInfo::EMailTag);
				emailNode.setContent(principalInfo.getEmmail());
				principalInfoNode.addChild(std::move(emailNode));

				root.addChild(std::move(principalInfoNode));
			}

			{
				XMLUtils::XMLNode dateInfoNode(DateInfo::Tag);
				const auto &dateInfo(info->getDateInfo());

				for (const auto &date : dateInfo.getDates())
				{
					XMLUtils::XMLNode dateNode(DateInfo::DateTag);
					dateNode.setContent(date.toString());
					dateInfoNode.addChild(std::move(dateNode));
				}
				root.addChild(std::move(dateInfoNode));
			}

			{
				XMLUtils::XMLNode typeInfoNode(TypeInfo::Tag);
				const auto &typeInfo(info->getTypeInfo());

				for (const auto &type : typeInfo.getTypes())
				{
					XMLUtils::XMLNode typeNode(TypeInfo::TypeTag);
					typeNode.addAttr(std::make_pair(TypeInfo::Attributes::Id, type.first));
					typeNode.setContent(type.second);
					typeInfoNode.addChild(std::move(typeNode));
				}

				root.addChild(std::move(typeInfoNode));
			}

			{
				XMLUtils::XMLNode rankInfoNode(RankInfo::Tag);
				const auto &rankInfo(info->getRankInfo());

				rankInfoNode.addAttr(std::make_pair(RankInfo::Attributes::Enabled, StringUtils::to_string(rankInfo.getEnabled())));
				if (rankInfo.getEnabled())
				{
					rankInfoNode.addAttr(std::make_pair(RankInfo::Attributes::Forced, StringUtils::to_string(rankInfo.getForced())));

					for (const auto &rank : rankInfo.getRanks())
					{
						XMLUtils::XMLNode rankNode(RankInfo::RankTag);
						rankNode.addAttr(std::make_pair(RankInfo::Attributes::Id, rank.first));
						rankNode.setContent(rank.second);
						rankInfoNode.addChild(std::move(rankNode));
					}

					XMLUtils::XMLNode defaultRankNode(RankInfo::DefaultRankTag);
					defaultRankNode.addAttr(std::make_pair(RankInfo::Attributes::DefaultRankType, RankInfo::DefaultRankTypes::Id));
					defaultRankNode.setContent(rankInfo.getDefaultRank().first);
					rankInfoNode.addChild(std::move(defaultRankNode));
				}

				root.addChild(std::move(rankInfoNode));
			}

			{
				XMLUtils::XMLNode teamInfoNode(TeamInfo::Tag);
				const auto &teamInfo(info->getTeamInfo());

				for (const auto &team : teamInfo.getTeams())
				{
					XMLUtils::XMLNode teamNode(TeamInfo::TeamTag);
					teamNode.addAttr(std::make_pair(TeamInfo::Attributes::Id, team.id));
					teamNode.addAttr(std::make_pair(TeamInfo::Attributes::ShortName, team.shortName));
					teamNode.setContent(team.name);
					teamInfoNode.addChild(std::move(teamNode));
				}

				root.addChild(std::move(teamInfoNode));
			}

			{
				XMLUtils::XMLNode publicScoreInfoNode(CompetitionInfo::PublicScoreInfoTag);
				const auto &publicScoreInfo(info->getPublicScoreInfo());

				XMLUtils::XMLNode scoresNode(ScoreInfo::ScoresTag);
				scoresNode.setContent(StringUtils::join(publicScoreInfo.getScores()));
				publicScoreInfoNode.addChild(std::move(scoresNode));

				XMLUtils::XMLNode scoreRateNode(ScoreInfo::ScoreRateTag);
				scoreRateNode.setContent(std::to_string(publicScoreInfo.getScoreRate()));
				publicScoreInfoNode.addChild(std::move(scoreRateNode));

				XMLUtils::XMLNode breakRecordRateNode(ScoreInfo::BreakRecordRateTag);
				breakRecordRateNode.addAttr(std::make_pair(ScoreInfo::Attributes::Enabled, StringUtils::to_string(publicScoreInfo.getBreakRecordRateEnable())));
				if (publicScoreInfo.getBreakRecordRateEnable())
				{
					breakRecordRateNode.setContent(std::to_string(publicScoreInfo.getBreakRecordRate()));
				}
				publicScoreInfoNode.addChild(breakRecordRateNode);

				root.addChild(publicScoreInfoNode);
			}

			return XMLUtils::saveToFile(outUrl, root);
		}

		const std::shared_ptr<CompetitionInfo> readFromXML(const std::string & inUrl)
		{
			return std::shared_ptr<CompetitionInfo>();
		}
	};
};
