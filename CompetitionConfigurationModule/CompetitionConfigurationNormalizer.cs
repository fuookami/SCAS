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
                NormalizeDates, 
                NormalizeAthleteCategories, 
                NormalizeRankInfo, 
                NormalizeTeamCategories
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

        private XmlElement NormalizeDates(XmlDocument doc, CompetitionInfo outputData)
        {
            List<Date> data = outputData.Dates;
            XmlElement datesNode = doc.CreateElement("Dates");

            foreach (var date in data)
            {
                XmlElement dateNode = doc.CreateElement("Date");
                dateNode.AppendChild(doc.CreateTextNode(date.ToString()));
                datesNode.AppendChild(dateNode);
            }

            return datesNode;
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
                XmlElement teamNodes = doc.CreateElement("Team");
            }

            return teamsNode;
        }

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
