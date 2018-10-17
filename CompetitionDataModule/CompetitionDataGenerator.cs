using System;
using System.Collections.Generic;
using System.Linq;
using SCAS.CompetitionConfiguration;
using SCAS.EntryBlank;

namespace SCAS
{
    namespace CompetitionData
    {
        public class Generator : ErrorStorer
        {
            private static readonly Int32 MaxTryTime = 5;
            private static readonly Dictionary<UInt32, List<Int32>> EvenLineNumberOrders;

            private CompetitionInfo _conf;
            private String _rankTableUrl;
            Dictionary<EntryBlank.Athlete, Athlete> _blankAthlete2DataAthlete;
            private Dictionary<Event, List<Tuple<Team, EntryItem>>> _personalEntry;
            private Dictionary<Event, Dictionary<Team, List<EntryItemList>>> _teamworkEntry;

            static Generator()
            {
                EvenLineNumberOrders = new Dictionary<UInt32, List<Int32>>();
                EvenLineNumberOrders.Add(4, new List<Int32> { 1, 2, 0, 3 });
                EvenLineNumberOrders.Add(6, new List<Int32> { 2, 3, 1, 4, 0, 5 });
                EvenLineNumberOrders.Add(8, new List<Int32> { 3, 4, 2, 5, 1, 6, 0, 7 });
                EvenLineNumberOrders.Add(10, new List<Int32> { 4, 5, 3, 6, 2, 7, 1, 8, 0, 9 });
            }

            public CompetitionInfo Conf
            {
                get
                {
                    return _conf;
                }
            }

            public List<Blank> EntryBlanks
            {
                get;
            }

            public Competition Result
            {
                get;
                private set;
            }

            public Generator(CompetitionInfo conf, String rankTableUrl = "")
            {
                _conf = conf;
                _rankTableUrl = rankTableUrl;
                _blankAthlete2DataAthlete = new Dictionary<EntryBlank.Athlete, Athlete>();
                _personalEntry = new Dictionary<Event, List<Tuple<Team, EntryItem>>>();
                _teamworkEntry = new Dictionary<Event, Dictionary<Team, List<EntryItemList>>>();
                EntryBlanks = new List<Blank>();
                Result = null;
            }

            public bool Generate()
            {
                Competition temp = new Competition(_conf);

                if (!_conf.Sessions.CheckOrderIsContinuous())
                {
                    return false;
                }

                foreach (var datePair in _conf.Sessions)
                {
                    for (Int32 i = 0, j = datePair.Value.Count; i != j; ++i)
                    {
                        var session = datePair.Value[i];
                        temp.FieldInfos.Add(session, new FieldInfo(session));
                    }
                }

                foreach (var eventConf in _conf.EventInfos)
                {
                    Event eventTemp = new Event(eventConf);
                    foreach (var gameConf in eventConf.GameInfos)
                    {
                        Game game = new Game(gameConf);
                        eventTemp.Games.Add(game);

                        if (!temp.Games.ContainsKey(gameConf.GameSession))
                        {
                            temp.Games.Add(gameConf.GameSession, new List<Game>());
                        }
                        temp.Games[gameConf.GameSession].Add(game);
                    }

                    if (!eventConf.EventTeamworkInfo.BeTeamwork)
                    {
                        _personalEntry.Add(eventTemp, new List<Tuple<Team, EntryItem>>());
                    }
                    else
                    {
                        _teamworkEntry.Add(eventTemp, new Dictionary<Team, List<EntryItemList>>());
                    }
                    eventTemp.Games.Sort((lhs, rhs) => lhs.Conf.OrderInEvent.CompareTo(rhs.Conf.OrderInEvent));
                    temp.Events.Add(eventTemp);
                }

                foreach (var sessionPair in temp.Games)
                {
                    sessionPair.Value.Sort((lhs, rhs) => lhs.Conf.OrderInSession.CompareTo(rhs.Conf.OrderInSession));
                }

                if (_conf.CompetitionRankInfo.Enabled && _conf.CompetitionRankInfo.Forced)
                {
                    var rankTable = LoadRankTable(_rankTableUrl);
                    if (rankTable == null)
                    {
                        return false;
                    }
                    foreach (var blank in EntryBlanks)
                    {
                        if (!GenerateTeamDatas(temp, blank, rankTable))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    foreach (var blank in EntryBlanks)
                    {
                        if (!GenerateTeamDatas(temp, blank))
                        {
                            return false;
                        }
                    }
                }
                temp.Teams.Sort((lhs, rhs) => lhs.Conf.Order.CompareTo(rhs.Conf.Order));

                foreach (var eventData in temp.Events)
                {
                    if (!eventData.Conf.EventTeamworkInfo.BeTeamwork)
                    {
                        if (!ArrangePersonalGroups(eventData))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!ArrangeTeamworkGroups(eventData))
                        {
                            return false;
                        }
                    }
                }

                temp.Events.RemoveAll((ele) =>
                {
                    return ele.Games[0].Groups.Count == 0;
                });

                Result = temp;
                return true;
            }

