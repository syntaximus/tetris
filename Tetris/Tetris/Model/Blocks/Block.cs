﻿using System.Windows.Media;

namespace Tetris.Model.Blocks
{
    public abstract class Block
    {
        public int X;
        public int Y;

        public Brush Color;
        public bool[,] Surface;
        public abstract void Rotate();
        public abstract bool[,] ShowRotate();

        public bool CanRotate()
        {
            bool[,] surface = ShowRotate();
            if (X == 0)
                return false;
            for (int i = 0; i != surface.GetLength(1); i++)
            {
                if (surface[i, 0])
                {
                    if (X + 1 <= 2)
                        return false;
                }
                if (surface[i, 1])
                {
                    if (X + 1 <= 1)
                        return false;
                }
            }
            if (X == 10)
                return false;

            for (int i = 0; i != surface.GetLength(1); i++)
            {
                if (surface[i, 3])
                {
                    if (X - 1 >= 8)
                        return false;
                }

                if (surface[i, 2])
                {
                    if (X - 1 >= 9)
                        return false;
                }
                if (surface[1, i])
                {
                    if (Y - 1 >= 18)
                        return false;
                }
                if (surface[2, i])
                {
                    if (Y - 1 >= 17)
                        return false;
                }
                if (surface[3, i])
                {
                    if (Y - 1 >= 16)
                        return false;
                }


            }
            return true;
        }

        public bool CanDown()
        {
            for (int i = 0; i != Surface.GetLength(0); i++)
            {
                if (Surface[1, i])
                {
                    if (Y >= 18)
                        return false;
                }
                if (Surface[2, i])
                {
                    if (Y >= 17)
                        return false;
                }
                if (Surface[3, i])
                {
                    if (Y >= 16)
                        return false;
                }

            }
            return true;
        }

        public void Left()
        {
            if (X == 0)
                return;
            for (int i = 0; i != Surface.GetLength(1); i++)
            {
                if (Surface[i, 0])
                {
                    if (X <= 2)
                        return;
                }
                if (Surface[i, 1])
                {
                    if (X <= 1)
                        return;
                }
            }
            X--;
        }

        public void Right()
        {
            if (X == 10)
                return;

            for (int i = 0; i != Surface.GetLength(1); i++)
            {
                if (Surface[i, 3])
                {
                    if (X >= 8)
                        return;
                }

                if (Surface[i, 2])
                {
                    if (X >= 9)
                        return;
                }


            }
            X++;
        }

        public void MoveDown()
        {
            if (CanDown())
            {
                Y++;
            }
        }
    }
}