#pragma once

#include "EventInfo.h"

#include <vector>
#include <memory>
#include "DateTimeUtils.h"

namespace SCAS
{
	namespace CompCfg
	{
		class GroupInfo
		{
		public:
			static const int NoGroup = -1;

			GroupInfo(void);
			GroupInfo(const GroupInfo &ano);
			GroupInfo(const GroupInfo &&ano);
			GroupInfo &operator=(const GroupInfo &rhs);
			GroupInfo &operator=(const GroupInfo &&rhs);
			~GroupInfo(void);

			inline const bool getEnabled(void) const { return m_enabled; }
			inline const int getPeoplePerGroup(void) const { return m_peoplePerGroup; }

			inline void setDisabled(void) { m_enabled = false; m_peoplePerGroup = NoGroup; }
			inline void setEnabled(const int peoplePerGroup) { m_enabled = true; m_peoplePerGroup = peoplePerGroup; }

		private:
			bool m_enabled;
			int m_peoplePerGroup;
		};

		class GameTypeInfo
		{
		public:
			enum class eType
			{
				Group,				// 小组赛
				QuarterFinal,		// 复赛
				SemiFinal,			// 半决赛
				ThridPlaceMatch,	// 三四名决赛（仅在对抗性运动中出现）
				Fianls				// 决赛
			};

			enum class ePattern
			{
				Elimination,		// 淘汰制 / 竞标赛制
				Ranking,			// 名次制（仅在非对抗性运动中出现）
				Circulation			// 循环制（仅在对抗性运动中出现）
			};
		private:
			std::string m_eventInfoId;
			std::shared_ptr<EventInfo> ref_eventInfo;

			eType type;
			ePattern m_pattern;
		};

		class GameInfo
		{
		private:
			std::string m_id;
			std::string m_name;
			
			DateTimeUtils::TimeDuration m_planIntervalTime;
			DateTimeUtils::TimeDuration m_planTimePerGroup;

			GameTypeInfo m_gameTypeInfo;
			GroupInfo m_groupInfo;
		};
	};
};