            private Dictionary<String, AthleteRank> LoadRankTable(String url)
            {
                Dictionary<String, AthleteRank> ret = new Dictionary<string, AthleteRank>();

                return ret;
            }

            private bool GenerateTeamDatas(Competition data, Blank blank, Dictionary<String, AthleteRank> rankTable = null)
            {
                var conf = _conf.TeamInfos.Find((ele) => ele == blank.Team);
                if (conf == null)
                {
                    return false;
                }

                Team temp = new Team(conf);
                SetLeader(temp.TeamLeader, blank.TeamLeader);
                foreach (var subLeader in blank.TeamSubLeader)
                {
                    if (subLeader != null)
                    {
                        Leader newSubLeader = new Leader();
                        SetLeader(newSubLeader, subLeader);
                        temp.TeamSubLeaders.Add(newSubLeader);
                    }
                }
                if (blank.TeamCoach != null)
                {
                    temp.Coach = new Leader();
                    SetLeader(temp.Coach, blank.TeamCoach);
                }

                foreach (var athlete in blank.Athletes)
                {
                    if (athlete != null)
                    {
                        var newAthlete = temp.Athletes.GenerateNewAthlete();
                        _blankAthlete2DataAthlete.Add(athlete, newAthlete);

                        newAthlete.Name = athlete.Name;
                        newAthlete.Sid = athlete.Sid;
                        var category = _conf.AthleteCategories.Find((ele) => ele.Name == athlete.Category);
                        if (category == null)
                        {
                            return false;
                        }
                        newAthlete.Category = category;

                        if (_conf.CompetitionRankInfo.Enabled)
                        {
                            var rank = _conf.CompetitionRankInfo.Forced ?
                                    (rankTable.ContainsKey(newAthlete.Sid) ?
                                        rankTable[newAthlete.Sid] : _conf.CompetitionRankInfo.DefaultRank
                                    ) : _conf.CompetitionRankInfo.AthleteRanks.Find((ele) => ele.Name == athlete.Rank);
                            if (rank == null)
                            {
                                return false;
                            }
                            newAthlete.Rank = rank;
                        }
                    }
                }
                temp.Athletes.TidyUpCodes();

                foreach (var personalEntry in blank.PersonalEntries)
                {
                    var eventData = data.Events.Find((ele) => ele.Conf == personalEntry.Conf);
                    if (eventData == null)
                    {
                        return false;
                    }
                    foreach (var entryItem in personalEntry.Items)
                    {
                        var blankAthlete = entryItem.Value;
                        if (!_blankAthlete2DataAthlete.ContainsKey(blankAthlete))
                        {
                            return false;
                        }
                        _personalEntry[eventData].Add(new Tuple<Team, EntryItem>(temp, entryItem));
                    }
                }

                foreach (var teamworkEntry in blank.TeamworkEntries)
                {
                    var eventData = data.Events.Find((ele) => ele.Conf == teamworkEntry.Conf);
                    if (eventData == null)
                    {
                        return false;
                    }
                    foreach (var teamworkEntryItem in teamworkEntry.ItemLists)
                    {
                        if (!_teamworkEntry[eventData].ContainsKey(temp))
                        {
                            _teamworkEntry[eventData].Add(temp, new List<EntryItemList>());
                        }
                        _teamworkEntry[eventData][temp].Add(teamworkEntryItem);
                    }
                }

                data.Teams.Add(temp);
                return true;
            }

            private bool ArrangePersonalGroups(Event eventData)
            {
                if (eventData.Games.Count == 0)
                {
                    return false;
                }

                var firstGame = eventData.Games[0];
                var entries = _personalEntry[eventData];
                var participants = new List<Participant>();

                foreach (var entry in entries)
                {
                    List<Athlete> athletes = new List<Athlete>();
                    if (_blankAthlete2DataAthlete[entry.Item2.Value] == null)
                    {
                        return false;
                    }
                    athletes.Add(_blankAthlete2DataAthlete[entry.Item2.Value]);
                    Participant temp = new Participant(entry.Item1, athletes);
                    temp.BestGrade = entry.Item2.BestGrade;
                    participants.Add(temp);
                }

                var groups = GenerateGroups(firstGame.Conf.GameGroupInfo, participants);
                if (groups == null)
                {
                    return false;
                }
                firstGame.Groups = groups;
                return true;
            }

