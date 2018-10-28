using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SCAS.CompetitionData;

namespace SCAS
{
    namespace DocumentGenerator
    {
        public class EventGradeExporter : ErrorStorer
        {
            const String Head = "<style>" +
                "tr" +
                "	{mso-height-source:auto;" +
                "	mso-ruby-visibility:none;}" +
                "col" +
                "	{mso-width-source:auto;" +
                "	mso-ruby-visibility:none;}" +
                "br" +
                "   {mso-data-placement:same-cell;}" +
                "ruby" +
                "	{ruby-align:left;}" +
                ".style0" +
                "	{mso-number-format:General;" +
                "	text-align:general;" +
                "	vertical-align:middle;" +
                "   white-space:nowrap;" +
                "	mso-rotate:0;" +
                "	mso-background-source:auto;" +
                "	mso-pattern:auto;" +
                "	color:black;" +
                "	font-size:11.0pt;" +
                "	font-weight:400;" +
                "	font-style:normal;" +
                "	text-decoration:none;" +
                "	font-family:宋体;" +
                "	mso-generic-font-family:auto;" +
                "	mso-font-charset:134;" +
                "	border:none;" +
                "	mso-protection:locked visible;" +
                "mso-style-name:宋体;" +
                "	mso-style-id:0;}" +
                "td" +
                "	{mso-style-parent:style0;" +
                "	padding-top:1px;" +
                "	padding-right:1px;" +
                "	padding-left:1px;" +
                "	mso-ignore:padding;" +
                "	color:black;" +
                "	font-size:11.0pt;" +
                "	font-weight:400;" +
                "	font-style:normal;" +
                "	text-decoration:none;" +
                "	font-family:宋体;" +
                "	mso-generic-font-family:auto;" +
                "	mso-font-charset:134;" +
                "	mso-number-format:General;" +
                "	text-align:general;" +
                "	vertical-align:middle;" +
                "	border:none;" +
                "	mso-background-source:auto;" +
                "	mso-pattern:auto;" +
                "	mso-protection:locked visible;" +
                "	white-space:nowrap;" +
                "	mso-rotate:0;}" +
                ".xl65" +
                "	{mso-style-parent:style0;" +
                "	text-align:left;" +
                "	padding-left:96px;" +
                "	mso-char-indent-count:8;}" +
                ".xl66" +
                "	{mso-style-parent:style0;" +
                "	font-size:10.0pt;}" +
                ".xl67" +
                "	{mso-style-parent:style0;" +
                "	font-size:9.0pt;" +
                "	text-align:center;}" +
                ".xl68" +
                "	{mso-style-parent:style0;" +
                "	font-size:9.0pt;" +
                "	text-align:center;" +
                "	border-top:.5pt solid windowtext;" +
                "	border-right:none;" +
                "	border-bottom:.5pt solid windowtext;" +
                "	border-left:none;}" +
                ".xl69" +
                "	{mso-style-parent:style0;" +
                "	font-size:9.0pt;" +
                "	mso-number-format:\"\\@\";" +
                "   text-align:center;" +
                "	padding-left:12px;" +
                "	mso-char-indent-count:1;}" +
                ".xl70" +
                "	{mso-style-parent:style0;" +
                "	font-size:9.0pt;" +
                "	text-align:center;" +
                "	border-top:.5pt solid windowtext;" +
                "	border-right:none;" +
                "	border-bottom:.5pt solid windowtext;" +
                "	border-left:none;" +
                "	padding-left:24px;" +
                "	mso-char-indent-count:2;}" +
                ".xl71" +
                "	{mso-style-parent:style0;" +
                "	font-size:10.0pt;" +
                "	text-align:left;" +
                "	padding-left:48px;" +
                "	mso-char-indent-count:4;}" +
                ".xl72" +
                "	{mso-style-parent:style0;" +
                "	font-size:9.0pt;" +
                "	text-align:center;" +
                "	border-top:none;" +
                "	border-right:none;" +
                "   border-bottom:.5pt solid windowtext;" +
                "	border-left:none;}" +
                ".xl73" +
                "	{mso-style-parent:style0;" +
                "	font-size:9.0pt;" +
                "	mso-number-format:\"\\@\";" +
                "	text-align:left;" +
                "	border-top:none;" +
                "	border-right:none;" +
                "	border-bottom:.5pt solid windowtext;" +
                "	border-left:none;" +
                "	padding-left:12px;" +
                "	mso-char-indent-count:1;}" +
                ".xl74" +
                "	{mso-style-parent:style0;" +
                "	text-align:left;}" +
                ".xl75" +
                "	{mso-style-parent:style0;" +
                "	mso-number-format:\"\\@\";" +
                "	text-align:right;}" +
                ".btd" +
                "	{mso-style-parent:style0;" +
                "	border-top:.5pt solid windowtext;" +
                "	}" +
                "</style>" +
                "<table border = 0 cellpadding=0 cellspacing=0 width=779 style='border-collapse:" +
                " collapse;table-layout:fixed; width:585pt'>" +
                " <col width = 38 style='mso-width-source:userset;mso-width-alt:1216;width:29pt'>" +
                " <col width = 51 style='mso-width-source:userset;mso-width-alt:1632;width:38pt'>" +
                " <col width = 68 style='mso-width-source:userset;mso-width-alt:2176;width:51pt'>" +
                " <col width = 72 style='width:100pt'>" +
                " <col width = 478 style='mso-width-source:userset;mso-width-alt:15296;width:359pt'>" +
                " <col width = 72 style='width:54pt'>" +
                " <tr height = 19 valign=middle style = 'height:14.25pt' >" +
                "  <td height=19 width=38 style='height:14.25pt;width:29pt'></td>" +
                "  <td width = 51 style='width:38pt'></td>" +
                "  <td width = 68 style='width:51pt'></td>" +
                "  <td width = 150 style='width:54pt'></td>";
            const String Tail = "<td class=xl66></td>" +
                "<td></td>" +
                "</tr><![if supportMisalignedColumns]>" +
                    "<tr height=0 style='display:none'>" +
                    "<td width=38 style='width:29pt'></td>" +
                    "<td width=51 style='width:38pt'></td>" +
                    "<td width=68 style='width:51pt'></td>" +
                    "<td width=72 style='width:100pt'></td>" +
                    "<td width=478 style='width:359pt'></td>" +
                    "<td width=72 style='width:54pt'></td>" +
                "</tr>" +
                "<![endif]>" +
                "</table>";

