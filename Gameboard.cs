using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rduplooyBattleshipFinal
{
    internal class Gameboard
    {
        char[,] board = new char[10, 10];

        // Clears board by filling all possible locations with '~'
        public void ClearBoard()
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    board[row, col] = '~';
                }
            }
        }

        // Prints the board but replaces any S characters with ~ to not reveal ships
        public void PrintBoard()
        {
            PrintColNumbers();

            for (int row = 0; row < 10; row++)
            {

                Console.Write(row + 1 + "\t");

                for (int col = 0; col < 10; col++)
                {
                    if (board[col, row].Equals('S'))
                    {
                        Console.Write('~' + " ");
                    }
                    else
                    {
                        Console.Write(board[col, row] + " ");
                    }
                }
                Console.WriteLine();
            }

            
        }

        // Prints the board without hiding S characters to reveal ship locations
        public void HackPrintBoard()
        {
            PrintColNumbers();

            for (int row = 0; row < 10; row++)
            {

                Console.Write(row + 1 + "\t");

                for (int col = 0; col < 10; col++)
                {
                    Console.Write(board[col, row] + " ");
                }
                Console.WriteLine();
            }
        }

        // Prints column numbers for displaying board
        private void PrintColNumbers()
        {
            Console.Write("\t");
            for (int row = 0; row < 10; row++)
            {
                Console.Write(row + 1 + " ");
            }
            Console.WriteLine();
        }

        // Replace a tile with input character
        public void ReplaceTile((int, int) coords, char letter)
        {
            int x = coords.Item1;
            int y = coords.Item2;
            board[x, y] = letter;
        }

        public void SpawnShip(Ship shipToSpawn)
        {
            (int, int)[] spawnCoordinates = shipToSpawn.GetCoordinates();
            for (int i = 0; i < shipToSpawn.GetLength(); i++)
            {
                ReplaceTile(spawnCoordinates[i], 'S');
            }
        }

        // Return the character at a coordinate location
        public char GetTileValue((int, int) coords)
        {
            int x = coords.Item1;
            int y = coords.Item2;
            return board[x, y];
        }
    }
}
