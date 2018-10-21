using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS
{
    public class DisplayWindow : Window
    {
        public DisplayWindow()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
