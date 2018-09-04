using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SCAS
{
    namespace EntryBlank
    {
        public class Exporter : ErrorStorer
        {
            private EntryBlank _blank;

            public EntryBlank Blank
            {
                get
                {
                    return _blank;
                }
                set
                {
                    _blank = value ?? throw new Exception("设置的报名表是个无效值");
                }
            }

            public String TargetUrl
            {
                get;
                set;
            }

            public Exporter(EntryBlank blank, String targetUrl = "")
            {
                _blank = blank;
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

                if ((Blank.Conf.CompetitionRankInfo.Enabled && Blank.Conf.CompetitionRankInfo.Forced)
                    || !Blank.Conf.CompetitionRankInfo.Enabled)
                {
                    return ExportForcedOrNotRankEntryBlank(newFile);
                }
                else
                {
                    return ExportNotForcedRankEntryBlank(newFile);
                }
            }

            private void AddLeaderInfoBlank(ExcelWorksheet worksheet, Int32 row, String title)
            {
                worksheet.Cells[row, 1].Value = title;
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Size += 2;
                worksheet.Cells[row, 1, row + 3, 1].Merge = true;
                worksheet.Cells[row, 2].Value = "姓名：";
                worksheet.Cells[row, 2].Style.Font.Bold = true;
                worksheet.Cells[row, 3, row, 6].Merge = true;
                worksheet.Cells[row + 1, 2].Value = "学号/工号：";
                worksheet.Cells[row + 1, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 1, 3, row + 1, 6].Merge = true;
                worksheet.Cells[row + 2, 2].Value = "联系方式：";
                worksheet.Cells[row + 2, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 2, 3, row + 2, 6].Merge = true;
                worksheet.Cells[row + 3, 2].Value = "邮箱：";
                worksheet.Cells[row + 3, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 3, 3, row + 3, 6].Merge = true;
            }

            private bool ExportForcedOrNotRankEntryBlank(FileInfo file)
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("报名表");
                    worksheet.Column(1).Style.WrapText = true;
                    worksheet.Column(1).Width = 18;
                    worksheet.Column(2).Width = 12;

                    worksheet.Cells[1, 1].Value = "院名：";
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size += 4;
                    worksheet.Cells[1, 2, 1, 6].Merge = true;

                    Int32 row = 2;

                    AddLeaderInfoBlank(worksheet, row, "领队");
                    row += 4;
                    foreach (var subLeader in Blank.TeamSubLeader)
                    {
                        AddLeaderInfoBlank(worksheet, row, !subLeader.Optional ? "副领队" : "副领队\n（可选）");
                        row += 4;
                    }
                    AddLeaderInfoBlank(worksheet, row, !Blank.TeamCoach.Optional ? "教练" : "教练\n（可选）");
                    row += 4;

                    worksheet.Cells[row, 1].Value = "个人项目报名表";
                    worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 1].Style.Font.Bold = true;
                    worksheet.Cells[row, 1].Style.Font.Size += 4;
                    worksheet.Cells[row, 1, row, 6].Merge = true;
                    worksheet.Row(row).Height = 20;
                    row += 1;

                    foreach (var entry in Blank.PersonalEntries)
                    {
                        Int32 bgRow = row;
                        worksheet.Cells[row, 1].Value = entry.Conf.Name;
                        worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 1].Style.Font.Bold = true;
                        worksheet.Cells[row, 1].Style.Font.Size += 2;

                        foreach (var item in entry.Items)
                        {
                            worksheet.Cells[row, 3].Value = item.Key;
                            worksheet.Cells[row, 5].Value = item.SidKey;
                            ++row;
                        }

                        worksheet.Cells[bgRow, 1, row - 1, 2].Merge = true;
                    }

                    worksheet.Cells[row, 1].Value = "团体项目报名表";
                    worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 1].Style.Font.Bold = true;
                    worksheet.Cells[row, 1].Style.Font.Size += 4;
                    worksheet.Cells[row, 1, row, 6].Merge = true;
                    worksheet.Row(row).Height = 20;
                    row += 1;

                    foreach (var entry in Blank.TeamworkEntries)
                    {

                    }

                    worksheet.Cells[1, 1, row, 6].Style.Font.Name = "微软雅黑";
                    worksheet.Cells[1, 1, row, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    // worksheet.Cells[1, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(255, 192, 192, 192));

                    package.Save();
                }

                return true;
            }

            private bool ExportNotForcedRankEntryBlank(FileInfo file)
            {

                return true;
            }
        }
    };
};
