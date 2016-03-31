using Caliburn.Micro;
using System;
using System.Windows;
using System.Windows.Input;

namespace Tetris.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {

        private Visibility _recordsVisibility;
        private Visibility _gameVisibility;
        private Visibility _menuVisibility;

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
            get { return _menuVisibility; }

            set
            {
                _menuVisibility = value;
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





        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        public void NewGame()
        {
            MenuVisibility = Visibility.Hidden;

        }
        

    }
}
