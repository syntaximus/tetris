using System;
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
        #region CONST
        /// <summary>
        /// Zmienna która odpowiada za odświeżanie widoku kontrolek. Po takim czasie jest przekazywana informacja do view modelu o aktualizacji wyświetlania menu
        /// (W milisekundach).
        /// </summary>
        public const int ControlsVisibleInterval = 100;
        #endregion

        #region FIELDS
        private Visibility _gameVisibility;
        private Visibility _endGameVisibility;
        private Visibility _menuVisibility;
        private Visibility _recordsVisibility;
        private BindableCollection<Record> _recordsList;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Właściwość odpowiedzialna za widoczność menu głównego.
        /// </summary>
        public Visibility MenuVisibility
        {
            get { return _menuVisibility; }

            set
            {
                _menuVisibility = value;
                NotifyOfPropertyChange("MenuVisibility");
            }
        }
        /// <summary>
        /// Właściwość odpowiedzialna za widoczność ekranu końcowego z zapisaniem wyniku gry.
        /// </summary>
        public Visibility EndGameVisibility
        {
            get { return _endGameVisibility; }

            set
            {
                _endGameVisibility = value;
                NotifyOfPropertyChange("EndGameVisibility");
            }
        }
        /// <summary>
        /// Właściwość odpowiedzialna za widoczność ekranu gry.
        /// </summary>
        public Visibility GameVisibility
        {
            get { return _gameVisibility; }

            set
            {
                _gameVisibility = value;
                NotifyOfPropertyChange("GameVisibility");
            }
        }
        /// <summary>
        /// Właściwość odpowiedzialna za widoczność tabeli rekordów.
        /// </summary>
        public Visibility RecordsVisibility
        {
            get { return _recordsVisibility; }

            set
            {
                _recordsVisibility = value;
                NotifyOfPropertyChange("RecordsVisibility");
            }
        }
        /// <summary>
        /// Właściwość odpowiedzialna za widoczność poziomu podczas gry.
        /// </summary>
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
        /// <summary>
        /// Właściwość odpowiedzialna za widoczność ilości punktów podczas gry.
        /// </summary>
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
        /// <summary>
        /// Właściowość odpowiedzialna za wyświetlenie ilości punktów podczas zakończenia gry.
        /// </summary>
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
        /// <summary>
        /// Właściwość odpowiedzialna za przetrzymywanie nazwy gracza wpisanego w textboxie na konie rozgrywki.
        /// </summary>
        public string PlayerName { get; set; }
        /// <summary>
        /// Kolekcja elementów, która jest wyświetlana na canvasie w czasie gry.
        /// </summary>
        public ObservableCollection<RectItem> RectItems { get; set; }
        /// <summary>
        /// Kolekcja elementów, która jest wyświetlana w okienku "następny klocek".
        /// </summary>
        public ObservableCollection<RectItem> NextBlockRectItems { get; set; }
        /// <summary>
        /// Kolejkca elementów z wynikami gry, które są wyświetlane w tablicy rekordów.
        /// </summary>
        public BindableCollection<Record> RecordsList
        {

            get { return _recordsList; }
            set
            {
                _recordsList = value;
                NotifyOfPropertyChange("RecordsList");
            }

        }
        /// <summary>
        /// Instancja gry, która jest odpowiedzialna za logikę rozgrywki.
        /// </summary>
        public Game Game { get; set; }

        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Kontruktor. Wyświetla menu główne i inicjalizuje zmienne. 
        /// </summary>
        public MainWindowViewModel()
        {
            MenuVisibility = Visibility.Visible;
            EndGameVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Hidden;
            RectItems = new ObservableCollection<RectItem>();
            NextBlockRectItems = new ObservableCollection<RectItem>();
        }
        /// <summary>
        /// Funkcja wykonuje się po kliknięciu w przycisk "Nowa gra". Rozpoczyna nową grę i uruchamia timery aktualizujące wyświetlanie ilości punktów i poziomu.
        /// </summary>
        public void StartNewGame()
        {
            Game = new Game(RectItems, NextBlockRectItems);
            Game.GameCompleted += GameOnGameCompleted;
            MenuVisibility = Visibility.Hidden;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Visible;
            EndGameVisibility = Visibility.Hidden;

            var notifyOfPropertyChangeDispatcherTimer = new DispatcherTimer();
            notifyOfPropertyChangeDispatcherTimer.Tick += NotifyOfPropertyChange_Tick;
            notifyOfPropertyChangeDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, ControlsVisibleInterval);
            notifyOfPropertyChangeDispatcherTimer.Start();
        }
        /// <summary>
        /// Funkcja wykonuje się po kliknięciu w przycisk "Najalepsze wyniki". Wczytuje z pliku 10 najlepszych wyników i aktualizuj kolejkcje służącą za wyświetlanie wyników.
        /// </summary>
        public void ShowRecords()
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
            for (int i = 0; i != records.Count && i < 10; i++)
                recrd.Add(records[i]);
            RecordsList = recrd;
        }
        /// <summary>
        /// Funkcja wykonuje się po kliknięciu w przycisk "Wyjście". Zamyka aplikacje.
        /// </summary>
        public void Exit()
        {
            Application.Current.MainWindow.Close();
        }
        /// <summary>
        /// Funkcja wykonuje się po kliknięciu w przycisk "Zapisz wynik" po zakończeniu gry (o ile wynik gracza jest w najlepszych 10 wynikach). Zapisuje ten wynik do pliku.
        /// </summary>
        public void SaveRecord()
        {
            Record record = new Record(Game.Points, ProcessPlayerName(PlayerName));
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
        /// <summary>
        /// Funkcja wykonuje się po kliknięciu w przycisk "Ok" w czasie wyśweitlania najlepszych wyników. Wyświetla wtedy menu.
        /// </summary>
        public void RecordsOk()
        {
            MenuVisibility = Visibility.Visible;
            RecordsVisibility = Visibility.Hidden;
            GameVisibility = Visibility.Hidden;
            EndGameVisibility = Visibility.Hidden;
        }
        /// <summary>
        /// Funkcja przechwytuje zdarzenia kliknięcia w klawiature przez użytkownika, a następnie przekazuje je do gry (o ile aktualnie prowadzone jest rozgrywka).
        /// </summary>
        /// <param name="context"></param>
        public void ExecuteKeyboardCommand(ActionExecutionContext context)
        {
            var keyArgs = context.EventArgs as KeyEventArgs;

            if (GameVisibility == Visibility.Visible && keyArgs != null)
            {
                Game.KeyboardEventHandler(keyArgs);
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Przetwarze nazwę gracza (zapobiega wpisaniu pustej nazwy i nazwy o długości powyżej 16).
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        private string ProcessPlayerName(string playerName)
        {
            if (playerName == null)
                return "<<nieznane>>";
            if (playerName.Length > 14)
                playerName = playerName.Remove(14);
            return playerName;

        }
        /// <summary>
        /// Obsłużenie zdarzenia końca gry. Jeżeli gra się skończyła i gracz zdobył ilość punktów znajdującą się w 10 najlepszych wynikach, to wyświetla się ekran zapisu wyniku. W przeciwnym wypadku wyświetla się menu. Gra zostaje zakończona.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void GameOnGameCompleted(object sender, EventArgs eventArgs)
        {
            {
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
                if (records.Count < 10 || records[records.Count - 1].Points < Game.Points)
                {
                    NotifyOfPropertyChange("RecordPoints");
                    MenuVisibility = Visibility.Hidden;
                    RecordsVisibility = Visibility.Hidden;
                    GameVisibility = Visibility.Hidden;
                    EndGameVisibility = Visibility.Visible;
                }
                else
                {
                    NotifyOfPropertyChange("RecordPoints");
                    MenuVisibility = Visibility.Visible;
                    RecordsVisibility = Visibility.Hidden;
                    GameVisibility = Visibility.Hidden;
                    EndGameVisibility = Visibility.Hidden;
                }
                Game.StopGame();

            };
        }
        /// <summary>
        /// Tick timera odpowiedzialny za aktualizacje punktów i poziomu w View.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyOfPropertyChange_Tick(object sender, EventArgs e)
        {
                NotifyOfPropertyChange("Points");
                NotifyOfPropertyChange("Level");
        }
        #endregion
    }
}