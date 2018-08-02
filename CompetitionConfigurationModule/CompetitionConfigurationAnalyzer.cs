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
        
        private InputType inputType;
        private ErrorCode lastErrorCode;
        private String lastError;
        private CompetitionInfo result;
        private readonly List<AnalyzeNodeFunctionType<CompetitionInfo>> AnalyzeCompetitionInfoFunctions;

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

        public CompetitionConfigurationAnalyzer(InputType dataInputType = InputType.File)
        {
            inputType = dataInputType;
            lastErrorCode = ErrorCode.NoError;
            result = null;

            AnalyzeCompetitionInfoFunctions = new List<AnalyzeNodeFunctionType<CompetitionInfo>>
            {
                AnalyzeApplicationValidatorNode, 
                AnalyzePrincipleNode, 
                AnalyzePublicPointInfoNode, 
                AnalyzeDatesNode, 
                AnalyzeAthleteCategoriesNode, 
                AnalyzeRankInfoNode
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
                temp.IsTemplate = Boolean.Parse(isTemplateNode.InnerText);
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

        private bool AnalyzeApplicationValidatorNode(XmlElement parent, CompetitionInfo data)
        {
            XmlElement node = (XmlElement)parent.GetElementsByTagName("ApplicationValidator")[0];
            ApplicationValidator ret = new ApplicationValidator();

            XmlElement enabledNode = (XmlElement)node.GetElementsByTagName("Enabled")[0];
            ret.Enabled = Boolean.Parse(enabledNode.InnerText);

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

        private bool AnalyzeDatesNode(XmlElement parent, CompetitionInfo data)
        {
            XmlElement node = (XmlElement)parent.GetElementsByTagName("Dates")[0];
            var dateNodes = node.GetElementsByTagName("Date");
            foreach (XmlElement dateNode in dateNodes)
            {
                data.Dates.Add(Date.Parse(dateNode.InnerText));
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

            XmlElement enabledNode = (XmlElement)node.GetElementsByTagName("Enabled")[0];
            rankInfo.Enabled = Boolean.Parse(enabledNode.InnerText);

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

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
