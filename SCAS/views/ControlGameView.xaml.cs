using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SSUtils;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlGameView : ControlDataViewBase
    {
        public class GroupItem
        {
            private UInt32 _index;

            public Group Data
            {
                get;
            }
            
            public GroupItem(UInt32 index, Group data)
            {
                _index = index;
                Data = data;
            }

            public override String ToString()
            {
                return String.Format("第{0}组", ChineseNumber.ToChineseNumber(_index));
            }
        }

        public class LineItem
        {
            public Line Data
            {
                get;
            }

            public String Line
            {
                get
                {
                    return Data.Order.Value.ToString();
                }
            }

            public String Name
            {
                get
                {
                    return Data.LineParticipant == null ? "" 
                        : Data.LineParticipant.Athletes.Count == 1 ? Data.LineParticipant.Athletes[0].Name : Data.LineParticipant.Name;
                }
            }

            public String Team
            {
                get
                {
                    return Data.LineParticipant == null ? ""
                        : Data.LineParticipant.ParticipantTeam.Conf.ShortName;
                }
            }

            public String Min
            {
                get;
                set;
            }

            public String Sec
            {
                get;
                set;
            }

            public String HMillSec
            {
                get;
                set;
            }

            public LineItem(Line data)
            {
                Data = data;
            }
        }

        public class Model
        {
            public List<GroupItem> Groups
            {
                get;
            }

            public List<LineItem> CurrGroupLines
            {
                get;
            }

            public Model()
            {
                Groups = new List<GroupItem>();
                CurrGroupLines = new List<LineItem>();
            }
        }

        private Game _data;
        private Model _model;
        private Group _currGroup;

        public ControlGameView()
        {
            this.InitializeComponent();
            _model = new Model();
            DataContext = _model;

            var groupSelect = this.FindControl<DropDown>("GroupSelect");
            groupSelect.SelectionChanged += delegate
            {
                //TODO Binding: Error in binding to "Avalonia.Controls.TreeView"."Items": "Could not convert 'SCAS.ControlWindow+Model' to 'IEnumerable'."
                //显示不出来
                var item = (GroupItem)groupSelect.SelectedItem;
                if (_currGroup != item.Data)
                {
                    _currGroup = item.Data;
                    _model.CurrGroupLines.Clear();
                    foreach (var line in _currGroup.Lines)
                    {
                        _model.CurrGroupLines.Add(new LineItem(line));
                    }
                }
            };
        }

        public override void Clear()
        {
            _model.Groups.Clear();
            _model.CurrGroupLines.Clear();
        }

        public void Refresh(Game data)
        {
            _data = data;

            Clear();
            UInt32 index = 1;
            foreach (var group in _data.Groups)
            {
                _model.Groups.Add(new GroupItem(index, group));
                ++index;
            }
        }
    }
}
