using System;
using System.Xml;

namespace CompetitionConfigurationModule
{
    public class CompetitionConfigurationNormalizer
    {
        public enum ErrorCode
        {
            NoError
        }

        private ErrorCode lastErrorCode;
        private String lastError;
        private CompetitionInfo outputData;
        private XmlDocument docData;
        private String binaryData;

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
        }

        public bool Normalize()
        {
            if (docData != null)
            {
                return true;
            }

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("SCAS_CompCfg");

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
            }

            XmlElement applicationValidatorNode = NormalizeApplicationValidator(doc, outputData.CompetitionApplicationValidator);
            if (applicationValidatorNode == null)
            {
                return false;
            }
            root.AppendChild(applicationValidatorNode);

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

        private static XmlElement NormalizeApplicationValidator(XmlDocument doc, ApplicationValidator data)
        {
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

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
