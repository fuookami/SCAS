using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SSUtils;
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

            public String Min
            {
                get;
                set;
            }

            public String Sec
            {
                get;
                set;
            }

            public String HMillSec
            {
                get;
                set;
            }

            public LineItem(Line data)
            {
                Data = data;
                Min = "";
                Sec = "";
                HMillSec = "";
            }

            public override String ToString()
            {
                return Line;
            }
        }

        private LineItem _model;

        public ControlGameViewLineItem(LineItem model)
        {
            _model = model;
            DataContext = _model;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            if (_model.Data.LineParticipant == null)
            {
                this.FindControl<StackPanel>("GradeInputBox").IsVisible = false;
            }
        }
    }
}
