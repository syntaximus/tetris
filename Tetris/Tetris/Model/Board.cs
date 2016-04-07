﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Tetris.Model.Blocks;
using Tetris.ViewModels;

namespace Tetris.Model
{
    public class Board : IDisposable
    {
        #region PROPERTIES
        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po przesunięciu klocka o jeden w dół.
        /// </summary>

        public bool CanCurrentBlockMoveDown
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y + i + 1 < 20 && CurrentBlock.X - 2 + j < 10 &&
                                GameBoard[CurrentBlock.Y + i + 1, CurrentBlock.X - 2 + j] != null &&
                                GameBoard[CurrentBlock.Y + i + 1, CurrentBlock.X - 2 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po przesunięciu aktualnego bloku w prawo.
        /// </summary>
        public bool CanCurrentBlockMoveLeft
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y + i < 20 && CurrentBlock.X - 3 + j >= 0 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 3 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 3 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po przesunięciu aktualnego bloku w lewo.
        /// </summary>
        public bool CanCurrentBlockMoveRight
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y < 20 && CurrentBlock.X - 1 + j < 10 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 1 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 1 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy nie dojdzie do koloizji aktualnie spadającego bloku z jakimś innym blokiem po wykonaniu obrotu przez aktualny blok.
        /// </summary>
        public bool CanCurrentBlockRotate
        {
            get
            {
                bool[,] surface = CurrentBlock.ShowRotate();
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (surface[i, j])
                        {
                            if (CurrentBlock.Y < 20 && CurrentBlock.X - 2 + j < 10 && CurrentBlock.X - 2 + j >= 0 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;

            }
        }

        /// <summary>
        /// Właściwość która zwarca informacje czy blok można postawić na planszy (czy nie zachodzi kolizja z innymi blokami).
        /// </summary>
        public bool CanPlaceCurrentBlock
        {
            get
            {
                for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
                {
                    for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                    {
                        if (CurrentBlock.Surface[i, j])
                        {
                            if (CurrentBlock.Y + i < 20 && CurrentBlock.X - 2 + j >= 0 &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j] != null &&
                                GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j].Active)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        public int NextBlock { get; set; }

        public Field[,] GameBoard { get; set; }

        public Block CurrentBlock { get; set; }
        #endregion

        #region PUBLIC METHODS
        public Board()
        {
            GameBoard = new Field[20, 10];
            Random rand = new Random();
            NextBlock = rand.Next(0, 6);
        }
        public void CurrentBlockMoveLeft()
        {
            if (CanCurrentBlockMoveLeft)
            {
                CurrentBlock.TryMoveLeft();
            }
        }
        public void CurrentBlockMoveRight()
        {
            if (CanCurrentBlockMoveRight)
            {
                CurrentBlock.TryMoveRight();
            }
        }
        public void CurrentBlockRotate()
        {
            if (CanCurrentBlockRotate)
            {
                CurrentBlock.TryRotate();
            }
        }
        public bool CurrentBlockMoveDown()
        {
            if (CanCurrentBlockMoveDown && CurrentBlock.TryMoveDown())
            {
                return true;
            }
            else
            {
                SaveCurrentBlockInBoard();
                return GenerateNewCurrentBlock();
            }
        }
        public bool GenerateNewCurrentBlock()
        {
            Random rand = new Random();
            IEnumerable<Block> exporters = (typeof(Block)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Block)) && !t.IsAbstract)
                .Select(t => (Block)Activator.CreateInstance(t)));
            CurrentBlock = exporters.ToList()[NextBlock];

            CurrentBlock.X = 5;
            CurrentBlock.Y = 0;
            NextBlock = rand.Next(0, exporters.Count());

            return CanPlaceCurrentBlock;
        }
        public int CheckRows()
        {
            int deletedRows = 0;
            for (int i = 0; i != 20; i++)
            {
                for (int j = 0; j != 10; j++)
                {
                    if (GameBoard[i, j] == null || !GameBoard[i, j].Active)
                        break;
                    if (j == 9)
                    {
                        DeleteRow(i);
                        deletedRows++;
                    }
                }
            }
            return deletedRows;
        }
        #endregion

        #region PRIVATE METHODS
        private void DeleteRow(int row)
        {
            for (int i = row; i != 0; i--)
            {
                for (int j = 0; j != 10; j++)
                {
                    GameBoard[i, j] = GameBoard[i - 1, j];
                }
            }
        }
        private void SaveCurrentBlockInBoard()
        {
            for (int i = 0; i != CurrentBlock.Surface.GetLength(0); i++)
            {
                for (int j = 0; j != CurrentBlock.Surface.GetLength(1); j++)
                {
                    if (CurrentBlock.Surface[i, j])
                    {
                        GameBoard[CurrentBlock.Y + i, CurrentBlock.X - 2 + j] = new Field(CurrentBlock.Color);
                    }
                }
            }
        }
        #endregion

        #region IDisposable
        private bool _disposed = false;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
                GameBoard = null;
                CurrentBlock = null;

            }
            _disposed = true;
        }
        #endregion
    }
}