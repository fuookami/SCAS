using System;
using System.Collections.Generic;

namespace TestDataModule
{
    class Program
    {
        static void Main(string[] args)
        {
//             var data1 = Generate1();
//             if (data1 != null)
//             {
//                 TestWriteAndReadData(data1, "IntercollegeCup.xml");
//             }
//             var data2 = Generate2();
//             if (data2 != null)
//             {
//                 TestWriteAndReadData(data2, "FreshmanCup.xml");
//             }
            var data3 = Generate3();
            if (data3 != null)
            {
                TestWriteAndReadData(data3, "FreshmanCup.xml");
            }

            Console.ReadKey();
        }

        static SCAS.EntryBlank.Blank ReadBlank(String prefix, String name, SCAS.CompetitionConfiguration.CompetitionInfo conf)
        {
            SCAS.EntryBlank.Analyzer analyzer = new SCAS.EntryBlank.Analyzer(conf);
            if (analyzer.Analyze(String.Format("{0}\\{1}.xlsx", prefix, name)))
            {
                Console.WriteLine("OK to read entry blank.");
            }
            else
            {
                Console.WriteLine("False to read entry blank {0}. {1}", name, analyzer.LastError);
            }

            return analyzer.Result;
        }

        static SCAS.CompetitionData.Competition Generate1()
        {
            SCAS.CompetitionConfiguration.Analyzer confAnalyzer = new SCAS.CompetitionConfiguration.Analyzer();
            if (confAnalyzer.Analyze("IntercollegeCupConf.xml"))
            {
                Console.WriteLine("OK to read conf.");
            }
            else
            {
                Console.WriteLine("False to read conf. {0}", confAnalyzer.LastError);
                return null;
            }

            var conf = confAnalyzer.Result;
             SCAS.CompetitionData.Generator generator = new SCAS.CompetitionData.Generator(conf, "");
            List<String> readTargets = new List<String> { "1", "3", "4", "5", "6", "7", "8", "9", "11", "12", "15", "16" };
            foreach (var target in readTargets)
            {
                var blank = ReadBlank("1", target, conf);
                if (blank != null)
                {
                    generator.EntryBlanks.Add(blank);
                }
            }

            if (generator.Generate())
            {
                Console.WriteLine("Ok to generate data.");
            }
            else
            {
                Console.WriteLine("False to generate data. {0}", generator.LastError);
            }

            SCAS.DocumentGenerator.ProgramExporter exporter = new SCAS.DocumentGenerator.ProgramExporter(generator.Result, "IntercollegeCup.docx");
            if (exporter.Export())
            {
                Console.WriteLine("Ok to export program.");
            }
            else
            {
                Console.WriteLine("False to export program. {0}", exporter.LastError);
            }

            return generator.Result;
        }

        static SCAS.CompetitionData.Competition Generate2()
        {
            SCAS.CompetitionConfiguration.Analyzer confAnalyzer = new SCAS.CompetitionConfiguration.Analyzer();
            if (confAnalyzer.Analyze("FreshmanCupConf.xml"))
            {
                Console.WriteLine("OK to read conf.");
            }
            else
            {
                Console.WriteLine("False to read conf. {0}", confAnalyzer.LastError);
                return null;
            }

            var conf = confAnalyzer.Result;
            SCAS.CompetitionData.Generator generator = new SCAS.CompetitionData.Generator(conf, "");
            List<String> readTargets = new List<String> { "1", "3", "4", "5", "6", "7", "8", "9", "11", "12", "15", "16" };
            foreach (var target in readTargets)
            {
                var blank = ReadBlank("2", target, conf);
                if (blank != null)
                {
                    generator.EntryBlanks.Add(blank);
                }
            }

            if (generator.Generate())
            {
                Console.WriteLine("Ok to generate data.");
            }
            else
            {
                Console.WriteLine("False to generate data. {0}", generator.LastError);
            }

            SCAS.DocumentGenerator.ProgramExporter exporter = new SCAS.DocumentGenerator.ProgramExporter(generator.Result, "FreshmanCup.docx");
            if (exporter.Export())
            {
                Console.WriteLine("Ok to export program.");
            }
            else
            {
                Console.WriteLine("False to export program. {0}", exporter.LastError);
            }

            return generator.Result;
        }

        static SCAS.CompetitionData.Competition Generate3()
        {
            SCAS.CompetitionConfiguration.Analyzer confAnalyzer = new SCAS.CompetitionConfiguration.Analyzer();
            if (confAnalyzer.Analyze("FreshmanCupConf.xml"))
            {
                Console.WriteLine("OK to read conf.");
            }
            else
            {
                Console.WriteLine("False to read conf. {0}", confAnalyzer.LastError);
                return null;
            }

            var conf = confAnalyzer.Result;
            SCAS.CompetitionData.Generator generator = new SCAS.CompetitionData.Generator(conf, "");
            List<String> readTargets = new List<String> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "15", "16" };
            foreach (var target in readTargets)
            {
                var blank = ReadBlank("3", target, conf);
                if (blank != null)
                {
                    generator.EntryBlanks.Add(blank);
                }
            }

            if (generator.Generate())
            {
                Console.WriteLine("Ok to generate data.");
            }
            else
            {
                Console.WriteLine("False to generate data. {0}", generator.LastError);
            }

            SCAS.DocumentGenerator.ProgramExporter exporter = new SCAS.DocumentGenerator.ProgramExporter(generator.Result, "3.docx");
            if (exporter.Export())
            {
                Console.WriteLine("Ok to export program.");
            }
            else
            {
                Console.WriteLine("False to export program. {0}", exporter.LastError);
            }

            return generator.Result;
        }

        static void TestWriteAndReadData(SCAS.CompetitionData.Competition data, String name)
        {
            SCAS.CompetitionData.Normalizer normalizer = new SCAS.CompetitionData.Normalizer(data);
            if (normalizer.NormalizeToFile(name))
            {
                Console.WriteLine("OK to write data.");
            }
            else
            {
                Console.WriteLine("False to read data. {0}", normalizer.LastError);
                return;
            }

            SCAS.CompetitionData.Analyzer analyzer = new SCAS.CompetitionData.Analyzer(data.Conf);
            if (analyzer.Analyze(name))
            {
                Console.WriteLine("OK to read data.");
            }
            else
            {
                Console.WriteLine("False to read data. {0}", analyzer.LastError);
                return;
            }

            SCAS.CompetitionData.Normalizer normalizer2 = new SCAS.CompetitionData.Normalizer(analyzer.Result);
            if (normalizer2.NormalizeToFile(String.Format("_{0}", name)))
            {
                Console.WriteLine("OK to write data2.");
            }
            else
            {
                Console.WriteLine("False to write data2. {0}", analyzer.LastError);
                return;
            }
        }
    }
}
