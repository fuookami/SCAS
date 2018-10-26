using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class DisplayGameView : DisplayDataViewBase
    {
        const double TitleHeight = 50;
        private Group _currGroup;
        private StackPanel _lineItemsBox;

        public DisplayGameView()
        {
            DataContext = TitleHeight;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _lineItemsBox = this.FindControl<StackPanel>("LineItemsBox");
        }

        public override void Clear()
        {
            _lineItemsBox.Children.Clear();
        }

        public void Refresh(Group group, double windowHeight)
        {
            if (_currGroup != group)
            {
                Clear();
                _currGroup = group;
                var boxHeight = windowHeight - TitleHeight;
                var height = boxHeight / _currGroup.Lines.Count;
                foreach (var line in _currGroup.Lines)
                {
                    _lineItemsBox.Children.Add(new DisplayGameViewLineItem(new DisplayGameViewLineItem.LineItem(line), height));
                }
            }
        }
    }
}
