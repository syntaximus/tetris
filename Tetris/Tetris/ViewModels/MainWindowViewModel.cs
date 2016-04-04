using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using Tetris.Model;

namespace Tetris.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private Visibility _gameVisibility;
        private Visibility _menuVisibility;
        private Visibility _recordsVisibility;
        private Game _game;
        public int x = 45;
        public int y;

        public MainWindowViewModel()
        {
            MenuVisibility = Visibility.Visible;

            GameVisibility = Visibility.Hidden;
            RectItems = new ObservableCollection<RectItem>();
        }

        public Visibility MenuVisibility
        {
            get { return _menuVisibility; }

            set
            {
                _menuVisibility = value;
                NotifyOfPropertyChange("MenuVisibility");
            }
        }

        public Visibility GameVisibility
        {
            get { return _gameVisibility; }

            set
            {
                _gameVisibility = value;
                NotifyOfPropertyChange("GameVisibility");
            }
        }

        public Visibility RecordsVisibility
        {
            get { return _menuVisibility; }

            set
            {
                _menuVisibility = value;
                NotifyOfPropertyChange("RecordsVisibility");
            }
        }

        public string Level
        {
            get { return _game.Level.ToString(); }
            set
            {
                _game.Level = int.Parse(value);
                NotifyOfPropertyChange("Level");
            }
        }

        public string Points
        {
            get { return _game.Points.ToString(); }
            set
            {
                _game.Points = int.Parse(value);
                NotifyOfPropertyChange("Points");
            }
        }

        public ObservableCollection<RectItem> RectItems { get; set; }

        public Game Game
        {
            get { return _game; }

            set { _game = value; }
        }

        public Views.MainWindowView Binding
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public void ExecuteFilterView(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if (GameVisibility == Visibility.Visible && keyArgs != null)
            {
                var add = new RectItem();
                switch (keyArgs.Key)
                {
                    case Key.Up:
                        add = new RectItem();
                        y -= 30;
                        add.Height = 30;
                        add.Width = 30;
                        add.X = x;
                        add.Y = y;
                        add.Color = new SolidColorBrush(Colors.Blue);
                        RectItems[0] = add;

                        break;
                    case Key.Down:
                        y += 30;
                        add.Height = 30;
                        add.Width = 30;
                        add.X = x;
                        add.Y = y;
                        add.Color = new SolidColorBrush(Colors.Blue);
                        RectItems[0] = add;

                        break;
                    case Key.Right:
                        x += 30;
                        add = new RectItem();
                        add.Height = 30;
                        add.Width = 30;
                        add.X = x;
                        add.Y = y;
                        add.Color = new SolidColorBrush(Colors.Blue);
                        RectItems[0] = add;

                        break;
                    case Key.Left:
                        x -= 30;
                        add = new RectItem();
                        add.Height = 30;
                        add.Width = 30;
                        add.X = x;
                        add.Y = y;
                        add.Color = new SolidColorBrush(Colors.Blue);
                        RectItems[0] = add;

                        break;
                }
            }
        }

        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        public async void NewGame()
        {
            Game = new Game();
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Visible;

            var add = new RectItem();
            add.Height = 40;
            add.Width = 40;
            add.X = 40;
            add.Y = 40;
            add.Color = new SolidColorBrush(Colors.Red);
            RectItems.Add(add);
            NotifyOfPropertyChange("RectItems");
            NotifyOfPropertyChange("Points");
            NotifyOfPropertyChange("Level");

            var dispatcherTimer = new DispatcherTimer();

            var dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Start();
            dispatcherTimer1.Tick += dispatcherTimer_Tick1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer1.Start();
        }

        private void dispatcherTimer_Tick1(object sender, EventArgs e)
        {
            Level = (int.Parse(Level) + 1).ToString();
            NotifyOfPropertyChange("Level");
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            y += 30;
            var add = new RectItem();
            add.Height = 30;
            add.Width = 30;
            add.X = x;
            add.Y = y;
            add.Color = new SolidColorBrush(Colors.Blue);
            RectItems[0] = add;

            NotifyOfPropertyChange("RectItems");
            CommandManager.InvalidateRequerySuggested();
        }

        public void Records()
        {
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Visible;
            GameVisibility = Visibility.Hidden;
        }

        public class RectItem
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public Brush Color { get; set; }
        }
    }
}