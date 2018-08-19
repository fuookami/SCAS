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

            private BetterType betterType;
            private PatternType patternType;
            private String pattern;

            public BetterType GradeBetterType
            {
                get { return betterType; }
                set { betterType = value; }
            }

            public PatternType GradePatternType
            {
                get { return patternType; }
            }

            public String GradePattern
            {
                get { return pattern; }
            }

            public void SetGradePattern(PatternType _gradePatternType, String _gradePattern)
            {
                patternType = _gradePatternType;
                pattern = _gradePattern;
            }

            public static CompareResult Compare(String lhs, String rhs, GradeComparer comparer, BetterType betterType)
            {
                return CompareResultTranslators[betterType][comparer(lhs, rhs)];
            }

            public CompareResult Compare(String lhs, String rhs, GradeComparer comparer)
            {
                return Compare(lhs, rhs, comparer, betterType);
            }
        };
    };
};
