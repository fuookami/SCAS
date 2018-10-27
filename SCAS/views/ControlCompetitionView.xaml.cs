using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS.views
{
    public class ControlCompetitionView : ControlDataViewBase
    {
        public ControlCompetitionView(DisplayWindow display)
            : base(display)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
