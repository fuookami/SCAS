#pragma once

#include "SCASPublic.h"

#include <map>

namespace SCAS
{
	namespace CompCfg
	{
		// ֻ��¼����ĳɼ�
		// �ײ�ĳɼ����뷨����¼һ��XSD Schema�����Լ�һ���ַ������н�����洢 / ֻ��¼һ��JSON�ַ���,�����ʹ洢����Ӧ�ò���
		class GradeInfo	
		{
		public:
			static const uint32 NoPrecision = 0;

			enum class eType
			{
				Speed,			// �ٶ�
				Duration,		// ����ʱ��
				Score			// ����
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
				static_assert(false, std::string("����ֱ�ӵ��ã�����������ҲӦ�õ�������"));
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

			uint32 m_precision;		// С��λ�������ɼ�ֵӦ����2λС������ʵ�ʱ����ֵΪ�ɼ�*pow(10, 2)ת���ɵ�����
		};
	};
};