            private bool ArrangeTeamworkGroups(Event eventData)
            {
                if (eventData.Games.Count == 0)
                {
                    return false;
                }

                var firstGame = eventData.Games[0];
                var entries = _teamworkEntry[eventData];
                var participants = new List<Participant>();

                foreach (var entryList in entries)
                {
                    var team = entryList.Key;
                    if (entryList.Value.Count == 1)
                    {
                        List<Athlete> athletes = new List<Athlete>();
                        var entry = entryList.Value[0];
                        foreach (var item in entry.Items)
                        {
                            if (_blankAthlete2DataAthlete[item.Value] == null)
                            {
                                return false;
                            }
                            athletes.Add(_blankAthlete2DataAthlete[item.Value]);
                        }
                        Participant temp = new Participant(team, athletes);
                        temp.Name = team.Conf.ShortName;
                        temp.BestGrade = entry.BestGrade;
                        participants.Add(temp);
                    }
                    else
                    {
                        Int32 index = 0;
                        foreach (var entry in entryList.Value)
                        {
                            List<Athlete> athletes = new List<Athlete>();
                            foreach (var item in entry.Items)
                            {
                                if (_blankAthlete2DataAthlete[item.Value] == null)
                                {
                                    return false;
                                }
                                athletes.Add(_blankAthlete2DataAthlete[item.Value]);
                            }
                            Participant temp = new Participant(team, athletes);
                            temp.Name = team.Conf.ShortName + entry.Name;
                            temp.OrderInTeam = new SSUtils.Order(index++);
                            temp.BestGrade = entry.BestGrade;
                            participants.Add(temp);
                        }
                    }
                }

                var groups = GenerateGroups(firstGame.Conf.GameGroupInfo, participants);
                if (groups == null)
                {
                    return false;
                }
                firstGame.Groups = groups;
                return true;
            }

            private void SetLeader(Leader data, EntryBlank.Leader origin)
            {
                data.Sid = origin.Sid;
                data.Name = origin.Name;
                data.Telephone = origin.Telephone;
                data.EMail = origin.EMail;
            }

            private List<Group> GenerateGroups(GroupInfo conf, List<Participant> participantList)
            {
                List<List<Participant>> participantLists = new List<List<Participant>>();
                if (!conf.Enabled)
                {
                    var rnd = new Random();
                    participantLists.Add(participantList.OrderBy(item => rnd.Next()).ToList());
                }
                else
                {
                    Int32 groupNumber = (Int32)(participantList.Count / conf.NumberPerGroup.Maximum) + 1;
                    if (groupNumber == 0)
                    {
                        return new List<Group>();
                    }
                    for (Int32 i = 0; i != groupNumber; ++i)
                    {
                        participantLists.Add(new List<Participant>());
                    }

                    participantList.Sort((lhs, rhs) => CompareBestGrade(lhs.BestGrade, rhs.BestGrade));
                    if (participantList.FindAll((ele) => ele.BestGrade == TimeSpan.Zero).Count <= 1
                        || !_conf.AllRandomIfNoAllBestGrade)
                    {
                        ArrangeParticipantWithBestGrade(participantLists, participantList, groupNumber);
                    }
                    ArrangeParticipantWithoutBestGrade(participantLists, participantList, groupNumber, conf.NumberPerGroup);

                    foreach (var participants in participantLists)
                    {
                        List<Int32> order = _conf.NumberOfUseLines % 2 == 0
                    ? EvenLineNumberOrders[_conf.NumberOfUseLines] : GenerateOddLineNumberOder(_conf.NumberOfUseLines);
                        participantList.Sort((lhs, rhs) => CompareBestGrade(lhs.BestGrade, rhs.BestGrade));

                        var copy = new List<Participant>(participants.ToArray());
                        participants.Clear();
                        for (Int32 i = 0, j = (Int32)_conf.NumberOfUseLines; i != j; ++i)
                        {
                            participants.Add(null);
                        }
                        for (Int32 i = 0, j = copy.Count; i != j; ++i)
                        {
                            participants[order[i]] = copy[i];
                        }
                    }
                }
                
                return TransformParticipantListsToGroups(participantLists);
            }

            private void ArrangeParticipantWithBestGrade(List<List<Participant>> participantLists, List<Participant> participantList, Int32 groupNumber)
            {
                Int32 i = 0;
                if (groupNumber <= 3)
                {
                    for (Int32 j = -1, k = participantList.Count; i != k && participantList[i].BestGrade != TimeSpan.Zero; ++i, --j)
                    {
                        j = j == -1 ? groupNumber - 1 : j;
                        var thisParticipant = participantList[i];
                        participantLists[j].Add(thisParticipant);
                        participantList.Remove(thisParticipant);
                    }
                }
                else
                {
                    for (Int32 j = groupNumber - 4, k = participantList.Count; i != k && participantList[i].BestGrade != TimeSpan.Zero; ++i, --j)
                    {
                        j = j == (groupNumber - 4) ? groupNumber - 1 : j;
                        var thisParticipant = participantList[i];
                        participantLists[j].Add(thisParticipant);
                        participantList.Remove(thisParticipant);
                    }
                }
            }

