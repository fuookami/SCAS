using System;
using System.IO;
using SCAS.CompetitionData;

namespace SCAS
{
    namespace DocumentGenerator
    {
        public class ProgramExporter : ErrorStorer
        {
            private Competition _data;

            public Competition Data
            {
                get
                {
                    return _data;
                }
                set
                {
                    _data = value ?? throw new Exception("设置的输出数据是个无效值");
                }
            }

            public String TargetUrl
            {
                get;
                set;
            }

            public ProgramExporter(Competition data, String targetUrl = "")
            {
                _data = data;
                TargetUrl = targetUrl;
            }

            public bool Export()
            {
                return Export(TargetUrl);
            }

            public bool Export(String targetUrl)
            {

                return true;
            }
        }
    }
};
