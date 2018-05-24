#include "test.h"
#include "SSUtils\XMLUtils.h"
#include "SSUtils\StringUtils.h"
#include "SCASCompCfgParser.h"

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> generateTestCompetitionInfo(void)
{
	using namespace SCAS::CompCfg;

	std::shared_ptr<CompetitionInfo> ret(new CompetitionInfo());

	ret->setName("ĳ��Ӿ����");
	ret->setSubName("��ĳ��Ӿ����");
	ret->setVersion("ĳ��ĳ������ĳ�棨���ԣ�");
	ret->setIdentifier("���Լ����ı�ʶ��/��ǩ֮���");

	ApplyValidator applyValidator;
	applyValidator.setEnabled();
	applyValidator.setEnabledInTeamwork(false);
	applyValidator.setMaxApply(2);
	ret->setApplyValidator(std::move(applyValidator));

	PrincipalInfo principalInfo;
	principalInfo.setName("�����˵�����");
	principalInfo.setTelephone("�����˵��ֻ�");
	principalInfo.setEmail("�����˵�����@xxx.xxx");
	ret->setPrincipalInfo(std::move(principalInfo));

	DateInfo dateinfo;
	dateinfo.getDates().push_back(SSUtils::Datetime::getLocalDate());
	dateinfo.getDates().push_back(SSUtils::Datetime::getDateAfterLocalDate(1));
	ret->setDateInfo(std::move(dateinfo));

	TypeInfo typeInfo;
	typeInfo.getTypes().push_back(TypeInfo::generate("ѧ������"));
	typeInfo.getTypes().push_back(TypeInfo::generate("ѧ��Ů��"));
	typeInfo.getTypes().push_back(TypeInfo::generate("��ʦ����"));
	typeInfo.getTypes().push_back(TypeInfo::generate("��ʦŮ��"));
	ret->setTypeInfo(std::move(typeInfo));

	RankInfo rankInfo;
	rankInfo.setEnabled();
	rankInfo.setForced(true);
	rankInfo.getRanks().push_back(RankInfo::generate("����"));
	rankInfo.getRanks().push_back(RankInfo::generate("����"));
	RankInfo::RankDataType defaultRank(RankInfo::generate("����"));
	rankInfo.getRanks().push_back(defaultRank);
	rankInfo.setDefaultRank(std::move(defaultRank));
	ret->setRankInfo(std::move(rankInfo));

	ScoreInfo scoreInfo;
	scoreInfo.setScores(std::vector<int>({ 9, 7, 6, 5, 4, 3, 2, 1 }));
	scoreInfo.setScoreRate(1.0f);
	scoreInfo.setBreakRecordRateEnable(true);
	scoreInfo.setBreakRecordRate(2.0f);
	ret->setPublicScoreInfo(std::move(scoreInfo));

	TeamInfo teamInfo;
	static const std::vector<std::pair<std::string, std::string>> teamNames =
	{
		std::make_pair("һԺ", "�����ѧԺ"), std::make_pair("��Ժ", "��Դ�붯��ѧԺ"), std::make_pair("��Ժ", "�Զ���ѧԺ")
	};
	for (const auto &teamName : teamNames)
	{
		teamInfo.getTeams().push_back(std::move(TeamInfo::generate(teamName.first, teamName.second)));
	}
	ret->setTeamInfo(std::move(teamInfo));

 	// �Կ�������
	std::shared_ptr<EventInfo> eventInfo_dual(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_dual->getId(), eventInfo_dual));
	ret->setName("һ���Կ��������ӣ�ѭ��С���� + ��̭��������");

	std::shared_ptr<EventInfo> eventInfo_single_rank(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_single_rank->getId(), eventInfo_single_rank));
	ret->setName("һ�������������ӣ��������ƣ������������Σ�");

	std::shared_ptr<EventInfo> eventInfo_plural_rank(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_plural_rank->getId(), eventInfo_plural_rank));
	ret->setName("һ�������������ӣ������ƣ�ȫ������һ�������Σ�");

	std::shared_ptr<EventInfo> eventInfo_dual_team(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_dual_team->getId(), eventInfo_dual_team));
	ret->setName("һ���Կ��������ӣ��Ŷӣ�ѭ��С���� + ��̭��������");

	std::shared_ptr<EventInfo> eventInfo_single_rank_team(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_single_rank_team->getId(), eventInfo_single_rank_team));
	ret->setName("һ�������������ӣ��Ŷӣ��������ƣ������������Σ�");

	std::shared_ptr<EventInfo> eventInfo_plural_rank_team(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_plural_rank_team->getId(), eventInfo_plural_rank_team));
	ret->setName("һ�������������ӣ��Ŷӣ������ƣ�ȫ������һ�������Σ�");

	return ret;
}

void testSaveToXML(const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> info)
{
	if (SCAS::CompCfg::saveToXML(info, "testSetting.xml"))
	{
		std::cout << "OK to write" << std::endl;
	}
	else
	{
		std::cout << "False to write" << std::endl;
	}
}

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> testLoadFromXML(void)
{
	auto ret(SCAS::CompCfg::readFromXML("testSetting.xml"));
	if (ret != nullptr)
	{
		std::cout << "OK to read" << std::endl;
	}
	else
	{
		std::cout << "OK to write" << std::endl;
	}
	return ret;
}
