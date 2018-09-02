using System;
using System.Collections.Generic;
using SSUtils;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public class TeamworkInfo
        {
            private bool _beTeamwork;
            private bool _beMultiRank;
            private bool _needEveryPerson;

            private NumberRange _rangesOfTeam;

            public bool BeTeamwork
            {
                get
                {
                    return _beTeamwork;
                }
                set
                {
                    if (_beTeamwork != value)
                    {
                        if (value)
                        {
                            SetIsTeamwork();
                        }
                        else
                        {
                            SetIsNotTeamwork();
                        }
                    }
                }
            }

            public bool BeMultiRank
            {
                get
                {
                    return _beMultiRank;
                }
                set
                {
                    if (BeTeamwork)
                    {
                        _beMultiRank = value;
                    }
                }
            }

            public bool NeedEveryPerson
            {
                get
                {
                    return _needEveryPerson;
                }
                set
                {
                    if (_needEveryPerson != value)
                    {
                        if (value)
                        {
                            SetNeedEveryPerson();
                        }
                        else
                        {
                            SetNotNeedEveryPerson();
                        }
                    }
                }
            }

            public Dictionary<AthleteCategory, NumberRange> RangesOfCategories
            {
                get;
                private set;
            }

            public NumberRange RangesOfTeam
            {
                get
                {
                    return _rangesOfTeam;
                }
                set
                {
                    if (_needEveryPerson)
                    {
                        _rangesOfTeam = value ?? throw new Exception("设置的队伍人数是个无效值");
                    }
                }
            }

            public Boolean BeInOrder
            {
                get;
                private set;
            }

            public List<AthleteCategory> Order
            {
                get;
                private set;
            }

            public TeamworkInfo()
            {
                SetIsNotTeamwork();
            }

            public void SetIsTeamwork()
            {
                if (!_beTeamwork)
                {
                    _beTeamwork = true;
                }
            }

            public void SetIsNotTeamwork()
            {
                SetNotNeedEveryPerson();

                _beTeamwork = false;
                _beMultiRank = false;
            }

            public void SetNeedEveryPerson()
            {
                if (!_needEveryPerson)
                {
                    SetIsTeamwork();
                    _needEveryPerson = true;

                    RangesOfCategories = new Dictionary<AthleteCategory, NumberRange>();
                    RangesOfTeam = new NumberRange();

                    BeInOrder = false;
                    Order = null;
                }
            }

            public void SetNotNeedEveryPerson()
            {
                SetNotInOrder();

                _needEveryPerson = false;

                RangesOfCategories = null;
                RangesOfTeam = null;
            }

            public void SetInOrder(List<AthleteCategory> order = null)
            {
                if (!_needEveryPerson)
                {
                    return;
                }
                
                if (SetOrder(order))
                {
                    BeInOrder = true;
                }
            }

            public void SetNotInOrder()
            {
                BeInOrder = false;
                Order = null;
            }

            private Dictionary<AthleteCategory, UInt32> GetCounter()
            {
                Dictionary<AthleteCategory, UInt32> ret = new Dictionary<AthleteCategory, uint>();
                UInt32 sum = 0;
                foreach (var item in RangesOfCategories)
                {
                    if (item.Value.Minimun == 0 || item.Value.Minimun != item.Value.Maximun)
                    {
                        return null;
                    }

                    ret.Add(item.Key, item.Value.Minimun);
                    sum += item.Value.Minimun;
                }

                if (RangesOfTeam.Valid())
                {
                    if (RangesOfTeam.Minimun != RangesOfTeam.Maximun)
                    {
                        return null;
                    }
                    if (RangesOfTeam.Minimun != sum)
                    {
                        return null;
                    }
                }

                return ret;
            }

            static private List<AthleteCategory> GenerateDefaultOrder(Dictionary<AthleteCategory, UInt32> counters)
            {
                List<AthleteCategory> order = new List<AthleteCategory>();
                foreach (var counter in counters)
                {
                    for (UInt32 i = 0; i != counter.Value; ++i)
                    {
                        order.Add(counter.Key);
                    }
                }
                order.Sort();
                return order;
            }

            static private bool OrderCanBeSet(List<AthleteCategory> order, Dictionary<AthleteCategory, UInt32> counters)
            {
                Dictionary<AthleteCategory, UInt32> orderCounters = new Dictionary<AthleteCategory, UInt32>();
                foreach (var category in order)
                {
                    if (!orderCounters.ContainsKey(category))
                    {
                        orderCounters.Add(category, 0);
                    }
                    orderCounters[category] += 1;
                }

                if (counters.Count != orderCounters.Count)
                {
                    return false;
                }

                foreach (var item in orderCounters)
                {
                    if (item.Value != counters[item.Key])
                    {
                        return false;
                    }
                }

                return true;
            }

            private bool SetOrder(List<AthleteCategory> order = null)
            {
                var counter = GetCounter();
                if (counter == null)
                {
                    return false;
                }

                if (order == null)
                {
                    Order = GenerateDefaultOrder(counter);
                    return true;
                }
                else if (OrderCanBeSet(order, counter))
                {
                    Order = order;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        };
    };
};
