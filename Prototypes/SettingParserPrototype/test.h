#pragma once

#include <memory>
#include "SCASCompCfgParser.h"

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> generateTestCompetitionInfo(void);
void testSaveToXML(const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> info);
const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> testLoadFromXML(void);
