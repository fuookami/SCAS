using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS.views
{
    public abstract class ControlDataViewBase : UserControl
    {
        DisplayWindow _display;

        public void SetDisplay(DisplayWindow display)
        {
            _display = display;
        }

        public abstract void Clear();

        protected void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
