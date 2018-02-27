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

			return XMLUtils::saveToFile(outUrl, root);
		}

		const std::shared_ptr<CompetitionInfo> readFromXML(const std::string & inUrl)
		{
			return std::shared_ptr<CompetitionInfo>();
		}
	};
};
