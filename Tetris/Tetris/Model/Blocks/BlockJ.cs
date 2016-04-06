﻿using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    internal class BlockJ : Block
    {
        public BlockJ()
        {
            /*
            0 1 0 0
            0 1 0 0
            1 1 0 0
            0 0 0 0
            */
            Surface = new[,]
            {
                {false, true, false, false},
                {false, true, false, false},
                {true, true, false, false},
                {false, false, false, false}
            };
            Color = new SolidColorBrush(Colors.MediumVioletRed);
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
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    rotateSurface[j, i] = Surface[2 - i, j];
                }
            }
            return rotateSurface;
        }
    }
}