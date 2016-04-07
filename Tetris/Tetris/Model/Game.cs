using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32.SafeHandles;
using Tetris.Model.Blocks;
using Tetris.ViewModels;
using Block = System.Windows.Documents.Block;

namespace Tetris.Model
{
    public class Game : IDisposable
    {
        #region CONST VALUES
        private const int BlockWidth = 30;
        private const int BlockHeight = 30;
        #endregion

        #region PROPERTIES
        public ObservableCollection<RectItem> CanvasRectItems;
        public ObservableCollection<RectItem> CanvasNextBlockRectItems;

        public bool GamePaused { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public Board Board { get; set; }
        #endregion

        #region FIELDS
        private DispatcherTimer _moveDownTimer;
        private DispatcherTimer _levelUpTimer;
        private DispatcherTimer _refreshCanvasTimer;
        #endregion

        #region PUBLIC METHODS
        public Game(ObservableCollection<RectItem> rectItems, ObservableCollection<RectItem> nextBlockRectItems)
        {
            Board = new Board();
            Board.GenerateNewCurrentBlock();
            Points = 0;
            Level = 1;
            CanvasRectItems = rectItems;
            CanvasNextBlockRectItems = nextBlockRectItems;
            InitializeTimers();
        }
        public void KeyboardEventHandler(KeyEventArgs keyArgs)
        {
            switch (keyArgs.Key)
            {
                case Key.Up:
                    Board.CurrentBlockRotate();
                    break;
                case Key.Right:
                    Board.CurrentBlockMoveRight();
                    break;
                case Key.Left:
                    Board.CurrentBlockMoveLeft();
                    break;
                case Key.Down:
                    if (!Board.CurrentBlockMoveDown())
                    {
                        EndGame();
                        break;
                    }
                    Points += (int)Math.Pow(Board.CheckRows(), 2) * Level;
                    break;
                case Key.Escape:
                    if (GamePaused)
                    {
                        ResumeGame();
                    }
                    else
                    {
                        PauseGame();
                    }
                    GamePaused = !GamePaused;
                    break;
            }

        }
        #endregion

        #region PRIVATE METHODS

        private void EndGame()
        {

            OnGameCompleted(new EventArgs());
        }
        public void NewGame(ObservableCollection<RectItem> rectItems)
        {
            Board = new Board();
            Board.GenerateNewCurrentBlock();
            Points = 0;
            Level = 1;
            CanvasRectItems = rectItems;
            InitializeTimers();
        }

        public void PauseGame()
        {
            _moveDownTimer.Stop();
            _levelUpTimer.Stop();
            _refreshCanvasTimer.Stop();
        }


        public void ResumeGame()
        {
            _moveDownTimer.Start();
            _levelUpTimer.Start();
            _refreshCanvasTimer.Start();
        }

        public void StopGame()
        {
            Board.GameBoard = new Field[20, 10];
            Board.CurrentBlock = null;
            RefreshCanvas();
            _moveDownTimer.Stop();
            _levelUpTimer.Stop();
            _refreshCanvasTimer.Stop();

        }

        private void RefreshCanvas()
        {
            CanvasRectItems.Clear();
            if (Board.CurrentBlock == null)
                return;
            for (int i = 0; i != Board.CurrentBlock.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != Board.CurrentBlock.Surface.GetLength(1); j++)
                {
                    if (Board.CurrentBlock.Surface[i, j])
                    {
                        var add = new RectItem();
                        add.Height = BlockHeight;
                        add.Width = BlockWidth;
                        add.X = BlockWidth * (Board.CurrentBlock.X - 2 + j);
                        add.Y = BlockHeight * (Board.CurrentBlock.Y + i);
                        add.Color = Board.CurrentBlock.Color;
                        CanvasRectItems.Add(add);
                    }
                }
            }
            for (int i = 0; i != 20; i++)
            {
                for (int j = 0; j != 10; j++)
                {
                    if (Board.GameBoard[i, j] != null && Board.GameBoard[i, j].Active)
                    {
                        var add = new RectItem();
                        add.Height = BlockHeight;
                        add.Width = BlockWidth;
                        add.X = BlockWidth * j;
                        add.Y = BlockHeight * i;
                        add.Color = Board.GameBoard[i, j].Color;
                        CanvasRectItems.Add(add);
                    }
                }
            }
        }
        private void RefreshNextBlockCanvas()
        {
            CanvasNextBlockRectItems.Clear();
            Model.Blocks.Block block = null;
            switch (Board.NextBlock)
            {
                case 0:
                    block = new BlockI();
                    break;
                case 1:
                    block = new BlockJ();
                    break;
                case 2:
                    block = new BlockL();
                    break;
                case 3:
                    block = new BlockO();
                    break;
                case 4:
                    block = new BlockS();
                    break;
                case 5:
                    block = new BlockT();
                    break;
                case 6:
                    block = new BlockZ();
                    break;

            }
            for (int i = 0; i != block.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != block.Surface.GetLength(1); j++)
                {
                    if (block.Surface[i, j])
                    {
                        var add = new RectItem();
                        add.Height = BlockHeight/3;
                        add.Width = BlockWidth/3;
                        add.X = BlockWidth/3 * (j);
                        add.X += 3;
                        add.Y = BlockHeight/3 * i;
                        add.Y -= 3;
                        add.Color = block.Color;
                        CanvasNextBlockRectItems.Add(add);
                    }
                }
            }
        }

