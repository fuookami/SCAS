using System;
using System.Collections.Generic;
using System.Linq;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    namespace EntryBlank
    {
        public class Generator : ErrorStorer
        {
            private CompetitionInfo _conf;

            public CompetitionInfo Conf
            {
                get
                {
                    return _conf;
                }
                set
                {
                    _conf = value ?? throw new Exception("设置的要用于生成报名表的比赛信息是个无效值");
                    Result = null;
                }
            }

            public EntryBlank Result
            {
                get;
                private set;
            }

            public String TargetUrl
            {
                get;
                set;
            }

            public Generator(CompetitionInfo conf)
            {
                _conf = conf;
                Result = null;
            }

            public bool Generate(bool export = true)
            {
                return Generate(TargetUrl);
            }

            public bool Generate(String targetUrl, bool export = true)
            {
                if (Result == null)
                {
                    EntryBlank temp = new EntryBlank(_conf);

                    for (Int32 i = 0, j = (Int32)_conf.NumberOfSubLeader.Minimum; i != j; ++i)
                    {
                        temp.TeamSubLeader.Add(new Leader());
                    }
                    for (Int32 i = (Int32)_conf.NumberOfSubLeader.Minimum, j = (Int32)_conf.NumberOfSubLeader.Maximum; i != j; ++i)
                    {
                        temp.TeamSubLeader.Add(new Leader(true));
                    }
                    if (temp.TeamSubLeader.Count == 0)
                    {
                        temp.TeamSubLeader.Add(new Leader(true));
                    }
                    temp.TeamCoach.Optional = _conf.CoachOptional;

                    foreach (var eventConf in _conf.EventInfos)
                    {
                        if (!eventConf.EventTeamworkInfo.BeTeamwork)
                        {
                            var entry = GeneratePersonalEntry(eventConf);
                            if (entry == null)
                            {
                                return false;
                            }
                            temp.PersonalEntries.Add(entry);
                        }
                        else
                        {
                            var entry = GenerateTeamworkEntry(eventConf);
                            if (entry == null)
                            {
                                return false;
                            }
                            temp.TeamworkEntries.Add(entry);
                        }
                    }

                    Result = temp;
                }

                if (!export)
                {
                    return true;
                }

                Exporter exporter = new Exporter(Result, targetUrl);
                if (!exporter.Export())
                {
                    RefreshError(exporter.LastErrorCode, exporter.LastError);
                    return false;
                }

                return true;
            }

            private PersonalEntry GeneratePersonalEntry(EventInfo eventConf)
            {
                PersonalEntry entry = new PersonalEntry(eventConf);

                UInt32 number = eventConf.EventParticipantValidator.NumberPerTeam.Maximum != SSUtils.NumberRange.NoLimit ?
                                eventConf.EventParticipantValidator.NumberPerTeam.Maximum : 5;

                List<String> categoryNames = new List<String>();
                List<String> sidKeys = new List<String>();
                foreach (var category in entry.Conf.EventParticipantValidator.Categories)
                {
                    categoryNames.Add(category.Name);
                    sidKeys.Add(category.SidKey);
                }
                String categoryNamesString = String.Join("/", categoryNames.Distinct().ToList());
                String sidKeysString = String.Join("/", sidKeys.Distinct().ToList());

                for (UInt32 i = 0; i != number; ++i)
                {
                    Athlete athlete = new Athlete(false);
                    if (eventConf.EventParticipantValidator.NumberPerTeam.Minimum != SSUtils.NumberRange.NoLimit
                        && i >= eventConf.EventParticipantValidator.NumberPerTeam.Minimum)
                    {
                        athlete.Optional = true;
                    }

                    entry.Items.Add(new EntryItem {
                        Key = athlete.Optional ? (categoryNamesString + "(可选)") : categoryNamesString,
                        SidKey = sidKeysString,
                        Value = athlete
                    });
                }

                return entry;
            }

            private TeamworkEntry GenerateTeamworkEntry(EventInfo eventConf)
            {
                TeamworkEntry entry = new TeamworkEntry(eventConf);

                List<EntryItem> items = new List<EntryItem>();
                if (eventConf.EventTeamworkInfo.BeInOrder)
                {
                    // the minimum of each category equal to the maximum
                    // the number of each category equal to the number in the order

                    UInt32 order = 1;
                    foreach (var category in eventConf.EventTeamworkInfo.Order)
                    {
                        items.Add(new EntryItem
                        {
                            Key = String.Format("{0}-{1}", order, category.Name),
                            SidKey = category.SidKey,
                            Value = new Athlete(false)
                        });
                        ++order;
                    }
                }
                else
                {
                    foreach (var categoryRange in eventConf.EventTeamworkInfo.RangesOfCategories)
                    {
                        for (UInt32 i = 0, j = categoryRange.Value.Minimum; i != j; ++i)
                        {
                            items.Add(new EntryItem
                            {
                                Key = categoryRange.Key.Name,
                                SidKey = categoryRange.Key.SidKey,
                                Value = new Athlete(false)
                            });
                        }
                    }

                    if (eventConf.EventTeamworkInfo.RangesOfTeam.Maximum == SSUtils.NumberRange.NoLimit)
                    {
                        foreach (var categoryRange in eventConf.EventTeamworkInfo.RangesOfCategories)
                        {
                            for (UInt32 i = categoryRange.Value.Minimum, j = categoryRange.Value.Maximum; i != j; ++i)
                            {
                                items.Add(new EntryItem
                                {
                                    Key = categoryRange.Key.Name + "(可选)",
                                    SidKey = categoryRange.Key.SidKey,
                                    Value = new Athlete(true)
                                });
                            }
                        }
                    }
                    else
                    {
                        UInt32 optionalSum = 0;
                        AthleteCategoryList noLimitCategories = new AthleteCategoryList();
                        foreach (var categoryRange in eventConf.EventTeamworkInfo.RangesOfCategories)
                        {
                            if (categoryRange.Value.Maximum == SSUtils.NumberRange.NoLimit)
                            {
                                noLimitCategories.Add(categoryRange.Key);
                            }
                            else
                            {
                                optionalSum += categoryRange.Value.Maximum - categoryRange.Value.Minimum;
                            }
                        }

                        UInt32 optionalNum = eventConf.EventTeamworkInfo.RangesOfTeam.Maximum - eventConf.EventTeamworkInfo.RangesOfTeam.Minimum;
                        if (optionalSum > optionalNum)
                        {
                            // the sum of minimum of categories smaller than or equal the minimum of team
                            // the difference between the minimum and the maximum of each category and the difference between the minimum and the maximum of team is the same
                            // there is no category in no limit categories

                            List<String> categoryNames = new List<String>();
                            List<String> sidKeys = new List<String>();
                            foreach (var categoryRange in eventConf.EventTeamworkInfo.RangesOfCategories)
                            {
                                if (categoryRange.Value.Minimum != categoryRange.Value.Maximum)
                                {
                                    categoryNames.Add(categoryRange.Key.Name);
                                    sidKeys.Add(categoryRange.Key.SidKey);
                                }
                            }
                            String categoryNamesString = String.Join("/", categoryNames.Distinct().ToList());
                            String sidKeysString = String.Join("/", sidKeys.Distinct().ToList());

                            for (UInt32 i = (UInt32)items.Count, j = eventConf.EventTeamworkInfo.RangesOfTeam.Maximum; i != j; ++i)
                            {
                                Athlete athlete = new Athlete(false);
                                if (i >= eventConf.EventTeamworkInfo.RangesOfTeam.Minimum)
                                {
                                    athlete.Optional = true;
                                }

                                items.Add(new EntryItem
                                {
                                    Key = athlete.Optional ? (categoryNamesString + "(可选)") : categoryNamesString,
                                    SidKey = sidKeysString,
                                    Value = athlete
                                });
                            }
                        }
                        else if (optionalSum == optionalNum)
                        {
                            // the sum of minimum of categories equal to the minimum of team
                            // the sum of maximum of categories equal to the maximum of team
                            // there is no category in no limit categories

                            foreach (var categoryRange in eventConf.EventTeamworkInfo.RangesOfCategories)
                            {
                                for (UInt32 i = categoryRange.Value.Minimum, j = categoryRange.Value.Maximum; i != j; ++i)
                                {
                                    items.Add(new EntryItem
                                    {
                                        Key = categoryRange.Key.Name + "(可选)",
                                        SidKey = categoryRange.Key.SidKey,
                                        Value = new Athlete(true)
                                    });
                                }
                            }
                        }
                        else
                        {
                            // the sum of minimum of categories equal to the minimum of team
                            // the sum of maximum of categories smaller than the maximum of team
                            // there is category in no limit categories

                            foreach (var categoryRange in eventConf.EventTeamworkInfo.RangesOfCategories)
                            {
                                if (categoryRange.Value.Maximum != SSUtils.NumberRange.NoLimit)
                                {
                                    continue;
                                }

                                for (UInt32 i = categoryRange.Value.Minimum, j = categoryRange.Value.Maximum; i != j; ++i)
                                {
                                    items.Add(new EntryItem
                                    {
                                        Key = categoryRange.Key.Name + "(可选)",
                                        SidKey = categoryRange.Key.SidKey,
                                        Value = new Athlete(true)
                                    });
                                }
                            }

                            List<String> categoryNames = new List<String>();
                            List<String> sidKeys = new List<String>();
                            foreach (var category in noLimitCategories)
                            {
                                categoryNames.Add(category.Name);
                                sidKeys.Add(category.SidKey);
                            }
                            String categoryNamesString = String.Join("/", categoryNames.Distinct().ToList());
                            String sidKeysString = String.Join("/", sidKeys.Distinct().ToList());

                            for (UInt32 i = (UInt32)items.Count, j = eventConf.EventTeamworkInfo.RangesOfTeam.Maximum; i != j; ++i)
                            {
                                items.Add(new EntryItem
                                {
                                    Key = categoryNamesString + "(可选)",
                                    SidKey = sidKeysString,
                                    Value = new Athlete(true)
                                });
                            }
                        }
                    }
                }

                UInt32 number = eventConf.EventParticipantValidator.NumberPerTeam.Maximum != SSUtils.NumberRange.NoLimit ?
                    eventConf.EventParticipantValidator.NumberPerTeam.Maximum : 1;
                for (UInt32 i = 0, j = number; i != j; ++i)
                {
                    EntryItemList itemList = new EntryItemList
                    {
                        Name = String.Format("{0}队", i + 1),
                        Items = new List<EntryItem>(items),
                        Optional = false
                    };
                    if (eventConf.EventParticipantValidator.NumberPerTeam.Minimum != SSUtils.NumberRange.NoLimit
                        && i >= eventConf.EventParticipantValidator.NumberPerTeam.Minimum)
                    {
                        itemList.Optional = true;
                        itemList.Name += "(可选)";
                    }

                    entry.ItemLists.Add(itemList);
                }

                return entry;
            }
        }
    };
};
