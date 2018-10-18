using System;
using System.Collections.Generic;
using System.Xml;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Normalizer : ErrorStorer
        {
            private Competition _outputData;
            private XmlDocument _docData;
            private String _binaryData;

            public Competition Data
            {
                get
                {
                    return _outputData;
                }
                set
                {
                    _outputData = value ?? throw new Exception("设置的要输出的比赛数据是个无效值");
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

            public Normalizer(Competition data)
            {
                _outputData = data;
                _binaryData = "";
            }

            public bool Normalize()
            {
                if (_docData != null)
                {
                    return true;
                }

                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("SCASCompetitionData");

                root.SetAttribute("id", Data.Conf.Id);

                {
                    XmlElement fieldInfosNode = doc.CreateElement("FieldInfos");
                    foreach (var sessionFieldInfoPair in Data.FieldInfos)
                    {
                        var fieldInfoNode = NormalizeFieldInfo(doc, sessionFieldInfoPair.Value);
                        if (fieldInfoNode == null)
                        {
                            return false;
                        }
                        fieldInfosNode.AppendChild(fieldInfoNode);
                    }
                    root.AppendChild(fieldInfosNode);
                }

                {
                    XmlElement eventsNode = doc.CreateElement("Events");
                    foreach (var eventData in Data.Events)
                    {
                        var eventNode = NormalizeEvent(doc, eventData);
                        if (eventNode == null)
                        {
                            return false;
                        }
                        eventsNode.AppendChild(eventNode);
                    }
                    root.AppendChild(eventsNode);
                }

                {
                    XmlElement teamsNode = doc.CreateElement("Teams");
                    foreach (var team in Data.Teams)
                    {
                        var teamNode = NormalizeTeam(doc, team);
                        if (teamNode == null)
                        {
                            return false;
                        }
                        teamsNode.AppendChild(teamNode);
                    }
                    root.AppendChild(teamsNode);
                }

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

            public XmlElement NormalizeFieldInfo(XmlDocument doc, FieldInfo fieldInfo)
            {
                XmlElement root = doc.CreateElement("FieldInfo");

                XmlElement sessionNode = doc.CreateElement("Session");
                sessionNode.SetAttribute("date", fieldInfo.FieldInfoSession.SessionDate.ToString());
                sessionNode.SetAttribute("order", fieldInfo.FieldInfoSession.OrderInDate.Value.ToString());
                root.AppendChild(sessionNode);

                XmlElement IndoorTemperatureNode = doc.CreateElement("IndoorTemperature");
                IndoorTemperatureNode.AppendChild(doc.CreateTextNode(fieldInfo.IndoorTemperature.ToString()));
                root.AppendChild(IndoorTemperatureNode);

                XmlElement WaterTemperatureNode = doc.CreateElement("WaterTemperature");
                WaterTemperatureNode.AppendChild(doc.CreateTextNode(fieldInfo.WaterTemperature.ToString()));
                root.AppendChild(WaterTemperatureNode);

                XmlElement ResidualChlorineNode = doc.CreateElement("ResidualChlorine");
                ResidualChlorineNode.AppendChild(doc.CreateTextNode(fieldInfo.ResidualChlorine.ToString()));
                root.AppendChild(ResidualChlorineNode);

                XmlElement PHValueNode = doc.CreateElement("PHValue");
                PHValueNode.AppendChild(doc.CreateTextNode(fieldInfo.PHValue.ToString()));
                root.AppendChild(PHValueNode);

                return root;
            }

            public XmlElement NormalizeEvent(XmlDocument doc, Event eventData)
            {
                XmlElement root = doc.CreateElement("Event");
                root.SetAttribute("id", eventData.Conf.Id);

                if (eventData.MatchRecord != null)
                {
                    XmlElement matchGradeNode = NormalizeRecordGrade(doc, eventData.MatchRecord, "MatchGrade");
                    if (matchGradeNode == null)
                    {
                        return null;
                    }
                    root.AppendChild(matchGradeNode);
                }

                XmlElement gamesNode = doc.CreateElement("Games");
                foreach (var game in eventData.Games)
                {
                    XmlElement gameNode = NormalizeGame(doc, game);
                    if (gameNode == null)
                    {
                        return null;
                    }
                    gamesNode.AppendChild(gameNode);
                }
                root.AppendChild(gamesNode);

                XmlElement pointsNode = doc.CreateElement("Points");
                foreach (var point in eventData.Points)
                {
                    XmlElement pointNode = NormalizePoint(doc, point);
                    if (pointNode == null)
                    {
                        return null;
                    }
                    pointsNode.AppendChild(pointNode);
                }
                root.AppendChild(pointsNode);

                return root;
            }

            public XmlElement NormalizePoint(XmlDocument doc, Point point)
            {
                XmlElement root = doc.CreateElement("Point");
                root.SetAttribute("teamId", point.PointTeam.Conf.Id);
                root.SetAttribute("participantId", point.PointParticipant.Id);

                XmlElement rankNode = doc.CreateElement("Rank");
                rankNode.AppendChild(doc.CreateTextNode(point.Ranking.Value.ToString()));
                root.AppendChild(rankNode);

                XmlElement valueNode = doc.CreateElement("Value");
                valueNode.SetAttribute("breakRecord", point.BreakRecord.ToString());
                valueNode.AppendChild(doc.CreateTextNode(point.PointValue.ToString()));
                root.AppendChild(valueNode);

                return root;
            }

            public XmlElement NormalizeGame(XmlDocument doc, Game gameData)
            {
                XmlElement root = doc.CreateElement("Game");
                root.SetAttribute("id", gameData.Conf.Id);

                XmlElement groupsNode = doc.CreateElement("Groups");
                foreach (var group in gameData.Groups)
                {
                    XmlElement groupNode = NormalizeGroup(doc, group);
                    if (groupNode == null)
                    {
                        return null;
                    }
                    groupsNode.AppendChild(groupNode);
                }
                root.AppendChild(groupsNode);

                return root;
            }

            public XmlElement NormalizeGroup(XmlDocument doc, Group group)
            {
                XmlElement root = doc.CreateElement("Group");

                XmlElement linesNode = doc.CreateElement("Lines");
                foreach (var line in group.Lines)
                {
                    XmlElement lineNode = NormalizeLine(doc, line);
                    if (lineNode == null)
                    {
                        return null;
                    }
                    linesNode.AppendChild(lineNode);
                }
                root.AppendChild(linesNode);

                return root;
            }

            public XmlElement NormalizeLine(XmlDocument doc, Line line)
            {
                XmlElement root = doc.CreateElement("Line");

                XmlElement orderNode = doc.CreateElement("Order");
                orderNode.AppendChild(doc.CreateTextNode(line.Order.Value.ToString()));
                root.AppendChild(orderNode);

                if (line.LineParticipant != null)
                {
                    XmlElement participantNode = NormalizeParticipant(doc, line.LineParticipant);
                    if (participantNode == null)
                    {
                        return null;
                    }
                    root.AppendChild(participantNode);
                }

                return root;
            }

            public XmlElement NormalizeParticipant(XmlDocument doc, Participant participant)
            {
                XmlElement root = doc.CreateElement("Participant");
                root.SetAttribute("id", participant.Id);
                root.SetAttribute("teamId", participant.ParticipantTeam.Conf.Id);
                root.SetAttribute("orderInTeam", participant.OrderInTeam.Value.ToString());

                if (participant.Name != null && participant.Name.Length != 0)
                {
                    XmlElement nameNode = doc.CreateElement("Name");
                    nameNode.AppendChild(doc.CreateTextNode(participant.Name));
                    root.AppendChild(nameNode);
                }

                XmlElement athletesNode = doc.CreateElement("Athletes");
                foreach (var athlete in participant.Athletes)
                {
                    XmlElement athleteNode = doc.CreateElement("Athlete");
                    athleteNode.SetAttribute("id", athlete.Id);
                    if (participant.OrdersOfAthletes.ContainsKey(athlete))
                    {
                        athleteNode.SetAttribute("order", participant.OrdersOfAthletes[athlete].Value.ToString());
                    }
                    athletesNode.AppendChild(athleteNode);
                }
                root.AppendChild(athletesNode);

                XmlElement gradeNode = NormalizeGrade(doc, participant.ParticipantGrade);
                if (gradeNode == null)
                {
                    return null;
                }
                if (participant.BestGrade != TimeSpan.Zero)
                {
                    gradeNode.SetAttribute("best", participant.BestGrade.ToString());
                }
                root.AppendChild(gradeNode);

                return root;
            }
            
            public XmlElement NormalizeRecordGrade(XmlDocument doc, RecordGrade recordGrade, String title)
            {
                XmlElement root = doc.CreateElement(title);

                XmlElement competitionNode = doc.CreateElement("Competition");
                competitionNode.SetAttribute("id", recordGrade.Competition.Item1);
                competitionNode.SetAttribute("order", recordGrade.Competition.Item3.ToString());
                competitionNode.AppendChild(doc.CreateTextNode(recordGrade.Competition.Item2));
                root.AppendChild(competitionNode);

                XmlElement eventNode = doc.CreateElement("Event");
                eventNode.SetAttribute("id", recordGrade.Event.Item1);
                eventNode.AppendChild(doc.CreateTextNode(recordGrade.Event.Item2));
                root.AppendChild(eventNode);

                XmlElement gameNode = doc.CreateElement("Event");
                gameNode.SetAttribute("id", recordGrade.Game.Item1);
                gameNode.AppendChild(doc.CreateTextNode(recordGrade.Game.Item2));
                root.AppendChild(eventNode);

                XmlElement participantNode = doc.CreateElement("Participant");
                participantNode.SetAttribute("id", recordGrade.ParticipantId);
                root.AppendChild(participantNode);

                if (recordGrade.Athletes.Count == 1)
                {
                    var athletePair = recordGrade.Athletes[0];
                    XmlElement athleteNode = doc.CreateElement("Athlete");
                    athleteNode.SetAttribute("id", athletePair.Item1);
                    athleteNode.SetAttribute("sid", athletePair.Item2);
                    athleteNode.AppendChild(doc.CreateTextNode(athletePair.Item3));
                    root.AppendChild(athleteNode);
                }
                else
                {
                    XmlElement athletesNode = doc.CreateElement("Athletes");
                    foreach (var athletePair in recordGrade.Athletes)
                    {
                        XmlElement athleteNode = doc.CreateElement("Athlete");
                        athleteNode.SetAttribute("id", athletePair.Item1);
                        athleteNode.SetAttribute("sid", athletePair.Item2);
                        athleteNode.AppendChild(doc.CreateTextNode(athletePair.Item3));
                        athletesNode.AppendChild(athleteNode);
                    }
                    root.AppendChild(athletesNode);
                }

                XmlElement valueNode = NormalizeGradeBase(doc, recordGrade);
                if (valueNode == null)
                {
                    return null;
                }
                root.AppendChild(valueNode);

                return root;
            }

            public XmlElement NormalizeGrade(XmlDocument doc, Grade grade)
            {
                XmlElement root = doc.CreateElement("Grade");

                XmlElement participantNode = doc.CreateElement("Participant");
                participantNode.SetAttribute("id", grade.GradeParticipator.Id);
                root.AppendChild(participantNode);

                XmlElement valueNode = NormalizeGradeBase(doc, grade);
                if (valueNode == null)
                {
                    return null;
                }
                root.AppendChild(valueNode);

                return root;
            }

            public XmlElement NormalizeGradeBase(XmlDocument doc, GradeBase gradeBase)
            {
                XmlElement root = doc.CreateElement("Value");
                root.SetAttribute("code", gradeBase.GradeCode.ToString());
                if (((Int32)gradeBase.GradeCode & (Int32)GradeBase.Code.Normal) != 0)
                {
                    root.SetAttribute("time", gradeBase.Time.ToString());
                }
                return root;
            }

            public XmlElement NormalizeTeam(XmlDocument doc, Team team)
            {
                XmlElement root = doc.CreateElement("Team");
                root.SetAttribute("id", team.Conf.Id);

                XmlElement leaderNode = NormalizeLeader(doc, team.TeamLeader, "Leader");
                if (leaderNode == null)
                {
                    return null;
                }
                root.AppendChild(leaderNode);

                if (team.TeamCoaches.Count != 0)
                {
                    XmlElement coachesNode = doc.CreateElement("Coaches");
                    foreach (var coach in team.TeamCoaches)
                    {
                        XmlElement coachNode = NormalizeLeader(doc, coach, "Coach");
                        if (coachNode == null)
                        {
                            return null;
                        }
                        coachesNode.AppendChild(coachNode);
                    }
                    root.AppendChild(coachesNode);
                }

                if (team.TeamSubLeaders.Count != 0)
                {
                    XmlElement subLeadersNode = doc.CreateElement("SubLeaders");
                    foreach (var subLeader in team.TeamSubLeaders)
                    {
                        XmlElement subLeaderNode = NormalizeLeader(doc, subLeader, "SubLeader");
                        if (subLeaderNode == null)
                        {
                            return null;
                        }
                        subLeadersNode.AppendChild(subLeaderNode);
                    }
                    root.AppendChild(subLeadersNode);
                }

                XmlElement athletesNode = doc.CreateElement("Athletes");
                foreach (var athlete in team.Athletes)
                {
                    XmlElement athleteNode = NormalizeAthlete(doc, athlete.Value);
                    if (athleteNode == null)
                    {
                        return null;
                    }
                    athletesNode.AppendChild(athleteNode);
                }
                root.AppendChild(athletesNode);

                return root;
            }

            public XmlElement NormalizeLeader(XmlDocument doc, Leader leader, String title)
            {
                XmlElement root = doc.CreateElement(title);

                XmlElement sidNode = doc.CreateElement("Sid");
                sidNode.AppendChild(doc.CreateTextNode(leader.Sid));
                root.AppendChild(sidNode);

                XmlElement nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(leader.Name));
                root.AppendChild(nameNode);

                XmlElement telephoneNode = doc.CreateElement("Telephone");
                telephoneNode.AppendChild(doc.CreateTextNode(leader.Telephone));
                root.AppendChild(telephoneNode);

                XmlElement emailNode = doc.CreateElement("EMail");
                emailNode.AppendChild(doc.CreateTextNode(leader.EMail));
                root.AppendChild(emailNode);

                return root;
            }

            public XmlElement NormalizeAthlete(XmlDocument doc, Athlete athlete)
            {
                XmlElement root = doc.CreateElement("Athlete");
                root.SetAttribute("id", athlete.Id);

                XmlElement sidNode = doc.CreateElement("Sid");
                sidNode.AppendChild(doc.CreateTextNode(athlete.Sid));
                root.AppendChild(sidNode);

                XmlElement nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(athlete.Name));
                root.AppendChild(nameNode);

                XmlElement codeNode = doc.CreateElement("Code");
                codeNode.AppendChild(doc.CreateTextNode(athlete.Code));
                root.AppendChild(codeNode);

                XmlElement categoryNode = doc.CreateElement("Category");
                categoryNode.AppendChild(doc.CreateTextNode(athlete.Category.Name));
                root.AppendChild(categoryNode);

                if (athlete.Rank != null)
                {
                    XmlElement rankNode = doc.CreateElement("Rank");
                    rankNode.AppendChild(doc.CreateTextNode(athlete.Rank.Name));
                    root.AppendChild(rankNode);
                }

                return root;
            }
        }
    };
};
