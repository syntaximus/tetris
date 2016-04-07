﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using Tetris.Model;

namespace Tetris.ViewModels
{
    [Serializable]
    public class Record
    {
        public Record(int points, string player)
        {
            Points = points;
            Player = player;
        }
        public string Player { get; set; }
        public int Points { get; set; }
    }
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
        private Visibility _endGameVisibility;
        private Visibility _menuVisibility;
        private Visibility _recordsVisibility;
        private BindableCollection<Record> _recordsList;
        public int x = 0;
        public int y;

        public MainWindowViewModel()
        {
            MenuVisibility = Visibility.Visible;
            EndGameVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Hidden;
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
        public Visibility EndGameVisibility
        {
            get { return _endGameVisibility; }

            set
            {
                _endGameVisibility = value;
                NotifyOfPropertyChange("EndGameVisibility");
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
            get { return _recordsVisibility; }

            set
            {
                _recordsVisibility = value;
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
        public string RecordPoints
        {
            get
            {
                return Game.Points.ToString();
            }
            set
            {
                Game.Points = int.Parse(value);
                NotifyOfPropertyChange("RecordPoints");
            }
        }
        public string PlayerName { get; set; }

        public ObservableCollection<RectItem> RectItems { get; set; }

        public BindableCollection<Record> RecordsList
        {

            get { return _recordsList; }
            set
            {
                _recordsList = value;
                NotifyOfPropertyChange("RecordsList");
            }

        }

        public Game Game { get; set; }


        public void ExecuteFilterView(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if (GameVisibility == Visibility.Visible && keyArgs != null)
            {
                Game.KeyboardEventHandler(keyArgs);
            }
        }

        public string ProcessPlayerName(string playerName)
        {
            if (playerName.Length > 16)
                playerName = playerName.Remove(16);
            else
            {
                int buforLength = playerName.Length;
                for (int i = 0; i != 16 - buforLength; i++)
                {
                    playerName += " ";
                }
            }
            return playerName;
            
        }

        public void SaveRecord()
        {
            Record record = new Record(Int32.Parse(Points), ProcessPlayerName(PlayerName));
            List<Record> records;
            IFormatter formatter = new BinaryFormatter();
            using (
                Stream stream = new FileStream("Records.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                try
                {
                    records = (List<Record>)formatter.Deserialize(stream);
                }
                catch (SerializationException)
                {
                    records = new List<Record>();
                }
                records.Add(record);

            }

            try
            {
                using (Stream stream = File.Open("Records.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, records);
                }
            }
            catch (IOException)
            {
            }
            MenuVisibility = Visibility.Visible;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Hidden;
            EndGameVisibility = Visibility.Hidden;

        }

        public void RecordsOk()
        {
            MenuVisibility = Visibility.Visible;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Hidden;
            EndGameVisibility = Visibility.Hidden;
        }
        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }

        public void NewGame()
        {
            try
            {
                Game = new Game(RectItems);
                Game.GameCompleted += delegate (object sender, EventArgs args)
                {
                    NotifyOfPropertyChange("RecordPoints");
                    MenuVisibility = Visibility.Hidden;
                    RecordsVisibility = Visibility.Hidden;
                    GameVisibility = Visibility.Hidden;
                    EndGameVisibility = Visibility.Visible;
                    Game.StopGame();

                };
                MenuVisibility = Visibility.Hidden;
                RecordsVisibility = Visibility.Hidden;
                GameVisibility = Visibility.Visible;
                EndGameVisibility = Visibility.Hidden;

                var notifyOfPropertyChangeDispatcherTimer = new DispatcherTimer();
                notifyOfPropertyChangeDispatcherTimer.Tick += notifyOfPropertyChange_Tick;
                notifyOfPropertyChangeDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, ControlsVisibleInterval);
                notifyOfPropertyChangeDispatcherTimer.Start();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void notifyOfPropertyChange_Tick(object sender, EventArgs e)
        {
            try
            {
                NotifyOfPropertyChange("Points");
                NotifyOfPropertyChange("Level");

            }
            catch (Exception ew)
            {


                throw ew;
            }

        }

        public void Records()
        {
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Visible;
            GameVisibility = Visibility.Hidden;
            BindableCollection<Record> recrd = new BindableCollection<Record>();
            List<Record> records;
            IFormatter formatter = new BinaryFormatter();
            using (
                Stream stream = new FileStream("Records.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                try
                {
                    records = (List<Record>)formatter.Deserialize(stream);
                }
                catch (SerializationException)
                {
                    records = new List<Record>();
                }
            }
            records = records.OrderByDescending(x => x.Points).ToList();
            for(int i =0 ;i!=records.Count;i++)
                recrd.Add(records[i]);
            RecordsList = recrd;
        }

    }
}