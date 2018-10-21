using System;
using System.IO;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
using SCAS.CompetitionConfiguration;
using SCAS.CompetitionData;

namespace SCAS
{
    namespace DocumentGenerator
    {
        public class ProgramExporter : ErrorStorer
        {
            private const Int32 NumberOfAthletePerLineInAthleteList = 4;
            private const Int32 NumberOfAthletePerLineInTeamworkAthleteList = 6;

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
                String html = NormalizeToHtml();
                if (html == null)
                {
                    return false;
                }

                if (File.Exists(targetUrl))
                {
                    File.Delete(targetUrl);
                }

                using (MemoryStream generatedDocument = new MemoryStream())
                {
                    using (WordprocessingDocument package = WordprocessingDocument.Create(
                           generatedDocument, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = package.MainDocumentPart;
                        if (mainPart == null)
                        {
                            mainPart = package.AddMainDocumentPart();
                            new Document(new Body()).Save(mainPart);
                        }

                        HtmlConverter converter = new HtmlConverter(mainPart);
                        Body body = mainPart.Document.Body;

                        SectionProperties properties = new SectionProperties();
                        PageSize pageSize;

                        pageSize = new PageSize()
                        {
                            Width = (UInt32Value)12173U,
                            Height = (UInt32Value)16838U,
                            Orient = PageOrientationValues.Landscape
                        };
                        properties.Append(pageSize);

                        PageMargin pageMargin = new PageMargin()
                        {
                            Top = 720,
                            Right = (UInt32Value)1008U,
                            Bottom = 720,
                            Left = (UInt32Value)1008U,
                            Header = (UInt32Value)720U,
                            Footer = (UInt32Value)720U,
                            Gutter = (UInt32Value)0U
                        };
                        properties.Append(pageMargin);

                        body.Append(properties);

                        SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
                        ParagraphProperties paragraphProperties = new ParagraphProperties();
                        Paragraph paragraph = new Paragraph();

                        paragraphProperties.Append(spacing);
                        paragraph.Append(paragraphProperties);
                        body.Append(paragraph);

                        var paragraphs = converter.Parse(html);
                        for (int i = 0; i < paragraphs.Count; i++)
                        {
                            body.Append(paragraphs[i]);
                        }

                        mainPart.Document.Save();
                    }

                    FileStream fout = new FileStream(targetUrl, FileMode.Create);
                    generatedDocument.WriteTo(fout);
                    fout.Close();
                }

                return true;
            }

