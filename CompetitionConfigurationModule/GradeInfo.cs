using System;
using System.Collections.Generic;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class GradeInfo
        {
            public enum BetterType
            {
                Smaller,
                Bigger
            };

            public enum CompareResult
            {
                Worse,
                Equal,
                Better
            };

            public enum PatternType
            {
                None,
                XSD,
                JSON,
                Other
            };

            public delegate CompareResult GradeComparer(String lhs, String rhs);

            public static readonly Dictionary<BetterType, Dictionary<CompareResult, CompareResult>> CompareResultTranslators
                = new Dictionary<BetterType, Dictionary<CompareResult, CompareResult>>
                { { BetterType.Bigger, new Dictionary<CompareResult, CompareResult>
                { { CompareResult.Worse, CompareResult.Worse }, { CompareResult.Equal, CompareResult.Equal }, { CompareResult.Better, CompareResult.Better } } },
              { BetterType.Smaller, new Dictionary<CompareResult, CompareResult>
                { { CompareResult.Worse, CompareResult.Better }, { CompareResult.Equal, CompareResult.Equal }, { CompareResult.Better, CompareResult.Worse } } }
                };

            public BetterType GradeBetterType
            {
                get;
                set;
            }

            public PatternType GradePatternType
            {
                get;
                set;
            }

            public String GradePattern
            {
                get;
                set;
            }

            public void SetGradePattern(PatternType gradePatternType, String gradePattern)
            {
                GradePatternType = gradePatternType;
                GradePattern = gradePattern;
            }

            public static CompareResult Compare(String lhs, String rhs, GradeComparer comparer, BetterType betterType)
            {
                return CompareResultTranslators[betterType][comparer(lhs, rhs)];
            }

            public CompareResult Compare(String lhs, String rhs, GradeComparer comparer)
            {
                return Compare(lhs, rhs, comparer, GradeBetterType);
            }
        };
    };
};
