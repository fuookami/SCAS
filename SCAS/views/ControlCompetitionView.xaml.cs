using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS.views
{
    public class ControlCompetitionView : UserControl
    {
        public ControlCompetitionView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
