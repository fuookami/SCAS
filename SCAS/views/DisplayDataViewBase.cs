using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS.views
{
    public abstract class DisplayDataViewBase : UserControl
    {
        public abstract void Clear();

        protected void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
