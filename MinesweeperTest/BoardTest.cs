﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Minesweeper;

namespace MinesweeperTests
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void Board_has_specified_properties()
        {
            var b = new Board(10, 20, 30);
            Assert.AreEqual(b.Rows, 10);
            Assert.AreEqual(b.Cols, 20);
            Assert.AreEqual(b.Mines, 30);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Only_valid_sizes_are_accepted()
        {
            new Board(0, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Only_valid_number_of_mines_are_accepted()
        {
            new Board(10, 10, 10*10 + 1);
        }

        [TestMethod]
        public void Number_of_mines_should_match_number_of_tiles_with_mines()
        {
            var board = new Board(3, 3, 5);
            var minecount = 0;

            foreach (var tile in board.Grid)
            {
                if (tile.Mine)
                    minecount++;
            }

            Assert.AreEqual(minecount, board.Mines);
        }

        [TestMethod]
        public void Opening_a_free_tile_should_return_true()
        {
            var board = Board.FromString(
                "[ ]"
            );

            bool open = board.Open(0, 0);

            Assert.IsTrue(open);
        }

        [TestMethod]
        public void Opening_a_mine_should_return_false()
        {
            var board = Board.FromString(
                "[x]"
            );

            bool open = board.Open(0, 0);

            Assert.IsFalse(open);
        }

        [TestMethod]
        public void All_adjacent_tiles_are_opened_when_field_contains_no_nearby_mines()
        {
            var board = Board.FromString(
                "[ ] [ ] [ ]",
                "[ ] [ ] [ ]",
                "[ ] [ ] [ ]"
            );

            board.Open(1, 1);

            foreach (var t in board.Grid)
            {
                Assert.AreEqual(TileStatus.OPEN, t.Status);
            }
        }

        [TestMethod]
        public void All_adjacent_tiles_are_opened_when_opening_empty_tile_in_a_corner()
        {
            var board = Board.FromString(
                "[ ] [ ]",
                "[ ] [ ]"
            );

            board.Open(0, 0);

            foreach (var t in board.Grid)
            {
                Assert.AreEqual(TileStatus.OPEN, t.Status);
            }
        }

        [TestMethod]
        public void Should_show_number_of_mines_when_opening_a_tile_with_neighboring_mines()
        {
            var board = Board.FromString(
                "[x] [ ] [ ]",
                "[ ] [x] [x]",
                "[ ] [x] [x]"
            );

            board.Open(2, 0);
            
            Assert.AreEqual(BoardOutput(
                "[ ] [ ] [ ]",
                "[ ] [ ] [ ]",
                " 2  [ ] [ ]"
                ),
                board.Print()
            );
        }

        [TestMethod]
        public void Should_mark_flagged_mine_with_capital_f()
        {
            var board = Board.FromString(
                "[x]"
            );

            board.Flag(0, 0);

            Assert.AreEqual(BoardOutput(
                "[F]"
                ),
                board.Print()
            );
        }

        [TestMethod]
        public void Flagging_an_open_tile_should_do_nothing()
        {
            var board = Board.FromString(
                "[x] [ ]"
            );

            board.Open(0, 1);
            board.Flag(0, 1);

            Assert.AreEqual(BoardOutput(
                "[ ]  1 "
                ),
                board.Print()
            );
        }

        [TestMethod]
        public void Flagging_a_second_time_should_change_status_back_to_closed()
        {
            var board = Board.FromString(
                "[ ]"
            );

            board.Flag(0, 0);
            board.Flag(0, 0);

            Assert.AreEqual(BoardOutput(
                "[ ]"
                ),
                board.Print()
            );
        }

        private static string BoardOutput(params string[] s)
        {
            return string.Join(Environment.NewLine, s) + Environment.NewLine;
        }
    }
}
