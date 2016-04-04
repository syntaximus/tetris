namespace Tetris.Model
{
    public class Game
    {
        public int Level;
        public int Points;

        public Game()
        {
            Board = new Board();
            Points = 0;
            Level = 1;
        }

        public Board Board
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