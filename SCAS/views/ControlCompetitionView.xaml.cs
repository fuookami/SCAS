using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlCompetitionView : ControlDataViewBase
    {
        Competition _data;

        public ControlCompetitionView(Competition data, DisplayWindow display)
            : base(display)
        {
            _data = data;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("SavePointResult").Click += async delegate
            {
                DocumentGenerator.PointResultExporter exporter = new DocumentGenerator.PointResultExporter(_data);
                var result = await new OpenFolderDialog
                {
                    Title = "选择保存路径"
                }.ShowAsync((Window)VisualRoot);
                if (result != null && result.Length != 0)
                {
                    if (exporter.Export(String.Format("{0}\\{1}总积分.xlsx", result, _data.Conf.Name)))
                    {
                        await new dialogs.InformationDialog("导出成功").ShowDialog();
                    }
                    else
                    {
                        await new dialogs.InformationDialog("导出失败").ShowDialog();
                    }
                }
            };
        }
    }
}
