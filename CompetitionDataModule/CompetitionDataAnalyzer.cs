using System;
using System.Collections.Generic;
using System.Xml;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Analyzer : ErrorStorer
        {
            private CompetitionInfo _conf;
            private Competition _temp;

            public enum InputType
            {
                File,
                Binary
            }

            public InputType DataInputType
            {
                get;
                set;
            }

            public Competition Result
            {
                get;
                private set;
            }

            public Analyzer(CompetitionInfo conf, Boolean beFaultTolerant = false, InputType dataInputType = InputType.File)
            {
                _conf = conf;
                DataInputType = dataInputType;
                Result = null;
            }

            public bool Analyze(String data)
            {
                switch (DataInputType)
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
                _temp = null;
                String id = root.GetAttribute("id");
                if (id != _conf.Id)
                {
                    return false;
                }
                _temp = new Competition(_conf);

                {
                    XmlElement fieldInfosNode = (XmlElement)root.GetElementsByTagName("FieldInfos")[0];
                    var fieldInfoNodes = fieldInfosNode.GetElementsByTagName("FieldInfo");
                    foreach (XmlElement fieldInfoNode in fieldInfosNode)
                    {
                        if (!AnalyzeFieldInfoNode(fieldInfoNode, _temp))
                        {
                            return false;
                        }
                    }
                }

                {
                    XmlElement teamsNode = (XmlElement)root.GetElementsByTagName("Teams")[0];
                    var teamNodes = teamsNode.GetElementsByTagName("Team");
                    foreach (XmlElement teamNode in teamsNode)
                    {
                        if (!AnalyzeTeamNode(teamNode, _temp))
                        {
                            return false;
                        }
                    }
                }

                {
                    XmlElement eventsNode = (XmlElement)root.GetElementsByTagName("Events")[0];
                    var eventNodes = eventsNode.GetElementsByTagName("Event");
                    foreach (XmlElement eventNode in eventNodes)
                    {
                        if (!AnalyzeEventNode(eventNode, _temp))
                        {
                            return false;
                        }
                    }
                }

                Result = _temp;
                return true;
            }

            private bool AnalyzeFieldInfoNode(XmlElement node, Competition parentData)
            {
                XmlElement sessionNode = (XmlElement)node.GetElementsByTagName("Session")[0];
                var session = _conf.Sessions[Date.Parse(sessionNode.GetAttribute("date"))][Int32.Parse(sessionNode.GetAttribute("order"))];
                if (!parentData.FieldInfos.ContainsKey(session))
                {
                    return false;
                }
                var ret = parentData.FieldInfos[session];

                XmlElement indoorTemperatureNode = (XmlElement)node.GetElementsByTagName("IndoorTemperature")[0];
                ret.IndoorTemperature = Double.Parse(indoorTemperatureNode.InnerText);

                XmlElement waterTemperatureNode = (XmlElement)node.GetElementsByTagName("WaterTemperature")[0];
                ret.WaterTemperature = Double.Parse(waterTemperatureNode.InnerText);

                XmlElement residualChlorineNode = (XmlElement)node.GetElementsByTagName("ResidualChlorine")[0];
                ret.ResidualChlorine = Double.Parse(residualChlorineNode.InnerText);

                XmlElement PHValueNode = (XmlElement)node.GetElementsByTagName("PHValue")[0];
                ret.PHValue = Double.Parse(PHValueNode.InnerText);

                return true;
            }

            private bool AnalyzeEventNode(XmlElement node, Competition parentData)
            {
                String id = node.GetAttribute("id");
                var conf = _conf.EventInfos.Find((ele) => ele.Id == id);
                if (conf == null)
                {
                    return false;
                }

                Event ret = null;
                if (node.GetElementsByTagName("MatchRecord").Count != 0)
                {
                    var recordGrade = AnalyzeRecordGradeNode((XmlElement)node.GetElementsByTagName("MatchRecord")[0]);
                    if (recordGrade == null)
                    {
                        return false;
                    }
                    ret = new Event(parentData, conf, recordGrade);
                }
                else
                {
                    ret = new Event(parentData, conf);
                }

                var gamesNode = (XmlElement)node.GetElementsByTagName("Games")[0];
                var gameNodes = gamesNode.GetElementsByTagName("Game");
                foreach (XmlElement gameNode in gameNodes)
                {
                    if (!AnalyzeGameNode(gameNode, ret))
                    {
                        return false;
                    }
                }

                var pointsNode = (XmlElement)node.GetElementsByTagName("Points")[0];
                var pointNodes = pointsNode.GetElementsByTagName("Point");
                foreach (XmlElement pointNode in pointNodes)
                {
                    if (!AnalyzePointNode(pointNode, ret))
                    {
                        return false;
                    }
                }

                parentData.Events.Add(ret);
                return true;
            }

            private bool AnalyzeGameNode(XmlElement node, Event parentData)
            {
                String id = node.GetAttribute("id");
                var conf = parentData.Conf.GameInfos.Find((ele) => ele.Id == id);
                if (conf == null)
                {
                    return false;
                }

                Game ret = new Game(parentData, conf);

                var groupsNode = (XmlElement)node.GetElementsByTagName("Groups")[0];
                var groupNodes = node.GetElementsByTagName("Group");
                foreach (XmlElement groupNode in groupNodes)
                {
                    if (!AnalyzeGroupNode(groupNode, ret))
                    {
                        return false;
                    }
                }

                parentData.Games.Add(ret);
                parentData.Parent.Games[conf.GameSession].Add(ret);
                return true;
            }

            private bool AnalyzeGroupNode(XmlElement node, Game parentData)
            {
                Group ret = new Group(parentData);

                var linesNode = (XmlElement)node.GetElementsByTagName("Lines")[0];
                var lineNodes = linesNode.GetElementsByTagName("Line");
                foreach (XmlElement lineNode in lineNodes)
                {
                    if (!AnalyzeLineNode(lineNode, ret))
                    {
                        return false;
                    }
                }

                parentData.Groups.Add(ret);
                return true;
            }

            private bool AnalyzeLineNode(XmlElement node, Group parentData)
            {
                XmlElement orderNode = (XmlElement)node.GetElementsByTagName("Order")[0];
                var order = UInt32.Parse(orderNode.InnerText);

                Line ret = null;
                if (node.GetElementsByTagName("Participant").Count != 0)
                {
                    var participant = AnalyzeParticipantNode((XmlElement)node.GetElementsByTagName("Participant")[0], parentData.Parent);
                    if (participant == null)
                    {
                        return false;
                    }
                    ret = new Line(parentData, order, participant);
                }
                else
                {
                    ret = new Line(parentData, order, null);
                }

                parentData.Lines.Add(ret);
                return true;
            }

            private Participant AnalyzeParticipantNode(XmlElement node, Game parentData)
            {
                String id = node.GetAttribute("id");
                String teamId = node.GetAttribute("teamId");
                var team = _temp.Teams.Find((ele) => ele.Conf.Id == teamId);
                if (team == null)
                {
                    return null;
                }

                List<Tuple<Athlete, UInt32>> athleteOrderList = new List<Tuple<Athlete, UInt32>>();
                var athletesNode = (XmlElement)node.GetElementsByTagName("Athletes")[0];
                var athleteNodes = athletesNode.GetElementsByTagName("Athlete");
                foreach (XmlElement athleteNode in athleteNodes)
                {
                    String athleteId = athleteNode.GetAttribute("id");
                    if (!team.Athletes.ContainsKey(athleteId))
                    {
                        return null;
                    }
                    var athlete = team.Athletes[athleteId];
                    athleteOrderList.Add(new Tuple<Athlete, UInt32>(athlete, UInt32.Parse(athleteNode.GetAttribute("order"))));
                }
                athleteOrderList.Sort((lhs, rhs) => lhs.Item2.CompareTo(rhs.Item2));
                List<Athlete> athleteInOrder = new List<Athlete>();
                foreach (var tuple in athleteOrderList)
                {
                    athleteInOrder.Add(tuple.Item1);
                }

                var ret = new Participant(team, athleteInOrder, id);
                if (node.HasAttribute("orderInTeam"))
                {
                    ret.OrderInTeam = new SSUtils.Order(Int32.Parse(node.GetAttribute("orderInTeam")));
                }

                var gradeNode = (XmlElement)node.GetElementsByTagName("Grade")[0];
                ret.ParticipantGrade.Set(AnalyzeGradeBase(gradeNode));
                if (gradeNode.HasAttribute("best"))
                {
                    ret.BestGrade = TimeSpan.Parse(gradeNode.GetAttribute("best"));
                }
                foreach (var athlete in athleteInOrder)
                {
                    if (!athlete.Grades.ContainsKey(parentData.Parent))
                    {
                        athlete.Grades.Add(parentData.Parent, new Dictionary<Game, Grade>());
                    }
                    athlete.Grades[parentData.Parent].Add(parentData, ret.ParticipantGrade);
                }

                if (node.GetElementsByTagName("Name").Count != 0)
                {
                    ret.Name = node.GetElementsByTagName("Name")[0].InnerText;
                }

                return ret;
            }

            private bool AnalyzePointNode(XmlElement node, Event parentData)
            {
                String teamId = node.GetAttribute("teamId");
                var team = _temp.Teams.Find((ele) => ele.Conf.Id == teamId);
                if (team == null)
                {
                    return false;
                }
                String participantId = node.GetAttribute("participantId");
                Participant participant = null;
                var lastGame = parentData.Games[parentData.Games.Count - 1];
                foreach (var group in lastGame.Groups)
                {
                    foreach (var line in group.Lines)
                    {
                        if (line.LineParticipant.Id == participantId)
                        {
                            participant = line.LineParticipant;
                        }
                        if (participant != null)
                        {
                            break;
                        }
                    }
                    if (participant != null)
                    {
                        break;
                    }
                }
                if (participant == null)
                {
                    return false;
                }

                XmlElement rankNode = (XmlElement)node.GetElementsByTagName("Rank")[0];
                XmlElement valueNode = (XmlElement)node.GetElementsByTagName("Value")[0];

                Point ret = new Point(participant, parentData.Conf.EventPointInfo, new SSUtils.Order(Int32.Parse(rankNode.InnerText)), Boolean.Parse(valueNode.GetAttribute("breakRecord")));
                parentData.Points.Add(ret);
                if (!team.Points.ContainsKey(parentData))
                {
                    team.Points.Add(parentData, new List<Point>());
                }
                team.Points[parentData].Add(ret);
                foreach (var athlete in participant.Athletes)
                {
                    athlete.Points.Add(parentData, ret);
                }

                return true;
            }

            private RecordGrade AnalyzeRecordGradeNode(XmlElement node)
            {
                var competitioNode = (XmlElement)node.GetElementsByTagName("Competition")[0];
                var competitionInfo = new Tuple<String, String, UInt32>(competitioNode.GetAttribute("id"), competitioNode.InnerText, UInt32.Parse(competitioNode.GetAttribute("order")));

                var eventNode = (XmlElement)node.GetElementsByTagName("Event")[0];
                var eventInfo = new Tuple<String, String>(eventNode.GetAttribute("id"), eventNode.InnerText);

                var gameNode = (XmlElement)node.GetElementsByTagName("Game")[0];
                var gameInfo = new Tuple<String, String>(gameNode.GetAttribute("id"), gameNode.InnerText);

                var participantNode = (XmlElement)node.GetElementsByTagName("Participant")[0];
                var participantInfo = new Tuple<String, UInt32, UInt32>(participantNode.GetAttribute("id"), UInt32.Parse(participantNode.GetAttribute("group")), UInt32.Parse(participantNode.GetAttribute("line")));

                List<Tuple<String, String, String>> athletes = new List<Tuple<String, String, String>>();
                if (node.GetElementsByTagName("Athlete").Count != 0)
                {
                    var athleteNode = (XmlElement)node.GetElementsByTagName("Athlete")[0];
                    athletes.Add(new Tuple<String, String, String>(athleteNode.GetAttribute("id"), athleteNode.GetAttribute("sid"), athleteNode.InnerText));
                }
                else if (node.GetElementsByTagName("Athletes").Count != 0)
                {
                    var athletesNode = (XmlElement)node.GetElementsByTagName("Athletes")[0];
                    var athleteNodes = athletesNode.GetElementsByTagName("Athlete");
                    foreach (XmlElement athleteNode in athleteNodes)
                    {
                        athletes.Add(new Tuple<String, String, String>(athleteNode.GetAttribute("id"), athleteNode.GetAttribute("sid"), athleteNode.InnerText));
                    }
                }
                else
                {
                    return null;
                }

                var gradeBase = AnalyzeGradeBase(node);
                var ret = new RecordGrade(competitionInfo, eventInfo, gameInfo, participantInfo, athletes, gradeBase.Item1, gradeBase.Item2);
                return ret;
            }

            private Tuple<GradeBase.Code, TimeSpan> AnalyzeGradeBase(XmlElement node)
            {
                var valueNode = (XmlElement)node.GetElementsByTagName("Value")[0];
                var code = (GradeBase.Code)Enum.Parse(typeof(GradeBase.Code), valueNode.GetAttribute("code"));
                return GradeBase.HasTime(code) 
                    ? new Tuple<GradeBase.Code, TimeSpan>(code, TimeSpan.Parse(node.GetAttribute("time")))
                    : new Tuple<GradeBase.Code, TimeSpan>(code, TimeSpan.Zero);
            }

            private bool AnalyzeTeamNode(XmlElement node, Competition parentData)
            {
                String id = node.GetAttribute("id");
                var teamConf = parentData.Conf.TeamInfos.Find((ele) => ele.Id == id);
                if (teamConf == null)
                {
                    return false;
                }
                Team ret = new Team(teamConf);

                {
                    XmlElement leaderNode = (XmlElement)node.GetElementsByTagName("Leader")[0];
                    if (!AnalyzeLeaderNode(leaderNode, ret.TeamLeader))
                    {
                        return false;
                    }

                    if (node.GetElementsByTagName("Coaches").Count != 0)
                    {
                        XmlElement coachesNode = (XmlElement)node.GetElementsByTagName("Coaches")[0];
                        var coachNodes = coachesNode.GetElementsByTagName("Coach");
                        foreach (XmlElement coachNode in coachNodes)
                        {
                            Leader coach = new Leader();
                            if (!AnalyzeLeaderNode(coachNode, coach))
                            {
                                return false;
                            }
                            ret.TeamCoaches.Add(coach);
                        }
                    }

                    if (node.GetElementsByTagName("SubLeaders").Count != 0)
                    {
                        XmlElement subLeadersNode = (XmlElement)node.GetElementsByTagName("SubLeaders")[0];
                        var leaderNodes = subLeadersNode.GetElementsByTagName("SubLeader");
                        foreach (XmlElement subLeaderNode in leaderNodes)
                        {
                            Leader subLeader = new Leader();
                            if (!AnalyzeLeaderNode(subLeaderNode, subLeader))
                            {
                                return false;
                            }
                            ret.TeamSubLeaders.Add(subLeader);
                        }
                    }
                }

                {
                    XmlElement athletesNode = (XmlElement)node.GetElementsByTagName("Athletes")[0];
                    var athleteNodes = athletesNode.GetElementsByTagName("Athlete");
                    foreach (XmlElement athleteNode in athleteNodes)
                    {
                        if (!AnalyzeAthleteNode(athleteNode, ret))
                        {
                            return false;
                        }
                    }
                }

                parentData.Teams.Add(ret);
                return true;
            }

            private bool AnalyzeLeaderNode(XmlElement node, Leader leader)
            {
                XmlElement sidNode = (XmlElement)node.GetElementsByTagName("Sid")[0];
                leader.Sid = sidNode.InnerText;

                XmlElement nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                leader.Name = nameNode.InnerText;

                XmlElement telephoneNode = (XmlElement)node.GetElementsByTagName("Telephone")[0];
                leader.Telephone = telephoneNode.InnerText;

                XmlElement emailNode = (XmlElement)node.GetElementsByTagName("EMail")[0];
                leader.EMail = emailNode.InnerText;

                return true;
            }

            private bool AnalyzeAthleteNode(XmlElement node, Team parentData)
            {
                String id = node.GetAttribute("id");
                var codeNode = (XmlElement)node.GetElementsByTagName("Code")[0];
                String code = codeNode.InnerText;
                Athlete ret = parentData.Athletes.GenerateNewAthlete(id, code);

                var sidNode = (XmlElement)node.GetElementsByTagName("Sid")[0];
                ret.Sid = sidNode.InnerText;

                var nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                ret.Name = nameNode.InnerText;

                var categoryNode = (XmlElement)node.GetElementsByTagName("Category")[0];
                String categoryName = categoryNode.InnerText;
                var category = _conf.AthleteCategories.Find((ele) => ele.Name == categoryName);
                if (category == null)
                {
                    return false;
                }
                ret.Category = category;

                return true;
            }
        }
    };
};
