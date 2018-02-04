#include "GradeInfo.h"

namespace SCAS
{
	namespace CompCfg
	{
		GradeInfo::GradeInfo(void)
			: m_type(eType::Duration), m_betterType(eBetterType::Smaller), m_precision(NoPrecision)
		{
		}
	};
};
