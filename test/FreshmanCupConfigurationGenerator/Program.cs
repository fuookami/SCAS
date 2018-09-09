using System;
using System.Collections.Generic;
using SCAS.CompetitionConfiguration;
using SSUtils;

namespace FreshmanCupConfigurationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = GenerateFreshmanCupCompetitionInfoTemplate();

            Normalizer normalizer = new Normalizer(data);
            if (normalizer.NormalizeToFile("conf.xml"))
            {
                Console.WriteLine("OK to write.");
            }
            else
            {
                Console.WriteLine("False to write. {0}", normalizer.LastError);
            }
        }

        static CompetitionInfo GenerateFreshmanCupCompetitionInfoTemplate()
        {
            CompetitionInfo ret = new CompetitionInfo();

            return ret;
        }
    }
}
