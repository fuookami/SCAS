using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SCAS.CompetitionConfiguration;
using SCAS.CompetitionData;

namespace SCAS.views
{
    public class ControlView : UserControl
    {
        public class Node
        {
            public enum Type
            {
                Empty,
                Competition,
                Event,
                Session,
                Game,
                Team
            }

            public Type NodeType
            {
                get;
            }

            public String Header
            {
                get;
            }

            public List<Node> Children
            {
                get;
            }

            public Node(String header)
                : this(Type.Empty, header) { }

            public Node(Type nodeType = Type.Empty, String header = "")
            {
                NodeType = nodeType;
                Header = header;
                Children = new List<Node>();
            }
        }

        public class DataNode<T> : Node
        {
            public T Data
            {
                get;
            }

            protected DataNode(T data, Type nodeType, String header)
                : base(nodeType, header)
            {
                Data = data;
            }
        }

        public class CompetitionNode : DataNode<Competition>
        {
            public CompetitionNode(Competition data)
                : base(data, Type.Competition, data.Conf.Name) { }
        }

        public class EventNode : DataNode<Event>
        {
            public EventNode(Event data)
                : base(data, Type.Event, data.Conf.Name) { }
        }

        public class SessionNode : DataNode<Session>
        {
            public SessionNode(Session data)
                : base(data, Type.Session, data.FullName) { }
        }

        public class GameNode : DataNode<Game>
        {
            public GameNode(Game data)
                : base(data, Type.Game, data.Conf.Name) { }
        }

        public class TeamNode : DataNode<Team>
        {
            public TeamNode(Team data)
                : base(data, Type.Team, data.Conf.Name) { }
        }

        Node _treeModel;
        Competition _data;
        StackPanel _viewBox;
        DisplayWindow _display;

        public ControlView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _viewBox = this.FindControl<StackPanel>("ViewBox");
            var tree = this.FindControl<TreeView>("CompTree");
            tree.Tapped += delegate
            {
                OnTreeViewItemSelected((Node)tree.SelectedItem);
            };
            tree.DoubleTapped += delegate
            {
                OnTreeViewItemSelected((Node)tree.SelectedItem);
            };
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            ((ControlWindow)VisualRoot).InitCompleted += delegate(Competition data, DisplayWindow display)
            {
                _data = data;
                _display = display;
                _treeModel = new Node();
                var root = new CompetitionNode(data);

                var gameRoot = new Node("比赛子项");
                foreach (var gameList in _data.Games)
                {
                    Node newSessionNode = new SessionNode(gameList.Key);
                    foreach (var game in gameList.Value)
                    {
                        Node newGameNode = new GameNode(game);
                        newSessionNode.Children.Add(newGameNode);
                    }
                    gameRoot.Children.Add(newSessionNode);
                }

                var eventRoot = new Node("比赛项目");
                foreach (var eventData in _data.Events)
                {
                    Node newEventNode = new EventNode(eventData);
                    eventRoot.Children.Add(newEventNode);
                }

                root.Children.Add(gameRoot);
                root.Children.Add(eventRoot);
                _treeModel.Children.Add(root);
                
                DataContext = _treeModel.Children;
            };
        }

        private void OnTreeViewItemSelected(Node item)
        {
            if (item != null)
            {
                _viewBox.Children.Clear();

                switch (item.NodeType)
                {
                    case Node.Type.Competition:
                        _viewBox.Children.Add(new ControlCompetitionView(((CompetitionNode)item).Data, _display));
                        break;
                    case Node.Type.Event:
                        _viewBox.Children.Add(new ControlEventView(((EventNode)item).Data, _display));
                        break;
                    case Node.Type.Game:
                        _viewBox.Children.Add(new ControlGameView(((GameNode)item).Data, _display));
                        break;
                }
            }
        }
    }
}
