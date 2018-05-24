#include "test.h"
#include "SSUtils\XMLUtils.h"
#include "SSUtils\StringUtils.h"
#include "SCASCompCfgParser.h"

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> generateTestCompetitionInfo(void)
{
	using namespace SCAS::CompCfg;

	std::shared_ptr<CompetitionInfo> ret(new CompetitionInfo());

	ret->setName("某游泳比赛");
	ret->setSubName("暨某游泳比赛");
	ret->setVersion("某年某比赛第某版（测试）");
	ret->setIdentifier("给自己看的标识符/标签之类的");

	ApplyValidator applyValidator;
	applyValidator.setEnabled();
	applyValidator.setEnabledInTeamwork(false);
	applyValidator.setMaxApply(2);
	ret->setApplyValidator(std::move(applyValidator));

	PrincipalInfo principalInfo;
	principalInfo.setName("负责人的姓名");
	principalInfo.setTelephone("负责人的手机");
	principalInfo.setEmail("负责人的邮箱@xxx.xxx");
	ret->setPrincipalInfo(std::move(principalInfo));

	DateInfo dateinfo;
	dateinfo.getDates().push_back(SSUtils::Datetime::getLocalDate());
	dateinfo.getDates().push_back(SSUtils::Datetime::getDateAfterLocalDate(1));
	ret->setDateInfo(std::move(dateinfo));

	TypeInfo typeInfo;
	typeInfo.getTypes().push_back(TypeInfo::generate("学生男子"));
	typeInfo.getTypes().push_back(TypeInfo::generate("学生女子"));
	typeInfo.getTypes().push_back(TypeInfo::generate("教师男子"));
	typeInfo.getTypes().push_back(TypeInfo::generate("教师女子"));
	ret->setTypeInfo(std::move(typeInfo));

	RankInfo rankInfo;
	rankInfo.setEnabled();
	rankInfo.setForced(true);
	rankInfo.getRanks().push_back(RankInfo::generate("乙组"));
	rankInfo.getRanks().push_back(RankInfo::generate("丙组"));
	RankInfo::RankDataType defaultRank(RankInfo::generate("甲组"));
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
		std::make_pair("一院", "航空宇航学院"), std::make_pair("二院", "能源与动力学院"), std::make_pair("三院", "自动化学院")
	};
	for (const auto &teamName : teamNames)
	{
		teamInfo.getTeams().push_back(std::move(TeamInfo::generate(teamName.first, teamName.second)));
	}
	ret->setTeamInfo(std::move(teamInfo));

 	// 对抗赛例子
	std::shared_ptr<EventInfo> eventInfo_dual(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_dual->getId(), eventInfo_dual));
	ret->setName("一个对抗赛的例子（循环小组赛 + 淘汰晋级赛）");

	std::shared_ptr<EventInfo> eventInfo_single_rank(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_single_rank->getId(), eventInfo_single_rank));
	ret->setName("一个名次赛的例子（竞标赛制，单组内算名次）");

	std::shared_ptr<EventInfo> eventInfo_plural_rank(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_plural_rank->getId(), eventInfo_plural_rank));
	ret->setName("一个名次赛的例子（名次制，全部组在一起算名次）");

	std::shared_ptr<EventInfo> eventInfo_dual_team(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_dual_team->getId(), eventInfo_dual_team));
	ret->setName("一个对抗赛的例子（团队，循环小组赛 + 淘汰晋级赛）");

	std::shared_ptr<EventInfo> eventInfo_single_rank_team(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_single_rank_team->getId(), eventInfo_single_rank_team));
	ret->setName("一个名次赛的例子（团队，竞标赛制，单组内算名次）");

	std::shared_ptr<EventInfo> eventInfo_plural_rank_team(new EventInfo());
	ret->getEventInfos().insert(std::make_pair(eventInfo_plural_rank_team->getId(), eventInfo_plural_rank_team));
	ret->setName("一个名次赛的例子（团队，名次制，全部组在一起算名次）");

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
