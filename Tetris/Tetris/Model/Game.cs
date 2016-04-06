using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Tetris.ViewModels;

namespace Tetris.Model
{
    public class Game
    {
        const int BlockWidth = 30;
        const int BlockHeight = 30;

        public ObservableCollection<RectItem> CanvasRectItems;
        public int Level { get; set; }
        public int Points { get; set; }
        public Board Board { get; set; }

        private int x = 0;
        private int y = 0;
        DispatcherTimer moveDownDispatcherTimer;

        public Game(ObservableCollection<RectItem> rectItems)
        {
            Board = new Board();
            Board.GenerateNewCurrentBlock();
            Points = 0;
            Level = 1;
            CanvasRectItems = rectItems;

            var dispatcherTimer = new DispatcherTimer();

            moveDownDispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick1;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            dispatcherTimer.Start();
            moveDownDispatcherTimer.Tick += MoveDown_Tick;
            moveDownDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            moveDownDispatcherTimer.Start();

            var dispatcherTimerRef = new DispatcherTimer();

            dispatcherTimerRef.Tick += dispatcherTimerRef_Tick;
            dispatcherTimerRef.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimerRef.Start();
        }


        private void dispatcherTimerRef_Tick(object sender, EventArgs e)
        {
            RefreshCanvas();
        }
        private void dispatcherTimer_Tick1(object sender, EventArgs e)
        {
            Level = Level + 1;

            CommandManager.InvalidateRequerySuggested();
        }

        private void MoveDown_Tick(object sender, EventArgs e)
        {
            if (Board.CurrentBlock.CanMoveDown)
            {
                Board.CurrentBlockMoveDown();
            }
            else
            {
                Board.SaveCurrentBlockInBoard();
                Board.GenerateNewCurrentBlock();
            }
            moveDownDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, Level < 80 ? 1000 - (Level * 10) : 200);
        }

        public void RefreshCanvas()
        {
            CanvasRectItems.Clear();
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
                    Board.CurrentBlockMoveDown();
                    break;
            }

        }
    }
}