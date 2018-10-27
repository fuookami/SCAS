using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SCAS.dialogs
{
    public class InformationDialog : Window
    {
        public class Model
        {
            public String Information
            {
                get;
                internal set;
            }
        }

        public InformationDialog(String information, String title = "信息")
        {
            DataContext = new Model
            {
                Information = information,
            };
            this.InitializeComponent();
            this.AttachDevTools();
            this.Title = title;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("Comfirm").Click += delegate
            {
                this.Close();
            };
        }
    }
}
