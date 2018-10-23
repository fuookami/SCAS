using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlEventView : ControlDataViewBase
    {
        public Event Data
        {
            get;
            private set;
        }

        public ControlEventView()
        {
            this.InitializeComponent();
        }

        public override void Clear()
        {
            
        }

        public void Refresh(Event data)
        {
            Data = data;

            //TODO refresh data binded
        }
    }
}
