using System;
using SCAS;

namespace TestEntryBlankModule
{
    class Program
    {
        static void Main(string[] args)
        {
            Generate("test");
            Generate("FreshmanCupConf");
            
            Console.ReadKey();
        }

        static void Generate(String name)
        {
            SCAS.CompetitionConfiguration.Analyzer analyzer = new SCAS.CompetitionConfiguration.Analyzer();
            if (analyzer.Analyze(String.Format("{0}.xml", name)))
            {
                Console.WriteLine("OK to read conf.");
            }
            else
            {
                Console.WriteLine("False to read conf. {0}", analyzer.LastError);
                return;
            }

            SCAS.CompetitionConfiguration.CompetitionInfo competitionInfo = analyzer.Result;

            SCAS.EntryBlank.Generator generator = new SCAS.EntryBlank.Generator(competitionInfo);
            if (generator.Generate(String.Format("{0}.xlsx", name)))
            {
                Console.WriteLine("OK to generate entry blank.");
            }
            else
            {
                Console.WriteLine("False to generate entry blank. {0}", generator.LastError);
            }
        }
    }
}
