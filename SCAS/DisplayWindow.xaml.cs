using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace SCAS
{
    public class DisplayWindow : Window
    {
        const double NUAADisplayAreaWidth = 1258;
        const double NUAADisplayAreaHeight = 574;
        Screen _screen;

        public DisplayWindow(Screen screen, double width = NUAADisplayAreaWidth, double height = NUAADisplayAreaHeight)
        {
            _screen = screen;
            this.InitializeComponent();
            this.AttachDevTools();

            this.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
            this.Bounds = new Rect(new Size(width, height));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
