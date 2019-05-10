using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using SCAS.CompetitionData;
using SCAS.CompetitionConfiguration;


namespace SCAS
{
    public class ControlWindow : Window
    {
        const double WinMaxWidth = 1600;
        const double WinMaxHeight = 900;

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

            public String Width
            {
                get
                {
                    return WidthValue.ToString();
                }
                set
                {
                    if (IsPositiveInteger(value, UInt32.MaxValue))
                    {
                        WidthValue = UInt32.Parse(value);
                    }
                }
            }

            public UInt32 WidthValue
            {
                get;
                internal set;
            }

            public String Height
            {
                get
                {
                    return HeightValue.ToString();
                }
                set
                {
                    if (IsPositiveInteger(value, UInt32.MaxValue))
                    {
                        HeightValue = UInt32.Parse(value);
                    }
                }
            }

            public UInt32 HeightValue
            {
                get;
                internal set;
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

            this.FindControl<Button>("SaveDataFile").Click += async delegate
            {
                if (_data != null)
                {
                    CompetitionData.Normalizer normalizer = new CompetitionData.Normalizer(_data);
                    var result = await new SaveFileDialog
                    {
                        Title = "选择保存路径",
                        Filters = GenerateFilters("比赛数据文件")
                    }.ShowAsync((Window)VisualRoot);
                    if (result != null && result.Length != 0)
                    {
                        if (normalizer.NormalizeToFile(result))
                        {
                            await new dialogs.InformationDialog("保存成功").ShowDialog(this);
                        }
                        else
                        {
                            await new dialogs.InformationDialog("保存失败").ShowDialog(this);
                        }
                    }
                }
            };

            this.FindControl<Button>("EndSession").Click += delegate
            {
                _display.SetEnd();
            };

            var sessionSelect = this.FindControl<ComboBox>("SessionSelect");
            var checkSelect = this.FindControl<ComboBox>("CheckSelect");
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
                _display.SetCheck(item != null ? item.ToString() : "");
            };

            this.FindControl<TextBox>("WidthInput").TextInput += delegate (object obj, TextInputEventArgs e)
            {
                NumberInputHandler((TextBox)obj, e, this._model.Width);
            };
            this.FindControl<TextBox>("HeightInput").TextInput += delegate (object obj, TextInputEventArgs e)
            {
                NumberInputHandler((TextBox)obj, e, this._model.Height);
            };
            this.FindControl<Button>("ResizeDisplay").Click += delegate
            {
                _display.Width = _model.WidthValue;
                _display.Height = _model.HeightValue;
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
            
            this.Width = width;
            this.Height = height;
            Position = new Avalonia.PixelPoint(controlScreen.Bounds.X + (int)(controlScreen.Bounds.Width - width) / 2, controlScreen.Bounds.Y + (int)(controlScreen.Bounds.Height - height) / 2);

            InitCompleted(data, _display);

            foreach (var gameList in data.Games)
            {
                _model.Sessions.Add(new SessionItem(gameList.Key));
            }

            _model.WidthValue = (UInt32)_display.Width;
            this.FindControl<TextBox>("WidthInput").Text = _model.Width;
            _model.HeightValue = (UInt32)_display.Height;
            this.FindControl<TextBox>("HeightInput").Text = _model.Height;
        }

        static private List<FileDialogFilter> GenerateFilters(String name)
        {
            return new List<FileDialogFilter>
            {
                new FileDialogFilter()
                {
                    Extensions = new List<String>
                    {
                        "xml"
                    },
                    Name = name
                }
            };
        }

        private static bool NumberInputHandler(TextBox src, TextInputEventArgs e, String origin, UInt32 maximum = UInt32.MaxValue)
        {
            if (IsPositiveInteger(src.Text, maximum))
            {
                return true;
            }
            else
            {
                src.Text = origin;
                return false;
            }
        }

        private static bool IsPositiveInteger(String src, UInt32 maximum)
        {
            UInt32 temp = 0;
            if (UInt32.TryParse(src, out temp))
            {
                return temp < maximum;
            }
            else
            {
                return false;
            }
        }
    }
}
