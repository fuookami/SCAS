using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS.views
{
    public abstract class ControlDataViewBase : UserControl
    {
        protected DisplayWindow Display
        {
            get;
            private set;
        }

        public void SetDisplay(DisplayWindow display)
        {
            Display = display;
        }

        public abstract void Clear();

        protected void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
