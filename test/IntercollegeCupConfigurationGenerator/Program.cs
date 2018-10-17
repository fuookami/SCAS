﻿using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;
using SSUtils;

namespace IntercollegeCupConfigurationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = GenerateIntercollegeCupCompetitionInfoTemplate();

            Normalizer normalizer = new Normalizer(data);
            if (normalizer.NormalizeToFile("IntercollegeCupConf.xml"))
            {
                Console.WriteLine("OK to write.");
            }
            else
            {
                Console.WriteLine("False to write. {0}", normalizer.LastError);
            }

            Console.ReadKey();
        }

        static CompetitionInfo GenerateIntercollegeCupCompetitionInfoTemplate()
        {
            CompetitionInfo ret = new CompetitionInfo();

            ret.Name = "南京航空航天大学第X届“院际杯”游泳比赛";
            ret.EntryClosingDate = new Date(2017, 5, 14);

            ret.CompetitionRegulationInfo.Organizers.Add("南京航空航天大学体育部");
            ret.CompetitionRegulationInfo.Undertakers.Add("南京航空航天大学学生会");
            ret.CompetitionRegulationInfo.Undertakers.Add("南京航空航天大学游泳协会");
            ret.CompetitionRegulationInfo.Coorganizers.Add("南京航空航天大学（江宁校区）游泳馆");
            ret.CompetitionRegulationInfo.Plans.Add(new Tuple<String, String>("8:00 - 8:50", "游泳池开放给运动员热身"));
            ret.CompetitionRegulationInfo.Plans.Add(new Tuple<String, String>("8:50 - 9:00", "游泳池清池准备比赛，第一个项目开始检录"));
            ret.CompetitionRegulationInfo.Plans.Add(new Tuple<String, String>("9:00整", "当天第一个项目第一组开赛"));
            ret.CompetitionRegulationInfo.Plans.Add(new Tuple<String, String>("11:00前", "完成当天所有项目的比赛"));
            ret.CompetitionRegulationInfo.Plans.Add(new Tuple<String, String>("12:00前", "完成当天所有项目的奖牌、奖状颁发以及场地收拾"));
            ret.CompetitionRegulationInfo.Contestants.Add("南京航空航天大学各院系");

            ret.CompetitionPrincipalInfo.Name = "负责人的姓名";
            ret.CompetitionPrincipalInfo.Telephone = "负责人的手机";
            ret.CompetitionPrincipalInfo.Email = "负责人的邮箱@xxx.xx";

            ret.NumberOfSubLeader.Set(0, 0);
            ret.CoachOptional = true;

            ret.PublicPointInfo.Points = new List<UInt32> { 9, 7, 6, 5, 4, 3, 2, 1 };
            ret.PublicPointInfo.PointRate = 1.0;
            ret.PublicPointInfo.BreakRecordPointRateEnabled = true;
            ret.PublicPointInfo.BreakRecordPointRate = 2.0;

//             ret.CompetitionEntryValidator.Enabled = true;
//             ret.CompetitionEntryValidator.EnabledInTeamwork = false;
//             ret.CompetitionEntryValidator.EntryNumberPerAthlete.Set(0, 2);

            SessionPool sessions = ret.Sessions;
            Date date = new Date(2017, 5, 20);
            sessions.AddDate(date);
            var session1 = sessions.GenerateNewSession(date, new TimeSpan(9, 0, 0));
            session1.FullName = String.Format("{0}月{1}日", date.Month, date.Day);
            date = new Date(2017, 5, 21);
            sessions.AddDate(date);
            var session2 = sessions.GenerateNewSession(date, new TimeSpan(9, 0, 0));
            session2.FullName = String.Format("{0}月{1}日", date.Month, date.Day);

            ret.Field = "南京航空航天大学（江宁校区）游泳馆";

            var category1 = ret.AthleteCategories.GenerateNewCategory();
            category1.Name = "学生男子";
            category1.SidKey = "学号";
            var category2 = ret.AthleteCategories.GenerateNewCategory();
            category2.Name = "学生女子";
            category2.SidKey = "学号";
            var category3 = ret.AthleteCategories.GenerateNewCategory();
            category3.Name = "教职工男子";
            category3.SidKey = "工号";
            var category4 = ret.AthleteCategories.GenerateNewCategory();
            category4.Name = "教职工女子";
            category4.SidKey = "工号";

            ret.CompetitionRankInfo.Enabled = true;
            var rank1 = ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank();
            rank1.Name = "甲组";
            var rank2 = ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank();
            rank2.Name = "乙组";
            var rank3 = ret.CompetitionRankInfo.AthleteRanks.GenerateNewRank();
            rank3.Name = "丙组";
            ret.CompetitionRankInfo.Forced = true;
            ret.CompetitionRankInfo.DefaultRank = rank3;

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
                eventInfo.EventParticipantValidator.Categories.Add(category4);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session1;
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
                eventInfo.EventParticipantValidator.Categories.Add(category3);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session1;
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
                eventInfo.Name = "女子50米蛙泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.Categories.Add(category4);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session2;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(2);

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
                eventInfo.EventParticipantValidator.Categories.Add(category3);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session2;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(3);

                    gameInfo.PlanIntervalTime = new TimeSpan(0, 4, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "女子25米自由泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.Categories.Add(category4);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session2;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(4);

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
                eventInfo.EventParticipantValidator.Categories.Add(category3);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 5);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session2;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(4);

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
                eventInfo.EventParticipantValidator.Categories.Add(category4);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session1;
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
                eventInfo.EventParticipantValidator.Categories.Add(category3);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session1;
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
                eventInfo.Name = "女子25米仰泳";

                eventInfo.EventParticipantValidator.Categories.Add(category2);
                eventInfo.EventParticipantValidator.Categories.Add(category4);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session2;
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
                eventInfo.Name = "男子25米仰泳";

                eventInfo.EventParticipantValidator.Categories.Add(category1);
                eventInfo.EventParticipantValidator.Categories.Add(category3);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 4);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session2;
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
                eventInfo.EventParticipantValidator.Categories.Add(category4);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 3);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session1;
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
                eventInfo.EventParticipantValidator.Categories.Add(category3);
                eventInfo.EventParticipantValidator.NumberPerTeam.Set(0, 3);
                eventInfo.EventParticipantValidator.Ranks.Add(rank1);
                eventInfo.EventParticipantValidator.Ranks.Add(rank2);
                eventInfo.EventParticipantValidator.Ranks.Add(rank3);
                eventInfo.EventParticipantValidator.BePointForEveryRank = true;

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

                    gameInfo.GameSession = session1;
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

                    gameInfo.GameSession = session1;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(6);

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

                    gameInfo.GameSession = session1;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(7);

                    gameInfo.PlanOffsetTime = new TimeSpan(0, 15, 0);
                    gameInfo.PlanIntervalTime = new TimeSpan(0, 5, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            {
                EventInfo eventInfo = ret.GenerateNewEventInfo();
                eventInfo.Name = "男女混合6X25米自由泳";

                eventInfo.EventTeamworkInfo.SetIsTeamwork();
                eventInfo.EventTeamworkInfo.SetNeedEveryPerson();
                eventInfo.EventTeamworkInfo.RangesOfCategories.Add(category1, new NumberRange(3, 3));
                eventInfo.EventTeamworkInfo.RangesOfCategories.Add(category2, new NumberRange(3, 3));
                eventInfo.EventTeamworkInfo.RangesOfTeam.Set(6, 6);
                eventInfo.EventTeamworkInfo.SetInOrder(new List<AthleteCategory>
                {
                    category2, category1, category1, category2, category2, category1
                });
                eventInfo.EventPointInfo.PointRate = 2.0;
                eventInfo.EventPointInfo.BreakRecordPointRate = 3.0;

                eventInfo.EventParticipantValidator.Categories.Add(category1);
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

                    gameInfo.GameSession = session2;
                    gameInfo.OrderInEvent = new Order(0);
                    gameInfo.OrderInSession = new Order(6);

                    gameInfo.PlanOffsetTime = new TimeSpan(0, 15, 0);
                    gameInfo.PlanIntervalTime = new TimeSpan(0, 5, 0);
                    gameInfo.PlanTimePerGroup = new TimeSpan(0, 1, 0);

                    gameInfo.GameGroupInfo.Enabled = true;
                    gameInfo.GameGroupInfo.NumberPerGroup.Set(8);
                }
            }

            return ret;
        }
    };
};
