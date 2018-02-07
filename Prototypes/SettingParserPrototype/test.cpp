#include "test.h"

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> generateTestCompetitionInfo(void)
{
	using namespace SCAS::CompCfg;

	std::shared_ptr<CompetitionInfo> ret(new CompetitionInfo());

	ret->setName("ĳ��Ӿ����");
	ret->setSubName("��ĳ��Ӿ����");

	PrincipalInfo principalInfo;
	principalInfo.setName("�����˵�����");
	principalInfo.setTelephone("�����˵��ֻ�");
	principalInfo.setEmail("�����˵�����@xxx.xxx");
	ret->setPrincipalInfo(std::move(principalInfo));

	DateInfo dateinfo;
	dateinfo.getDates().push_back(DatetimeUtils::getLocalDate());
	dateinfo.getDates().push_back(DatetimeUtils::getDateAfterLocalDate(1));
	ret->setDateInfo(std::move(dateinfo));

	return ret;
}

void testSaveToXML(const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> info)
{
}

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> testLoadFromXML(void)
{
	return nullptr;
}
