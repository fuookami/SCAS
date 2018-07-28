using CompetitionConfigurationModule;
using System;
using System.Collections.Generic;

namespace TestConfigurationModule
{
    class Program
    {
        static void Main(string[] args)
        {
            var data1 = GenerateTestCompetitionInfo();
            TestSaveToXML(data1);
            var data2 = TestLoadFromXML();
            Console.ReadKey();
        }

        static CompetitionInfo GenerateTestCompetitionInfo()
        {
            CompetitionInfo ret = new CompetitionInfo();

            ret.Name = "某游泳比赛";
            ret.SubName = "暨某游泳比赛";
            ret.Version = "某年某比赛第某版本（测试）";
            ret.Identifier = "给自己看的标识符/标签之类的";

            ret.CompetitionApplicationValidator = new ApplicationValidator
            {
                Enabled = true,
                EnabledInTeamwork = false,
                MaxApplicationNumberPerTeam = 2
            };

            ret.CompetitionPrincipalInfo = new PrincipalInfo()
            {
                Name = "负责人的姓名",
                Telephone = "负责人的手机",
                Email = "负责人的邮箱@xxx.xx"
            };

            ret.Dates = new List<Date> { Date.Today, Date.Today.Tomorrow };

            ret.AthleteCategories.GenerateNewCategory().Name = "学生男子";
            ret.AthleteCategories.GenerateNewCategory().Name = "学生女子";
            ret.AthleteCategories.GenerateNewCategory().Name = "教师男子";
            ret.AthleteCategories.GenerateNewCategory().Name = "教师女子";

            ret.CompetitionRankInfo.Enabled = true;
            ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank().Name = "甲组";
            ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank().Name = "乙组";
            ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank().Name = "丙组";
            ret.CompetitionRankInfo.Forced = true;

            ret.PublicPointInfo.Points = new List<UInt32> { 9, 7, 6, 5, 4, 3, 2, 1 };
            ret.PublicPointInfo.PointRate = 1.0;
            ret.PublicPointInfo.BreakRecordPointRateEnabled = true;
            ret.PublicPointInfo.BreakRecordPointRate = 2.0;

            ret.TeamCategories.GenerateNewCategory().Name = "普通队";

            List<Tuple<String, String>> teamInfos = new List<Tuple<String, String>>
            {
                new Tuple<String, String >("一院", "航空宇航学院"),
                new Tuple<String, String >("二院", "能源动力学院" ),
                new Tuple<String, String >("三院", "自动化学院" ),
            };
            foreach (var info in teamInfos)
            {
                var teamInfo = ret.TeamInfos.GenerateNewInfo(ret.TeamCategories[0]);
                teamInfo.ShortName = info.Item1;
                teamInfo.Name = info.Item2;
            }
            
            return ret;
        }

        static void TestSaveToXML(CompetitionInfo info)
        {
            CompetitionConfigurationNormalizer normalizer = new CompetitionConfigurationNormalizer(info);
            if (normalizer.NormalizeToFile("test.xml"))
            {
                Console.WriteLine("OK to write.");
            }
            else
            {
                Console.WriteLine("False to write. {0}", normalizer.LastError);
            }
        }

        static CompetitionInfo TestLoadFromXML()
        {
            CompetitionConfigurationAnalyzer analyzer = new CompetitionConfigurationAnalyzer();
            if (analyzer.Analyze("test.xml"))
            {
                Console.WriteLine("OK to read.");
                return analyzer.Result;
            }
            else
            {
                Console.WriteLine("False to read. {0}", analyzer.LastError);
                return null;
            }
        }
    }
}
