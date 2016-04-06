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
    public class RectItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Brush Color { get; set; }
    }
    public class MainWindowViewModel : PropertyChangedBase
    {

        /// <summary>
        /// Zmienna która odpowiada za odświeżanie widoku kontrolek. Po takim czasie jest przekazywana informacja do view modelu o aktualizacji wyświetlania menu
        /// (W milisekundach).
        /// </summary>
        public const int ControlsVisibleInterval = 100;

        private Visibility _gameVisibility;
        private Visibility _menuVisibility;
        private Visibility _recordsVisibility;
        public int x = 0;
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
            get
            {
                return Game.Level.ToString();
            }
            set
            {
                Game.Level = int.Parse(value);
                NotifyOfPropertyChange("Level");
            }
        }

        public string Points
        {
            get
            {
                return Game.Points.ToString();
            }
            set
            {
                Game.Points = int.Parse(value);
                NotifyOfPropertyChange("Points");
            }
        }

        public ObservableCollection<RectItem> RectItems { get; set; }

        public Game Game { get; set; }


        public void ExecuteFilterView(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if (GameVisibility == Visibility.Visible && keyArgs != null)
            {
                Game.KeyboardEventHandler(keyArgs);
            }
        }

        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        public void NewGame()
        {
            Game = new Game(RectItems);
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Visible;

            var notifyOfPropertyChangeDispatcherTimer = new DispatcherTimer();
            notifyOfPropertyChangeDispatcherTimer.Tick += notifyOfPropertyChange_Tick;
            notifyOfPropertyChangeDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, ControlsVisibleInterval);
            notifyOfPropertyChangeDispatcherTimer.Start();
        }

        private void notifyOfPropertyChange_Tick(object sender, EventArgs e)
        {

            NotifyOfPropertyChange("Points");
            NotifyOfPropertyChange("Level");
        }

        public void Records()
        {
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Visible;
            GameVisibility = Visibility.Hidden;
        }

    }
}