            private Event _data;
            private String _binaryData;

            public Event Data
            {
                get
                {
                    return _data;
                }
                set
                {
                    _data = value ?? throw new Exception("设置的输出数据是个无效值");
                    _binaryData = null;
                }
            }

            public String Binary
            {
                get
                {
                    if (_binaryData == null)
                    {
                        Export();
                    }
                    return _binaryData;
                }
            }

            public EventGradeExporter(Event data)
            {
                _data = data;
            }

            public bool Export()
            {
                if (_binaryData != null)
                {
                    return true;
                }

                _binaryData = NormalizeToHtml5(_data);
                return _binaryData != null;
            }

            public bool ExportToFile(String url)
            {
                if (_binaryData == null && !Export())
                {
                    return false;
                }

                if (File.Exists(url))
                {
                    File.Delete(url);
                }
                Encoder enc = Encoding.UTF8.GetEncoder();
                FileStream fout = new FileStream(url, FileMode.Create);
                StreamWriter writer = new StreamWriter(fout);
                writer.Write(_binaryData);
                writer.Flush();
                fout.Close();
                return true;
            }

            private static String NormalizeToHtml5(Event data)
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);

                writer.Write(Head);
                var date = data.Games[data.Games.Count - 1].Conf.GameSession.SessionDate;
                writer.Write(String.Format("<td colspan=2 class=xl75 align=right width=550 style='width:413pt'>{0}</td></tr>", date.ToLongString()));

