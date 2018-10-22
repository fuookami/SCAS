using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace SCAS
{
    public class InitWindow : Window
    {
        public delegate void InitComfirmHandler(CompetitionData.Competition data, Screen controlScreen, Screen displayScreen);
        public event InitComfirmHandler InitComfirmed;

        public InitWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        internal void OpenControlAndDisplayWindow(CompetitionData.Competition data, Screen controlScreen, Screen displayScreen)
        {
            InitComfirmed(data, controlScreen, displayScreen);
            Close();
        }
    }
}
