using System;
using System.IO;
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

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("报名表");
                    worksheet.Column(1).Style.WrapText = true;
                    worksheet.Column(1).Width = 18;
                    worksheet.Column(2).Width = 12;

                    worksheet.Cells[1, 1].Value = "院名：";
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size += 4;
                    worksheet.Cells[1, 2, 1, 7].Merge = true;

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

                    worksheet.Cells[row, 1].Value = "报名表";
                    worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 1].Style.Font.Bold = true;
                    worksheet.Cells[row, 1].Style.Font.Size += 4;
                    worksheet.Cells[row, 1, row, 7].Merge = true;

                    row += 1;

                    foreach (var entry in Blank.Entries)
                    {
                        worksheet.Cells[row, 1].Value = entry.Conf.Name;
                        worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 1].Style.Font.Bold = true;
                        worksheet.Cells[row, 1].Style.Font.Size += 2;

                        row += 1;
                    }

                    worksheet.Cells[1, 1, row, 7].Style.Font.Name = "微软雅黑";
                    worksheet.Cells[1, 1, row, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    package.Save();
                }

                return true;
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
                worksheet.Cells[row, 3, row, 7].Merge = true;
                worksheet.Cells[row + 1, 2].Value = "学号/工号：";
                worksheet.Cells[row + 1, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 1, 3, row + 1, 7].Merge = true;
                worksheet.Cells[row + 2, 2].Value = "联系方式：";
                worksheet.Cells[row + 2, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 2, 3, row + 2, 7].Merge = true;
                worksheet.Cells[row + 3, 2].Value = "邮箱：";
                worksheet.Cells[row + 3, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 3, 3, row + 3, 7].Merge = true;
            }
        }
    };
};
