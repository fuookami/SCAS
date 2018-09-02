using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;
using SSUtils;

namespace TestConfigurationModule
{
    class Program
    {
        static void Main(string[] args)
        {
            var data1 = GenerateTestCompetitionInfo();
            TestSaveToXML(data1, "test.xml");
            var data2 = TestLoadFromXML();
            TestSaveToXML(data2, "test2.xml");
            Console.ReadKey();
        }

        static CompetitionInfo GenerateTestCompetitionInfo()
        {
            CompetitionInfo ret = new CompetitionInfo();

            ret.Name = "某游泳比赛";
            ret.SubName = "暨某游泳比赛";
            ret.Version = "某年某比赛第某版本（测试）";
            ret.Identifier = "给自己看的标识符/标签之类的";
            ret.EntryClosingDate = new Date(2018, 10, 14);

            ret.CompetitionEntryValidator.SetEnabled(2);
            ret.CompetitionEntryValidator.SetEnabledInTeamwork();

            ret.CompetitionPrincipalInfo.Name = "负责人的姓名";
            ret.CompetitionPrincipalInfo.Telephone = "负责人的手机";
            ret.CompetitionPrincipalInfo.Email = "负责人的邮箱@xxx.xx";
            ret.CompetitionPrincipalInfo.Others = new Dictionary<string, string>
            {
                { "QQ", "假装是个QQ号" },
                { "微信", "假装是个微信号" },
                { "备注", "随便写点什么" }
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
            ret.Sessions = sessions;

            ret.Field = "某游泳馆";

            AthleteCategory category = null;
            category = ret.AthleteCategories.GenerateNewCategory();
            category.Name = "学生男子";
            category.SidKey = "学号";
            category = ret.AthleteCategories.GenerateNewCategory();
            category.Name = "学生女子";
            category.SidKey = "学号";
            category = ret.AthleteCategories.GenerateNewCategory();
            category.Name = "教师男子";
            category.SidKey = "工号";
            category = ret.AthleteCategories.GenerateNewCategory();
            category.Name = "教师女子";
            category.SidKey = "工号";

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

        static void TestSaveToXML(CompetitionInfo info, String url)
        {
            Normalizer normalizer = new Normalizer(info);
            if (normalizer.NormalizeToFile(url))
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
            Analyzer analyzer = new Analyzer();
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

            ret.Name = "一个名次赛的例子（团队，名次制预决赛，成绩越小越好）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Smaller;

            ret.EventTeamworkInfo.SetNeedEveryPerson();
            ret.EventTeamworkInfo.BeMultiRank = true;
            ret.EventTeamworkInfo.RangesOfCategories.Add(parent.AthleteCategories.Find((element) => element.Name == "学生男子"), new NumberRange(4, 4));

            ret.EventParticipantValidator.Categories.Add(parent.AthleteCategories.Find((element) => element.Name == "学生男子"));
            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventParticipantValidator.Ranks.Add(rank);
            }
            ret.EventParticipantValidator.Ranks.Sort();
            ret.EventParticipantValidator.NumberPerTeam.Set(1);
            ret.EventParticipantValidator.BePointForEveryRank = true;

            ret.EventPointInfo.PointRate = 2;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();

            {
                GameInfo game = ret.GameInfos.GenerateNewGameInfo();
                game.Name = "XX项目预决赛";

                game.Type = GameInfo.GameType.Finals;
                game.Pattern = GameInfo.GamePattern.Ranking;

                game.GameSession = parent.Sessions[Date.Today][0];
                game.OrderInEvent = new Order(0);
                game.OrderInSession = new Order(0);

                game.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game.GameGroupInfo.Enabled = true;
                game.GameGroupInfo.NumberPerGroup.Set(8);
            }
        }

        static void GenerateEvent2(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（团队，竞标赛制小组赛+名次制决赛，成绩越大越好）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Bigger;

            ret.EventTeamworkInfo.SetNeedEveryPerson();
            ret.EventTeamworkInfo.RangesOfCategories.Add(parent.AthleteCategories.Find((element) => element.Name == "学生男子"), new NumberRange(4, 8));
            ret.EventTeamworkInfo.RangesOfCategories.Add(parent.AthleteCategories.Find((element) => element.Name == "教师男子"), new NumberRange(4, 8));
            ret.EventTeamworkInfo.RangesOfTeam.Set(10, 12);

            ret.EventParticipantValidator.Categories.AddRange(parent.AthleteCategories.FindAll((element) =>
            {
                return element.Name == "学生男子" || element.Name == "教师男子";
            }));
            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventParticipantValidator.Ranks.Add(rank);
            }
            ret.EventParticipantValidator.Ranks.Sort();
            ret.EventParticipantValidator.NumberPerTeam.Set(1);
            ret.EventParticipantValidator.BePointForEveryRank = false;

            ret.EventPointInfo.PointRate = 2;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();

            {
                GameInfo game1 = ret.GameInfos.GenerateNewGameInfo();
                game1.Name = "XX项目预赛";

                game1.Type = GameInfo.GameType.Preliminary;
                game1.Pattern = GameInfo.GamePattern.Elimination;

                game1.GameSession = parent.Sessions[Date.Today][1];
                game1.OrderInEvent = new Order(0);
                game1.OrderInSession = new Order(0);

                game1.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game1.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game1.GameGroupInfo.Enabled = true;
                game1.GameGroupInfo.NumberPerGroup.Set(8);

                GameInfo game2 = ret.GameInfos.GenerateNewGameInfo();
                game2.Name = "XX项目决赛";

                game2.Type = GameInfo.GameType.Finals;
                game2.Pattern = GameInfo.GamePattern.Ranking;

                game2.NumberOfParticipants = 8;
                game2.GameSession = parent.Sessions[Date.Today.Tomorrow][0];
                game2.OrderInEvent = new Order(1);
                game2.OrderInSession = new Order(0);

                game2.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game2.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game2.GameGroupInfo.Enabled = true;
                game2.GameGroupInfo.NumberPerGroup.Set(8);
            }
        }

