using Tetris.Model.Blocks;

namespace Tetris.Model
{
    public class Board
    {
        public Board()
        {
            GameBoard = new Field[20, 10];
        }

        public Field[,] GameBoard
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public Block CurrentBlock
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}