using CompetitionConfigurationModule;
using System;

namespace TestConfigurationModule
{
    class Program
    {
        static void Main(string[] args)
        {
            var data1 = GenerateTestCompetitionInfo();
            TestSaveToXML(data1);
            var data2 = TestLoadFromXML();
            Console.ReadKey();
        }

        static CompetitionInfo GenerateTestCompetitionInfo()
        {
            CompetitionInfo ret = new CompetitionInfo();
            return ret;
        }

        static void TestSaveToXML(CompetitionInfo info)
        {
            CompetitionConfigurationNormalizer normalizer = new CompetitionConfigurationNormalizer(info);
            if (normalizer.NormalizeToFile("test.xml"))
            {
                Console.WriteLine("OK to write.");
            }
            else
            {
                Console.WriteLine("False to write. {0}", normalizer.LastError);
            }
        }

        static CompetitionInfo TestLoadFromXML()
        {
            CompetitionConfigurationAnalyzer analyzer = new CompetitionConfigurationAnalyzer();
            if (analyzer.Analyze("test.xml"))
            {
                Console.WriteLine("OK to read.");
                return analyzer.Result;
            }
            else
            {
                Console.WriteLine("False to read. {0}", analyzer.LastError);
                return null;
            }
        }
    }
}
