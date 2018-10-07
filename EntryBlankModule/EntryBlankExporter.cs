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

            private bool ExportForcedOrNotRankEntryBlank(FileInfo file)
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet basicInfoWorksheet = package.Workbook.Worksheets.Add("基本信息");
                    basicInfoWorksheet.Column(1).Style.WrapText = true;
                    basicInfoWorksheet.Column(1).Width = 18;
                    basicInfoWorksheet.Column(2).Width = 12;

                    basicInfoWorksheet.Cells[1, 1].Value = "院名：";
                    basicInfoWorksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    basicInfoWorksheet.Cells[1, 1].Style.Font.Bold = true;
                    basicInfoWorksheet.Cells[1, 1].Style.Font.Size += 4;
                    basicInfoWorksheet.Cells[1, 2, 1, 5].Merge = true;

                    Int32 row = 2;

                    AddLeaderInfoBlank(basicInfoWorksheet, row, 5, "领队");
                    row += 4;
                    foreach (var subLeader in Blank.TeamSubLeader)
                    {
                        AddLeaderInfoBlank(basicInfoWorksheet, row, 5, !subLeader.Optional ? "副领队" : "副领队\n（可选）");
                        row += 4;
                    }
                    AddLeaderInfoBlank(basicInfoWorksheet, row, 5, !Blank.TeamCoach.Optional ? "教练" : "教练\n（可选）");
                    row += 4;

                    basicInfoWorksheet.Cells[row, 1].Value = "运动员报名表";
                    basicInfoWorksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    basicInfoWorksheet.Cells[row, 1].Style.Font.Bold = true;
                    basicInfoWorksheet.Cells[row, 1].Style.Font.Size += 4;
                    basicInfoWorksheet.Cells[row, 1, row, 5].Merge = true;
                    basicInfoWorksheet.Row(row).Height = 20;
                    row += 1;

                    Int32 infoBeginRow = row;
                    foreach (var category in Blank.Conf.AthleteCategories)
                    {
                        basicInfoWorksheet.Cells[row, 1].Value = category.Name;
                        basicInfoWorksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        basicInfoWorksheet.Cells[row, 1].Style.Font.Bold = true;
                        basicInfoWorksheet.Cells[row, 1].Style.Font.Size += 2;

                        for (Int32 i = 0; i != 5; ++i)
                        {
                            basicInfoWorksheet.Cells[row + i, 2].Value = "姓名";
                            basicInfoWorksheet.Cells[row + i, 2].Style.Font.Bold = true;
                            basicInfoWorksheet.Cells[row + i, 4].Value = category.SidKey;
                            basicInfoWorksheet.Cells[row + i, 4].Style.Font.Bold = true;
                        }

                        basicInfoWorksheet.Cells[row, 1, row + 4, 1].Merge = true;
                        row += 5;
                    }

                    basicInfoWorksheet.Cells[1, 1, row, 8].Style.Numberformat.Format = "@";
                    basicInfoWorksheet.Cells[1, 1, row, 8].Style.Font.Name = "微软雅黑";
                    basicInfoWorksheet.Cells[1, 1, row, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ExcelWorksheet entryWorksheet = package.Workbook.Worksheets.Add("报名表");
                    Int32 temp = ExportEntries(entryWorksheet, 1, Blank, infoBeginRow);
                    if (temp == 0)
                    {
                        return false;
                    }
                    row = temp;

                    entryWorksheet.Cells[1, 1, row, 8].Style.Numberformat.Format = "@";
                    entryWorksheet.Cells[1, 1, row, 8].Style.Font.Name = "微软雅黑";
                    entryWorksheet.Cells[1, 1, row, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    package.Save();
                }

                return true;
            }

            private bool ExportNotForcedRankEntryBlank(FileInfo file)
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet basicInfoWorksheet = package.Workbook.Worksheets.Add("基本信息");
                    basicInfoWorksheet.Column(1).Style.WrapText = true;
                    basicInfoWorksheet.Column(1).Width = 18;
                    basicInfoWorksheet.Column(2).Width = 12;

                    basicInfoWorksheet.Cells[1, 1].Value = "院名：";
                    basicInfoWorksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    basicInfoWorksheet.Cells[1, 1].Style.Font.Bold = true;
                    basicInfoWorksheet.Cells[1, 1].Style.Font.Size += 4;
                    basicInfoWorksheet.Cells[1, 2, 1, 7].Merge = true;

                    Int32 row = 2;

                    AddLeaderInfoBlank(basicInfoWorksheet, row, 7, "领队");
                    row += 4;
                    foreach (var subLeader in Blank.TeamSubLeader)
                    {
                        AddLeaderInfoBlank(basicInfoWorksheet, row, 7, !subLeader.Optional ? "副领队" : "副领队\n（可选）");
                        row += 4;
                    }
                    AddLeaderInfoBlank(basicInfoWorksheet, row, 7, !Blank.TeamCoach.Optional ? "教练" : "教练\n（可选）");
                    row += 4;

                    basicInfoWorksheet.Cells[row, 1].Value = "运动员报名表";
                    basicInfoWorksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    basicInfoWorksheet.Cells[row, 1].Style.Font.Bold = true;
                    basicInfoWorksheet.Cells[row, 1].Style.Font.Size += 4;
                    basicInfoWorksheet.Cells[row, 1, row, 7].Merge = true;
                    basicInfoWorksheet.Row(row).Height = 20;
                    row += 1;

                    Int32 infoBeginRow = row;
                    foreach (var category in Blank.Conf.AthleteCategories)
                    {
                        basicInfoWorksheet.Cells[row, 1].Value = category.Name;
                        basicInfoWorksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        basicInfoWorksheet.Cells[row, 1].Style.Font.Bold = true;
                        basicInfoWorksheet.Cells[row, 1].Style.Font.Size += 2;

                        for (Int32 i = 0; i != 5; ++i)
                        {
                            basicInfoWorksheet.Cells[row + i, 2].Value = "姓名";
                            basicInfoWorksheet.Cells[row + i, 2].Style.Font.Bold = true;
                            basicInfoWorksheet.Cells[row + i, 4].Value = category.SidKey;
                            basicInfoWorksheet.Cells[row + i, 4].Style.Font.Bold = true;
                            basicInfoWorksheet.Cells[row + i, 6].Value = "组别";
                            basicInfoWorksheet.Cells[row + i, 6].Style.Font.Bold = true;
                        }

                        basicInfoWorksheet.Cells[row, 1, row + 4, 1].Merge = true;
                        row += 5;
                    }

                    basicInfoWorksheet.Cells[1, 1, row, 7].Style.Numberformat.Format = "@";
                    basicInfoWorksheet.Cells[1, 1, row, 7].Style.Font.Name = "微软雅黑";
                    basicInfoWorksheet.Cells[1, 1, row, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ExcelWorksheet entryWorksheet = package.Workbook.Worksheets.Add("报名表");
                    Int32 temp = ExportEntries(entryWorksheet, 1, Blank, infoBeginRow);
                    if (temp == 0)
                    {
                        return false;
                    }
                    row = temp;

                    entryWorksheet.Cells[1, 1, row, 8].Style.Numberformat.Format = "@";
                    entryWorksheet.Cells[1, 1, row, 8].Style.Font.Name = "微软雅黑";
                    entryWorksheet.Cells[1, 1, row, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    package.Save();
                };
                
                return true;
            }

            private void AddLeaderInfoBlank(ExcelWorksheet worksheet, Int32 row, Int32 rcol, String title)
            {
                worksheet.Cells[row, 1].Value = title;
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                worksheet.Cells[row, 1].Style.Font.Size += 2;
                worksheet.Cells[row, 1, row + 3, 1].Merge = true;
                worksheet.Cells[row, 2].Value = "姓名：";
                worksheet.Cells[row, 2].Style.Font.Bold = true;
                worksheet.Cells[row, 3, row, rcol].Merge = true;
                worksheet.Cells[row + 1, 2].Value = "学号/工号：";
                worksheet.Cells[row + 1, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 1, 3, row + 1, rcol].Merge = true;
                worksheet.Cells[row + 2, 2].Value = "联系方式：";
                worksheet.Cells[row + 2, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 2, 3, row + 2, rcol].Merge = true;
                worksheet.Cells[row + 3, 2].Value = "邮箱：";
                worksheet.Cells[row + 3, 2].Style.Font.Bold = true;
                worksheet.Cells[row + 3, 3, row + 3, rcol].Merge = true;
            }

            private Int32 ExportEntries(ExcelWorksheet sheet, Int32 row, EntryBlank blank, Int32 infoBeginRow)
            {
                sheet.Column(1).Style.WrapText = true;
                sheet.Column(1).Width = 16;
                sheet.Column(2).Width = 8;

                sheet.Cells[row, 1].Value = "个人项目报名表";
                sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 1].Style.Font.Bold = true;
                sheet.Cells[row, 1].Style.Font.Size += 4;
                sheet.Cells[row, 1, row, 8].Merge = true;
                sheet.Row(row).Height = 20;
                row += 1;

                foreach (var entry in Blank.PersonalEntries)
                {
                    Int32 bgRow = row;
                    sheet.Cells[row, 1].Value = entry.Conf.Name;
                    sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    sheet.Cells[row, 1].Style.Font.Size += 2;

                    foreach (var item in entry.Items)
                    {
                        sheet.Cells[row, 3].Value = item.Key;
                        sheet.Cells[row, 5].Value = item.SidKey;
                        sheet.Cells[row, 6].Formula = String.Format("IF(D{0}<>\"\", VLOOKUP(D{0}, 基本信息!C{1}:E65536, 3, FALSE ), \"\")", row, infoBeginRow);
                        sheet.Cells[row, 7].Value = "最好成绩（选填）";
                        ++row;
                    }

                    sheet.Cells[bgRow, 1, row - 1, 2].Merge = true;
                }

                sheet.Cells[row, 1].Value = "团体项目报名表";
                sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[row, 1].Style.Font.Bold = true;
                sheet.Cells[row, 1].Style.Font.Size += 4;
                sheet.Cells[row, 1, row, 8].Merge = true;
                sheet.Row(row).Height = 20;
                row += 1;

                foreach (var entry in Blank.TeamworkEntries)
                {
                    Int32 bgRow = row;
                    sheet.Cells[row, 1].Value = entry.Conf.Name;
                    sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    sheet.Cells[row, 1].Style.Font.Size += 2;

                    if (entry.ItemLists.Count == 1)
                    {
                        var itemList = entry.ItemLists[0];

                        Int32 thisBgRow = row;

                        foreach (var item in itemList.Items)
                        {
                            sheet.Cells[row, 3].Value = item.Key;
                            sheet.Cells[row, 5].Value = item.SidKey;
                            sheet.Cells[row, 6].Formula = String.Format("IF(D{0}<>\"\", VLOOKUP(D{0}, 基本信息!C{1}:E65536, 3, FALSE ), \"\")", row, infoBeginRow);
                            ++row;
                        }

                        sheet.Cells[thisBgRow, 2, row - 1, 2].Merge = true;
                        sheet.Cells[thisBgRow, 7].Value = "最好成绩（选填）";
                        sheet.Cells[thisBgRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet.Cells[thisBgRow, 7, row - 1, 7].Merge = true;
                        sheet.Cells[thisBgRow, 8, row - 1, 8].Merge = true;

                        sheet.Cells[bgRow, 1, row - 1, 2].Merge = true;
                    }
                    else
                    {
                        foreach (var itemList in entry.ItemLists)
                        {
                            Int32 thisBgRow = row;
                            sheet.Cells[row, 2].Value = itemList.Name;

                            foreach (var item in itemList.Items)
                            {
                                sheet.Cells[row, 3].Value = item.Key;
                                sheet.Cells[row, 5].Value = item.SidKey;
                                sheet.Cells[row, 6].Formula = String.Format("IF(D{0}<>\"\", VLOOKUP(D{0}, 基本信息!C{1}:E65536, 3, FALSE ), \"\")", row, infoBeginRow);
                                ++row;
                            }

                            sheet.Cells[thisBgRow, 2, row - 1, 2].Merge = true;
                            sheet.Cells[thisBgRow, 7].Value = "最好成绩（选填）";
                            sheet.Cells[thisBgRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[thisBgRow, 7, row - 1, 7].Merge = true;
                            sheet.Cells[thisBgRow, 8, row - 1, 8].Merge = true;
                        }

                        sheet.Cells[bgRow, 1, row - 1, 1].Merge = true;
                    }
                }

                return row;
            }
        }
    };
};
