using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlGameViewLineItem : UserControl
    {

        public class LineItem
        {
            public Line Data
            {
                get;
            }

            public String Line
            {
                get
                {
                    return Data.Order.Value.ToString();
                }
            }

            public String Name
            {
                get
                {
                    return Data.LineParticipant == null ? ""
                        : Data.LineParticipant.Athletes.Count == 1 ? Data.LineParticipant.Athletes[0].Name : Data.LineParticipant.Name;
                }
            }

            public String Team
            {
                get
                {
                    return Data.LineParticipant == null ? ""
                        : Data.LineParticipant.ParticipantTeam.Conf.ShortName;
                }
            }

            public Boolean IsDQS
            {
                get;
                set;
            }

            public Boolean IsDNS
            {
                get;
                set;
            }

            public String Min
            {
                get
                {
                    return MinValue == 0 ? "" : MinValue.ToString();
                }
                set
                {
                    if (value == "")
                    {
                        MinValue = 0;
                    }
                    else if (IsPositiveInteger(value, UInt32.MaxValue))
                    {
                        MinValue = UInt32.Parse(value);
                    }
                }
            }

            public UInt32 MinValue
            {
                get;
                private set;
            }

            public String Sec
            {
                get
                {
                    return SecValue == 0 ? "" : SecValue.ToString();
                }
                set
                {
                    if (value == "")
                    {
                        SecValue = 0;
                    }
                    else if (IsPositiveInteger(value, 60))
                    {
                        SecValue = UInt32.Parse(value);
                    }
                }
            }

            public UInt32 SecValue
            {
                get;
                private set;
            }

            public String TMillSec
            {
                get
                {
                    return TMillSecValue == 0 ? "" : TMillSecValue.ToString();
                }
                set
                {
                    if (value == "")
                    {
                        TMillSecValue = 0;
                    }
                    else if (IsPositiveInteger(value, 100))
                    {
                        TMillSecValue = UInt32.Parse(value);
                        if (TMillSecValue < 10)
                        {
                            TMillSecValue *= 10;
                        }
                    }
                }
            }

            public UInt32 TMillSecValue
            {
                get;
                private set;
            }

            public String Message
            {
                get
                {
                    return Data.LineParticipant == null ? ""
                        : Data.LineParticipant.Athletes.Count == 1 && Data.LineParticipant.Athletes[0].Rank != null ? Data.LineParticipant.Athletes[0].Rank.Name : "";
                }
            }

            public LineItem(Line data)
            {
                Data = data;
                MinValue = 0;
                SecValue = 0;
                TMillSecValue = 0;
                IsDQS = false;
                IsDNS = false;
                if (data.ParticipantGrade.Valid())
                {
                    if (data.ParticipantGrade.GradeCode == GradeBase.Code.DSQ)
                    {
                        IsDQS = true;
                    }
                    else if (data.ParticipantGrade.GradeCode == GradeBase.Code.DNS)
                    {
                        IsDNS = true;
                    }
                    else if (data.ParticipantGrade.HasTime())
                    {
                        MinValue = (UInt32)data.ParticipantGrade.Time.Hours * 60 + (UInt32)data.ParticipantGrade.Time.Minutes;
                        SecValue = (UInt32)data.ParticipantGrade.Time.Seconds;
                        TMillSecValue = (UInt32)(data.ParticipantGrade.Time.Milliseconds / 10);
                    }
                }
            }

            public bool ValueValid()
            {
                return !(MinValue == 0 && SecValue == 0 && TMillSecValue == 0);
            }

            public Tuple<Grade.Code, TimeSpan> ToGradeTuple()
            {
                if (IsDNS)
                {
                    return new Tuple<GradeBase.Code, TimeSpan>(Grade.Code.DNS, TimeSpan.Zero);
                }
                else if (IsDQS)
                {
                    return new Tuple<GradeBase.Code, TimeSpan>(Grade.Code.DSQ, TimeSpan.Zero);
                }
                else if (ValueValid())
                {
                    return new Tuple<GradeBase.Code, TimeSpan>(Grade.Code.Normal, new TimeSpan(0, 0, (Int32)MinValue, (Int32)SecValue, (Int32)TMillSecValue * 10));
                }
                else
                {
                    return new Tuple<GradeBase.Code, TimeSpan>(Grade.Code.None, TimeSpan.Zero);
                }
            }

            public override String ToString()
            {
                return Line;
            }
        }

        private TextBlock _message;

        public LineItem Model
        {
            get;
        }

        public ControlGameViewLineItem(LineItem model)
        {
            Model = model;
            DataContext = Model;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            if (Model.Data.LineParticipant == null)
            {
                this.FindControl<StackPanel>("GradeInputBox").IsVisible = false;
            }

            _message = this.FindControl<TextBlock>("Message");
            var isDQSBtn = this.FindControl<CheckBox>("IsDQSBtn");
            var isDNSBtn = this.FindControl<CheckBox>("IsDNSBtn");
            isDQSBtn.Click += delegate
            {
                if (isDQSBtn.IsChecked.Value)
                {
                    isDNSBtn.IsChecked = false;
                }
                var tuple = Model.ToGradeTuple();
                _message.Text = Grade.ToDisplayFormatString(tuple.Item1, tuple.Item2);
            };
            isDNSBtn.Click += delegate
            {
                if (isDNSBtn.IsChecked.Value)
                {
                    isDQSBtn.IsChecked = false;
                }
                var tuple = Model.ToGradeTuple();
                _message.Text = Grade.ToDisplayFormatString(tuple.Item1, tuple.Item2);
            };

            this.FindControl<TextBox>("MinInput").TextInput += delegate (object obj, TextInputEventArgs e)
            {
                if (!GradeInputHandler((TextBox)obj, e, this.Model.Min))
                {
                    _message.Text = "分数必须是个正整数";
                }
                else
                {
                    var tuple = Model.ToGradeTuple();
                    _message.Text = Grade.ToDisplayFormatString(tuple.Item1, tuple.Item2);
                }
            };
            this.FindControl<TextBox>("SecInput").TextInput += delegate (object obj, TextInputEventArgs e)
            {
                if (!GradeInputHandler((TextBox)obj, e, this.Model.Sec, 60))
                {
                    _message.Text = "秒数必须是个正整数且小于60";
                }
                else
                {
                    var tuple = Model.ToGradeTuple();
                    _message.Text = Grade.ToDisplayFormatString(tuple.Item1, tuple.Item2);
                }
            };
            this.FindControl<TextBox>("TMillSecInput").TextInput += delegate (object obj, TextInputEventArgs e)
            {
                if (!GradeInputHandler((TextBox)obj, e, this.Model.TMillSec, 100))
                {
                    _message.Text = "十毫秒数必须是个正整数且小于100";
                }
                else
                {
                    var tuple = Model.ToGradeTuple();
                    _message.Text = Grade.ToDisplayFormatString(tuple.Item1, tuple.Item2);
                }
            };
        }

        private static bool GradeInputHandler(TextBox src, TextInputEventArgs e, String origin, UInt32 maximum = UInt32.MaxValue)
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