            private String NormalizeToHtml()
            {
                MemoryStream stream = new MemoryStream();  
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("<body width='46pt'>");

                String regulationPart = NormalizeRegulationPart();
                if (regulationPart == null)
                {
                    return null;
                }
                writer.WriteLine(String.Format("<p><p>{0}</p><p style='page-break-after: always;'></p></p>", regulationPart));

                String athleteListPart = NormalizeAthleteListPart();
                if (athleteListPart == null)
                {
                    return null;
                }
                writer.WriteLine(String.Format("<p><p>{0}</p><p style='page-break-after: always;'></p></p>", athleteListPart));

                String groupListPart = NormalizeGroupListPart();
                if (groupListPart == null)
                {
                    return null;
                }
                writer.WriteLine(String.Format("<p><p>{0}</p><p style='page-break-after: always;'></p></p>", groupListPart));

                writer.WriteLine("</body>");

                writer.Flush();
                stream.Position = 0;  
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            private String NormalizeRegulationPart()
            {
                var conf = _data.Conf;
                var regulationInfo = conf.CompetitionRegulationInfo;

                MemoryStream stream = new MemoryStream();  
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine(String.Format("<h1 style='text-align: center;'>{0}规程</h1>", conf.Name));
                if (conf.SubName != null && conf.SubName.Length != 0)
                {
                    writer.WriteLine(String.Format("<p style='font-size: 24pt;'><b>暨{0}规程</b></p>", conf.SubName));
                }
                writer.WriteLine(String.Format("<p style='font-size: 13pt;'><b>主办单位：</b>{0}</p>", String.Join("、", regulationInfo.Organizers)));
                writer.WriteLine(String.Format("<p style='font-size: 13pt;'><b>承办单位：</b>{0}</p>", String.Join("、", regulationInfo.Undertakers)));
                writer.WriteLine(String.Format("<p style='font-size: 13pt;'><b>协办单位：</b>{0}</p>", String.Join("、", regulationInfo.Coorganizers)));

                writer.WriteLine(String.Format("<p style='font-size: 13pt;'><b>报名截止日期：</b>{0}</p>", conf.EntryClosingDate.ToLongString()));
                List<String> dates = new List<String>();
                foreach (var date in conf.Sessions)
                {
                    dates.Add(date.Key.ToLongString());
                }
                writer.WriteLine(String.Format("<p style='font-size: 13pt;'><b>比赛时间：</b>{0}</p>", String.Join("、", dates)));
                writer.WriteLine(String.Format("<p style='font-size: 13pt;'><b>比赛地点：</b>{0}</p>", conf.Field));
                
                writer.WriteLine("<p style='font-size: 13pt;'><b>赛程时间安排：</b></p>");
                writer.WriteLine("<table width='100%' border='0'>");
                foreach (var plan in regulationInfo.Plans)
                {
                    writer.WriteLine(String.Format("<tr><td style='font-size: 11pt;'><b>{0}</b></td><td style='font-size: 11pt;'>{1}</td></tr>", plan.Item1, plan.Item2));
                }
                writer.WriteLine("</table>");

                writer.WriteLine("<p style='font-size: 13pt;'><b>比赛项目安排：</b></p>");
                writer.WriteLine("<ol>");
                foreach (var sessionGames in _data.Games)
                {
                    writer.WriteLine("<li style='font-size: 13pt;'><b>{0}</b>", sessionGames.Key.FullName);
                    UInt32 i = 1;
                    foreach (var game in sessionGames.Value)
                    {
                        writer.WriteLine("<p style='text-indent: 2em; font-size: 11pt;'>{0}) {1}</p>", i++, game.Conf.Name);
                    }
                    writer.WriteLine("</li>");
                }
                writer.WriteLine("</ol>");

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            private String NormalizeAthleteListPart()
            {
                MemoryStream stream = new MemoryStream();  
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("<h1>运动员名单</h1>");
                writer.WriteLine("<ol>");
                foreach (var team in _data.Teams)
                {
                    writer.WriteLine(String.Format("<li><b style='font-size: 13pt;'>{0}</b>", team.Conf.Name));
                    writer.Write(String.Format("<p style='font-size: 12pt;'>领队：{0}", team.TeamLeader.Name));
                    if (team.TeamSubLeaders.Count != 0)
                    {
                        List<String> names = new List<String>();
                        foreach (var subLeader in team.TeamSubLeaders)
                        {
                            names.Add(subLeader.Name);
                        }
                        writer.Write(String.Format("    副领队：{0}", String.Join("、", names)));
                    }
                    if (team.TeamCoaches.Count != 0)
                    {
                        List<String> names = new List<String>();
                        foreach (var coach in team.TeamCoaches)
                        {
                            names.Add(coach.Name);
                        }
                        writer.Write(String.Format("    教练：{0}", String.Join("、", names)));
                    }
                    writer.WriteLine("</p>");

                    var athelteLists = DivideAthletesInGroupByCategory(team.Athletes);
                    writer.WriteLine("<table border='0'>");
                    foreach (var athleteList in athelteLists)
                    {
                        for (Int32 i = 0, m = athleteList.Item2.Count; i < m; i += NumberOfAthletePerLineInAthleteList)
                        {
                            writer.Write(String.Format("<tr><td width='5pt' style='font-size: 13pt;'>{0}</td>", i == 0 ? athleteList.Item1.Name : ""));
                            Int32 j = i, k = 0;
                            for ( ; j != m && k != NumberOfAthletePerLineInAthleteList; ++j, ++k)
                            {
                                writer.Write(String.Format("<td width='1.5pt' style='text-align: right; font-size: 11pt;'>{0}</td><td width='4.5pt' style='text-align: left; font-size: 11pt;'>{1}</td>", athleteList.Item2[j].Code, athleteList.Item2[j].Name));
                            }
                            if (j == m && m % NumberOfAthletePerLineInAthleteList != 0)
                            {
                                for (; k != NumberOfAthletePerLineInAthleteList; ++k)
                                {
                                    writer.Write("<td width='1.5pt' style='text-align: right; padding-right: 2.25em;'></td><td width='4.5pt' style='text-align: left; padding-left: 1.5em;'></td>");
                                }
                            }
                            writer.Write("</tr>");
                        }
                    }
                    
                    writer.WriteLine("</table></li>");
                }
                writer.WriteLine("</ol>");

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            static private List<Tuple<AthleteCategory, List<Athlete>>> DivideAthletesInGroupByCategory(AthletePool athletes)
            {
                Dictionary<AthleteCategory, Int32> index = new Dictionary<AthleteCategory, int>();
                List<Tuple<AthleteCategory, List<Athlete>>> ret = new List<Tuple<AthleteCategory, List<Athlete>>>();

                foreach (var athlete in athletes)
                {
                    if (!index.ContainsKey(athlete.Value.Category))
                    {
                        index.Add(athlete.Value.Category, ret.Count);
                        ret.Add(new Tuple<AthleteCategory, List<Athlete>>(athlete.Value.Category, new List<Athlete>()));
                    }
                    ret[index[athlete.Value.Category]].Item2.Add(athlete.Value);
                }

                ret.Sort((lhs, rhs) => lhs.Item1.CompareTo(rhs.Item1));
                foreach (var athleteList in ret)
                {
                    athleteList.Item2.Sort();
                }
                return ret;
            }

            private String NormalizeGroupListPart()
            {
                MemoryStream stream = new MemoryStream();  
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine("<h1 style='text-align: center;'>竞赛分组</h1>");
                foreach (var sessionGames in _data.Games)
                {
                    TimeSpan beginTime = sessionGames.Key.BeginTime;
                    writer.WriteLine(String.Format("<h2 style='text-align: center;'>{0}</h2>", sessionGames.Key.FullName));
                    Int32 gameIndex = 1;
                    foreach (var game in sessionGames.Value)
                    {
                        beginTime += game.Conf.PlanOffsetTime;
                        String unit = game.Conf.Event.EventTeamworkInfo.BeTeamwork ? "队" : "人";
                        TimeSpan endTime = beginTime;
                        if (game.Conf.OrderInEvent != 0)
                        {
                            if (game.Conf.GameGroupInfo.Enabled)
                            {
                                var planGroupNumber = game.Conf.NumberOfParticipants / game.Conf.GameGroupInfo.NumberPerGroup.Minimum;
                                endTime += new TimeSpan(0, 0, (Int32)(game.Conf.PlanTimePerGroup.TotalSeconds * planGroupNumber));
                                endTime += new TimeSpan(0, 0, (Int32)(game.Conf.PlanIntervalTime.TotalSeconds * (planGroupNumber == 1 ? 1 : planGroupNumber - 1)));
                                writer.WriteLine(String.Format("<h3>{8}. {0} {1}{2} {3}组  （{4}:{5:D2} - {6}:{7:D2}）</h3>", 
                                    game.Conf.Name, game.Conf.NumberOfParticipants, unit, game.Conf.NumberOfParticipants / game.Conf.GameGroupInfo.NumberPerGroup.Minimum, 
                                    beginTime.Hours, beginTime.Minutes, endTime.Hours, endTime.Minutes, gameIndex
                                ));
                            }
                            else
                            {
                                endTime += game.Conf.PlanTimePerGroup  + game.Conf.PlanIntervalTime;
                                writer.WriteLine(String.Format("<h3>{7}. {0} {1}{2}   （{3}:{4:D2} - {5}:{6:D2}）</h3>", 
                                    game.Conf.Name, game.Conf.NumberOfParticipants, unit, 
                                    beginTime.Hours, beginTime.Minutes, endTime.Hours, endTime.Minutes, gameIndex
                                ));
                            }
                        }
                        else
                        {
                            if (game.Conf.GameGroupInfo.Enabled)
                            {
                                endTime += new TimeSpan(0, 0, (Int32)(game.Conf.PlanTimePerGroup.TotalSeconds * game.Groups.Count));
                                endTime += new TimeSpan(0, 0, (Int32)(game.Conf.PlanIntervalTime.TotalSeconds * (game.Groups.Count == 1 ? 1 : game.Groups.Count - 1)));
                                writer.WriteLine(String.Format("<h3>{8}. {0} {1}{2} {3}组  （{4}:{5:D2} - {6}:{7:D2}）</h3>", 
                                    game.Conf.Name, CountNumberOfParticipant(game), unit, game.Groups.Count, 
                                    beginTime.Hours, beginTime.Minutes, endTime.Hours, endTime.Minutes, gameIndex
                                ));
                            }
                            else
                            {
                                endTime += game.Conf.PlanTimePerGroup + game.Conf.PlanIntervalTime;
                                writer.WriteLine(String.Format("<h3>{7}. {0} {1}{2}  （{3}:{4:D2} - {5}:{6:D2}）</h3>", 
                                    game.Conf.Name, CountNumberOfParticipant(game), unit, game.Groups.Count, 
                                    beginTime.Hours, beginTime.Minutes, endTime.Hours, endTime.Minutes, gameIndex
                                ));
                            }

                            writer.WriteLine("<table style='text-align: center;' border='0'>");
                            writer.Write("<tr><th width='4pt'>组别\\道次</th>");
                            foreach (var line in _data.Conf.UseLines)
                            {
                                writer.Write(String.Format("<th width='3pt'>{0}</th>", line));
                            }
                            writer.WriteLine("</tr>");

                            Int32 index = 1;
                            foreach (var group in game.Groups)
                            {
                                String thisGroupPart = !game.Conf.Event.EventTeamworkInfo.BeTeamwork ? NormalizePersonalGroup(group, index, _data.Conf.CompetitionRankInfo.Enabled) : NormalizeTeamworkGroup(group, index);
                                ++index;
                                if (thisGroupPart == null)
                                {
                                    return null;
                                }
                                writer.WriteLine(thisGroupPart);
                            }
                            writer.WriteLine("</table>");

                            if (game.Conf.Event.EventTeamworkInfo.BeTeamwork)
                            {
                                List<Participant> participants = new List<Participant>();
                                foreach (var group in game.Groups)
                                {
                                    foreach (var line in group.Lines)
                                    {
                                        if (line.LineParticipant != null)
                                        {
                                            participants.Add(line.LineParticipant);
                                        }
                                    }
                                }
                                participants.Sort((lhs, rhs) => 
                                {
                                    var ret = lhs.ParticipantTeam.Conf.Order.CompareTo(rhs.ParticipantTeam.Conf.Order);
                                    if (ret != 0)
                                    {
                                        return ret;
                                    }
                                    return lhs.OrderInTeam.CompareTo(rhs.OrderInTeam);
                                });

                                writer.WriteLine("<table border='0'>");
                                foreach (var pariticipant in participants)
                                {
                                    String thisAthletesPart = NormalizeTeamworkAthletes(pariticipant, game.Conf.Event.EventTeamworkInfo.BeInOrder);
                                    if (thisAthletesPart == null)
                                    {
                                        return null;
                                    }
                                    writer.WriteLine(thisAthletesPart);
                                }
                                writer.WriteLine("</table>");
                            }
                        }
                        beginTime = endTime;
                        ++gameIndex;
                    }
                }

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            static private Int32 CountNumberOfParticipant(Game game)
            {
                Int32 counter = 0;
                foreach (var group in game.Groups)
                {
                    foreach (var line in group.Lines)
                    {
                        if (line.LineParticipant != null)
                        {
                            ++counter;
                        }
                    }
                }
                return counter;
            }

            private String NormalizePersonalGroup(Group group, Int32 index, Boolean rank)
            {
                MemoryStream stream = new MemoryStream();  
                StreamWriter writer = new StreamWriter(stream);

                writer.Write(String.Format("<tr><td>{0}</td>", index));
                foreach (var line in group.Lines)
                {
                    writer.Write(String.Format("<td>{0}</td>", line.LineParticipant != null ? line.LineParticipant.Athletes[0].Name : ""));
                }
                writer.Write("</tr>");

                writer.Write("<tr><td></td>");
                foreach (var line in group.Lines)
                {
                    writer.Write(String.Format("<td style='font-size: 8pt;'>{0}</td>", line.LineParticipant != null ? line.LineParticipant.Athletes[0].Sid : ""));
                }
                writer.Write("</tr>");

                writer.Write("<tr><td></td>");
                foreach (var line in group.Lines)
                {
                    writer.Write(String.Format("<td>{0}</td>", line.LineParticipant != null ? line.LineParticipant.Athletes[0].Code : ""));
                }
                writer.Write("</tr>");

                if (rank)
                {
                    writer.Write("<tr><td></td>");
                    foreach (var line in group.Lines)
                    {
                        writer.Write(String.Format("<td>{0}</td>", line.LineParticipant != null ? line.LineParticipant.Athletes[0].Rank.Name : ""));
                    }
                    writer.Write("</tr>");
                }

                writer.Write("<tr><td></td>");
                foreach (var line in group.Lines)
                {
                    writer.Write("<td></td>");
                }
                writer.Write("</tr>");

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            private String NormalizeTeamworkGroup(Group group, Int32 index)
            {
                MemoryStream stream = new MemoryStream();  
                StreamWriter writer = new StreamWriter(stream);

                writer.Write(String.Format("<tr><td>{0}</td>", index));
                foreach (var line in group.Lines)
                {
                    writer.Write(String.Format("<td>{0}</td>", line.LineParticipant != null ? line.LineParticipant.Name : ""));
                }
                writer.WriteLine("</tr>");

                writer.Write("<tr><td></td>");
                foreach (var line in group.Lines)
                {
                    writer.Write("<td></td>");
                }
                writer.Write("</tr>");

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            private String NormalizeTeamworkAthletes(Participant participant, bool inOrder)
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                
                for (Int32 i = 0, m = participant.Athletes.Count; i < m; i += NumberOfAthletePerLineInTeamworkAthleteList)
                {
                    writer.Write(String.Format("<tr><td width='5pt'>{0}</td>", i == 0 ? participant.Name : ""));
                    Int32 j = i;
                    if (inOrder)
                    {
                        for (Int32 k = 0; j != m && k != NumberOfAthletePerLineInTeamworkAthleteList; ++j, ++k)
                        {
                            writer.Write(String.Format("<td width='4pt'>{0} - {1}</td>", j + 1, participant.Athletes[j].Name));
                        }
                    }
                    else
                    {
                        for (Int32 k = 0; j != m && k != NumberOfAthletePerLineInTeamworkAthleteList; ++j, ++k)
                        {
                            writer.Write(String.Format("<td width='4pt'>{0}</td>", participant.Athletes[j].Name));
                        }
                    }
                    
                    if (j == m && m % NumberOfAthletePerLineInTeamworkAthleteList != 0)
                    {
                        for (Int32 k = 0, n = NumberOfAthletePerLineInTeamworkAthleteList - m % NumberOfAthletePerLineInTeamworkAthleteList; k != n; ++k)
                        {
                            writer.Write("<td></td>");
                        }
                    }
                    writer.Write("</tr>");
                }

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
};
