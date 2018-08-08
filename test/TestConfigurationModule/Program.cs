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
                MaxApplicationNumberPerAthlete = 2
            };

            ret.CompetitionPrincipalInfo = new PrincipalInfo()
            {
                Name = "负责人的姓名",
                Telephone = "负责人的手机",
                Email = "负责人的邮箱@xxx.xx",
                Others = new Dictionary<string, string>
                {
                    { "QQ", "假装是个QQ号" },
                    { "微信", "假装是个微信号" },
                    { "备注", "随便写点什么" }
                }
            };

            ret.PublicPointInfo.Points = new List<UInt32> { 9, 7, 6, 5, 4, 3, 2, 1 };
            ret.PublicPointInfo.PointRate = 1.0;
            ret.PublicPointInfo.BreakRecordPointRateEnabled = true;
            ret.PublicPointInfo.BreakRecordPointRate = 2.0;

            SessionPool sessions = new SessionPool();
            sessions.AddDate(Date.Today);
            var todaySession1 = sessions.GenerateNewSession(Date.Today);
            todaySession1.Name = "上半场";
            todaySession1.FullName = String.Format("{0}月{1}日 上半场", Date.Today.Month, Date.Today.Day);
            var todaySession2 = sessions.GenerateNewSession(Date.Today);
            todaySession2.Name = "下半场";
            todaySession2.FullName = String.Format("{0}月{1}日 下半场", Date.Today.Month, Date.Today.Day);
            sessions.AddDate(Date.Today.Tomorrow);
            var tomorrowSession1 = sessions.GenerateNewSession(Date.Today.Tomorrow);
            tomorrowSession1.Name = "上半场";
            tomorrowSession1.FullName = String.Format("{0}月{1}日 上半场", Date.Today.Tomorrow.Month, Date.Today.Tomorrow.Day);
            var tomorrowSession2 = sessions.GenerateNewSession(Date.Today.Tomorrow);
            tomorrowSession2.Name = "下半场";
            tomorrowSession2.FullName = String.Format("{0}月{1}日 下半场", Date.Today.Tomorrow.Month, Date.Today.Tomorrow.Day);
            ret.Sessions = sessions;

            ret.Field = "某游泳馆";

            ret.AthleteCategories.GenerateNewCategory().Name = "学生男子";
            ret.AthleteCategories.GenerateNewCategory().Name = "学生女子";
            ret.AthleteCategories.GenerateNewCategory().Name = "教师男子";
            ret.AthleteCategories.GenerateNewCategory().Name = "教师女子";

            ret.CompetitionRankInfo.Enabled = true;
            ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank().Name = "甲组";
            ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank().Name = "乙组";
            ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank().Name = "丙组";
            ret.CompetitionRankInfo.Forced = true;

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

            GenerateEvent1(ret);
            GenerateEvent2(ret);
            GenerateEvent3(ret);
            GenerateEvent4(ret);

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

        static void GenerateEvent1(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（团队，名次制，全部组在一起算名次，成绩越小越好）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Smaller;

            ret.EventTeamworkInfo.SetNeedEveryPerson();
            ret.EventTeamworkInfo.BeMultiRank = true;
            ret.EventTeamworkInfo.RangesOfCategories = new Dictionary<AthleteCategory, UInt32Range>
            {
                { parent.AthleteCategories.Find((element) => element.Name == "学生男子"), new UInt32Range(4, 4) }
            };

            ret.EventAthleteValidator.Categories.Add(parent.AthleteCategories.Find((element) => element.Name == "学生男子"));
            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventAthleteValidator.Ranks.Add(rank);
            }
            ret.EventAthleteValidator.Ranks.Sort();
            ret.EventAthleteValidator.MaxNumberPerTeam = 1;
            ret.EventAthleteValidator.BePointForEveryRank = true;

            ret.EventPointInfo.PointRate = 2;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();
        }

        static void GenerateEvent2(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（团队，竞标赛制，单组内算名次，成绩越大越好）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Bigger;

            ret.EventTeamworkInfo.SetNeedEveryPerson();
            ret.EventTeamworkInfo.RangesOfCategories = new Dictionary<AthleteCategory, UInt32Range>
            {
                { parent.AthleteCategories.Find((element) => element.Name == "学生男子"), new UInt32Range(4, 8) }, 
                { parent.AthleteCategories.Find((element) => element.Name == "教师男子"), new UInt32Range(4, 8) }
            };
            ret.EventTeamworkInfo.RangesOfTeam = new UInt32Range(10, 12);

            ret.EventAthleteValidator.Categories.AddRange(parent.AthleteCategories.FindAll((element) =>
            {
                return element.Name == "学生男子" || element.Name == "教师男子";
            }));
            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventAthleteValidator.Ranks.Add(rank);
            }
            ret.EventAthleteValidator.Ranks.Sort();
            ret.EventAthleteValidator.MaxNumberPerTeam = 1;
            ret.EventAthleteValidator.BePointForEveryRank = false;

            ret.EventPointInfo.PointRate = 2;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();
        }

        static void GenerateEvent3(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（个人，名次制，全部组在一起算名次）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Smaller;

            ret.EventTeamworkInfo.SetIsNotTeamwork();

            ret.EventAthleteValidator.Categories.Add(parent.AthleteCategories.Find((element) => element.Name == "学生女子"));

            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventAthleteValidator.Ranks.Add(rank);
            }
            ret.EventAthleteValidator.Ranks.Sort();
            ret.EventAthleteValidator.MaxNumberPerTeam = 1;
            ret.EventAthleteValidator.BePointForEveryRank = true;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();
        }

        static void GenerateEvent4(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（个人，三轮比赛，竞标赛制 + 名次制，单组内算名次）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Smaller;

            ret.EventTeamworkInfo.SetIsNotTeamwork();

            ret.EventAthleteValidator.Categories.AddRange(parent.AthleteCategories.FindAll((element) =>
            {
                return element.Name == "学生女子" || element.Name == "教师女子";
            }));
            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventAthleteValidator.Ranks.Add(rank);
            }
            ret.EventAthleteValidator.Ranks.Sort();
            ret.EventAthleteValidator.MaxNumberPerTeam = 1;
            ret.EventAthleteValidator.BePointForEveryRank = false;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();
        }
    }
}
