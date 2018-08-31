using System;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace CompetitionData
    {
        public class FieldInfo
        {
            private const Double DefaultIndoorTemperature = 24;
            private const Double DefaultWaterTemperature = 26;
            private const Double DefaultResidualChlorine = 0.3;
            private const Double DefaultPHValue = 7.2;

            public Double IndoorTemperature
            {
                get;
                set;
            }

            public Double WaterTemperature
            {
                get;
                set;
            }

            public Double ResidualChlorine
            {
                get;
                set;
            }

            public Double PHValue
            {
                get;
                set;
            }

            public Session FieldInfoSession
            {
                get;
            }

            public FieldInfo(Session session, 
                Double indoorTemperature = DefaultIndoorTemperature, 
                Double waterTemperature = DefaultWaterTemperature, 
                Double residualChlorine = DefaultResidualChlorine, 
                Double phValue = DefaultPHValue)
            {
                IndoorTemperature = indoorTemperature;
                WaterTemperature = waterTemperature;
                ResidualChlorine = residualChlorine;
                PHValue = phValue;
                FieldInfoSession = session;
            }
        };
    };
};
