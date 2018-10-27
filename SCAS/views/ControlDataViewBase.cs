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

        public ControlDataViewBase(DisplayWindow display)
        {
            Display = display;
        }
    }
}
