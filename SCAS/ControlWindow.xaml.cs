using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using SCAS.CompetitionData;
using SCAS.CompetitionConfiguration;


namespace SCAS
{
    public class ControlWindow : Window
    {
        const double WinMaxWidth = 1280;
        const double WinMaxHeight = 720;

        public delegate void InitCompletedHandler(CompetitionData.Competition data, DisplayWindow display);
        public event InitCompletedHandler InitCompleted;

        public class SessionItem
        {
            public Session Data
            {
                get;
            }

            public SessionItem(Session session)
            {
                Data = session;
            }

            public override String ToString()
            {
                return Data.FullName;
            }
        };

        public class GameItem
        {
            public Game Data
            {
                get;
            }

            public GameItem(Game game = null)
            {
                Data = game;
            }

            public override String ToString()
            {
                return Data == null ? "已结束" : Data.Conf.Name;
            }
        };

        public class Model
        {
            public List<SessionItem> Sessions
            {
                get;
            }

            public List<GameItem> Games
            {
                get;
            }

            public Model()
            {
                Sessions = new List<SessionItem>();
                Games = new List<GameItem>();
            }
        }

        private Model _model;
        private Competition _data;
        private DisplayWindow _display;
        private Session _currSession;

        public ControlWindow()
        {
            this.Activated += OpenInitWindow;
            _model = new Model();
            DataContext = _model;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("SaveDataFile").Click += delegate
            {
                //TODO
            };

            this.FindControl<Button>("EndSession").Click += delegate
            {
                _display.SetEnd();
            };

            var sessionSelect = this.FindControl<DropDown>("SessionSelect");
            var checkSelect = this.FindControl<DropDown>("CheckSelect");
            sessionSelect.SelectionChanged += delegate
            {
                var item = (SessionItem)sessionSelect.SelectedItem;
                if (_currSession != item.Data)
                {
                    _currSession = item.Data;
                    _model.Games.Clear();
                    foreach (var game in _data.Games[_currSession])
                    {
                        _model.Games.Add(new GameItem(game));
                    }
                    _model.Games.Add(new GameItem());
                    checkSelect.SelectedIndex = -1;
                }
                _display.SetSession(_currSession);
            };
            checkSelect.SelectionChanged += delegate
            {
                var item = (GameItem)checkSelect.SelectedItem;
                _display.SetCheck(item.ToString());
            };

            this.Closed += delegate
            {
                _display.Close();
                Application.Current.Exit();
            };
        }

        private static void OpenInitWindow(object src, EventArgs e)
        {
            ControlWindow self = (ControlWindow)src;
            self.Activated -= OpenInitWindow;
            var initWindow = new InitWindow();
            self.Hide();

            initWindow.Show();
            initWindow.InitComfirmed += new InitWindow.InitComfirmHandler(self.Init);
        }

        internal void Init(Competition data, Screen controlScreen, Screen displayScreen)
        {
            _data = data;
            InitializeComponent();
            Show();

            _display = new DisplayWindow(displayScreen, data);
            _display.Show();

            double width = controlScreen.Bounds.Width <= WinMaxWidth ? controlScreen.Bounds.Width : WinMaxWidth;
            double height = controlScreen.Bounds.Height <= WinMaxHeight ? controlScreen.Bounds.Height : WinMaxHeight;

            Position = new Avalonia.Point(controlScreen.Bounds.X + (controlScreen.Bounds.Width - width) / 2, controlScreen.Bounds.Y + (controlScreen.Bounds.Height - height) / 2);
            Bounds = new Rect(new Size(width, height));

            InitCompleted(data, _display);

            foreach (var gameList in data.Games)
            {
                _model.Sessions.Add(new SessionItem(gameList.Key));
            }
        }
    }
}
