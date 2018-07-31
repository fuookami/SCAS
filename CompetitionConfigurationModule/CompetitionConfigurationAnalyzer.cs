using System;
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

        private InputType inputType;
        private ErrorCode lastErrorCode;
        private String lastError;
        private CompetitionInfo result;

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

            temp.CompetitionApplicationValidator = AnalyzeApplicationValidatorNode(
                (XmlElement)root.GetElementsByTagName("ApplicationValidator")[0]);

            result = temp;
            return true;
        }

        private ApplicationValidator AnalyzeApplicationValidatorNode(XmlElement node)
        {
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

            return ret;
        }

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
