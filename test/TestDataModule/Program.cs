﻿using System;
using System.Collections.Generic;

namespace TestDataModule
{
    class Program
    {
        static void Main(string[] args)
        {
            Generate();

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

        static void Generate()
        {
            SCAS.CompetitionConfiguration.Analyzer confAnalyzer = new SCAS.CompetitionConfiguration.Analyzer();
            if (confAnalyzer.Analyze("IntercollegeCupConf.xml"))
            {
                Console.WriteLine("OK to read conf.");
            }
            else
            {
                Console.WriteLine("False to read conf. {0}", confAnalyzer.LastError);
                return;
            }

            var conf = confAnalyzer.Result;
            SCAS.CompetitionData.Generator generator = new SCAS.CompetitionData.Generator(conf);
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
        }
    }
}
