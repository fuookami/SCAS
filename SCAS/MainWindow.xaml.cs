using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            DisplayWindow _display = new DisplayWindow();
            _display.Show();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
