using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    internal class BlockI : Block
    {
        public BlockI()
        {
            /*
            0 1 0 0
            0 1 0 0
            0 1 0 0
            0 1 0 0
            */
            Surface = new[,]
            {
                {false, true, false, false},
                {false, true, false, false},
                {false, true, false, false},
                {false, true, false, false}
            };
            Color = new SolidColorBrush(Colors.Red);
        }

        public override void Rotate()
        {
            Surface = ShowRotate();
        }

        public bool[,] ShowRotate()
        {
            var rotateSurface = new bool[4, 4];
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    rotateSurface[j, i] = Surface[3 - i, j];
                }
            }
            if (rotateSurface[2, 2])
            {
                rotateSurface[0, 2] = false;
                rotateSurface[1, 2] = false;
                rotateSurface[2, 2] = false;
                rotateSurface[3, 2] = false;

                rotateSurface[0, 1] = true;
                rotateSurface[1, 1] = true;
                rotateSurface[2, 1] = true;
                rotateSurface[3, 1] = true;
            }
            return rotateSurface;
        }
    }
}