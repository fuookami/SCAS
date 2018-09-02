using System;
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
                        Entry entry = new Entry(eventConf);

                        temp.Entries.Add(entry);
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
        }
    };
};