        static void GenerateEvent3(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（个人，名次制，全部组在一起算名次）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Smaller;

            ret.EventTeamworkInfo.SetIsNotTeamwork();

            ret.EventParticipantValidator.Categories.Add(parent.AthleteCategories.Find((element) => element.Name == "学生女子"));

            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventParticipantValidator.Ranks.Add(rank);
            }
            ret.EventParticipantValidator.Ranks.Sort();
            ret.EventParticipantValidator.NumberPerTeam.Set(2);
            ret.EventParticipantValidator.BePointForEveryRank = true;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();

            {
                GameInfo game = ret.GameInfos.GenerateNewGameInfo();
                game.Name = "XX项目预决赛";

                game.Type = GameInfo.GameType.Finals;
                game.Pattern = GameInfo.GamePattern.Ranking;

                game.GameSession = parent.Sessions[Date.Today][1];
                game.OrderInEvent = new Order(0);
                game.OrderInSession = new Order(1);

                game.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game.GameGroupInfo.Enabled = true;
                game.GameGroupInfo.NumberPerGroup.Set(8);
            }
        }

        static void GenerateEvent4(CompetitionInfo parent)
        {
            EventInfo ret = parent.GenerateNewEventInfo();

            ret.Name = "一个名次赛的例子（个人，三轮比赛，竞标赛制 + 名次制，单组内算名次）";
            ret.EventGradeInfo.GradeBetterType = GradeInfo.BetterType.Smaller;

            ret.EventTeamworkInfo.SetIsNotTeamwork();

            var category1 = parent.AthleteCategories.Find((element) => element.Name == "学生女子");
            var category2 = parent.AthleteCategories.Find((element) => element.Name == "教师女子");
            ret.EventTeamworkInfo.SetNeedEveryPerson();
            ret.EventTeamworkInfo.RangesOfCategories.Add(category1, new NumberRange(4, 4));
            ret.EventTeamworkInfo.RangesOfCategories.Add(category2, new NumberRange(4, 4));
            ret.EventTeamworkInfo.RangesOfTeam.Set(8, 8);
            ret.EventTeamworkInfo.SetInOrder(new List<AthleteCategory>
            {
                category1, category2, category1, category2, category1, category2, category1, category2
            });

            ret.EventParticipantValidator.Categories.AddRange(parent.AthleteCategories.FindAll((element) =>
            {
                return element.Name == "学生女子" || element.Name == "教师女子";
            }));

            foreach (var rank in parent.CompetitionRankInfo.AthleteRanks)
            {
                ret.EventParticipantValidator.Ranks.Add(rank);
            }
            ret.EventParticipantValidator.Ranks.Sort();
            ret.EventParticipantValidator.NumberPerTeam.Set(1);
            ret.EventParticipantValidator.BePointForEveryRank = false;

            foreach (var team in parent.TeamInfos)
            {
                ret.EnabledTeams.Add(team);
            }
            ret.EnabledTeams.Sort();

            {
                GameInfo game1 = ret.GameInfos.GenerateNewGameInfo();
                game1.Name = "XX项目预赛";

                game1.Type = GameInfo.GameType.Preliminary;
                game1.Pattern = GameInfo.GamePattern.Elimination;

                game1.GameSession = parent.Sessions[Date.Today][0];
                game1.OrderInEvent = new Order(0);
                game1.OrderInSession = new Order(1);

                game1.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game1.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game1.GameGroupInfo.Enabled = true;
                game1.GameGroupInfo.NumberPerGroup.Set(8);

                GameInfo game2 = ret.GameInfos.GenerateNewGameInfo();
                game2.Name = "XX项目复赛";

                game2.Type = GameInfo.GameType.SemiFinal;
                game2.Pattern = GameInfo.GamePattern.Elimination;

                game2.NumberOfParticipants = 16;
                game2.GameSession = parent.Sessions[Date.Today][1];
                game2.OrderInEvent = new Order(1);
                game2.OrderInSession = new Order(2);

                game2.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game2.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game2.GameGroupInfo.Enabled = true;
                game2.GameGroupInfo.NumberPerGroup.Set(8);

                GameInfo game3 = ret.GameInfos.GenerateNewGameInfo();
                game3.Name = "XX项目决赛";

                game3.Type = GameInfo.GameType.Finals;
                game3.Pattern = GameInfo.GamePattern.Ranking;

                game3.NumberOfParticipants = 8;
                game3.GameSession = parent.Sessions[Date.Today.Tomorrow][0];
                game3.OrderInEvent = new Order(2);
                game3.OrderInSession = new Order(1);

                game3.PlanIntervalTime = new TimeSpan(0, 2, 0);
                game3.PlanTimePerGroup = new TimeSpan(0, 3, 0);

                game3.GameGroupInfo.Enabled = true;
                game3.GameGroupInfo.NumberPerGroup.Set(8);
            }
        }
    }
}
