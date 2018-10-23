using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlGameView : ControlDataViewBase
    {
        public Game Data
        {
            get;
            private set;
        }

        public ControlGameView()
        {
            this.InitializeComponent();
        }

        public override void Clear()
        {

        }

        public void Refresh(Game data)
        {
            Data = data;

            //TODO refresh data binded
        }
    }
}
