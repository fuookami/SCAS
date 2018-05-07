#include "SCASCompCfgParser.h"
#include "XMLUtils.h"
#include "StringUtils.h"

namespace SCAS
{
	namespace CompCfg
	{
		const bool saveToXML(const std::shared_ptr<CompetitionInfo> info, const std::string & outUrl)
		{
			std::shared_ptr<SSUtils::XML::Node> root(SSUtils::XML::Node::generate(CompetitionInfo::RootTag));

			std::shared_ptr<SSUtils::XML::Node> nameNode(SSUtils::XML::Node::generate(CompetitionInfo::NameTag));
			nameNode->setContent(info->getName());
			root->addChild(std::move(nameNode));

			std::shared_ptr<SSUtils::XML::Node> subNameNode(SSUtils::XML::Node::generate(CompetitionInfo::SubNameTag));
			subNameNode->setContent(info->getSubName());
			root->addChild(std::move(subNameNode));

			std::shared_ptr<SSUtils::XML::Node> versionNode(SSUtils::XML::Node::generate(CompetitionInfo::VersionTag));
			versionNode->setContent(info->getVersion());
			root->addChild(std::move(versionNode));

			std::shared_ptr<SSUtils::XML::Node> identifierNode(SSUtils::XML::Node::generate(CompetitionInfo::IdentifierTag));
			identifierNode->setContent(info->getIdentifier());
			root->addChild(std::move(identifierNode));

			{
				std::shared_ptr<SSUtils::XML::Node> applyValidatorNode(SSUtils::XML::Node::generate(ApplyValidator::Tag));
				const auto &applyValidator(info->getApplyValidator());
				applyValidatorNode->addAttr(std::make_pair(ApplyValidator::Attributes::Enabled, SSUtils::String::toString(applyValidator.getEnabled())));
				if (applyValidator.getEnabled())
				{
					applyValidatorNode->addAttr(std::make_pair(ApplyValidator::Attributes::EnabledInTeamwork, SSUtils::String::toString(applyValidator.getEnabledInTeamwork())));
					applyValidatorNode->addAttr(std::make_pair(ApplyValidator::Attributes::MaxApply, std::to_string(applyValidator.getMaxApply())));
				}
				root->addChild(std::move(applyValidatorNode));
			}

			{
				std::shared_ptr<SSUtils::XML::Node> principalInfoNode(SSUtils::XML::Node::generate(PrincipalInfo::Tag));
				const auto &principalInfo(info->getPrincipalInfo());

				std::shared_ptr<SSUtils::XML::Node> nameNode(SSUtils::XML::Node::generate(PrincipalInfo::NameTag));
				nameNode->setContent(principalInfo.getName());
				principalInfoNode->addChild(std::move(nameNode));

				std::shared_ptr<SSUtils::XML::Node> telephoneNode(SSUtils::XML::Node::generate(PrincipalInfo::TelephoneTag));
				telephoneNode->setContent(principalInfo.getTelephone());
				principalInfoNode->addChild(std::move(telephoneNode));

				std::shared_ptr<SSUtils::XML::Node> emailNode(SSUtils::XML::Node::generate(PrincipalInfo::EMailTag));
				emailNode->setContent(principalInfo.getEmmail());
				principalInfoNode->addChild(std::move(emailNode));

				root->addChild(std::move(principalInfoNode));
			}

			{
				std::shared_ptr<SSUtils::XML::Node> dateInfoNode(SSUtils::XML::Node::generate(DateInfo::Tag));
				const auto &dateInfo(info->getDateInfo());

				for (const auto &date : dateInfo.getDates())
				{
					std::shared_ptr<SSUtils::XML::Node> dateNode(SSUtils::XML::Node::generate(DateInfo::DateTag));
					dateNode->setContent(date.toString());
					dateInfoNode->addChild(std::move(dateNode));
				}
				root->addChild(std::move(dateInfoNode));
			}

			{
				std::shared_ptr<SSUtils::XML::Node> typeInfoNode(SSUtils::XML::Node::generate(TypeInfo::Tag));
				const auto &typeInfo(info->getTypeInfo());

				for (const auto &type : typeInfo.getTypes())
				{
					std::shared_ptr<SSUtils::XML::Node> typeNode(SSUtils::XML::Node::generate(TypeInfo::TypeTag));
					typeNode->addAttr(std::make_pair(TypeInfo::Attributes::Id, type.first));
					typeNode->setContent(type.second);
					typeInfoNode->addChild(std::move(typeNode));
				}

				root->addChild(std::move(typeInfoNode));
			}

			{
				std::shared_ptr<SSUtils::XML::Node> rankInfoNode(SSUtils::XML::Node::generate(RankInfo::Tag));
				const auto &rankInfo(info->getRankInfo());

				rankInfoNode->addAttr(std::make_pair(RankInfo::Attributes::Enabled, SSUtils::String::toString(rankInfo.getEnabled())));
				if (rankInfo.getEnabled())
				{
					rankInfoNode->addAttr(std::make_pair(RankInfo::Attributes::Forced, SSUtils::String::toString(rankInfo.getForced())));

					for (const auto &rank : rankInfo.getRanks())
					{
						std::shared_ptr<SSUtils::XML::Node> rankNode(SSUtils::XML::Node::generate(RankInfo::RankTag));
						rankNode->addAttr(std::make_pair(RankInfo::Attributes::Id, rank.first));
						rankNode->setContent(rank.second);
						rankInfoNode->addChild(std::move(rankNode));
					}

					std::shared_ptr<SSUtils::XML::Node> defaultRankNode(SSUtils::XML::Node::generate(RankInfo::DefaultRankTag));
					defaultRankNode->addAttr(std::make_pair(RankInfo::Attributes::DefaultRankType, RankInfo::DefaultRankTypes::Id));
					defaultRankNode->setContent(rankInfo.getDefaultRank().first);
					rankInfoNode->addChild(std::move(defaultRankNode));
				}

				root->addChild(std::move(rankInfoNode));
			}

			{
				std::shared_ptr<SSUtils::XML::Node> teamInfoNode(SSUtils::XML::Node::generate(TeamInfo::Tag));
				const auto &teamInfo(info->getTeamInfo());

				for (const auto &team : teamInfo.getTeams())
				{
					std::shared_ptr<SSUtils::XML::Node> teamNode(SSUtils::XML::Node::generate(TeamInfo::TeamTag));
					teamNode->addAttr(std::make_pair(TeamInfo::Attributes::Id, team.id));
					teamNode->addAttr(std::make_pair(TeamInfo::Attributes::ShortName, team.shortName));
					teamNode->setContent(team.name);
					teamInfoNode->addChild(std::move(teamNode));
				}

				root->addChild(std::move(teamInfoNode));
			}

			{
				std::shared_ptr<SSUtils::XML::Node> publicScoreInfoNode(SSUtils::XML::Node::generate(CompetitionInfo::PublicScoreInfoTag));
				const auto &publicScoreInfo(info->getPublicScoreInfo());

				std::shared_ptr<SSUtils::XML::Node> scoresNode(SSUtils::XML::Node::generate(ScoreInfo::ScoresTag));
				scoresNode->setContent(SSUtils::String::join(publicScoreInfo.getScores()));
				publicScoreInfoNode->addChild(std::move(scoresNode));

				std::shared_ptr<SSUtils::XML::Node> scoreRateNode(SSUtils::XML::Node::generate(ScoreInfo::ScoreRateTag));
				scoreRateNode->setContent(std::to_string(publicScoreInfo.getScoreRate()));
				publicScoreInfoNode->addChild(std::move(scoreRateNode));

				std::shared_ptr<SSUtils::XML::Node> breakRecordRateNode(SSUtils::XML::Node::generate(ScoreInfo::BreakRecordRateTag));
				breakRecordRateNode->addAttr(std::make_pair(ScoreInfo::Attributes::Enabled, SSUtils::String::toString(publicScoreInfo.getBreakRecordRateEnable())));
				if (publicScoreInfo.getBreakRecordRateEnable())
				{
					breakRecordRateNode->setContent(std::to_string(publicScoreInfo.getBreakRecordRate()));
				}
				publicScoreInfoNode->addChild(breakRecordRateNode);

				root->addChild(publicScoreInfoNode);
			}

			SSUtils::XML::Document doc;
			doc.getRoots().push_back(root);
			return doc.toFile(outUrl);
		}

		const std::shared_ptr<CompetitionInfo> readFromXML(const std::string & inUrl)
		{
			return std::shared_ptr<CompetitionInfo>();
		}
	};
};
