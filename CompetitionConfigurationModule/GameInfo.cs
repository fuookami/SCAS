using System;
using System.Collections.Generic;

namespace CompetitionConfigurationModule
{
    class GameInfo
    {
    }

    class GameInfoList : List<GameInfo>
    {

    }

    class GameInfoPool : GameInfoList
    {
        private EventInfo eventInfo;

        public GameInfoPool(EventInfo parentEventInfo)
        {
            eventInfo = parentEventInfo;
        }
    }
}
