using Avalonia;
using Avalonia.Markup.Xaml;

namespace SCAS
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
