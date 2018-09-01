using System;
using System.Collections.Generic;
using System.Text;
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

            public Generator(CompetitionInfo conf)
            {
                _conf = conf;
                Result = null;
            }

            public bool Generate()
            {
                if (Result != null)
                {
                    return true;
                }

                EntryBlank temp = new EntryBlank(_conf);

                Result = temp;
                return true;
            }
        }
    };
};