            private void ArrangeParticipantWithoutBestGrade(List<List<Participant>> participantLists, List<Participant> participantList, Int32 groupNumber, SSUtils.NumberRange numberPerGroup)
            {
                var rnd = new Random();
                var shuffleParticipantList = participantList.OrderBy(item => rnd.Next()).ToList();
                var minimum = (numberPerGroup.Minimum == numberPerGroup.Maximum || numberPerGroup.Minimum < 3) ? 3 : numberPerGroup.Minimum;
                var maximum = numberPerGroup.Maximum;
                for (Int32 i = 0; i != groupNumber && shuffleParticipantList.Count != 0; ++i)
                {
                    while (participantLists[i].Count != minimum && shuffleParticipantList.Count != 0)
                    {
                        var thisParticipant = shuffleParticipantList[0];

                        if (participantLists[i].Find(ele => ele.ParticipantTeam == thisParticipant.ParticipantTeam) == null)
                        {
                            participantLists[i].Add(thisParticipant);
                            shuffleParticipantList.Remove(thisParticipant);
                        }
                        else
                        {
                            shuffleParticipantList.RemoveAt(0);
                            shuffleParticipantList.Add(thisParticipant);
                        }
                    }
                }

                if (shuffleParticipantList.Count != 0)
                {
                    var restNumbers = ArrangeRestNumber(groupNumber, shuffleParticipantList.Count, (Int32)(maximum - minimum));
                    for (Int32 i = 0, j = groupNumber - 1; i != j; ++i)
                    {
                        var counter = 0;
                        var timeCounter = 0;
                        while (counter != restNumbers[i])
                        {
                            var thisParticipant = shuffleParticipantList[0];

                            if (participantLists[i].Find(ele => ele.ParticipantTeam == thisParticipant.ParticipantTeam) == null
                                || timeCounter == MaxTryTime)
                            {
                                participantLists[i].Add(thisParticipant);
                                shuffleParticipantList.Remove(thisParticipant);
                                ++counter;
                                timeCounter = 0;
                            }
                            else
                            {
                                shuffleParticipantList.RemoveAt(0);
                                shuffleParticipantList.Add(thisParticipant);
                                ++timeCounter;
                            }
                        }
                    }
                    participantLists[groupNumber - 1].AddRange(shuffleParticipantList);
                }
            }

            private List<Int32> ArrangeRestNumber(Int32 groupNumber, Int32 number, Int32 diff)
            {
                List<Int32> ret = new List<Int32>();
                
                var rnd = new Random();
                do
                {
                    ret.Clear();
                    for (Int32 i = 0, j = groupNumber - 1; i != j; ++i)
                    {
                        ret.Add(rnd.Next(1, diff + 1));
                    }
                    var rest = number - ret.Sum();
                    if (rest > diff || rest <= 0)
                    {
                        continue;
                    }
                    ret.Add(rest);
                } while (ret.Count != groupNumber || ret.Sum() != number);
                ret.Sort();
                return ret;
            }

            private List<Int32> GenerateOddLineNumberOder(UInt32 number)
            {
                number += 1;
                List<Int32> orders = new List<Int32>();
                orders.Add((Int32)number / 2 - 1);
                var left = (Int32)number / 2 - 1;
                var right = (Int32)number / 2 + 1;
                for ( ; left != 0; --left, ++right)
                {
                    orders.Add(right - 1);
                    orders.Add(left - 1);
                }
                return orders;
            }

            private List<Group> TransformParticipantListsToGroups(List<List<Participant>> participantLists)
            {
                List<Group> ret = new List<Group>();

                foreach (var participantList in participantLists)
                {
                    var newGroup = new Group();
                    for (Int32 i = 0, j = (Int32)_conf.NumberOfUseLines; i != j; ++i)
                    {
                        newGroup.Lines.Add(new Line(_conf.UseLines[i], participantList[i]));
                    }
                    ret.Add(newGroup);
                }

                return ret;
            }

            private Int32 CompareBestGrade(TimeSpan lhs, TimeSpan rhs)
            {
                if (lhs == TimeSpan.Zero || rhs == TimeSpan.Zero)
                {
                    if (lhs == TimeSpan.Zero && rhs == TimeSpan.Zero)
                    {
                        return 0;
                    }
                    else if (lhs == TimeSpan.Zero)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                return lhs.CompareTo(rhs);
            }
        };
    };
};
