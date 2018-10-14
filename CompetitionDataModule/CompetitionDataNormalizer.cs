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
                        var fieldInfoNode = NormalizeFieldInfo(doc, sessionFieldInfoPair.Key, sessionFieldInfoPair.Value);
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

            public XmlElement NormalizeFieldInfo(XmlDocument doc, Session session, FieldInfo fieldInfo)
            {
                XmlElement root = doc.CreateElement("FieldInfo");

                return root;
            }

            public XmlElement NormalizeEvent(XmlDocument doc, Event eventData)
            {
                XmlElement root = doc.CreateElement("Event");

                return root;
            }

            public XmlElement NormalizeTeam(XmlDocument doc, Team team)
            {
                XmlElement root = doc.CreateElement("Team");

                return root;
            }
        }
    };
};
