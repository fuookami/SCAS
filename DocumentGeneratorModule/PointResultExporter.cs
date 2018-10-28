using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SCAS.CompetitionData;

namespace SCAS
{
    namespace DocumentGenerator
    {
        public class PointResultExporter : ErrorStorer
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
                    _data = value ?? throw new Exception("设置的数据是个无效值");
                }
            }

            public String TargetUrl
            {
                get;
                set;
            }

            public PointResultExporter(Competition data, String targetUrl = "")
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
                if (!targetUrl.EndsWith(".xlsx"))
                {
                    RefreshError(ErrorCode.MismatchedFileExtension, "不是一个excel表格文件");
                    return false;
                }

                FileInfo newFile = new FileInfo(targetUrl);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(targetUrl);
                }

                foreach (var eventData in Data.Events)
                {
                    eventData.RefreshPoints();
                }

                return ExportOrderToFile(GetOrder(), newFile);
            }

            private List<Tuple<SSUtils.Order, List<Team>>> GetOrder()
            {
                var teams = Data.Teams.ToArray().ToList();
                teams.Sort((lhs, rhs) => lhs.TotalPoints.CompareTo(rhs.TotalPoints));
                teams.Reverse();

                var groups = Data.Teams.GroupBy((ele) => ele.TotalPoints).OrderBy(grouping => grouping.Key).Reverse();
                var ret = new List<Tuple<SSUtils.Order, List<Team>>>();
                Int32 index = 1;
                foreach (var group in groups)
                {
                    var list = group.ToList();
                    ret.Add(new Tuple<SSUtils.Order, List<Team>>(new SSUtils.Order(index), list));
                    index += list.Count;
                }

                return ret;
            }

            private bool ExportOrderToFile(List<Tuple<SSUtils.Order, List<Team>>> order, FileInfo file)
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(String.Format("{0}总积分", Data.Conf.Name));
                    worksheet.Column(1).Width = 18;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(3).Width = 18;

                    worksheet.Cells[1, 1].Value = "名次";
                    worksheet.Cells[1, 2].Value = "院名";
                    worksheet.Cells[1, 3].Value = "积分";

                    Int32 row = 2;
                    foreach (var list in order)
                    {
                        foreach (var team in list.Item2)
                        {
                            worksheet.Cells[row, 1].Value = list.Item1.Value.ToString();
                            worksheet.Cells[row, 2].Value = team.Conf.Name;
                            worksheet.Cells[row, 3].Value = team.TotalPoints.ToString();
                            ++row;
                        }
                    }

                    worksheet.Cells[1, 1, row, 3].Style.Numberformat.Format = "@";
                    worksheet.Cells[1, 1, row, 3].Style.Font.Name = "微软雅黑";
                    worksheet.Cells[1, 1, row, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    package.Save();
                }

                return true;
            }
        }
    }
}
