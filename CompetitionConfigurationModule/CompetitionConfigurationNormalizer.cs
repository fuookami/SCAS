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

        private void RefreshError(ErrorCode code, String text)
        {
            lastErrorCode = code;
            lastError = String.Format("error {0}({1}): {2}", (int)code, code.ToString(), text);
        }
    }
}
