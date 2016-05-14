using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    internal class BlockOmega : Block
    {
        public BlockOmega()
        {
            Surface = new[,]
            {
                {true, false, false, true},
                {false, true, true, false},
                {false, true, true, false},
                {true, false, false, true}
            };
            Color = new SolidColorBrush(Colors.Orange);
        }

        public override bool TryRotate()
        {
            if (CanRotate)
            {
                Surface = ShowRotate();
                return true;
            }
            return false;
        }


        public override bool[,] ShowRotate()
        {
            var rotateSurface = new bool[4, 4];
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    rotateSurface[j, i] = Surface[3 - i, j];
                }
            }
            return rotateSurface;
        }
    }
}