        private void InitializeTimers()
        {
            _levelUpTimer = new DispatcherTimer();
            _moveDownTimer = new DispatcherTimer();
            _refreshCanvasTimer = new DispatcherTimer();

            _levelUpTimer.Tick += dispatcherTimerLevelUp_Tick;
            _levelUpTimer.Interval = new TimeSpan(0, 0, 1, 0, 0);
            _levelUpTimer.Start();

            _moveDownTimer.Tick += MoveDown_Tick;
            _moveDownTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            _moveDownTimer.Start();

            _refreshCanvasTimer.Tick += dispatcherTimerRefreshCanvas_Tick;
            _refreshCanvasTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _refreshCanvasTimer.Start();
        }
        private void dispatcherTimerRefreshCanvas_Tick(object sender, EventArgs e)
        {
            RefreshCanvas();
            RefreshNextBlockCanvas();
            CommandManager.InvalidateRequerySuggested();
        }
        private void dispatcherTimerLevelUp_Tick(object sender, EventArgs e)
        {
            try
            {

                Level = Level + 1;
                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ew)
            {


                throw ew;
            }
        }
        private void MoveDown_Tick(object sender, EventArgs e)
        {
            if (Board.GameBoard != null)
            {
                if (!Board.CurrentBlockMoveDown())
                {
                    EndGame();
                }
                Points += (int)Math.Pow(Board.CheckRows(), 2) * Level;
                _moveDownTimer.Interval = new TimeSpan(0, 0, 0, 0, Level < 8 ? 1100 - (Level * 100) : 100 + (int)(200 * (1 / (0.5 * Level - 3))));
            }
        }
        #endregion

        #region EVENTS
        public event EventHandler GameCompleted;
        protected virtual void OnGameCompleted(EventArgs e)
        {
            if (GameCompleted != null)
                GameCompleted(this, e);
        }
        #endregion

        #region IDisposable
        private bool _disposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
                _moveDownTimer.Stop();
                _levelUpTimer.Stop();
                _refreshCanvasTimer.Stop();
                _moveDownTimer = null;
                _levelUpTimer = null;
                _refreshCanvasTimer = null;
                CanvasRectItems = null;
                Board.Dispose();



            }
            _disposed = true;
        }
        ~Game()      // finalizer
        {
            Dispose(false);
        }
        #endregion
    }
}