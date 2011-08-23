﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minesweeper
{
    public class Board
    {
        public int Mines { get; private set; }

        public Tile[,] Grid { get; private set; }

        public Board(int rows, int cols)
        {
            if (rows < 1 || cols < 1)
                throw new ArgumentException("Board size can not be less than 1x1");
            SetDimensions(rows, cols);
            SetNumberOfMines(0);
        }

        private void SetDimensions(int rows, int cols)
        {
            Grid = new Tile[rows, cols];
            for (int r = 0; r < rows; ++r)
            {
                for (int c = 0; c < cols; ++c)
                {
                    Grid[r, c] = new Tile();
                }
            }
        }

        public void SetNumberOfMines(int count)
        {
            if (count > Rows * Cols)
                throw new ArgumentException("Illegal number of mines");
            Mines = count;
        }

        public bool Open(int row, int col)
        {
            var open = Grid[row, col].Open();
            if (!open) return false;
            if (open)
            {
                for (int r = row - 1; r <= row + 1; ++r)
                {
                    for (int c = col - 1; c <= col + 1; ++c)
                    {
                        if (IsInside(r, c))
                            Grid[r, c].Open();
                    }
                }
            }
            return true;
        }

        private bool IsInside(int r, int c)
        {
            return r >= 0 && c >= 0 && r < Rows && c < Cols;
        }

        public int Rows
        {
            get { return Grid.GetLength(0); }
        }

        public int Cols
        {
            get { return Grid.GetLength(1); }
        }

        /**
         * Creates a board from string input like this
         * [ ] [x] [ ]
         * [x] [ ] [ ]
         * [ ] [x] [x]
         */
        public static Board FromString(params string[] s)
        {
            var rows = s.Length;
            var cols = s[0].Count((c) => c == '[');

            var board = new Board(rows, cols);
            for (var r = 0; r < rows; ++r)
            {
                for (var c = 0; c < cols; ++c)
                {
                    var minechar = s[r].ElementAt(1 + "] [ ".Length * c);
                    if (minechar == 'x')
                        board.Grid[r, c].Mine = true;
                }
            }
            return board;
        }
    }
}
