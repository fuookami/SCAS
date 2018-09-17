using System;
using System.IO;
using SCAS.CompetitionConfiguration;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SCAS
{
    namespace EntryBlank
    {
        public class Analyzer
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
                    _conf = value ?? throw new Exception("设置的要用于读取报名表的比赛信息是个无效值");
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

            public Analyzer(CompetitionInfo conf)
            {
                _conf = conf;
                Result = null;
            }

            public bool Analyze(String targetUrl)
            {
                if (Result == null)
                {
                    Generator generator = new SCAS.EntryBlank.Generator(_conf);
                    if (!generator.Generate())
                    {
                        return false;
                    }
                    EntryBlank temp = generator.Result;

                    FileInfo file = new FileInfo(targetUrl);
                    if (!file.Exists)
                    {
                        return false;
                    }

                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        if (!AnalyzeBasicInfo(package.Workbook.Worksheets[0], temp)
                            || !AnalyzeEntryBlank(package.Workbook.Worksheets[1], temp))
                        {
                            return false;
                        }
                    }
                    
                    Result = temp;
                }
                return true;
            }

            private bool AnalyzeBasicInfo(ExcelWorksheet worksheet, EntryBlank blank)
            {
                int maxRow = worksheet.Dimension.End.Row;
                int maxColumn = worksheet.Dimension.End.Column;

                if (Conf.CompetitionRankInfo.Enabled && !Conf.CompetitionRankInfo.Forced)
                {
                    if (maxColumn < 7)
                    {
                        return false;
                    }
                }
                else if (maxColumn < 5)
                {
                    return false;
                }

                String teamName = worksheet.Cells[1, 2].Value.ToString();
                TeamInfo team = Conf.TeamInfos.Find((element) => element.Name == teamName);
                if (team == null)
                {
                    return false;
                }
                blank.Team = team;

                return true;
            }

            private bool AnalyzeEntryBlank(ExcelWorksheet worksheet, EntryBlank blank)
            {
                int maxRow = worksheet.Dimension.End.Row;
                int maxColumn = worksheet.Dimension.End.Column;

                if (maxColumn < 6)
                {
                    return false;
                }

                return true;
            }
        }
    };
};
