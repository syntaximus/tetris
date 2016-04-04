using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    public abstract class Block
    {
        public Brush Color;
        public bool[,] Surface;
        public abstract void Rotate();
    }
}