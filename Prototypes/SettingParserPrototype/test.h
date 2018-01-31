#pragma once

#include <memory>
#include "SCASCompCfgParser.h"

const std::shared_ptr<SCAS::CompCfg::CompetitionInfo> generateTestCompetitionInfo(void);
void testSaveToXML(void);
void testLoadFromXML(void);
