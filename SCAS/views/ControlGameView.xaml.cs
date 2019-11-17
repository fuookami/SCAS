using System;
using System.Collections.Generic;
using System.Linq;
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

        public class Model
        {
            public List<GroupItem> Groups
            {
                get;
            }

            public Model()
            {
                Groups = new List<GroupItem>();
            }
        }

        private Game _data;
        private Model _model;
        private DropDown _groupSelect;
        private GroupItem _currGroupItem;
        private Group _currGroup;
        private StackPanel _lineItemsBox;
        private List<ControlGameViewLineItem> _lineItems;

        public ControlGameView(Game data, DisplayWindow display)
            : base(display)
        {
            _model = new Model();
            _data = data;
            UInt32 index = 1;
            foreach (var group in _data.Groups)
            {
                _model.Groups.Add(new GroupItem(index, group));
                ++index;
            }
            DataContext = _model;
            _lineItems = new List<ControlGameViewLineItem>();
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _groupSelect = this.FindControl<DropDown>("GroupSelect");
            _lineItemsBox = this.FindControl<StackPanel>("LineItemsBox");
            _groupSelect.SelectionChanged += delegate
            {
                var item = (GroupItem)_groupSelect.SelectedItem;
                if (item != null && _currGroup != item.Data)
                {
                    _currGroupItem = item;
                    _currGroup = item.Data;
                    _lineItems.Clear();
                    _lineItemsBox.Children.Clear();
                    foreach (var line in _currGroup.Lines)
                    {
                        var newItem = new ControlGameViewLineItem(new ControlGameViewLineItem.LineItem(line));
                        _lineItems.Add(newItem);
                        _lineItemsBox.Children.Add(newItem);
                    }
                }
            };

            this.FindControl<Button>("DisplayGroup").Click += delegate
            {
                if (_currGroup != null)
                {
                    Display.SetGroup(_data.Conf.Name + _currGroupItem.ToString(), _currGroup);
                }
            };

            this.FindControl<Button>("SaveGroup").Click += delegate
            {
                foreach (var item in _lineItems)
                {
                    var model = item.Model;
                    if (model.Data.LineParticipant != null)
                    {
                        model.Data.ParticipantGrade.Set(model.ToGradeTuple());
                    }
                }
                Display.RefreshGroup();
            };
        }
    }
}
