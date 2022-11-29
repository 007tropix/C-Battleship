using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace rduplooyBattleshipFinal
{
    internal class Game
    {

        Gameboard gameboard = new Gameboard();
        List<(int, int)> guessedCoords = new List<(int, int)>();
        Ship[] shipList = new Ship[6];
        bool hackMode = false;
        int totalHits = 0;


        public void StartGame()
        {
            gameboard.ClearBoard();
            GenerateAllShips();
            gameboard.PrintBoard();
            GameLoop();
        }

        public void EndGame()
        {
            Console.Clear();
            Console.WriteLine("YOU WIN!!!");
            Console.WriteLine("Would you like to play again? (y/n)");
            if (Console.ReadLine()![0].Equals('y'))
            {
                guessedCoords = new List<(int, int)>();
                shipList = new Ship[6];
                hackMode = false;
                totalHits = 0;
                StartGame();
            }
            else
            {
                System.Environment.Exit(0);
            }
        }

        public void GameLoop()
        {
            
            bool gameLoop = true;

            while (gameLoop)
            {

                // Check if the total number of hits equals 19 (total tiles of ships)
                if (totalHits == 19)
                {
                    EndGame();
                }

                (int, int) attemptCoords = GetGuess();

                // Check if attempted coordinates are equal to cheat coordinates
                

                if (hackMode == false)
                {
                    Console.Clear();
                    if (attemptCoords.Item1 != 99 && attemptCoords.Item2 != 99)
                    {
                        AttemptHit(attemptCoords);
                    }
                    gameboard.PrintBoard();
                }
                else
                {
                    Console.Clear();
                    if (attemptCoords.Item1 != 99 && attemptCoords.Item2 != 99)
                    {
                        AttemptHit(attemptCoords);
                    }
                    gameboard.HackPrintBoard();
                }
            }
            
        }

        public void EnableHack()
        {
            Console.WriteLine("HACKS ENABLED");
            hackMode = true;
        }

        public (int, int) GetGuess()
        {
            Console.Write("X Coord: ");
            int x = Convert.ToInt32(Console.ReadLine());
            x = x - 1;

            Console.Write("Y Coord: ");
            int y = Convert.ToInt32(Console.ReadLine());
            y = y - 1;

            if (x == 99 && y == 99)
            {
                EnableHack();
            }


            // Check if coordinates have already been guessed before
            for (int i = 0; i < guessedCoords.Count; i++)
            {
                if (x == guessedCoords[i].Item1 && y == guessedCoords[i].Item2)
                {
                    Console.WriteLine("That coordinate has already been guessed.");
                    GetGuess();
                }
                
            }
            guessedCoords.Add((x, y));
            return (x, y);
        }

        public bool AttemptHit((int, int) attemptCoords)
        {
            for (int i = 0; i < shipList.Length; i++)
            {
                for (int j = 0; j < shipList[i].GetLength(); j++)
                {
                    //Console.WriteLine(shipList[i]);
                    (int, int)[] currentShipCoords = shipList[i].GetCoordinates();
                    if (attemptCoords.Item1 == currentShipCoords[j].Item1 && attemptCoords.Item2 == currentShipCoords[j].Item2)
                    {
                        shipList[i].HitShip();
                        if (shipList[i].IsSunk())
                        {
                            Console.WriteLine($"YOU SUNK A {shipList[i].GetName()}");
                        }

                        Console.WriteLine("HIT");
                        gameboard.ReplaceTile((attemptCoords.Item1, attemptCoords.Item2), 'O');
                        totalHits++;
                        return (true);
                    }
                }
            }
            Console.WriteLine("MISS");
            gameboard.ReplaceTile((attemptCoords.Item1, attemptCoords.Item2), 'X');
            return false;


        }

        #region Ship Generation

        public void GenerateAllShips()
        {
            Ship carrier = GenerateShip(5);
            gameboard.SpawnShip(carrier);
            shipList[0] = carrier;
            //gameboard.PrintBoard();

            Ship battleship = GenerateShip(4);
            gameboard.SpawnShip(battleship);
            shipList[1] = battleship;
            //gameboard.PrintBoard();

            Ship submarine1 = GenerateShip(3);
            gameboard.SpawnShip(submarine1);
            shipList[2] = submarine1;
            //gameboard.PrintBoard();

            Ship submarine2 = GenerateShip(3);
            gameboard.SpawnShip(submarine2);
            shipList[3] = submarine2;
            //gameboard.PrintBoard();

            Ship destroyer1 = GenerateShip(2);
            gameboard.SpawnShip(destroyer1);
            shipList[4] = destroyer1;
            //gameboard.PrintBoard();

            Ship destroyer2 = GenerateShip(2);
            gameboard.SpawnShip(destroyer2);
            shipList[5] = destroyer2;
            //gameboard.PrintBoard();


        }

        public Ship GenerateShip(int length)
        {
            // Generate random coordinate for new ship
            (int, int) randomCoords = GenerateCoord();

            // Check all potential directions from random coordinate
            char randomBearing = GenerateBearing(randomCoords, length);

            // If no bearings are availiable restart ship generating process
            if (randomBearing == 'F')
            {
                GenerateShip(length);
            }

            Ship newShip = new Ship(length, randomCoords.Item1, randomCoords.Item2, randomBearing);
            return newShip;




        }

        public (int, int) GenerateCoord()
        {
            Random randomGenerator = new Random();
            int x = randomGenerator.Next(0, 10);
            int y = randomGenerator.Next(0, 10);
            (int, int) coords = (x, y);

            // If the generated coordinate already has a ship on it then continue to generate new coords
            while (gameboard.GetTileValue(coords).Equals('S'))
            {
                x = randomGenerator.Next(0, 10);
                y = randomGenerator.Next(0, 10);
                coords = (x, y);
                //Console.WriteLine($"X: {x} Y: {y}");
            }

            return (x, y);

        }

        // GenerateBearing will check all directions around the generated point for the length of the ship
        // GenerateBearing will either return N, E, S, W or and F if all directions are unavailiable
        public char GenerateBearing((int, int) coords, int length)
        {
            if (CheckNorth(length, coords))
            {
                return 'N';
            }

            // If east is clear then return E
            else if (CheckEast(length, coords))
            {
                return 'E';
            }

            // If south is clear then return S
            else if (CheckSouth(length, coords))
            {
                return 'S';
            }

            // If west is clear then return W
            else if (CheckWest(length, coords))
            {
                return 'W';
            }

            // If no bearing is clear then return F
            else
            {
                return 'F';
            }

            #region Bearing Checks
            bool CheckNorth(int length, (int, int) coords)
            {
                // Check if ship will fall off board
                if ((coords.Item2 - length) < 0)
                {
                    return false;
                }

                // Check that all tiles in ship path are not already taken
                for (int i = 0; i < length; i++)
                {
                    if (gameboard.GetTileValue(coords).Equals('S'))
                    {
                        return false;
                    }

                    else
                    {
                        coords.Item2 -= 1;
                    }


                }
                return true;
            }

            bool CheckEast(int length, (int, int) coords)
            {
                // Check if ship will fall off board
                if ((coords.Item1 + length) > 10)
                {
                    return false;
                }

                // Check that all tiles in ship path are not already taken
                for (int i = 0; i < length; i++)
                {
                    if (gameboard.GetTileValue(coords).Equals('S'))
                    {
                        return false;
                    }

                    else
                    {
                        coords.Item1 += 1;
                    }


                }
                return true;
            }

            bool CheckSouth(int length, (int, int) coords)
            {
                // Check if ship will fall off board
                if ((coords.Item2 + length) > 10)
                {
                    return false;
                }

                // Check that all tiles in ship path are not already taken
                for (int i = 0; i < length; i++)
                {
                    if (gameboard.GetTileValue(coords).Equals('S'))
                    {
                        return false;
                    }

                    else
                    {
                        coords.Item2 += 1;
                    }


                }
                return true;
            }

            bool CheckWest(int length, (int, int) coords)
            {
                // Check if ship will fall off board
                if ((coords.Item1 - length) < 0)
                {
                    return false;
                }

                // Check that all tiles in ship path are not already taken
                for (int i = 0; i < length; i++)
                {
                    if (gameboard.GetTileValue(coords).Equals('S'))
                    {
                        return false;
                    }

                    else
                    {
                        coords.Item1 -= 1;
                    }


                }
                return true;
            }
            #endregion
        }

        #endregion
    }
}
