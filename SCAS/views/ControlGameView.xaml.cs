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

            public List<ControlGameViewLineItem.LineItem> CurrGroupLines
            {
                get;
            }

            public Model()
            {
                Groups = new List<GroupItem>();
                CurrGroupLines = new List<ControlGameViewLineItem.LineItem>();
            }
        }

        private Game _data;
        private Model _model;
        private Group _currGroup;
        private StackPanel _lineItemsBox;

        public ControlGameView()
        {
            _model = new Model();
            DataContext = _model;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            var groupSelect = this.FindControl<DropDown>("GroupSelect");
            _lineItemsBox = this.FindControl<StackPanel>("LineItemsBox");
            groupSelect.SelectionChanged += delegate
            {
                var item = (GroupItem)groupSelect.SelectedItem;
                if (_currGroup != item.Data)
                {
                    _currGroup = item.Data;
                    _model.CurrGroupLines.Clear();
                    foreach (var line in _currGroup.Lines)
                    {
                        _model.CurrGroupLines.Add(new ControlGameViewLineItem.LineItem(line));
                    }
                }

                _lineItemsBox.Children.Clear();
                Enumerable.Range(0, _model.CurrGroupLines.Count).Select(i => new ControlGameViewLineItem(_model.CurrGroupLines[i])).All(ele => 
                {
                    _lineItemsBox.Children.Add(ele);
                    return true;
                });
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
