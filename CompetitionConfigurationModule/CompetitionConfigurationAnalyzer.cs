using System;
using System.Collections.Generic;
using System.Xml;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class Analyzer : ErrorStorer
        {
            public enum InputType
            {
                File,
                Binary
            }

            private delegate bool AnalyzeNodeFunctionType<T>(XmlElement parent, T data);

            private Boolean faultTolerant;
            private InputType inputType;
            private CompetitionInfo result;
            private readonly List<AnalyzeNodeFunctionType<CompetitionInfo>> AnalyzeCompetitionInfoFunctions;
            private readonly List<AnalyzeNodeFunctionType<EventInfo>> AnalyzeEventInfoFunctions;
            private readonly List<AnalyzeNodeFunctionType<GameInfo>> AnalyzeGameInfoFunctions;

            public Boolean FaultTolerant
            {
                get { return faultTolerant; }
                set { faultTolerant = value; }
            }

            public InputType DataInputType
            {
                get { return inputType; }
                set { inputType = value; }
            }

            public CompetitionInfo Result
            {
                get { return result; }
            }

            public Analyzer(Boolean beFaultTolerant = false, InputType dataInputType = InputType.File)
            {
                faultTolerant = beFaultTolerant;
                inputType = dataInputType;
                result = null;

                AnalyzeCompetitionInfoFunctions = new List<AnalyzeNodeFunctionType<CompetitionInfo>>
                {
                    AnalyzeApplicationValidatorNode,
                    AnalyzePrincipleNode,
                    AnalyzePublicPointInfoNode,
                    AnalyzeSessionsNode,
                    AnalyzeAthleteCategoriesNode,
                    AnalyzeRankInfoNode,
                    AnalyzeTeamCategoriesNode,
                    AnalyzeTeamsNode
                };

                AnalyzeEventInfoFunctions = new List<AnalyzeNodeFunctionType<EventInfo>>
                {
                    AnalyzeGradeInfoNode,
                    AnalyzeTeamworkInfoNode,
                    AnalyzeAthleteValidatorNode,
                    AnalyzePointInfoNode,
                    AnalyzeEnabledTeamsNode
                };

                AnalyzeGameInfoFunctions = new List<AnalyzeNodeFunctionType<GameInfo>>
                {
                    AnalyzeGroupInfoNode
                };
            }

            public bool Analyze(String data)
            {
                switch (inputType)
                {
                    case InputType.Binary:
                        return AnalyzeFromBinary(data);
                    case InputType.File:
                        return AnalyzeFromFile(data);
                    default:
                        return false;
                }
            }

            private bool AnalyzeFromFile(String url)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url);

                return AnalyzeFromDocument(doc);
            }

            private bool AnalyzeFromBinary(String binary)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(new System.IO.StringReader(binary));

                return AnalyzeFromDocument(doc);
            }

            private bool AnalyzeFromDocument(XmlDocument doc)
            {
                XmlElement root = doc.DocumentElement;
                CompetitionInfo temp = null;

                {
                    temp = new CompetitionInfo(root.GetAttribute("id"));

                    XmlElement nameNode = (XmlElement)root.GetElementsByTagName("Name")[0];
                    temp.Name = nameNode.InnerText;

                    XmlElement subNameNode = (XmlElement)root.GetElementsByTagName("SubName")[0];
                    temp.SubName = subNameNode.InnerText;

                    XmlElement versionNode = (XmlElement)root.GetElementsByTagName("Version")[0];
                    temp.Version = versionNode.InnerText;

                    XmlElement identifierNode = (XmlElement)root.GetElementsByTagName("Identifier")[0];
                    temp.Identifier = identifierNode.InnerText;

                    XmlElement orderNode = (XmlElement)root.GetElementsByTagName("Order")[0];
                    temp.Order = UInt32.Parse(orderNode.InnerText);

                    XmlElement isTemplateNode = (XmlElement)root.GetElementsByTagName("Template")[0];
                    temp.BeTemplate = Boolean.Parse(isTemplateNode.InnerText);

                    XmlElement fieldNode = (XmlElement)root.GetElementsByTagName("Field")[0];
                    temp.Field = fieldNode.InnerText;

                    XmlElement displayBeginLineNode = (XmlElement)root.GetElementsByTagName("DisplayBeginLine")[0];
                    XmlElement numberOfDisplayLinesNode = (XmlElement)root.GetElementsByTagName("NumberOfDisplayLines")[0];
                    XmlElement useLinesNode = (XmlElement)root.GetElementsByTagName("UseLines")[0];
                    var useLinesStrings = useLinesNode.InnerText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List<UInt32> useLines = new List<UInt32>();
                    foreach (var useLineString in useLinesStrings)
                    {
                        useLines.Add(UInt32.Parse(useLineString));
                    }
                    temp.SetLineConfiguration(useLines, UInt32.Parse(displayBeginLineNode.InnerText), UInt32.Parse(numberOfDisplayLinesNode.InnerText));
                }

                foreach (var analyzezeFunction in AnalyzeCompetitionInfoFunctions)
                {
                    if (!analyzezeFunction(root, temp))
                    {
                        return false;
                    }
                }

                XmlElement eventsNode = (XmlElement)root.GetElementsByTagName("EventInfos")[0];
                XmlNodeList eventNodes = eventsNode.GetElementsByTagName("EventInfo");
                foreach (XmlElement eventNode in eventNodes)
                {
                    if (!AnalyzeEventInfo(eventNode, temp))
                    {
                        return false;
                    }
                }

                result = temp;
                return true;
            }

            public bool AnalyzeEventInfo(XmlElement node, CompetitionInfo data)
            {
                EventInfo temp = null;

                {
                    temp = new EventInfo(node.GetAttribute("id"), data);

                    XmlElement nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                    temp.Name = nameNode.InnerText;
                }

                foreach (var analyzeFunction in AnalyzeEventInfoFunctions)
                {
                    if (!analyzeFunction(node, temp))
                    {
                        data.EventInfos.Remove(temp);
                        return false;
                    }
                }

                XmlElement gamesNode = (XmlElement)node.GetElementsByTagName("GameInfos")[0];
                XmlNodeList gameNodes = gamesNode.GetElementsByTagName("GameInfo");
                foreach (XmlElement gameNode in gameNodes)
                {
                    if (!AnalyzeGameInfo(gameNode, temp))
                    {
                        data.EventInfos.Remove(temp);
                        return false;
                    }
                }

                return true;
            }

            private bool AnalyzeGameInfo(XmlElement node, EventInfo data)
            {
                GameInfo temp = null;

                {
                    temp = data.GameInfos.GenerateNewGameInfo(node.GetAttribute("id"));

                    XmlElement nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                    temp.Name = nameNode.InnerText;

                    XmlElement typeNode = (XmlElement)node.GetElementsByTagName("Type")[0];
                    temp.Type = (GameInfo.GameType)Enum.Parse(typeof(GameInfo.GameType), typeNode.InnerText);

                    XmlElement patternNode = (XmlElement)node.GetElementsByTagName("Pattern")[0];
                    temp.Pattern = (GameInfo.GamePattern)Enum.Parse(typeof(GameInfo.GamePattern), patternNode.InnerText);

                    XmlNodeList numberNodes = node.GetElementsByTagName("NumberOfParticipants");
                    if (numberNodes.Count != 0)
                    {
                        XmlElement numberNode = (XmlElement)numberNodes[0];
                        temp.NumberOfParticipants = UInt32.Parse(numberNode.InnerText);
                    }

                    XmlElement sessionNode = (XmlElement)node.GetElementsByTagName("Session")[0];
                    Date date = Date.Parse(sessionNode.GetAttribute("date"));
                    UInt32 orderInDate = UInt32.Parse(sessionNode.GetAttribute("order"));
                    if (!data.Competition.Sessions.ContainsKey(date))
                    {
                        data.GameInfos.Remove(temp);
                        return false;
                    }
                    Session session = data.Competition.Sessions[date].Find((element) => element.OrderInDate == orderInDate);
                    if (session == null)
                    {
                        data.GameInfos.Remove(temp);
                        return false;
                    }
                    temp.GameSession = session;

                    XmlElement orderInEventNode = (XmlElement)node.GetElementsByTagName("OrderInEvent")[0];
                    temp.OrderInEvent = Int32.Parse(orderInEventNode.InnerText);

                    XmlElement orderInSessionNode = (XmlElement)node.GetElementsByTagName("OrderInSession")[0];
                    temp.OrderInSession = Int32.Parse(orderInSessionNode.InnerText);

                    XmlElement planIntervalTimeNode = (XmlElement)node.GetElementsByTagName("PlanIntervalTime")[0];
                    temp.PlanIntervalTime = TimeSpan.Parse(planIntervalTimeNode.InnerText);

                    XmlElement planTimePerGroupNode = (XmlElement)node.GetElementsByTagName("PlanTimePerGroup")[0];
                    temp.PlanTimePerGroup = TimeSpan.Parse(planTimePerGroupNode.InnerText);
                }

                foreach (var analyzeFunction in AnalyzeGameInfoFunctions)
                {
                    if (!analyzeFunction(node, temp))
                    {
                        return false;
                    }
                }

                return true;
            }

            private bool AnalyzeApplicationValidatorNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("ApplicationValidator")[0];
                EntryValidator ret = new EntryValidator();
                ret.Enabled = Boolean.Parse(node.GetAttribute("enabled"));

                if (ret.Enabled)
                {
                    XmlElement enabledInTeamworkNode = (XmlElement)node.GetElementsByTagName("EnabledInTeamwork")[0];
                    ret.EnabledInTeamwork = Boolean.Parse(enabledInTeamworkNode.InnerText);

                    XmlElement maxApplicationNumberPerAthleteNode = (XmlElement)node.GetElementsByTagName("MaxApplicationNumberPerAthlete")[0];
                    ret.ApplicationNumberPerAthlete = Int32.Parse(maxApplicationNumberPerAthleteNode.InnerText);
                }

                data.CompetitionApplicationValidator = ret;
                return true;
            }

            private bool AnalyzePrincipleNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("Principle")[0];
                PrincipalInfo ret = new PrincipalInfo();

                XmlElement nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                ret.Name = nameNode.InnerText;

                XmlElement telephoneNode = (XmlElement)node.GetElementsByTagName("Telephone")[0];
                ret.Telephone = telephoneNode.InnerText;

                XmlElement emailNode = (XmlElement)node.GetElementsByTagName("Email")[0];
                ret.Email = emailNode.InnerText;

                XmlNodeList otherNodes = node.GetElementsByTagName("Other");
                foreach (XmlElement otherNode in otherNodes)
                {
                    ret.Others.Add(otherNode.GetAttribute("key"), otherNode.InnerText);
                }

                data.CompetitionPrincipalInfo = ret;
                return true;
            }

            private bool AnalyzePublicPointInfoNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("PublicPointInfo")[0];
                PointInfo ret = new PointInfo();

                XmlElement pointsNode = (XmlElement)node.GetElementsByTagName("Points")[0];
                var pointStrings = pointsNode.InnerText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ret.Points.Clear();
                foreach (var pointString in pointStrings)
                {
                    ret.Points.Add(UInt32.Parse(pointString));
                }

                XmlElement pointRateNode = (XmlElement)node.GetElementsByTagName("PointRate")[0];
                ret.PointRate = Double.Parse(pointRateNode.InnerText);

                XmlElement breakRecordPointRateNode = (XmlElement)node.GetElementsByTagName("BreakRecordPointRate")[0];
                var breakRecordPointRate = Double.Parse(breakRecordPointRateNode.InnerText);
                if (breakRecordPointRate == PointInfo.PointRateDisabled)
                {
                    ret.SetBreakRecordPointRateDisabled();
                }
                else
                {
                    ret.SetBreakRecordPointRateEnabled(breakRecordPointRate);
                }

                data.PublicPointInfo = ret;
                return true;
            }

            private bool AnalyzeSessionsNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement sessionsNode = (XmlElement)parent.GetElementsByTagName("Sessions")[0];
                XmlNodeList sessionNodes = sessionsNode.GetElementsByTagName("Session");

                foreach (XmlElement sessionNode in sessionNodes)
                {
                    Session session = new Session(Date.Parse(sessionNode.GetAttribute("date")), UInt32.Parse(sessionNode.GetAttribute("order")));

                    XmlElement nameNode = (XmlElement)sessionNode.GetElementsByTagName("Name")[0];
                    session.Name = nameNode.InnerText;

                    XmlElement fulleNameNode = (XmlElement)sessionNode.GetElementsByTagName("FullName")[0];
                    session.FullName = fulleNameNode.InnerText;

                    if (!data.Sessions.ContainsKey(session.SessionDate))
                    {
                        data.Sessions[session.SessionDate] = new List<Session>();
                    }
                    data.Sessions[session.SessionDate].Add(session);
                }

                return true;
            }

            private bool AnalyzeAthleteCategoriesNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("AthleteCategories")[0];
                AthleteCategoryPool athleteCategories = data.AthleteCategories;
                var categoryNodes = node.GetElementsByTagName("AthleteCategory");

                foreach (XmlElement categoryNode in categoryNodes)
                {
                    AthleteCategory newCategory = athleteCategories.GenerateNewCategory(categoryNode.GetAttribute("id"));
                    newCategory.Name = categoryNode.InnerText;
                }

                return true;
            }

            private bool AnalyzeRankInfoNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("RankInfo")[0];
                RankInfo rankInfo = data.CompetitionRankInfo;
                rankInfo.Enabled = Boolean.Parse(node.GetAttribute("enabled"));

                if (rankInfo.Enabled)
                {
                    XmlElement ranksNode = (XmlElement)node.GetElementsByTagName("AthleteRanks")[0];
                    AthleteRankPool athleteRanks = rankInfo.AthleteRanks;
                    var rankNodes = ranksNode.GetElementsByTagName("AthleteRank");

                    foreach (XmlElement rankNode in rankNodes)
                    {
                        AthleteRank newRank = athleteRanks.GenerateNewRank(rankNode.GetAttribute("id"));
                        newRank.Name = rankNode.InnerText;
                    }

                    XmlElement forcedNode = (XmlElement)node.GetElementsByTagName("Forced")[0];
                    rankInfo.Forced = Boolean.Parse(forcedNode.InnerText);

                    if (rankInfo.Forced)
                    {
                        String defaultRankName = ranksNode.GetAttribute("default");
                        AthleteRank defaultRank = athleteRanks.Find((element) => element.Name == defaultRankName);

                        if (defaultRank == null)
                        {
                            return false;
                        }
                        rankInfo.DefaultRank = defaultRank;
                    }
                }

                return true;
            }

            private bool AnalyzeTeamCategoriesNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("TeamCategories")[0];
                TeamCategoryPool teamCategories = data.TeamCategories;
                var categoryNodes = node.GetElementsByTagName("TeamCategory");

                foreach (XmlElement categoryNode in categoryNodes)
                {
                    TeamCategory newCategory = teamCategories.GenerateNewCategory(categoryNode.GetAttribute("id"));
                    newCategory.Name = categoryNode.InnerText;
                }

                return true;
            }

            private bool AnalyzeTeamsNode(XmlElement parent, CompetitionInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("Teams")[0];
                TeamCategoryPool teamCategories = data.TeamCategories;
                TeamInfoPool teams = data.TeamInfos;
                var teamNodes = node.GetElementsByTagName("Team");

                foreach (XmlElement teamNode in teamNodes)
                {
                    XmlElement categoryNode = (XmlElement)teamNode.GetElementsByTagName("Category")[0];
                    TeamCategory category = teamCategories.Find((element) => element.Name == categoryNode.InnerText);

                    if (category == null)
                    {
                        return false;
                    }

                    TeamInfo newTeam = teams.GenerateNewInfo(category, teamNode.GetAttribute("id"));
                    XmlElement nameNode = (XmlElement)teamNode.GetElementsByTagName("Name")[0];
                    XmlElement shortNameNode = (XmlElement)teamNode.GetElementsByTagName("ShortName")[0];
                    newTeam.Name = nameNode.InnerText;
                    newTeam.ShortName = shortNameNode.InnerText;
                }

                return true;
            }

            private bool AnalyzeGradeInfoNode(XmlElement parent, EventInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("GradeInfo")[0];
                GradeInfo gardeInfo = data.EventGradeInfo;

                XmlElement betterTypeNode = (XmlElement)node.GetElementsByTagName("BetterType")[0];
                gardeInfo.GradeBetterType = (GradeInfo.BetterType)Enum.Parse(typeof(GradeInfo.BetterType), betterTypeNode.InnerText);

                XmlNodeList patternNodes = node.GetElementsByTagName("Pattern");
                if (patternNodes.Count != 0)
                {
                    XmlElement patternNode = (XmlElement)patternNodes[0];
                    gardeInfo.SetGradePattern(
                        (GradeInfo.PatternType)Enum.Parse(typeof(GradeInfo.PatternType), patternNode.GetAttribute("type")),
                        patternNode.InnerText
                    );
                }

                return true;
            }

            private bool AnalyzeTeamworkInfoNode(XmlElement parent, EventInfo data)
            {
                XmlElement teamworkNode = (XmlElement)parent.GetElementsByTagName("TeamworkInfo")[0];
                TeamworkInfo ret = new TeamworkInfo();
                ret.BeTeamwork = Boolean.Parse(teamworkNode.GetAttribute("enabled"));

                if (ret.BeTeamwork)
                {
                    XmlElement beMultiRankNode = (XmlElement)teamworkNode.GetElementsByTagName("BeMultiRank")[0];
                    ret.BeMultiRank = Boolean.Parse(beMultiRankNode.InnerText);

                    XmlElement needEveryPersonNode = (XmlElement)teamworkNode.GetElementsByTagName("NeedEveryPerson")[0];
                    ret.NeedEveryPerson = Boolean.Parse(needEveryPersonNode.InnerText);

                    if (ret.NeedEveryPerson)
                    {
                        XmlElement numberNode = (XmlElement)teamworkNode.GetElementsByTagName("NumberOfPeople")[0];
                        XmlNodeList rangeNodes = numberNode.GetElementsByTagName("Range");

                        foreach (XmlElement rangeNode in rangeNodes)
                        {
                            if (!rangeNode.HasAttribute("category"))
                            {
                                return false;
                            }
                            var category = data.Competition.AthleteCategories.Find(element => element.Name == rangeNode.GetAttribute("category"));
                            if (category == null)
                            {
                                return false;
                            }

                            var range = new UInt32Range();
                            if (rangeNode.HasAttribute("min"))
                            {
                                range.Minimun = UInt32.Parse(rangeNode.GetAttribute("min"));
                            }

                            if (rangeNode.HasAttribute("max"))
                            {
                                range.Maximun = UInt32.Parse(rangeNode.GetAttribute("max"));
                            }

                            if (!range.Valid())
                            {
                                return false;
                            }
                            ret.RangesOfCategories.Add(category, range);
                        }

                        XmlNodeList teamRangeNodes = numberNode.GetElementsByTagName("TeamRange");
                        if (teamRangeNodes.Count != 0)
                        {
                            XmlElement teamRangeNode = (XmlElement)teamRangeNodes[0];
                            var range = new UInt32Range();

                            if (teamRangeNode.HasAttribute("min"))
                            {
                                range.Minimun = UInt32.Parse(teamRangeNode.GetAttribute("min"));
                            }

                            if (teamRangeNode.HasAttribute("max"))
                            {
                                range.Maximun = UInt32.Parse(teamRangeNode.GetAttribute("max"));
                            }

                            if (!range.Valid())
                            {
                                return false;
                            }
                            ret.RangesOfTeam = range;
                        }
                    }
                }

                data.EventTeamworkInfo = ret;
                return true;
            }

            private bool AnalyzeAthleteValidatorNode(XmlElement parent, EventInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("AthleteValidator")[0];
                ParticipantValidator ret = new ParticipantValidator();

                XmlElement categoriesNode = (XmlElement)node.GetElementsByTagName("EnabledCategories")[0];
                var enabledCategoriesNames = categoriesNode.InnerText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var enabledCategorieName in enabledCategoriesNames)
                {
                    var category = data.Competition.AthleteCategories.Find(element => element.Name == enabledCategorieName);
                    if (category == null)
                    {
                        return false;
                    }
                    ret.Categories.Add(category);
                }

                if (data.Competition.CompetitionRankInfo.Enabled)
                {
                    XmlNodeList ranksNodes = node.GetElementsByTagName("EnabledRanks");
                    if (ranksNodes.Count != 0)
                    {
                        XmlElement ranksNode = (XmlElement)ranksNodes[0];
                        var enabledRanksNames = ranksNode.InnerText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var enabledRankName in enabledRanksNames)
                        {
                            var rank = data.Competition.CompetitionRankInfo.AthleteRanks.Find(element => element.Name == enabledRankName);
                            if (rank == null)
                            {
                                return false;
                            }
                            ret.Ranks.Add(rank);
                        }
                    }
                }

                XmlNodeList maxNumberPerTeamNodes = node.GetElementsByTagName("MaxNumberPerTeam");
                if (maxNumberPerTeamNodes.Count != 0)
                {
                    XmlElement maxNumberPerTeamNode = (XmlElement)maxNumberPerTeamNodes[0];
                    ret.NumberPerTeam = UInt32.Parse(maxNumberPerTeamNode.InnerText);
                }

                XmlElement pointForEveryRankNode = (XmlElement)node.GetElementsByTagName("PointForEveryRank")[0];
                ret.BePointForEveryRank = Boolean.Parse(pointForEveryRankNode.InnerText);

                data.EventAthleteValidator = ret;
                return true;
            }

            private bool AnalyzePointInfoNode(XmlElement parent, EventInfo data)
            {
                XmlElement node = (XmlElement)parent.GetElementsByTagName("PointInfo")[0];
                PointInfo ret = new PointInfo();

                XmlElement pointsNode = (XmlElement)node.GetElementsByTagName("Points")[0];
                var pointStrings = pointsNode.InnerText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ret.Points.Clear();
                foreach (var pointString in pointStrings)
                {
                    ret.Points.Add(UInt32.Parse(pointString));
                }

                XmlElement pointRateNode = (XmlElement)node.GetElementsByTagName("PointRate")[0];
                ret.PointRate = Double.Parse(pointRateNode.InnerText);

                XmlElement breakRecordPointRateNode = (XmlElement)node.GetElementsByTagName("BreakRecordPointRate")[0];
                var breakRecordPointRate = Double.Parse(breakRecordPointRateNode.InnerText);
                if (breakRecordPointRate == PointInfo.PointRateDisabled)
                {
                    ret.SetBreakRecordPointRateDisabled();
                }
                else
                {
                    ret.SetBreakRecordPointRateEnabled(breakRecordPointRate);
                }

                data.EventPointInfo = ret;
                return true;
            }

            private bool AnalyzeEnabledTeamsNode(XmlElement parent, EventInfo data)
            {
                TeamInfoPool teams = data.Competition.TeamInfos;

                XmlElement enabledTeamsNode = (XmlElement)parent.GetElementsByTagName("EnabledTeams")[0];
                var enabledTeamsNames = enabledTeamsNode.InnerText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                data.EnabledTeams.Clear();
                foreach (var enabledTeamName in enabledTeamsNames)
                {
                    var team = teams.Find(element => element.Name == enabledTeamName);
                    if (team == null)
                    {
                        return false;
                    }
                    data.EnabledTeams.Add(team);
                }

                return true;
            }

            private bool AnalyzeGroupInfoNode(XmlElement parent, GameInfo data)
            {
                GroupInfo ret = new GroupInfo();
                XmlElement groupNode = (XmlElement)parent.GetElementsByTagName("GroupInfo")[0];
                ret.Enabled = Boolean.Parse(groupNode.GetAttribute("enabled"));

                if (ret.Enabled)
                {
                    XmlElement numberPerGroupNode = (XmlElement)parent.GetElementsByTagName("NumberPerGroup")[0];
                    ret.NumberPerGroup = Int32.Parse(numberPerGroupNode.InnerText);
                }

                data.GameGroupInfo = ret;
                return true;
            }
        }
    };
};
