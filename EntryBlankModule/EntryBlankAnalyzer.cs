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

                blank.TeamSubLeader.RemoveAll((element) => element.Optional);
                Int32 row = 2;
                while (worksheet.Cells[row, 1, row + 3, 1].Merge)
                {
                    String leaderTitle = worksheet.Cells[row, 1].Value.ToString();
                    
                    if (leaderTitle.StartsWith("领队"))
                    {
                        if (worksheet.Cells[row, 3].Value == null
                            || worksheet.Cells[row + 1, 3].Value == null
                            || worksheet.Cells[row + 2, 3].Value == null
                            || worksheet.Cells[row + 3, 3].Value == null)
                        {
                            return false;
                        }

                        Leader leader = blank.TeamLeader;

                        String name = worksheet.Cells[row, 3].Value.ToString();
                        String sid = worksheet.Cells[row + 1, 3].Value.ToString();
                        String telephone = worksheet.Cells[row + 2, 3].Value.ToString();
                        String email = worksheet.Cells[row + 3, 3].Value.ToString();

                        leader.Name = name;
                        leader.Sid = sid;
                        leader.Telephone = telephone;
                        leader.EMail = email;
                    }
                    else if (leaderTitle.StartsWith("副领队"))
                    {
                        Leader subLeader = blank.TeamSubLeader.Find((element) => !element.Optional && element.Sid == null);

                        if (worksheet.Cells[row, 3].Value == null
                            || worksheet.Cells[row + 1, 3].Value == null
                            || worksheet.Cells[row + 2, 3].Value == null
                            || worksheet.Cells[row + 3, 3].Value == null)
                        {
                            if (subLeader != null)
                            {
                                return false;
                            }
                        }

                        String name = worksheet.Cells[row, 3].Value.ToString();
                        String sid = worksheet.Cells[row + 1, 3].Value.ToString();
                        String telephone = worksheet.Cells[row + 2, 3].Value.ToString();
                        String email = worksheet.Cells[row + 3, 3].Value.ToString();

                        if (subLeader != null)
                        {
                            subLeader.Name = name;
                            subLeader.Sid = sid;
                            subLeader.Telephone = telephone;
                            subLeader.EMail = email;
                        }
                        else
                        {
                            subLeader = new Leader(true);

                            subLeader.Name = name;
                            subLeader.Sid = sid;
                            subLeader.Telephone = telephone;
                            subLeader.EMail = email;

                            blank.TeamSubLeader.Add(subLeader);
                        }
                    }
                    else if (leaderTitle.StartsWith("教练"))
                    {
                        if (worksheet.Cells[row, 3].Value == null
                            || worksheet.Cells[row + 1, 3].Value == null
                            || worksheet.Cells[row + 2, 3].Value == null
                            || worksheet.Cells[row + 3, 3].Value == null)
                        {
                            if (!blank.TeamCoach.Optional)
                            {
                                return false;
                            }
                            else
                            {
                                blank.TeamCoach = null;
                            }
                        }

                        Leader coach = blank.TeamCoach;

                        if (coach != null)
                        {
                            String name = worksheet.Cells[row, 3].Value.ToString();
                            String sid = worksheet.Cells[row + 1, 3].Value.ToString();
                            String telephone = worksheet.Cells[row + 2, 3].Value.ToString();
                            String email = worksheet.Cells[row + 3, 3].Value.ToString();

                            coach.Name = name;
                            coach.Sid = sid;
                            coach.Telephone = telephone;
                            coach.EMail = email;
                        }
                    }
                    else
                    {
                        break;
                    }

                    row += 4;
                }

                if (worksheet.Cells[row, 1].Value == null || worksheet.Cells[row, 1].Value.ToString() != "运动员报名表")
                {
                    return false;
                }
                row += 1;

                String categoryTitle = "";
                for (;row != maxRow; ++row)
                {
                    if (worksheet.Cells[row, 1].Value != null)
                    {
                        categoryTitle = worksheet.Cells[row, 1].Value.ToString();
                    }

                    if (worksheet.Cells[row, 3].Value != null && worksheet.Cells[row, 5].Value != null)
                    {
                        String name = worksheet.Cells[row, 3].Value.ToString();
                        String sid = worksheet.Cells[row, 5].Value.ToString();

                        if (Conf.CompetitionRankInfo.Enabled && !Conf.CompetitionRankInfo.Forced)
                        {
                            if (worksheet.Cells[row, 7].Value == null)
                            {
                                return false;
                            }

                            String rank = worksheet.Cells[row, 7].Value.ToString();
                            blank.Athletes.Add(new Athlete(sid, name, categoryTitle, false, rank));
                        }
                        else
                        {
                            blank.Athletes.Add(new Athlete(sid, name, categoryTitle));
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

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
