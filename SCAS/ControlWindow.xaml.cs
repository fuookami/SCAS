using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace SCAS
{
    public class ControlWindow : Window
    {
        private delegate void OpenInitWindowDelegate();

        CompetitionData.Competition _data;

        public ControlWindow()
        {
            _data = null;
            this.Activated += OpenInitWindow;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static void OpenInitWindow(object src, EventArgs e)
        {
            ControlWindow self = (ControlWindow)src;
            self.Activated -= OpenInitWindow;
            var initWindow = new InitWindow();
            self.Hide();

            initWindow.Show();
            initWindow.InitComfirmed += new InitWindow.InitComfirmHandler(self.Init);
        }

        internal void Init(CompetitionData.Competition data, Screen controlScreen, Screen displayScreen)
        {
            _data = data;
            InitializeComponent();
            Show();

            var displayWindow = new DisplayWindow();
            displayWindow.Show();
        }
    }
}
