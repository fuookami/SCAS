#include "test.h"

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> generateTestCompetitionInfo(void)
{
	using namespace SCAS::CompCfg;

	std::shared_ptr<CompetitionInfo> ret(new CompetitionInfo());

	ret->setName("某游泳比赛");
	ret->setSubName("暨某游泳比赛");

	PrincipalInfo principalInfo;
	principalInfo.setName("负责人的姓名");
	principalInfo.setTelephone("负责人的手机");
	principalInfo.setEmail("负责人的邮箱@xxx.xxx");
	ret->setPrincipalInfo(std::move(principalInfo));

	DateInfo dateinfo;
	dateinfo.getDates().push_back(DatetimeUtils::getLocalDate());
	dateinfo.getDates().push_back(DatetimeUtils::getDateAfterLocalDate(1));
	ret->setDateInfo(std::move(dateinfo));

	TypeInfo typeInfo;
	typeInfo.getTypes().push_back(std::make_pair(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "学生男子"));
	typeInfo.getTypes().push_back(std::make_pair(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "学生女子"));
	typeInfo.getTypes().push_back(std::make_pair(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "教师男子"));
	typeInfo.getTypes().push_back(std::make_pair(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "教师女子"));
	ret->setTypeInfo(std::move(typeInfo));

	RankInfo rankInfo;
	rankInfo.setEnabled();
	rankInfo.setForced(true);
	rankInfo.getRanks().push_back(RankInfo::RankDataType(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "乙组"));
	rankInfo.getRanks().push_back(RankInfo::RankDataType(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "丙组"));
	RankInfo::RankDataType defaultRank(DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1())), "甲组");
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
		TeamInfo::Team team;
		team.id = DataUtils::toBase64String(DataUtils::fromArray(UUIDUtil::generateUUIDV1()));
		team.shortName = teamName.first;
		team.name = teamName.second;
		teamInfo.getTeams().push_back(std::move(team));
	}
	ret->setTeamInfo(std::move(teamInfo));

	std::shared_ptr<EventInfo> eventInfo_dual(new EventInfo());
	std::shared_ptr<EventInfo> eventInfo_single_rank(new EventInfo());
	std::shared_ptr<EventInfo> eventInfo_plural_rank(new EventInfo());
	std::shared_ptr<EventInfo> eventInfo_dual_team(new EventInfo());
	std::shared_ptr<EventInfo> eventInfo_single_rank_team(new EventInfo());
	std::shared_ptr<EventInfo> eventInfo_plural_rank_team(new EventInfo());

	ret->getEventInfos().insert(std::make_pair(eventInfo_dual->getId(), eventInfo_dual));
	ret->getEventInfos().insert(std::make_pair(eventInfo_single_rank->getId(), eventInfo_single_rank));
	ret->getEventInfos().insert(std::make_pair(eventInfo_plural_rank->getId(), eventInfo_plural_rank));
	ret->getEventInfos().insert(std::make_pair(eventInfo_dual_team->getId(), eventInfo_dual_team));
	ret->getEventInfos().insert(std::make_pair(eventInfo_single_rank_team->getId(), eventInfo_single_rank_team));
	ret->getEventInfos().insert(std::make_pair(eventInfo_plural_rank_team->getId(), eventInfo_plural_rank_team));

	return ret;
}

void testSaveToXML(const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> info)
{
}

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> testLoadFromXML(void)
{
	return nullptr;
}
