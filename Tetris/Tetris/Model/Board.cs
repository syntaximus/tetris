using System;
using Tetris.Model.Blocks;
using Tetris.ViewModels;

namespace Tetris.Model
{
    public class Board
    {
        public Board()
        {
            GameBoard = new Field[20, 10];
        }

        public Field[,] GameBoard { get; set; }
        public Block CurrentBlock { get; set; }
        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po przesunięciu klocka o jeden w dół.
        /// </summary>
        public bool CanCurrentBlockMoveDown
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y + i + 1 < 20 && CurrentBlock.X - 2 + j < 10 &&
                                GameBoard[CurrentBlock.Y + i + 1, CurrentBlock.X - 2 + j] != null &&
                                GameBoard[CurrentBlock.Y + i + 1, CurrentBlock.X - 2 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po przesunięciu aktualnego bloku w prawo.
        /// </summary>
        public bool CanCurrentBlockMoveLeft
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y + i < 20 && CurrentBlock.X - 3 + j >= 0 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 3 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 3 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po przesunięciu aktualnego bloku w lewo.
        /// </summary>
        public bool CanCurrentBlockMoveRight
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y < 20 && CurrentBlock.X - 1 + j < 10 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 1 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 1 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po wykonaniu obrotu przez aktualny blok.
        /// </summary>
        public bool CanCurrentBlockRotate
        {
            get
            {
                bool[,] surface = CurrentBlock.ShowRotate();
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (surface[i, j])
                        {
                            if (CurrentBlock.Y < 20 && CurrentBlock.X - 2 + j < 10 && CurrentBlock.X - 2 + j >= 0 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;

            }
        }

        public void CurrentBlockMoveLeft()
        {
            if (CanCurrentBlockMoveLeft)
            {
                CurrentBlock.TryMoveLeft();
            }
        }
        public void CurrentBlockMoveRight()
        {
            if (CanCurrentBlockMoveRight)
            {
                CurrentBlock.TryMoveRight();
            }
        }
        public void CurrentBlockRotate()
        {
            if (CanCurrentBlockRotate)
            {
                CurrentBlock.TryRotate();
            }
        }
        public void CurrentBlockMoveDown()
        {
            if (CanCurrentBlockMoveDown)
            {
                CurrentBlock.TryMoveDown();
            }
            else
            {
                SaveCurrentBlockInBoard();
                GenerateNewCurrentBlock();
            }
        }




        public void GenerateNewCurrentBlock()
        {
            Random rand = new Random();
            switch (rand.Next(0, 6))
            {
                case 0:
                    CurrentBlock = new BlockI();
                    break;
                case 1:
                    CurrentBlock = new BlockJ();
                    break;
                case 2:
                    CurrentBlock = new BlockL();
                    break;
                case 3:
                    CurrentBlock = new BlockO();
                    break;
                case 4:
                    CurrentBlock = new BlockS();
                    break;
                case 5:
                    CurrentBlock = new BlockT();
                    break;
                case 6:
                    CurrentBlock = new BlockZ();
                    break;

            }
            CurrentBlock.X = 5;
            CurrentBlock.Y = 0;
        }

        internal void SaveCurrentBlockInBoard()
        {
            for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                {
                    if (CurrentBlock.Surface[i, j])
                    {
                        GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j] = new Field(CurrentBlock.Color);
                    }
                }
            }
        }
    }
}