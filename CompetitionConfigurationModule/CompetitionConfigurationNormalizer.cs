using System;
using System.Collections.Generic;
using System.Xml;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class Normalizer : ErrorStorer
        {
            private delegate XmlElement NormalizeInfoFunctionType<T>(XmlDocument doc, T outputData);

            private CompetitionInfo _outputData;
            private XmlDocument _docData;
            private String _binaryData;
            private readonly List<NormalizeInfoFunctionType<CompetitionInfo>> NormalizeCompetitionInfoFunctions;
            private readonly List<NormalizeInfoFunctionType<EventInfo>> NormalizeEventInfoFunctions;
            private readonly List<NormalizeInfoFunctionType<GameInfo>> NormalizeGameInfoFunctions;

            public CompetitionInfo Data
            {
                get { return _outputData; }
                set
                {
                    _outputData = value ?? throw new Exception("设置的要输出的比赛信息是个无效值");
                    _docData = null;
                    _binaryData = null;
                }
            }

            public String Binary
            {
                get
                {
                    if (_binaryData == null && _docData != null)
                    {
                        _binaryData = _docData.ToString();
                    }
                    return _binaryData;
                }
            }

            public Normalizer(CompetitionInfo data)
            {
                _outputData = data;
                _binaryData = "";

                NormalizeCompetitionInfoFunctions = new List<NormalizeInfoFunctionType<CompetitionInfo>>
                {
                    NormalizeEntryValidator,
                    NormalizePrincipalInfo,
                    NormalizePublicPointInfo,
                    NormalizeSessions,
                    NormalizeAthleteCategories,
                    NormalizeRankInfo,
                    NormalizeTeamCategories,
                    NormalizeTeamInfo
                };

                NormalizeEventInfoFunctions = new List<NormalizeInfoFunctionType<EventInfo>>
                {
                    NormalizeGradeInfo,
                    NormalizeTeamworkInfo,
                    NormalizeParticipantValidator,
                    NormalizePointInfo,
                    NormalizeEnabledTeams
                };

                NormalizeGameInfoFunctions = new List<NormalizeInfoFunctionType<GameInfo>>
                {
                    NormalizeGroupInfo,
                };
            }

            public bool Normalize()
            {
                if (_docData != null)
                {
                    return true;
                }

                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("SCASCompetitionConfiguration");

                {
                    root.SetAttribute("id", _outputData.Id);

                    XmlElement nameNode = doc.CreateElement("Name");
                    nameNode.AppendChild(doc.CreateTextNode(_outputData.Name));
                    root.AppendChild(nameNode);

                    XmlElement subNameNode = doc.CreateElement("SubName");
                    subNameNode.AppendChild(doc.CreateTextNode(_outputData.SubName));
                    root.AppendChild(subNameNode);

                    XmlElement versionNode = doc.CreateElement("Version");
                    versionNode.AppendChild(doc.CreateTextNode(_outputData.Version));
                    root.AppendChild(versionNode);

                    XmlElement identifierNode = doc.CreateElement("Identifier");
                    identifierNode.AppendChild(doc.CreateTextNode(_outputData.Identifier));
                    root.AppendChild(identifierNode);

                    XmlElement orderNode = doc.CreateElement("Order");
                    orderNode.AppendChild(doc.CreateTextNode(_outputData.Order.ToString()));
                    root.AppendChild(orderNode);

                    XmlElement beTemplateNode = doc.CreateElement("Template");
                    beTemplateNode.AppendChild(doc.CreateTextNode(_outputData.BeTemplate.ToString()));
                    root.AppendChild(beTemplateNode);

                    XmlElement entryClosingDate = doc.CreateElement("EntryClosingDate");
                    entryClosingDate.AppendChild(doc.CreateTextNode(_outputData.EntryClosingDate.ToString()));
                    root.AppendChild(entryClosingDate);
                    
                    if (_outputData.SubLeaderNumber.Minimun != SSUtils.NumberRange.NoLimit
                        && _outputData.SubLeaderNumber.Maximun != SSUtils.NumberRange.NoLimit)
                    {
                        XmlElement subLeaderNumberNode = doc.CreateElement("SubLeaderNumber");
                        if (_outputData.SubLeaderNumber.Minimun != SSUtils.NumberRange.NoLimit)
                        {
                            subLeaderNumberNode.SetAttribute("min", _outputData.SubLeaderNumber.Minimun.ToString());
                        }
                        if (_outputData.SubLeaderNumber.Maximun != SSUtils.NumberRange.NoLimit)
                        {
                            subLeaderNumberNode.SetAttribute("max", _outputData.SubLeaderNumber.Minimun.ToString());
                        }
                    }

                    XmlElement fieldNode = doc.CreateElement("Field");
                    fieldNode.AppendChild(doc.CreateTextNode(_outputData.Field.ToString()));
                    root.AppendChild(fieldNode);

                    XmlElement displayBeginLineNode = doc.CreateElement("DisplayBeginLine");
                    displayBeginLineNode.AppendChild(doc.CreateTextNode(_outputData.DisplayBeginLine.ToString()));
                    root.AppendChild(displayBeginLineNode);

                    XmlElement numberOfDisplayLinesNode = doc.CreateElement("NumberOfDisplayLines");
                    numberOfDisplayLinesNode.AppendChild(doc.CreateTextNode(_outputData.NumberOfDisplayLines.ToString()));
                    root.AppendChild(numberOfDisplayLinesNode);

                    XmlElement useLinesNode = doc.CreateElement("UseLines");
                    useLinesNode.AppendChild(doc.CreateTextNode(String.Join(", ", _outputData.UseLines)));
                    root.AppendChild(useLinesNode);
                }

                foreach (var normalizeFunction in NormalizeCompetitionInfoFunctions)
                {
                    XmlElement node = normalizeFunction(doc, _outputData);
                    if (node == null)
                    {
                        return false;
                    }
                    root.AppendChild(node);
                }

                XmlElement eventInfosNode = doc.CreateElement("EventInfos");
                foreach (var eventInfo in _outputData.EventInfos)
                {
                    XmlElement node = NormalizeEventInfo(doc, eventInfo);
                    if (node == null)
                    {
                        return false;
                    }
                    eventInfosNode.AppendChild(node);
                }
                root.AppendChild(eventInfosNode);

                doc.AppendChild(root);
                _docData = doc;
                return true;
            }

            public bool NormalizeToFile(String url)
            {
                if (_docData == null && !Normalize())
                {
                    return false;
                }
                _docData.Save(url);
                return true;
            }

            public XmlElement NormalizeEventInfo(XmlDocument doc, EventInfo eventInfo)
            {
                XmlElement root = doc.CreateElement("EventInfo");
                root.SetAttribute("id", eventInfo.Id);

                XmlElement nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(eventInfo.Name));
                root.AppendChild(nameNode);

                foreach (var normalizeFunction in NormalizeEventInfoFunctions)
                {
                    XmlElement node = normalizeFunction(doc, eventInfo);
                    if (node == null)
                    {
                        return null;
                    }
                    root.AppendChild(node);
                }

                XmlElement gameInfosNode = doc.CreateElement("GameInfos");
                foreach (var gameInfo in eventInfo.GameInfos)
                {
                    XmlElement node = NormalizeGameInfo(doc, gameInfo);
                    if (node == null)
                    {
                        return null;
                    }
                    gameInfosNode.AppendChild(node);
                }
                root.AppendChild(gameInfosNode);

                return root;
            }

            private XmlElement NormalizeGameInfo(XmlDocument doc, GameInfo gameInfo)
            {
                XmlElement root = doc.CreateElement("GameInfo");
                root.SetAttribute("id", gameInfo.Id);

                XmlElement nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(gameInfo.Name));
                root.AppendChild(nameNode);

                XmlElement typeNode = doc.CreateElement("Type");
                typeNode.AppendChild(doc.CreateTextNode(gameInfo.Type.ToString()));
                root.AppendChild(typeNode);

                XmlElement patternNode = doc.CreateElement("Pattern");
                patternNode.AppendChild(doc.CreateTextNode(gameInfo.Pattern.ToString()));
                root.AppendChild(patternNode);

                if (gameInfo.NumberOfParticipants != SSUtils.NumberRange.NoLimit)
                {
                    XmlElement numberNode = doc.CreateElement("NumberOfParticipants");
                    numberNode.AppendChild(doc.CreateTextNode(gameInfo.NumberOfParticipants.ToString()));
                    root.AppendChild(numberNode);
                }

                XmlElement sessionNode = doc.CreateElement("Session");
                sessionNode.SetAttribute("date", gameInfo.GameSession.SessionDate.ToString());
                sessionNode.SetAttribute("order", gameInfo.GameSession.OrderInDate.Value.ToString());
                root.AppendChild(sessionNode);

                XmlElement orderInEventNode = doc.CreateElement("OrderInEvent");
                orderInEventNode.AppendChild(doc.CreateTextNode(gameInfo.OrderInEvent.Value.ToString()));
                root.AppendChild(orderInEventNode);

                XmlElement orderInSessionNode = doc.CreateElement("OrderInSession");
                orderInSessionNode.AppendChild(doc.CreateTextNode(gameInfo.OrderInSession.Value.ToString()));
                root.AppendChild(orderInSessionNode);

                XmlElement planIntervalTimeNode = doc.CreateElement("PlanIntervalTime");
                planIntervalTimeNode.AppendChild(doc.CreateTextNode(gameInfo.PlanIntervalTime.ToString()));
                root.AppendChild(planIntervalTimeNode);

                XmlElement planTimePerGroupNode = doc.CreateElement("PlanTimePerGroup");
                planTimePerGroupNode.AppendChild(doc.CreateTextNode(gameInfo.PlanTimePerGroup.ToString()));
                root.AppendChild(planTimePerGroupNode);

                foreach (var normalizeFunction in NormalizeGameInfoFunctions)
                {
                    XmlElement node = normalizeFunction(doc, gameInfo);
                    if (node == null)
                    {
                        return null;
                    }
                    root.AppendChild(node);
                }

                return root;
            }

            private XmlElement NormalizeEntryValidator(XmlDocument doc, CompetitionInfo outputData)
            {
                EntryValidator data = outputData.CompetitionEntryValidator;
                XmlElement entryValidatorNode = doc.CreateElement("ApplicationValidator");
                entryValidatorNode.SetAttribute("enabled", data.Enabled.ToString());

                if (data.Enabled)
                {
                    XmlElement enabledInTeamworkNode = doc.CreateElement("EnabledInTeamwork");
                    enabledInTeamworkNode.AppendChild(doc.CreateTextNode(data.EnabledInTeamwork.ToString()));
                    entryValidatorNode.AppendChild(enabledInTeamworkNode);

                    if (data.EntryNumberPerAthlete.HasRange())
                    {
                        XmlElement rangeNode = doc.CreateElement("EntryNumberPerAthlete");

                        if (data.EntryNumberPerAthlete.Minimun != SSUtils.NumberRange.NoLimit)
                        {
                            rangeNode.SetAttribute("min", data.EntryNumberPerAthlete.Minimun.ToString());
                        }
                        if (data.EntryNumberPerAthlete.Maximun != SSUtils.NumberRange.NoLimit)
                        {
                            rangeNode.SetAttribute("max", data.EntryNumberPerAthlete.Maximun.ToString());
                        }

                        entryValidatorNode.AppendChild(rangeNode);
                    }
                }

                return entryValidatorNode;
            }

            private XmlElement NormalizePrincipalInfo(XmlDocument doc, CompetitionInfo outputData)
            {
                PrincipalInfo data = outputData.CompetitionPrincipalInfo;
                XmlElement principalNode = doc.CreateElement("Principle");

                XmlElement nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(data.Name));
                principalNode.AppendChild(nameNode);

                XmlElement telephoneNode = doc.CreateElement("Telephone");
                telephoneNode.AppendChild(doc.CreateTextNode(data.Telephone));
                principalNode.AppendChild(telephoneNode);

                XmlElement emailNode = doc.CreateElement("Email");
                emailNode.AppendChild(doc.CreateTextNode(data.Email));
                principalNode.AppendChild(emailNode);

                foreach (var other in data.Others)
                {
                    XmlElement otherNode = doc.CreateElement("Other");
                    otherNode.SetAttribute("key", other.Key);
                    otherNode.AppendChild(doc.CreateTextNode(other.Value));
                    principalNode.AppendChild(otherNode);
                }

                return principalNode;
            }

            private XmlElement NormalizePublicPointInfo(XmlDocument doc, CompetitionInfo outputData)
            {
                PointInfo data = outputData.PublicPointInfo;
                XmlElement pointNode = doc.CreateElement("PublicPointInfo");

                XmlElement pointsNode = doc.CreateElement("Points");
                pointsNode.AppendChild(doc.CreateTextNode(String.Join(", ", data.Points)));
                pointNode.AppendChild(pointsNode);

                XmlElement pointRateNode = doc.CreateElement("PointRate");
                pointRateNode.AppendChild(doc.CreateTextNode(data.PointRate.ToString()));
                pointNode.AppendChild(pointRateNode);

                XmlElement breakRecordPointNode = doc.CreateElement("BreakRecordPointRate");
                breakRecordPointNode.AppendChild(doc.CreateTextNode((data.BreakRecordPointRateEnabled ? data.BreakRecordPointRate : PointInfo.PointRateDisabled).ToString()));
                pointNode.AppendChild(breakRecordPointNode);

                return pointNode;
            }

            private XmlElement NormalizeSessions(XmlDocument doc, CompetitionInfo outputData)
            {
                SessionPool data = outputData.Sessions;
                XmlElement sessionsNode = doc.CreateElement("Sessions");

                foreach (var date in data)
                {
                    foreach (var session in date.Value)
                    {
                        XmlElement sessionNode = doc.CreateElement("Session");
                        sessionNode.SetAttribute("date", date.Key.ToString());
                        sessionNode.SetAttribute("order", session.OrderInDate.Value.ToString());

                        XmlElement nameNode = doc.CreateElement("Name");
                        nameNode.AppendChild(doc.CreateTextNode(session.Name));
                        sessionNode.AppendChild(nameNode);

                        XmlElement fullNameNode = doc.CreateElement("FullName");
                        fullNameNode.AppendChild(doc.CreateTextNode(session.FullName));
                        sessionNode.AppendChild(fullNameNode);

                        sessionsNode.AppendChild(sessionNode);
                    }
                }

                return sessionsNode;
            }

            private XmlElement NormalizeAthleteCategories(XmlDocument doc, CompetitionInfo outputData)
            {
                AthleteCategoryPool data = outputData.AthleteCategories;
                XmlElement athleteCategoriesNode = doc.CreateElement("AthleteCategories");

                foreach (var category in data)
                {
                    XmlElement categoryNode = doc.CreateElement("AthleteCategory");
                    categoryNode.SetAttribute("id", category.Id);
                    categoryNode.SetAttribute("sidKey", category.SidKey);
                    categoryNode.AppendChild(doc.CreateTextNode(category.Name));
                    athleteCategoriesNode.AppendChild(categoryNode);
                }

                return athleteCategoriesNode;
            }

            private XmlElement NormalizeRankInfo(XmlDocument doc, CompetitionInfo outputData)
            {
                RankInfo data = outputData.CompetitionRankInfo;
                XmlElement rankInfoNode = doc.CreateElement("RankInfo");
                rankInfoNode.SetAttribute("enabled", data.Enabled.ToString());

                if (data.Enabled)
                {
                    XmlElement forcedNode = doc.CreateElement("Forced");
                    forcedNode.AppendChild(doc.CreateTextNode(data.Forced.ToString()));
                    rankInfoNode.AppendChild(forcedNode);

                    XmlElement ranksNode = doc.CreateElement("AthleteRanks");
                    if (data.Forced)
                    {
                        ranksNode.SetAttribute("default", data.DefaultRank.Name);
                    }
                    foreach (var rank in data.AthleteRanks)
                    {
                        XmlElement rankNode = doc.CreateElement("AthleteRank");
                        rankNode.SetAttribute("id", rank.Id);
                        rankNode.AppendChild(doc.CreateTextNode(rank.Name));
                        ranksNode.AppendChild(rankNode);
                    }
                    rankInfoNode.AppendChild(ranksNode);
                }

                return rankInfoNode;
            }

            private XmlElement NormalizeTeamCategories(XmlDocument doc, CompetitionInfo outputData)
            {
                TeamCategoryPool data = outputData.TeamCategories;
                XmlElement teamCategoriesNode = doc.CreateElement("TeamCategories");

                foreach (var category in data)
                {
                    XmlElement categoryNode = doc.CreateElement("TeamCategory");
                    categoryNode.SetAttribute("id", category.Id);
                    categoryNode.AppendChild(doc.CreateTextNode(category.Name));
                    teamCategoriesNode.AppendChild(categoryNode);
                }

                return teamCategoriesNode;
            }

            private XmlElement NormalizeTeamInfo(XmlDocument doc, CompetitionInfo outputData)
            {
                TeamInfoPool data = outputData.TeamInfos;
                XmlElement teamsNode = doc.CreateElement("Teams");

                foreach (var team in data)
                {
                    XmlElement teamNode = doc.CreateElement("Team");
                    teamNode.SetAttribute("id", team.Id);

                    XmlElement nameNode = doc.CreateElement("Name");
                    nameNode.AppendChild(doc.CreateTextNode(team.Name));
                    teamNode.AppendChild(nameNode);

                    XmlElement shortNameNode = doc.CreateElement("ShortName");
                    shortNameNode.AppendChild(doc.CreateTextNode(team.ShortName));
                    teamNode.AppendChild(shortNameNode);

                    XmlElement categoryNode = doc.CreateElement("Category");
                    categoryNode.AppendChild(doc.CreateTextNode(team.Category.Name));
                    teamNode.AppendChild(categoryNode);

                    teamsNode.AppendChild(teamNode);
                }

                return teamsNode;
            }

            private XmlElement NormalizeGradeInfo(XmlDocument doc, EventInfo outputData)
            {
                GradeInfo data = outputData.EventGradeInfo;
                XmlElement gradeNode = doc.CreateElement("GradeInfo");

                XmlElement betterTypeNode = doc.CreateElement("BetterType");
                betterTypeNode.AppendChild(doc.CreateTextNode(data.GradeBetterType.ToString()));
                gradeNode.AppendChild(betterTypeNode);

                if (data.GradePatternType != GradeInfo.PatternType.None)
                {
                    XmlElement patternNode = doc.CreateElement("Pattern");
                    patternNode.SetAttribute("type", data.GradePatternType.ToString());
                    patternNode.AppendChild(doc.CreateTextNode(data.GradePattern));
                    gradeNode.AppendChild(patternNode);
                }

                return gradeNode;
            }

            private XmlElement NormalizeTeamworkInfo(XmlDocument doc, EventInfo outputData)
            {
                TeamworkInfo data = outputData.EventTeamworkInfo;
                XmlElement teamworkNode = doc.CreateElement("TeamworkInfo");
                teamworkNode.SetAttribute("enabled", data.BeTeamwork.ToString());

                if (data.BeTeamwork)
                {
                    XmlElement beMultiRankNode = doc.CreateElement("BeMultiRank");
                    beMultiRankNode.AppendChild(doc.CreateTextNode(data.BeMultiRank.ToString()));
                    teamworkNode.AppendChild(beMultiRankNode);

                    XmlElement needEveryPersonNode = doc.CreateElement("NeedEveryPerson");
                    needEveryPersonNode.AppendChild(doc.CreateTextNode(data.NeedEveryPerson.ToString()));
                    teamworkNode.AppendChild(needEveryPersonNode);

                    if (data.NeedEveryPerson)
                    {
                        XmlElement numberNode = doc.CreateElement("NumberOfPeople");

                        foreach (var range in data.RangesOfCategories)
                        {
                            if (range.Value.HasRange())
                            {
                                XmlElement rangeNode = doc.CreateElement("Range");

                                rangeNode.SetAttribute("category", range.Key.Name);
                                if (range.Value.Minimun != SSUtils.NumberRange.NoLimit)
                                {
                                    rangeNode.SetAttribute("min", range.Value.Minimun.ToString());
                                }
                                if (range.Value.Maximun != SSUtils.NumberRange.NoLimit)
                                {
                                    rangeNode.SetAttribute("max", range.Value.Maximun.ToString());
                                }

                                numberNode.AppendChild(rangeNode);
                            }
                        }

                        if (data.RangesOfTeam.HasRange())
                        {
                            XmlElement rangeNode = doc.CreateElement("TeamRange");

                            if (data.RangesOfTeam.Minimun != SSUtils.NumberRange.NoLimit)
                            {
                                rangeNode.SetAttribute("min", data.RangesOfTeam.Minimun.ToString());
                            }
                            if (data.RangesOfTeam.Maximun != SSUtils.NumberRange.NoLimit)
                            {
                                rangeNode.SetAttribute("max", data.RangesOfTeam.Maximun.ToString());
                            }

                            numberNode.AppendChild(rangeNode);
                        }

                        teamworkNode.AppendChild(numberNode);
                    }

                    if (data.BeInOrder)
                    {
                        XmlElement orderNode = doc.CreateElement("Order");

                        List<String> orders = new List<String>();
                        foreach (var category in data.Order)
                        {
                            orders.Add(category.Name);
                        }

                        orderNode.AppendChild(doc.CreateTextNode(String.Join(", ", orders)));

                        teamworkNode.AppendChild(orderNode);
                    }
                }

                return teamworkNode;
            }

            private XmlElement NormalizeParticipantValidator(XmlDocument doc, EventInfo outputData)
            {
                ParticipantValidator data = outputData.EventParticipantValidator;
                XmlElement athleteValidatorNode = doc.CreateElement("AthleteValidator");

                XmlElement categoriesNode = doc.CreateElement("EnabledCategories");
                List<String> categoriesNames = new List<String>();
                foreach (var category in data.Categories)
                {
                    categoriesNames.Add(category.Name);
                }
                categoriesNode.AppendChild(doc.CreateTextNode(String.Join(", ", categoriesNames)));
                athleteValidatorNode.AppendChild(categoriesNode);

                if (outputData.Competition.CompetitionRankInfo.Enabled)
                {
                    XmlElement ranksNode = doc.CreateElement("EnabledRanks");
                    List<String> ranksNames = new List<string>();
                    foreach (var rank in data.Ranks)
                    {
                        ranksNames.Add(rank.Name);
                    }
                    ranksNode.AppendChild(doc.CreateTextNode(String.Join(", ", ranksNames)));
                    athleteValidatorNode.AppendChild(ranksNode);
                }

                if (data.NumberPerTeam.HasRange())
                {
                    XmlElement rangeNode = doc.CreateElement("NumberPerTeam");

                    if (data.NumberPerTeam.Minimun != SSUtils.NumberRange.NoLimit)
                    {
                        rangeNode.SetAttribute("min", data.NumberPerTeam.Minimun.ToString());
                    }
                    if (data.NumberPerTeam.Maximun != SSUtils.NumberRange.NoLimit)
                    {
                        rangeNode.SetAttribute("max", data.NumberPerTeam.Maximun.ToString());
                    }

                    athleteValidatorNode.AppendChild(rangeNode);
                }

                XmlElement pointForEveryRankNode = doc.CreateElement("PointForEveryRank");
                pointForEveryRankNode.AppendChild(doc.CreateTextNode(data.BePointForEveryRank.ToString()));
                athleteValidatorNode.AppendChild(pointForEveryRankNode);

                return athleteValidatorNode;
            }

            private XmlElement NormalizePointInfo(XmlDocument doc, EventInfo outputData)
            {
                PointInfo data = outputData.EventPointInfo;
                XmlElement pointNode = doc.CreateElement("PointInfo");

                XmlElement pointsNode = doc.CreateElement("Points");
                pointsNode.AppendChild(doc.CreateTextNode(String.Join(", ", data.Points)));
                pointNode.AppendChild(pointsNode);

                XmlElement pointRateNode = doc.CreateElement("PointRate");
                pointRateNode.AppendChild(doc.CreateTextNode(data.PointRate.ToString()));
                pointNode.AppendChild(pointRateNode);

                XmlElement breakRecordPointNode = doc.CreateElement("BreakRecordPointRate");
                breakRecordPointNode.AppendChild(doc.CreateTextNode((data.BreakRecordPointRateEnabled ? data.BreakRecordPointRate : PointInfo.PointRateDisabled).ToString()));
                pointNode.AppendChild(breakRecordPointNode);

                return pointNode;
            }

            private XmlElement NormalizeEnabledTeams(XmlDocument doc, EventInfo outputData)
            {
                TeamInfoList data = outputData.EnabledTeams;
                XmlElement enabledTeamsNode = doc.CreateElement("EnabledTeams");
                List<String> teamNames = new List<String>();
                foreach (var team in data)
                {
                    teamNames.Add(team.Name);
                }
                enabledTeamsNode.AppendChild(doc.CreateTextNode(String.Join(", ", teamNames)));

                return enabledTeamsNode;
            }

            private XmlElement NormalizeGroupInfo(XmlDocument doc, GameInfo outputData)
            {
                GroupInfo data = outputData.GameGroupInfo;
                XmlElement groupNode = doc.CreateElement("GroupInfo");
                groupNode.SetAttribute("enabled", data.Enabled.ToString());

                if (data.Enabled && data.NumberPerGroup.HasRange())
                {
                    XmlElement rangeNode = doc.CreateElement("NumberPerGroup");

                    if (data.NumberPerGroup.Minimun != SSUtils.NumberRange.NoLimit)
                    {
                        rangeNode.SetAttribute("min", data.NumberPerGroup.Minimun.ToString());
                    }
                    if (data.NumberPerGroup.Maximun != SSUtils.NumberRange.NoLimit)
                    {
                        rangeNode.SetAttribute("max", data.NumberPerGroup.Maximun.ToString());
                    }

                    groupNode.AppendChild(rangeNode);
                }

                return groupNode;
            }
        };
    };
};
