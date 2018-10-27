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
        }

        public ControlEventView(Event data, DisplayWindow display)
            : base(display)
        {
            Data = data;
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
                    if (exporter.ExportToFile(String.Format("{0}\\{1}决赛.html", result, Data.Conf.Name)))
                    {
                        await new dialogs.InformationDialog("导出成功").ShowDialog();
                    }
                    else
                    {
                        await new dialogs.InformationDialog("导出失败").ShowDialog();
                    }
                }
            };

            this.FindControl<Button>("RefreshPoint").Click += delegate
            {
                Data.RefreshPoints();
                new dialogs.InformationDialog("计算成功").ShowDialog();
            };
        }
    }
}
