using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;

namespace SCAS.views
{
    public class InitView : UserControl
    {
        public class InitViewModel
        {
            public Dictionary<String, Screen> Name2Screen
            {
                get;
            }
            
            public List<String> Screens
            {
                get;
            }

            public InitViewModel()
            {
                Name2Screen = new Dictionary<String, Screen>();
                Screens = new List<String>();
            }
        }

        InitViewModel _model;
        UInt32 _counter;
        CompetitionData.Competition _result;

        public InitView()
        {
            _model = new InitViewModel();
            _counter = 0;
            _result = null;
            InitializeComponent();
            DataContext = _model;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("SelectConfFile").Click += async delegate
            {
                var result = await new OpenFileDialog()
                {
                    Title = "选择比赛设置文件",
                    Filters = GenerateFilters("配置文件")
                }.ShowAsync((Window)VisualRoot);
                if (result != null && result.Length != 0 && result[0].Length != 0)
                {
                    this.FindControl<TextBox>("ConfFileUrl").Text = result[0];
                    this._result = null;
                }
                this._counter = 0;
            };

            this.FindControl<Button>("SelectDataFile").Click += async delegate
            {
                var result = await new OpenFileDialog()
                {
                    Title = "选择比赛数据文件",
                    Filters = GenerateFilters("数据文件")
                }.ShowAsync((Window)VisualRoot);
                if (result != null && result.Length != 0 && result[0].Length != 0)
                {
                    this.FindControl<TextBox>("DataFileUrl").Text = result[0];
                    this._result = null;
                }
                this._counter = 0;
            };

            this.FindControl<Button>("RefreshScreens").Click += delegate
            {
                Window w = (Window)VisualRoot;
                List<Screen> screens = new List<Screen>(w.Screens.All);
                RefreshScreenList(screens);
            };

            this.FindControl<Button>("TestFiles").Click += delegate
            {
                if (ReadUrlAndLoadFiles())
                {
                    this.FindControl<TextBlock>("Message").Text = "文件选择正确";
                }
            };

            this.FindControl<DropDown>("ControlScreen").SelectionChanged += delegate
            {
                this._counter = 0;
            };

            this.FindControl<DropDown>("DisplayScreen").SelectionChanged += delegate
            {
                this._counter = 0;
            };

            this.FindControl<Button>("Comfirm").Click += delegate
            {
                if (this._result == null && !ReadUrlAndLoadFiles())
                {
                    return;
                }

                var controlScreen = (String)this.FindControl<DropDown>("ControlScreen").SelectedItem;
                if (controlScreen == null || controlScreen.Length == 0)
                {
                    this.FindControl<TextBlock>("Message").Text = "没有选择主控屏";
                    return;
                }
                var displayScreen = (String)this.FindControl<DropDown>("DisplayScreen").SelectedItem;
                if (displayScreen == null || displayScreen.Length == 0)
                {
                    this.FindControl<TextBlock>("Message").Text = "没有选择显示屏";
                    return;
                }
                if (controlScreen == displayScreen && this._counter == 0)
                {
                    this.FindControl<TextBlock>("Message").Text = "选择的显示屏是同一个显示屏，如确认请再点击一次确定";
                    ++this._counter;
                    return;
                }

                ((InitWindow)VisualRoot).OpenControlAndDisplayWindow(_result, _model.Name2Screen[controlScreen], _model.Name2Screen[displayScreen]);
            };
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            Window w = (Window)VisualRoot;
        }

        public override void Render(DrawingContext context)
        {
            Window w = (Window)VisualRoot;
            List<Screen> screens = new List<Screen>(w.Screens.All);
            RefreshScreenList(screens);

            Screen mainScreen = screens.Find((ele) => ele.Primary);
            w.Position = new Point((int)(mainScreen.Bounds.Width - w.Bounds.Width) / 2, (int)(mainScreen.Bounds.Height - w.Bounds.Height) / 2);
        }

        private void RefreshScreenList(List<Screen> screens)
        {
            for (Int32 i = 0, j = screens.Count; i != j; ++i)
            {
                String str = GenerateScreenString(i, screens[i]);
                if (!_model.Name2Screen.ContainsKey(str))
                {
                    _model.Screens.Add(str);
                    _model.Name2Screen.Add(str, screens[i]);
                }
            }

            if (screens.Count < 2)
            {
                this.FindControl<TextBlock>("Message").Text = "屏幕的数量少于两个，请确认是否连接成功";
            }
        }

        private String GenerateScreenString(Int32 index, Screen screen)
        {
            return String.Format("{0}.({1}, {2}), {3} * {4} {5}", index,
                screen.Bounds.TopLeft.X, screen.Bounds.TopLeft.Y, screen.Bounds.Width, screen.Bounds.Height,
                screen.Primary ? "(系统主显示器)" : "");
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

        private bool ReadUrlAndLoadFiles()
        {
            var confFileUrl = this.FindControl<TextBox>("ConfFileUrl").Text;
            if (confFileUrl == null || confFileUrl.Length == 0)
            {
                this.FindControl<TextBlock>("Message").Text = "未选择比赛设置文件";
                return false;
            }

            var dataFileUrl = this.FindControl<TextBox>("DataFileUrl").Text;
            if (dataFileUrl == null || dataFileUrl.Length == 0)
            {
                this.FindControl<TextBlock>("Message").Text = "未选择比赛数据文件";
                return false;
            }

            this._result = LoadFiles(confFileUrl, dataFileUrl);
            return this._result != null;
        }

        private CompetitionData.Competition LoadFiles(String confUrl, String dataUrl)
        {
//             try
//             {
                CompetitionConfiguration.Analyzer confAnalyzer = new CompetitionConfiguration.Analyzer();
                if (!confAnalyzer.Analyze(confUrl))
                {
                    this.FindControl<TextBlock>("Message").Text = String.Format("读取比赛设置文件失败： {0}", confAnalyzer.LastError);
                    return null;
                }

                CompetitionData.Analyzer dataAnalyzer = new CompetitionData.Analyzer(confAnalyzer.Result);
                if (!dataAnalyzer.Analyze(dataUrl))
                {
                    this.FindControl<TextBlock>("Message").Text = String.Format("读取比赛数据文件失败： {0}", dataAnalyzer.LastError);
                    return null;
                }

                return dataAnalyzer.Result;
//             }
//             catch (Exception e)
//             {
//                 this.FindControl<TextBlock>("Message").Text = String.Format("未知错误： {0}", e.Message);
//                 Console.Write("False, {0}", e.Message);
//                 return null;
//             }
        }
    }
}
