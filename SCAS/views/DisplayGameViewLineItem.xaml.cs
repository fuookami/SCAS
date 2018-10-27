using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class DisplayGameViewLineItem : UserControl
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

            public String Grade
            {
                get
                {
                    return Data.ParticipantGrade.ToFormatString();
                }
            }

            public String OrderInGroup
            {
                get
                {
                    var indexPair = Data.Parent.OrderInGroup.Find((ele) =>
                    {
                        return ele.Item2.FindIndex((l) => l == Data) != -1;
                    });
                    if (indexPair == null || !indexPair.Item1.Valid())
                    {
                        return "";
                    }
                    else
                    {
                        return indexPair.Item1.Value.ToString();
                    }
                }
            }

            public LineItem(Line data)
            {
                Data = data;
            }
        }

        private LineItem _model;

        public DisplayGameViewLineItem(LineItem model, double height)
        {
            _model = model;
            DataContext = _model;
            this.InitializeComponent();
            this.Height = height;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
