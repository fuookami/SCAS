using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace SCAS
{
    public class ControlWindow : Window
    {
        const double WinMaxWidth = 1280;
        const double WinMaxHeight = 720;

        public delegate void InitCompletedHandler(CompetitionData.Competition data, DisplayWindow display);
        public event InitCompletedHandler InitCompleted;

        public ControlWindow()
        {
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
            InitializeComponent();
            Show();

            DisplayWindow display = new DisplayWindow(displayScreen);
            display.Show();

            double width = controlScreen.Bounds.Width <= WinMaxWidth ? controlScreen.Bounds.Width : WinMaxWidth;
            double height = controlScreen.Bounds.Height <= WinMaxHeight ? controlScreen.Bounds.Height : WinMaxHeight;

            Position = new Point(controlScreen.Bounds.X + (controlScreen.Bounds.Width - width) / 2, controlScreen.Bounds.Y + (controlScreen.Bounds.Height - height) / 2);
            Bounds = new Rect(new Size(width, height));

            InitCompleted(data, display);
        }
    }
}
