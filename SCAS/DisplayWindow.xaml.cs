using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using SCAS.views;
using SCAS.CompetitionData;
using SCAS.CompetitionConfiguration;

namespace SCAS
{
    public class DisplayWindow : Window
    {
        const double NUAADisplayAreaWidth = 1258;
        const double NUAADisplayAreaHeight = 674;
        Screen _screen;
        Competition _data;
        Session _session;
        DisplayGameView _gameView;
        DisplayGameViewWithRank _gameViewWithRank;
        DisplayEventView _eventView;
        List<DisplayDataViewBase> _views;

        public DisplayWindow(Screen screen, Competition data, double width = NUAADisplayAreaWidth, double height = NUAADisplayAreaHeight)
        {
            _screen = screen;
            _data = data;
            this.InitializeComponent();
            this.AttachDevTools();

            this.Position = new Avalonia.PixelPoint(screen.Bounds.X, screen.Bounds.Y);
            this.Bounds = new Rect(new Size(width, height));

            this.FindControl<TextBlock>("Title").Text = data.Conf.Name;
            this.FindControl<TextBlock>("SubTitle").Text = data.Conf.SubName;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _gameView = this.FindControl<DisplayGameView>("GameView");
            _gameViewWithRank = this.FindControl<DisplayGameViewWithRank>("GameViewWithRank");
            _eventView = this.FindControl<DisplayEventView>("EventView");
            _views = new List<DisplayDataViewBase>();
            _views.Add(_gameView);
            _views.Add(_eventView);
            foreach (var view in _views)
            {
                view.IsVisible = false;
            }
        }

        public void SetSession(Session session)
        {
            _session = session;
            var fieldInfo = _data.FieldInfos[session];

            this.FindControl<TextBlock>("SessionTitle").Text = _session.FullName;
            RefreshField(fieldInfo);
        }

        public void RefreshField()
        {
            if (_session != null)
            {
                RefreshField(_data.FieldInfos[_session]);
            }
        }

        public void SetCheck(String checkName)
        {
            this.FindControl<TextBlock>("Check").Text = checkName;
        }

        public void SetGroup(String gameName, Group group)
        {
            this.FindControl<TextBlock>("Game").Text = gameName;
            foreach (var view in _views)
            {
                view.Clear();
                view.IsVisible = false;
            }
            if (_data.Conf.CompetitionRankInfo.Enabled)
            {
                _gameViewWithRank.IsVisible = true;
                _gameViewWithRank.Refresh(group, this.Height);
            }
            else
            {
                _gameView.IsVisible = true;
                _gameView.Refresh(group, this.Height);
            }
        }

        public void RefreshGroup()
        {
            if (_data.Conf.CompetitionRankInfo.Enabled)
            {
                _gameViewWithRank.Refresh();
            }
            else
            {
                _gameView.Refresh();
            }
        }

        public void SetEnd()
        {
            foreach (var view in _views)
            {
                view.Clear();
                view.IsVisible = false;
            }

            this.FindControl<TextBlock>("Game").Text = "已完赛";
            this.FindControl<TextBlock>("Check").Text = "已结束";
        }

        private void RefreshField(FieldInfo fieldInfo)
        {
            this.FindControl<TextBlock>("IndoorTemperature").Text = String.Format("{0:F1}℃", fieldInfo.IndoorTemperature);
            this.FindControl<TextBlock>("WaterTemperature").Text = String.Format("{0:F1}℃", fieldInfo.WaterTemperature);
            this.FindControl<TextBlock>("ResidualChlorine").Text = String.Format("{0:F1}mg/L", fieldInfo.ResidualChlorine);
            this.FindControl<TextBlock>("PHValue").Text = String.Format("{0:F1}", fieldInfo.PHValue);
        }
    }
}
