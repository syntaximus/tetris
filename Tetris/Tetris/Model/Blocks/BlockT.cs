using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    internal class BlockT : Block
    {
        public BlockT()
        {
            /*
            1 1 1 0   0 1 0 0   0 1 0 0   1 0 0 0
            0 1 0 0   1 1 0 0   1 1 1 0   1 1 0 0
            0 0 0 0   0 1 0 0   0 0 0 0   1 0 0 0
            0 0 0 0   0 0 0 0   0 0 0 0   0 0 0 0
            */
            Surface = new[,]
            {
                {true, true, true, false},
                {false, true, false, false},
                {false, false, false, false},
                {false, false, false, false}
            };
            Color = new SolidColorBrush(Colors.Gray);
        }

        public override void Rotate()
        {
            Surface = ShowRotate();
        }

        public bool[,] ShowRotate()
        {
            var rotateSurface = new bool[4, 4];
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    rotateSurface[j, i] = Surface[2 - i, j];
                }
            }
            if (rotateSurface[2, 2])
            {
                rotateSurface[2, 2] = false;
                rotateSurface[1, 2] = false;
                rotateSurface[0, 2] = false;

                rotateSurface[2, 1] = true;
                rotateSurface[1, 1] = true;
                rotateSurface[0, 1] = true;

                rotateSurface[1, 0] = true;
            }

            return rotateSurface;
        }
    }
}