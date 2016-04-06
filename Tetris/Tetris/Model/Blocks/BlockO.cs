using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    public class BlockO : Block
    {
        public BlockO()
        {
            /*
            1 1 0 0
            1 1 0 0
            0 0 0 0
            0 0 0 0
            */
            Surface = new[,]
            {
                {true, true, false, false},
                {true, true, false, false},
                {false, false, false, false},
                {false, false, false, false}
            };
            Color = new SolidColorBrush(Colors.Aqua);
        }

        public override bool TryRotate()
        {
            return true;
        }

        public override bool[,] ShowRotate()
        {
            return Surface;
        }
    }
}