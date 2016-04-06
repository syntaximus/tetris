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

        public void CurrentBlockMoveDown()
        {
            if (CanMoveDown())
            {
                CurrentBlock.MoveDown();
            }
            else
            {
                SaveCurrentBlockInBoard();
                GenerateNewCurrentBlock();
            }
        }

        public bool CanMoveDown()
        {
            for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                {
                    if (CurrentBlock.Surface[i, j])
                    {
                        if (CurrentBlock.Y + i + 1 < 20 && CurrentBlock.X - 2  + j < 10 && GameBoard[CurrentBlock.Y + i + 1, CurrentBlock.X - 2 + j] != null && GameBoard[CurrentBlock.Y + i + 1, CurrentBlock.X - 2 + j].Active)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool CanMoveLeft()
        {
            for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                {
                    if (CurrentBlock.Surface[i, j])
                    {
                        if (CurrentBlock.Y + i < 20 && CurrentBlock.X - 3 + j >= 0 && GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 3 + j] != null && GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 3 + j].Active)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        public bool CanMoveRight()
        {
            for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                {
                    if (CurrentBlock.Surface[i, j])
                    {
                        if (CurrentBlock.Y < 20 && CurrentBlock.X - 1 + j < 10 && GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 1 + j] != null && GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 1 + j].Active)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
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