using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;
using SSUtils;

namespace FreshmanCupConfigurationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = GenerateFreshmanCupCompetitionInfoTemplate();

            Normalizer normalizer = new Normalizer(data);
            if (normalizer.NormalizeToFile("FreshmanCupConf.xml"))
            {
                Console.WriteLine("OK to write.");
            }
            else
            {
                Console.WriteLine("False to write. {0}", normalizer.LastError);
            }

            Console.ReadKey();
        }

        static CompetitionInfo GenerateFreshmanCupCompetitionInfoTemplate()
        {
            CompetitionInfo ret = new CompetitionInfo();

            ret.Name = "南京航空航天大学第X届“新生杯”游泳比赛";
            ret.EntryClosingDate = new Date(2018, 10, 21);

            ret.CompetitionPrincipalInfo.Name = "负责人的姓名";
            ret.CompetitionPrincipalInfo.Telephone = "负责人的手机";
            ret.CompetitionPrincipalInfo.Email = "负责人的邮箱@xxx.xx";

            ret.NumberOfSubLeader.Set(0, 0);
            ret.CoachOptional = true;

            ret.PublicPointInfo.Points = new List<UInt32> { 9, 7, 6, 5, 4, 3, 2, 1 };
            ret.PublicPointInfo.PointRate = 1.0;
            ret.PublicPointInfo.BreakRecordPointRateEnabled = true;
            ret.PublicPointInfo.BreakRecordPointRate = 2.0;

            SessionPool sessions = ret.Sessions;
            Date date = new Date(2018, 10, 27);
            sessions.AddDate(date);
            var session = sessions.GenerateNewSession(date);
            session.FullName = String.Format("{0}月{1}日", date.Month, date.Day);

            ret.Field = "南京航空航天大学（江宁校区）游泳馆";
            
            var category1 = ret.AthleteCategories.GenerateNewCategory();
            category1.Name = "学生男子";
            category1.SidKey = "学号";
            var category2 = ret.AthleteCategories.GenerateNewCategory();
            category2.Name = "学生女子";
            category2.SidKey = "学号";

            ret.TeamCategories.GenerateNewCategory().Name = "普通队";

            List<Tuple<String, String, Int32>> teamInfos = new List<Tuple<String, String, Int32>>
            {
                new Tuple<String, String, Int32>("一院", "航空宇航学院", 1),
                new Tuple<String, String, Int32>("二院", "能源动力学院", 2),
                new Tuple<String, String, Int32>("三院", "自动化学院", 3),
                new Tuple<String, String, Int32>("四院", "电子信息工程学院", 4),
                new Tuple<String, String, Int32>("五院", "机电学院", 5),
                new Tuple<String, String, Int32>("六院", "材料科学与技术学院", 6),
                new Tuple<String, String, Int32>("七院", "民航飞行学院", 7),
                new Tuple<String, String, Int32>("八院", "理学院", 8),
                new Tuple<String, String, Int32>("九院", "经济与管理学院", 9),
                new Tuple<String, String, Int32>("十院", "人文学院", 10),
                new Tuple<String, String, Int32>("十一院", "艺术学院", 11),
                new Tuple<String, String, Int32>("十二院", "外国语学院", 12),
                new Tuple<String, String, Int32>("十五院", "航天学院", 15),
                new Tuple<String, String, Int32>("十六院", "计算机科学与技术学院", 16),
                new Tuple<String, String, Int32>("十九院", "国际教育学院", 19),
                new Tuple<String, String, Int32>("继教院", "继续教育学院", 20)
            };
            foreach (var info in teamInfos)
            {
                var teamInfo = ret.TeamInfos.GenerateNewInfo(ret.TeamCategories[0], info.Item3);
                teamInfo.ShortName = info.Item1;
                teamInfo.Name = info.Item2;
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子25米蛙泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);
                
                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(0);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子25米蛙泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(1);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子25米蝶泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 3);
                
                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(2);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子25米蝶泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 3);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(3);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子50米蛙泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(8);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 4, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子50米蛙泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(9);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 4, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子25米仰泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 3);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(6);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子25米仰泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 3);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(7);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子25米自由泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(10);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 2, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子25米自由泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(11);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 2, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子50米自由泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(4);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子50米自由泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(5);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 3, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子4X25米自由泳";

                eventInfo.EventTeamworkInfo.SetIsTeamwork();
                eventInfo.EventTeamworkInfo.SetNeedEveryPerson();
                eventInfo.EventTeamworkInfo.RangesOfCategories.Add(category2, new NumberRange(4, 4));
                eventInfo.EventTeamworkInfo.RangesOfTeam.Set(4, 4);
                eventInfo.EventTeamworkInfo.SetInOrder(new List<AthleteCategory>
                {
                    category2, category2, category2, category2
                });

                eventInfo.EventPointInfo.PointRate = 2.0;
                eventInfo.EventPointInfo.BreakRecordPointRate = 3.0;

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 1);
                eventInfo.EventParticipantValidator.BePointForEveryRank = false;

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(12);

                    gameInfo.PlanOffsetTime = new TimeSpan(0, 15, 0);
                    gameInfo.PlanIntervalTime = new TimeSpan(0, 5, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男子4X25米自由泳";

                eventInfo.EventTeamworkInfo.SetIsTeamwork();
                eventInfo.EventTeamworkInfo.SetNeedEveryPerson();
                eventInfo.EventTeamworkInfo.RangesOfCategories.Add(category1, new NumberRange(4, 4));
                eventInfo.EventTeamworkInfo.RangesOfTeam.Set(4, 4);
                eventInfo.EventTeamworkInfo.SetInOrder(new List<AthleteCategory>
                {
                    category1, category1, category1, category1
                });

                eventInfo.EventPointInfo.PointRate = 2.0;
                eventInfo.EventPointInfo.BreakRecordPointRate = 3.0;

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 1);
                eventInfo.EventParticipantValidator.BePointForEveryRank = false;

                foreach (var team in ret.TeamInfos)
                {
                    eventInfo.EnabledTeams.Add(team);
                }
                eventInfo.EnabledTeams.Sort();

                {
                    GameInfo gameInfo = eventInfo.GameInfos.GenerateNewGameInfo();
                    gameInfo.Name = String.Format("{0}预决赛", eventInfo.Name);

                    gameInfo.Type = GameInfo.GameType.Finals;
                    gameInfo.Pattern = GameInfo.GamePattern.Ranking;

                    gameInfo.GameSession = session;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(13);
                    
                    gameInfo.PlanIntervalTime = new TimeSpan(0, 5, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            return ret;
        }
    }
}
