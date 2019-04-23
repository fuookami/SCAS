using System;
using System.Collections.Generic;
using System.Linq;
using SSUtils;

namespace SCAS
{
    namespace CompetitionConfiguration
    {
        public partial class TeamworkInfo
        {
            private bool _beTeamwork;
            private bool _beMultiRank;
            private bool _needEveryPerson;
            private NumberRange _rangesOfTeam;

            private void SetIsTeamwork(bool value)
            {
                if (value != _beTeamwork)
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

            private void SetNeedEveryPerson(bool value)
            {
                if (value != _needEveryPerson)
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

            private void SetIsTeamwork()
            {
                if (!_beTeamwork)
                {
                    _beTeamwork = true;
                    SetNotNeedEveryPerson();
                }
            }

            private void SetIsNotTeamwork()
            {
                if (_beTeamwork)
                {
                    SetNotNeedEveryPerson();
                    _beTeamwork = false;
                    _beMultiRank = false;
                }
            }

            private void SetNeedEveryPerson()
            {
                if (!_needEveryPerson)
                {
                    SetIsTeamwork();
                    _needEveryPerson = true;
                    RangesOfCategories = new Dictionary<AthleteCategory, NumberRange>();
                    RangesOfTeam = new NumberRange();
                    SetNotInOrder();
                }
            }

            private void SetNotNeedEveryPerson()
            {
                if (_needEveryPerson) 
                {
                    SetNotInOrder();
                    _needEveryPerson = false;
                    RangesOfCategories = null;
                    RangesOfTeam = null;
                }
            }

            public void SetInOrder(List<AthleteCategory> order = null)
            {
                if (!_needEveryPerson)
                {
                    SetNeedEveryPerson();
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

            private Dictionary<AthleteCategory, UInt32> GetCounter()
            {
                Dictionary<AthleteCategory, UInt32> ret = new Dictionary<AthleteCategory, uint>();
                UInt32 sum = 0;
                foreach (var item in RangesOfCategories)
                {
                    if (item.Value.Minimum == 0 || item.Value.Minimum != item.Value.Maximum)
                    {
                        return null;
                    }

                    ret.Add(item.Key, item.Value.Minimum);
                    sum += item.Value.Minimum;
                }

                if (RangesOfTeam.Valid())
                {
                    if (RangesOfTeam.Minimum != RangesOfTeam.Maximum
                        || RangesOfTeam.Minimum != sum)
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

                return counters.Count != orderCounters.Count ? false 
                    : orderCounters.All(item => item.Value == counters[item.Key]);
            }
        };
    };
};
