using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private Visibility _recordsVisibility;
        private Visibility _gameVisibility;
        private Visibility _menuVisibility;
        private string _level;
        private string _points;

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
            get { return _level; }
            set
            {
                _level = value;
                NotifyOfPropertyChange("Level");
            }
        }
        public string Points
        {
            get { return _points; }
            set
            {
                _points = value;
                NotifyOfPropertyChange("Points");
            }
        }

        public class RectItem
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public Brush Color { get; set; }

        }

        public ObservableCollection<RectItem> RectItems { get; set; }

        public MainWindowViewModel()
        {
            MenuVisibility = Visibility.Visible;

            GameVisibility = Visibility.Hidden;
            RectItems = new ObservableCollection<RectItem>();
            RectItem add = new RectItem();
            add.Height = 40;
            add.Width = 40;
            add.X = 40;
            add.Y = 40;
            add.Color = new SolidColorBrush(Colors.Red);
            RectItems.Add(add);
        }

        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        public void NewGame()
        {
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Visible;
            Points = "0";
            Level = "0";
        }

        public void Records()
        {
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Visible;
            GameVisibility = Visibility.Hidden;
        }
    }
}
