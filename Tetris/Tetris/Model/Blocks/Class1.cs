//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Media;

//namespace Tetris.Model.Blocks
//{
//    internal class BlockTestow : Block
//    {
//        public BlockTestowy()
//        {
//            /*
//            1 1 0 0
//            0 1 1 0
//            0 0 0 0
//            0 0 0 0

//            0 1 0 0
//            1 1 0 0
//            1 0 0 0
//            0 0 0 0
//            */
//            Surface = new[,]
//            {
//                {true, true, false, true},
//                {false, true, true, false},
//                {false, false, true, false},
//                {false, false, false, true}
//            };
//            Color = new SolidColorBrush(Colors.DarkOrange);
//        }

//        public override bool TryRotate()
//        {
//            if (CanRotate)
//            {
//                Surface = ShowRotate();
//                return true;
//            }
//            return false;
//        }


//        public override bool[,] ShowRotate()
//        {
//            var rotateSurface = new bool[4, 4];
//            for (var i = 0; i < 4; i++)
//            {
//                for (var j = 0; j < 4; j++)
//                {
//                    rotateSurface[j, i] = Surface[3 - i, j];
//                }
//            }
//            return rotateSurface;
//        }
//    }
//}
