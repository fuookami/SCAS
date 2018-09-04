using System;
using System.Collections.Generic;
using System.Linq;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace EntryBlank
    {
        public class Generator : ErrorStorer
        {
            private CompetitionInfo _conf;

            public CompetitionInfo Conf
            {
                get
                {
                    return _conf;
                }
                set
                {
                    _conf = value ?? throw new Exception("设置的要用于生成报名表的比赛信息是个无效值");
                    Result = null;
                }
            }

            public EntryBlank Result
            {
                get;
                private set;
            }

            public String TargetUrl
            {
                get;
                set;
            }

            public Generator(CompetitionInfo conf)
            {
                _conf = conf;
                Result = null;
            }

            public bool Generate(bool export = true)
            {
                return Generate(TargetUrl);
            }

            public bool Generate(String targetUrl, bool export = true)
            {
                if (Result == null)
                {
                    EntryBlank temp = new EntryBlank(_conf);

                    for (Int32 i = 0, j = (Int32)_conf.NumberOfSubLeader.Minimun; i != j; ++i)
                    {
                        temp.TeamSubLeader.Add(new Leader());
                    }
                    for (Int32 i = (Int32)_conf.NumberOfSubLeader.Minimun, j = (Int32)_conf.NumberOfSubLeader.Maximun; i != j; ++i)
                    {
                        temp.TeamSubLeader.Add(new Leader(true));
                    }
                    temp.TeamCoach.Optional = _conf.CoachOptional;

                    foreach (var eventConf in _conf.EventInfos)
                    {
                        if (!eventConf.EventTeamworkInfo.BeTeamwork)
                        {
                            var entry = GeneratePersonalEntry(eventConf);
                            if (entry == null)
                            {
                                return false;
                            }
                            temp.PersonalEntries.Add(entry);
                        }
                        else
                        {
                            var entry = GenerateTeamworkEntry(eventConf);
                            if (entry == null)
                            {
                                return false;
                            }
                            temp.TeamworkEntries.Add(entry);
                        }
                    }

                    Result = temp;
                }

                if (!export)
                {
                    return true;
                }

                Exporter exporter = new Exporter(Result, targetUrl);
                if (!exporter.Export())
                {
                    RefreshError(exporter.LastErrorCode, exporter.LastError);
                    return false;
                }

                return true;
            }

            private PersonalEntry GeneratePersonalEntry(EventInfo eventConf)
            {
                PersonalEntry entry = new PersonalEntry(eventConf);

                UInt32 number = eventConf.EventParticipantValidator.NumberPerTeam.Maximun != SSUtils.NumberRange.NoLimit ?
                                eventConf.EventParticipantValidator.NumberPerTeam.Maximun : 5;

                List<String> categoryNames = new List<String>();
                List<String> sidKeys = new List<String>();
                foreach (var category in entry.Conf.EventParticipantValidator.Categories)
                {
                    categoryNames.Add(category.Name);
                    sidKeys.Add(category.SidKey);
                }
                String categoryNamesString = String.Join("/", categoryNames.Distinct().ToList());
                String sidKeysString = String.Join("/", sidKeys.Distinct().ToList());

                for (UInt32 i = 0; i != number; ++i)
                {
                    Athlete athlete = new Athlete(false);
                    if (eventConf.EventParticipantValidator.NumberPerTeam.Minimun != SSUtils.NumberRange.NoLimit
                        && i >= eventConf.EventParticipantValidator.NumberPerTeam.Minimun)
                    {
                        athlete.Optional = true;
                    }

                    entry.Items.Add(new EntryItem {
                        Key = athlete.Optional ? (categoryNamesString + "(可选)") : categoryNamesString,
                        SidKey = sidKeysString,
                        Value = athlete
                    });
                }

                return entry;
            }

            private TeamworkEntry GenerateTeamworkEntry(EventInfo eventConf)
            {
                TeamworkEntry entry = new TeamworkEntry(eventConf);

                return entry;
            }
        }
    };
};
