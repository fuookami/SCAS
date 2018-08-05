using System;
using System.Collections.Generic;
using System.Xml;

namespace CompetitionConfigurationModule
{
    public class CompetitionConfigurationNormalizer
    {
        public enum ErrorCode
        {
            NoError
        }

        private delegate XmlElement NormalizeInfoFunctionType<T>(XmlDocument doc, T outputData);

        private ErrorCode lastErrorCode;
        private String lastError;
        private CompetitionInfo outputData;
        private XmlDocument docData;
        private String binaryData;
        private readonly List<NormalizeInfoFunctionType<CompetitionInfo>> NormalizeCompetitionInfoFunctions;
        private readonly List<NormalizeInfoFunctionType<EventInfo>> NormalizeEventInfoFunctions;
        private readonly List<NormalizeInfoFunctionType<GameInfo>> NormalizeGameInfoFunctions;

        public ErrorCode LastErrorCode
        {
            get { return lastErrorCode; }
        }

        public String LastError
        {
            get { return lastError; }
        }

        public CompetitionInfo Data
        {
            get { return outputData; }
            set
            {
                outputData = value;
                docData = null;
                binaryData = null;
            }
        }

        public String Binary
        {
            get
            {
                if (binaryData == null && docData!= null)
                {
                    binaryData = docData.ToString();
                }
                return binaryData;
            }
        }

        public CompetitionConfigurationNormalizer(CompetitionInfo data)
        {
            lastErrorCode = ErrorCode.NoError;
            outputData = data;
            binaryData = "";

            NormalizeCompetitionInfoFunctions = new List<NormalizeInfoFunctionType<CompetitionInfo>>
            {
                NormalizeApplicationValidator,
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
                NormalizeAthleteValidator, 
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
            if (docData != null)
            {
                return true;
            }

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("SCASCompetitionConfiguration");

            {
                XmlElement idNode = doc.CreateElement("Id");
                idNode.AppendChild(doc.CreateTextNode(outputData.Id));
                root.AppendChild(idNode);

                XmlElement nameNode = doc.CreateElement("Name");
                nameNode.AppendChild(doc.CreateTextNode(outputData.Name));
                root.AppendChild(nameNode);

                XmlElement subNameNode = doc.CreateElement("SubName");
                subNameNode.AppendChild(doc.CreateTextNode(outputData.SubName));
                root.AppendChild(subNameNode);

                XmlElement versionNode = doc.CreateElement("Version");
                versionNode.AppendChild(doc.CreateTextNode(outputData.Version));
                root.AppendChild(versionNode);

                XmlElement identifierNode = doc.CreateElement("Identifier");
                identifierNode.AppendChild(doc.CreateTextNode(outputData.Identifier));
                root.AppendChild(identifierNode);

                XmlElement isTemplateNode = doc.CreateElement("Template");
                isTemplateNode.AppendChild(doc.CreateTextNode(outputData.IsTemplate.ToString()));
                root.AppendChild(isTemplateNode);

                XmlElement applicationTypeNode = doc.CreateElement("ApplicationType");
                applicationTypeNode.AppendChild(doc.CreateTextNode(outputData.CompetitionApplicationType.ToString()));
                root.AppendChild(applicationTypeNode);
            }

            foreach (var normalizeFunction in NormalizeCompetitionInfoFunctions)
            {
                XmlElement node = normalizeFunction(doc, outputData);
                if (node == null)
                {
                    return false;
                }
                root.AppendChild(node);
            }

            XmlElement eventInfosNode = doc.CreateElement("EventInfos");
            foreach (var eventInfo in outputData.EventInfos)
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
            docData = doc;
            return true;
        }

        public bool NormalizeToFile(String url)
        {
            if (docData == null && !Normalize())
            {
                return false;
            }
            docData.Save(url);
            return true;
        }

        public XmlElement NormalizeEventInfo(XmlDocument doc, EventInfo eventInfo)
        {
            XmlElement root = doc.CreateElement("EventInfo");
            root.SetAttribute("id", eventInfo.Id);

            XmlElement nameNode = doc.CreateElement("Name");
            nameNode.AppendChild(doc.CreateTextNode(eventInfo.Name));
            root.AppendChild(nameNode);

            XmlElement typeNode = doc.CreateElement("Type");
            typeNode.AppendChild(doc.CreateTextNode(eventInfo.Type.ToString()));
            root.AppendChild(typeNode);

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

        private XmlElement NormalizeApplicationValidator(XmlDocument doc, CompetitionInfo outputData)
        {
            ApplicationValidator data = outputData.CompetitionApplicationValidator;
            XmlElement applicationValidatorNode = doc.CreateElement("ApplicationValidator");

            XmlElement enabledNode = doc.CreateElement("Enabled");
            enabledNode.AppendChild(doc.CreateTextNode(data.Enabled.ToString()));
            applicationValidatorNode.AppendChild(enabledNode);

            if (data.Enabled)
            {
                XmlElement enabledInTeamworkNode = doc.CreateElement("EnabledInTeamwork");
                enabledInTeamworkNode.AppendChild(doc.CreateTextNode(data.EnabledInTeamwork.ToString()));
                applicationValidatorNode.AppendChild(enabledInTeamworkNode);

                XmlElement maxApplicationNumberPerAthleteNode = doc.CreateElement("MaxApplicationNumberPerAthlete");
                maxApplicationNumberPerAthleteNode.AppendChild(doc.CreateTextNode(data.MaxApplicationNumberPerAthlete.ToString()));
                applicationValidatorNode.AppendChild(maxApplicationNumberPerAthleteNode);
            }

            return applicationValidatorNode;
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
                    sessionNode.SetAttribute("order", session.OrderInDate.ToString());

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
                categoryNode.AppendChild(doc.CreateTextNode(category.Name));
                athleteCategoriesNode.AppendChild(categoryNode);
            }

            return athleteCategoriesNode;
        }

        private XmlElement NormalizeRankInfo(XmlDocument doc, CompetitionInfo outputData)
        {
            RankInfo data = outputData.CompetitionRankInfo;
            XmlElement rankInfoNode = doc.CreateElement("RankInfo");

            XmlElement enabledNode = doc.CreateElement("Enabled");
            enabledNode.AppendChild(doc.CreateTextNode(data.Enabled.ToString()));
            rankInfoNode.AppendChild(enabledNode);

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

            // to do

            return gradeNode;
        }

        private XmlElement NormalizeTeamworkInfo(XmlDocument doc, EventInfo outputData)
        {
            TeamworkInfo data = outputData.EventTeamworkInfo;
            XmlElement teamworkNode = doc.CreateElement("TeamworkInfo");

            // to do

            return teamworkNode;
        }

        private XmlElement NormalizeAthleteValidator(XmlDocument doc, EventInfo outputData)
        {
            AthleteValidator data = outputData.EvenetAthleteValidator;
            XmlElement athleteValidatorNode = doc.CreateElement("AthleteValidator");

            // to do

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

            // to do

            return enabledTeamsNode;
        }

        private XmlElement NormalizeGroupInfo(XmlDocument doc, GameInfo outputData)
        {
            GroupInfo data = outputData.EventGroupInfo;
            XmlElement groupNode = doc.CreateElement("GroupInfo");

            // to do

            return groupNode;
        }

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
