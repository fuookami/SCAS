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

        private bool _flag;

        public InitWindow()
        {
            _flag = true;
            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.Closed += delegate
            {
                if (_flag)
                {
                    Application.Current.Exit();
                }
            };
        }

        internal void OpenControlAndDisplayWindow(CompetitionData.Competition data, Screen controlScreen, Screen displayScreen)
        {
            InitComfirmed(data, controlScreen, displayScreen);
            _flag = false;
            Close();
        }
    }
}
