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
            CurrentBlock.MoveDown();
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