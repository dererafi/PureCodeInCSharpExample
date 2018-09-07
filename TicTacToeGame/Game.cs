﻿using System;

namespace TicTacToeGame
{
    [IsPure]
    public sealed class Game
    {
        public static int numberOfTimesXWon;
        public static int numberOfTimesOWon;

        private readonly Action<string> writeToConsole;
        private readonly Func<string> readFromConsole;

        public Game(Action<string> writeToConsole, Func<string> readFromConsole)
        {
            this.writeToConsole = writeToConsole;
            this.readFromConsole = readFromConsole;
        }

        public void PlayMultipleTimes()
        {
            do
            {
                PlayGame();

                writeToConsole("Number of times X won: " + numberOfTimesXWon);
                writeToConsole("Number of times O won: " + numberOfTimesOWon);
            } while (PlayAgain());
        }

        private bool PlayAgain()
        {

            bool IsYes(string input) => input.Equals("yes");
            bool IsNo(string input) => input.Equals("no");

            string line;
            do
            {
                writeToConsole("Play again? yes/no");

                line = readFromConsole();
            } while (!IsYes(line) && !IsNo(line));

            return IsYes(line);
        }

        private void PlayGame()
        {
            Board board = new Board();

            Player currentPlayer = Player.X;

            void SwichPlayer()
            {
                currentPlayer = currentPlayer == Player.X ? Player.O : Player.X;
            }

            while (!board.Winner.HasValue && !board.IsFull())
            {
                writeToConsole("It's player " + currentPlayer + "'s turn");

                PlayOneTurn(currentPlayer, board); 

                SwichPlayer();
            }

            if (board.Winner.HasValue)
            {
                writeToConsole(board.Winner + " is a winner");
            }
            else
            {
                writeToConsole("Game over. No winner");
            }
        }

        private void PlayOneTurn(Player currentPlayer, Board board)
        {
            while (true)
            {
                var (row, column) = ReadRowAndColumnFromConsole();

                if (!PlayBoard(currentPlayer, board, row, column))
                {
                    writeToConsole("Cell is not empty");
                }
                else
                {
                    break;
                }
            }
        }

        private (int row, int column) ReadRowAndColumnFromConsole()
        {
            writeToConsole("Please specify row (1-3):");

            int row;
            while (!PureInt.TryParseCultureInvariant(readFromConsole(), out row) || row < 1 || row > 3)
                writeToConsole("Invalid value");

            writeToConsole("Please specify column (1-3):");

            int column;
            while (!PureInt.TryParseCultureInvariant(readFromConsole(), out column) || column < 1 || column > 3)
                writeToConsole("Invalid value");

            return (row - 1, column - 1);
        }

        public bool PlayBoard(Player currentPlayer, Board board, int row, int column)
        {
            var cell = board.GetCell(row, column);

            if (cell == CellStatus.HasO || cell == CellStatus.HasX)
                return false;

            if (currentPlayer == Player.O)
                board.SetCell(row, column, CellStatus.HasO);
            else 
                board.SetCell(row, column, CellStatus.HasX);

            board.PrintToConsole(writeToConsole);

            return true;
        }
    }
}