                writer.Write("<tr height=19 valign=middle style='height:14.25pt'>");
                if (!data.Parent.Conf.CompetitionRankInfo.Enabled || data.Conf.EventTeamworkInfo.BeTeamwork)
                {
                    writer.Write(String.Format("<td colspan = 3 height = 19 class = xl74 style = 'height:14.25pt'>{0}</td>", data.Conf.Name));
                    writer.Write("<td></td><td class = xl65>决赛成绩公告</td><td></td></tr>");
                    writer.Write("<tr height = 19 valign = middle style = 'height:14.25pt'>" +
                        "<td height = 19 class = xl68 align = center style = 'height:14.25pt'>名次</td>" +
                        "<td class = xl68 align = center>道</td>" +
                        "<td class = xl68 align = center>姓名</td>" +
                        "<td class = xl68 align = center>单位</td>" +
                        "<td class = xl70 align = center>成绩</td>" +
                        "<td class = xl68 align = center>备注</td>" +
                        "</tr>");

                    var lines = data.GetOrderInEvent();
                    if (!data.Conf.EventTeamworkInfo.BeTeamwork)
                    {
                        writer.Write(NormalizePersonalEventGradeListToHtml5(lines[0].Item2));
                    }
                    else
                    {
                        
                        writer.Write(NormalizeTeamworkEventGradeListToHtml5(lines[0].Item2));
                    }

                    writer.Write("<tr height=19 valign=middle style='height:14.25pt'>" +
                            "<td height = 19 class = btd style = 'height:14.25pt'></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "</tr>");
                }
                else
                {
                    var lines = data.GetOrderInEvent();
                    foreach (var pair in lines)
                    {
                        writer.Write(String.Format("<td colspan = 3 height = 19 class = xl74 style = 'height:14.25pt'>{0}{1}</td>", data.Conf.Name, pair.Item1));
                        writer.Write("<td></td><td class = xl65>决赛成绩公告</td><td></td></tr>");
                        writer.Write("<tr height = 19 valign = middle style = 'height:14.25pt'>" +
                            "<td height = 19 class = xl68 align = center style = 'height:14.25pt'>名次</td>" +
                            "<td class = xl68 align = center>道</td>" +
                            "<td class = xl68 align = center>姓名</td>" +
                            "<td class = xl68 align = center>单位</td>" +
                            "<td class = xl70 align = center>成绩</td>" +
                            "<td class = xl68 align = center>备注</td>" +
                            "</tr>");

                        writer.Write(NormalizePersonalEventGradeListToHtml5(pair.Item2));

                        writer.Write("<tr height=19 valign=middle style='height:14.25pt'>" +
                            "<td height = 19 class = btd style = 'height:14.25pt'></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "<td class = btd></td>" +
                            "</tr>");
                    }
                }

                var field = data.Parent.FieldInfos[data.LastGame.Conf.GameSession];
                writer.Write("<tr height=19 valign=middle style='height:14.25pt'>" +
                    "<td height = 19 style = 'height:14.25pt'></td>" +
                    "<td class=xl66 colspan=4 style='mso-ignore:colspan'>备注：<span" +
                    "style = 'mso-spacerun:yes'></span>  DSQ：犯规<span style = 'mso-spacerun:yes'>" +
                    "</span>  DNS：弃权</td><td></td><td></td></tr>");
                writer.Write(String.Format("<tr height = 19 valign = middle style = 'height:14.25pt'>" +
                    "<td height = 19 style = 'height:14.25pt'></td>" +
                    "<td class = xl71 colspan = 4 style = 'mso-ignore:colspan'>" +
                    "室温 : {1}℃<span style = 'mso-spacerun:yes'></span>   水温 : {0}℃</td>" +
                    "<td></td></tr>", field.IndoorTemperature, field.WaterTemperature));
                writer.Write(Tail);

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            private static String NormalizePersonalEventGradeListToHtml5(List<Tuple<SSUtils.Order, List<Line>>> lines)
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);

                foreach (var order in lines)
                {
                    foreach (var line in order.Item2)
                    {
                        writer.Write("<tr height = 19 valign = middle style = 'height:14.25pt'>");
                        writer.Write(String.Format("<td height = 19 class = xl67 align = center style = 'height:14.25pt'><font color = '#000000'>{0}</font></td>", order.Item1.Valid() ? order.Item1.Value.ToString() : ""));
                        writer.Write(String.Format("<td class = xl67 align = center><font color = '#000000'>{0}</font></td>", line.Order.Value.ToString()));
                        writer.Write(String.Format("<td class = xl67 align = center>{0}</td>", line.LineParticipant.Athletes[0].Name));
                        writer.Write(String.Format("<td class = xl67 align = center>{0}</td>", line.LineParticipant.ParticipantTeam.Conf.Name));
                        writer.Write(String.Format("<td class = xl69 align = center>{0}</td>", line.ParticipantGrade.ToFormatString()));
                        writer.Write(String.Format("<td class = xl67 align = center>{0}</td>", line.ParticipantGrade.GradeCode == GradeBase.Code.MR ? "MR" : ""));
                        writer.Write("</tr>");
                    }
                }

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            private static String NormalizeTeamworkEventGradeListToHtml5(List<Tuple<SSUtils.Order, List<Line>>> lines)
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);

                foreach (var order in lines)
                {
                    foreach (var line in order.Item2)
                    {
                        writer.Write("<tr height = 19 valign = middle style = 'height:14.25pt'>");
                        writer.Write(String.Format("<td height = 19 class = xl67 align = center style = 'height:14.25pt'><font color = '#000000'>{0}</font></td>", order.Item1.Valid() ? order.Item1.Value.ToString() : ""));
                        writer.Write(String.Format("<td class = xl67 align = center><font color = '#000000'>{0}</font></td>", line.Order.Value.ToString()));
                        writer.Write(String.Format("<td class = xl67 align = center>{0}</td>", line.LineParticipant.Name));
                        writer.Write(String.Format("<td class = xl67 align = center>{0}</td>", line.LineParticipant.ParticipantTeam.Conf.Name));
                        writer.Write(String.Format("<td class = xl69 align = center>{0}</td>", line.ParticipantGrade.ToFormatString()));
                        writer.Write(String.Format("<td class = xl67 align = center>{0}</td>", line.ParticipantGrade.GradeCode == GradeBase.Code.MR ? "MR" : ""));
                        writer.Write("</tr>");
                    }
                }

                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }
}
