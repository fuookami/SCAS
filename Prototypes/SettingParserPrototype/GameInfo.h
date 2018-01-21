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

		public:
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

		public:
			GameTypeInfo(const std::string &eventInfoId, const std::shared_ptr<EventInfo> &refEventInfo = nullptr);
			GameTypeInfo(const GameTypeInfo &ano);
			GameTypeInfo(const GameTypeInfo &&ano);
			GameTypeInfo &operator=(const GameTypeInfo &rhs);
			GameTypeInfo &operator=(const GameTypeInfo &&rhs);
			~GameTypeInfo(void);

			inline const std::string &getEventInfoId(void) const { return m_eventInfoId; }
			
			inline const std::shared_ptr<EventInfo> &getRefEventInfo(void) const { return ref_eventInfo; }
			inline void setRefEventInfo(const std::shared_ptr<EventInfo> &refEventInfo) { ref_eventInfo = refEventInfo; }

			inline const eType getType(void) const { return m_type; }
			inline void setType(const eType type) { m_type = type; }

			inline const ePattern getPattern(void) const { return m_pattern; }
			inline void setPattern(const ePattern pattern) { m_pattern = pattern; }

			inline const uint32 getOrderInEvent(void) const { return m_orderInEvent; }
			inline void setOrderInEvent(const uint32 orderInEvent) { m_orderInEvent = orderInEvent; }

			inline const uint32 getOrderInType(void) const { return m_orderInType; }
			inline void setOrderInType(const uint32 orderInType) { m_orderInType = orderInType; }

		private:
			std::string m_eventInfoId;
			std::shared_ptr<EventInfo> ref_eventInfo;

			eType m_type;
			ePattern m_pattern;

			uint32 m_orderInEvent;
			uint32 m_orderInType;
		};

		class GameInfo
		{
		public:
			static const uint32 NoAthleteNumber = 0;

		public:
			GameInfo(const std::string &id = "");
			GameInfo(const std::string &&id);
			GameInfo(const GameInfo &ano);
			GameInfo(const GameInfo &&ano);
			GameInfo &operator=(const GameInfo &rhs);
			GameInfo &operator=(const GameInfo &&rhs);
			~GameInfo(void);

			inline const std::string &getId(void) const { return m_id; }

			inline const std::string &getName(void) const { return m_name; }
			inline void setName(const std::string &name) { m_name.assign(name); }
			inline void setName(const std::string &&name) { m_name.assign(std::move(name)); }

			inline const uint32 getAthleteNumber(void) const { return m_athleteNumber; }
			inline void setAthleteNumber(const uint32 athleteNumber) { m_athleteNumber = athleteNumber; }

			inline const uint32 getOrderInDay(void) const { return m_orderInDay; }
			inline void setOrderInDay(const uint32 orderInDay) { m_orderInDay = orderInDay; }

			inline DateTimeUtils::Time &getPlanIntervalTime(void) { return m_planIntervalTime; }
			inline const DateTimeUtils::Time &getPlanIntervalTime(void) const { return m_planIntervalTime; }
			inline void setPlanIntervalTime(const DateTimeUtils::Time &planIntervalTime) { m_planIntervalTime = planIntervalTime; }
			inline void setPlanIntervalTime(const DateTimeUtils::Time &&planIntervalTime) { m_planIntervalTime = std::move(planIntervalTime); }

			inline DateTimeUtils::Time &getPlanIntervalTime(void) { return m_planTimePerGroup; }
			inline const DateTimeUtils::Time &getPlanTimePerGroup(void) const { return m_planTimePerGroup; }
			inline void setPlanTimePerGroup(const DateTimeUtils::Time &planTimePerGroup) { m_planTimePerGroup = planTimePerGroup; }
			inline void setPlanTimePerGroup(const DateTimeUtils::Time &&planTimePerGroup) { m_planTimePerGroup = std::move(planTimePerGroup); }

			inline GameTypeInfo &getGameTypeInfo(void) { return m_gameTypeInfo; }
			inline const GameTypeInfo &getGameTypeInfo(void) const { return m_gameTypeInfo; }
			inline void getGameTypeInfo(const GameTypeInfo &gameTypeInfo) { m_gameTypeInfo = gameTypeInfo; }
			inline void getGameTypeInfo(const GameTypeInfo &&gameTypeInfo) { m_gameTypeInfo = std::move(gameTypeInfo); }

			inline GroupInfo &getGroupInfo(void) { return m_groupInfo; }
			inline const GroupInfo &getGroupInfo(void) const { return m_groupInfo; }
			inline void getGroupInfo(const GroupInfo &groupInfo) { m_groupInfo = groupInfo; }
			inline void getGroupInfo(const GroupInfo &&groupInfo) { m_groupInfo = std::move(groupInfo); }

		private:
			const std::string m_id;
			std::string m_name;

			uint32 m_athleteNumber;
			uint32 m_orderInDay;

			DateTimeUtils::Time m_planIntervalTime;
			DateTimeUtils::Time m_planTimePerGroup;

			GameTypeInfo m_gameTypeInfo;
			GroupInfo m_groupInfo;
		};
	};
};
