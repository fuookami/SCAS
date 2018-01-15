#pragma once

#include "SCASPublic.h"

#include <map>

namespace SCAS
{
	namespace CompCfg
	{
		// 只记录顶层的成绩
		// 底层的成绩的想法：记录一个XSD Schema对象以及一个字符串进行解析与存储 / 只记录一个JSON字符串,解析和存储交由应用层解决
		class GradeInfo	
		{
		public:
			static const uint32 NoPrecision = 0;

			enum class eType
			{
				Speed,			// 速度
				Duration,		// 持续时间
				Score			// 分数
			};

			enum class eBetterType
			{
				Smaller,
				Bigger
			};

			static const std::map<eType, eBetterType> Type2BetterType;

			template <GradeInfo::eBetterType betterType>
			static const bool _compareGrade(const int lhs, const int rhs)
			{
				static_assert(false, std::string("不可直接调用，按道理来讲也应该调不出来"));
				std::abort();
				return false;
			}

			template <>
			static const bool _compareGrade<GradeInfo::eBetterType::Smaller>(const int lhs, const int rhs)
			{
				return lhs < rhs;
			}

			template<>
			static const bool _compareGrade<GradeInfo::eBetterType::Bigger>(const int lhs, const int rhs)
			{
				return lhs > rhs;
			}

			template <GradeInfo::eType type>
			inline static const bool compareGrade(const int lhs, const int rhs)
			{
				return _compareGrade<Type2BetterType.find(type)->second>(lhs, rhs);
			}

		public:
			GradeInfo(void);
			GradeInfo(const GradeInfo &ano);
			GradeInfo(const GradeInfo &&ano);
			GradeInfo &operator=(const GradeInfo &rhs);
			GradeInfo &operator=(const GradeInfo &&rhs);
			~GradeInfo(void);

			inline const eType getType(void) const { return m_type; }
			inline void setType(const eType type) { m_type = type; m_betterType = Type2BetterType.find(type)->second; }
			inline void setType(const eType type, const eBetterType betterType) { m_type = type; m_betterType = betterType; }

			inline const eBetterType getBetterType(void) const { return m_betterType; }
			inline void setBetterType(const eBetterType betterType) { m_betterType = betterType; }

			inline const uint32 getPrecision(void) const { return m_precision; }
			inline void setPrecision(const uint32 precision) { m_precision = precision; }

		private:
			eType m_type;
			eBetterType m_betterType;

			uint32 m_precision;		// 小数位数，若成绩值应保留2位小数，则实际保存的值为成绩*pow(10, 2)转换成的整形
		};
	};
};
