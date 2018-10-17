using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;
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
                String html = "";

                {
                    String regulationPart = NormalizeRegulations();
                    if (regulationPart == null)
                    {
                        return null;
                    }
                    html += regulationPart;
                }

                return html;
            }

            private String NormalizeRegulations()
            {
                String ret = "";

                ret += String.Format("<h1>{0}</h1>", _data.Conf.Name);

                return ret;
            }
        }
    }
};
