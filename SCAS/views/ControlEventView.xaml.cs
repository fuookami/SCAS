using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlEventView : ControlDataViewBase
    {
        public Event Data
        {
            get;
            private set;
        }

        public ControlEventView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("SaveEventResult").Click += async delegate
            {
                DocumentGenerator.EventGradeExporter exporter = new DocumentGenerator.EventGradeExporter(Data);
                var result = await new OpenFolderDialog
                {
                    Title = "选择保存路径"
                }.ShowAsync((Window)VisualRoot);
                if (result != null && result.Length != 0)
                {
                    exporter.ExportToFile(String.Format("{0}\\{1}决赛.html", result, Data.Conf.Name));
                }
            };
        }

        public override void Clear()
        {
            
        }

        public void Refresh(Event data)
        {
            Data = data;
        }
    }
}
