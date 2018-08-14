using System;
using System.Collections.Generic;
using System.Xml;

namespace CompetitionConfigurationModule
{
    public class CompetitionConfigurationAnalyzer
    {
        public enum InputType
        {
            File,
            Binary
        }
        
        public enum ErrorCode
        {
            NoError
        }

        private delegate bool AnalyzeNodeFunctionType<T>(XmlElement parent, T data);

        private Boolean faultTolerant;
        private InputType inputType;
        private ErrorCode lastErrorCode;
        private String lastError;
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

        public ErrorCode LastErrorCode
        {
            get { return lastErrorCode; }
        }

        public String LastError
        {
            get { return lastError; }
        }
        
        public CompetitionInfo Result
        {
            get { return result; }
        }

        public CompetitionConfigurationAnalyzer(Boolean beFaultTolerant = false, InputType dataInputType = InputType.File)
        {
            faultTolerant = beFaultTolerant;
            inputType = dataInputType;
            lastErrorCode = ErrorCode.NoError;
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
                XmlElement idNode = (XmlElement)root.GetElementsByTagName("Id")[0];
                temp = new CompetitionInfo(idNode.InnerText);

                XmlElement nameNode = (XmlElement)root.GetElementsByTagName("Name")[0];
                temp.Name = nameNode.InnerText;

                XmlElement subNameNode = (XmlElement)root.GetElementsByTagName("SubName")[0];
                temp.SubName = subNameNode.InnerText;

                XmlElement versionNode = (XmlElement)root.GetElementsByTagName("Version")[0];
                temp.Version = versionNode.InnerText;

                XmlElement identifierNode = (XmlElement)root.GetElementsByTagName("Identifier")[0];
                temp.Identifier = identifierNode.InnerText;

                XmlElement isTemplateNode = (XmlElement)root.GetElementsByTagName("Template")[0];
                temp.BeTemplate = Boolean.Parse(isTemplateNode.InnerText);

                XmlElement fieldNode = (XmlElement)root.GetElementsByTagName("Field")[0];
                temp.Field = fieldNode.InnerText;
            }

            XmlNodeList eventNodes = root.GetElementsByTagName("EventInfos");
            foreach (XmlElement eventNode in eventNodes)
            {
                if (!AnalyzeEventInfo(eventNode, temp))
                {
                    return false;
                }
            }

            foreach (var analyzezeFunction in AnalyzeCompetitionInfoFunctions)
            {
                if (!analyzezeFunction(root, temp))
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
                XmlElement idNode = (XmlElement)node.GetElementsByTagName("Id")[0];
                temp = new EventInfo(idNode.InnerText);

                XmlElement nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                temp.Name = nameNode.InnerText;
            }

            XmlNodeList gameNodes = node.GetElementsByTagName("GameInfos");
            foreach (XmlElement gameNode in gameNodes)
            {
                if (!AnalyzeGameInfo(gameNode, null))
                {
                    return false;
                }
            }

            foreach (var analyzeFunction in AnalyzeEventInfoFunctions)
            {
                if (!analyzeFunction(node, temp))
                {
                    return false;
                }
            }

            data.EventInfos.Add(temp);

            return true;
        }

        private bool AnalyzeGameInfo(XmlElement node, EventInfo data)
        {
            GameInfo temp = null;

            {
                XmlElement idNode = (XmlElement)node.GetElementsByTagName("Id")[0];
                temp = data.GameInfos.GenerateNewGameInfo(idNode.InnerText);

                XmlElement nameNode = (XmlElement)node.GetElementsByTagName("Name")[0];
                temp.Name = nameNode.InnerText;

                XmlElement typeNode = (XmlElement)node.GetElementsByTagName("Type")[0];
                temp.Type = (GameInfo.GameType)Enum.Parse(typeof(GameInfo.GameType), typeNode.InnerText);

                XmlElement patternNode = (XmlElement)node.GetElementsByTagName("Pattern")[0];
                temp.Pattern = (GameInfo.GamePattern)Enum.Parse(typeof(GameInfo.GamePattern), typeNode.InnerText);

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
            ApplicationValidator ret = new ApplicationValidator();
            ret.Enabled = Boolean.Parse(node.GetAttribute("enabled"));

            if (ret.Enabled)
            {
                XmlElement enabledInTeamworkNode = (XmlElement)node.GetElementsByTagName("EnabledInTeamwork")[0];
                ret.EnabledInTeamwork = Boolean.Parse(enabledInTeamworkNode.InnerText);

                XmlElement maxApplicationNumberPerAthleteNode = (XmlElement)node.GetElementsByTagName("MaxApplicationNumberPerAthlete")[0];
                ret.MaxApplicationNumberPerAthlete = Int32.Parse(maxApplicationNumberPerAthleteNode.InnerText);
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
            var pointStrings = pointsNode.InnerText.Split(new char[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries);
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
            return true;
        }

        private bool AnalyzeTeamworkInfoNode(XmlElement parent, EventInfo data)
        {
            return true;
        }

        private bool AnalyzeAthleteValidatorNode(XmlElement parent, EventInfo data)
        {
            return true;
        }

        private bool AnalyzePointInfoNode(XmlElement parent, EventInfo data)
        {
            return true;
        }

        private bool AnalyzeEnabledTeamsNode(XmlElement parent, EventInfo data)
        {
            return true;
        }

        private bool AnalyzeGroupInfoNode(XmlElement parent, GameInfo data)
        {
            return true;
        }

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
