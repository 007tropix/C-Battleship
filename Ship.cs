using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rduplooyBattleshipFinal
{
    internal class Ship
    {
        int length, hits, x, y;
        char bearing;
        string name;
        (int, int)[] shipCoordinates;

        public Ship(int length, int x, int y, char bearing)
        {
            this.length = length;
            this.x = x;
            this.y = y;
            this.bearing = bearing;

            shipCoordinates = new (int, int)[length];
            shipCoordinates = CalcCoordinates(this);

            #region Determine Name
            if (length == 5)
            {
                name = "Carrier";
            }
            else if (length == 4)
            {
                name = "Battleship";
            }
            else if (length == 3)
            {
                name = "Submarine";
            }
            else
            {
                name = "Destroyer";
            }
            #endregion


        }

        public int GetLength()
        {
            return length;
        }

        public string GetName()
        {
            return name;
        }

        public (int, int)[] GetCoordinates()
        {
            return shipCoordinates;
        }

        public (int, int)[] CalcCoordinates(Ship currentShip)
        {
            (int, int)[] coordList = new (int, int)[currentShip.length];
            int x = currentShip.x;
            int y = currentShip.y;

            if (bearing.Equals('N'))
            {
                //Console.WriteLine("NORTH");
                for (int i = 0; i < currentShip.length; i++)
                {
                    coordList[i] = (x, y);
                    y--;
                }
                return coordList;
            }
            else if (bearing.Equals('E'))
            {
                //Console.WriteLine("EAST");
                for (int i = 0; i < length; i++)
                {
                    coordList[i] = (x, y);
                    x++;
                }
                return coordList;
            }
            else if (bearing.Equals('S'))
            {
                //Console.WriteLine("SOUTH");
                for (int i = 0; i < length; i++)
                {
                    coordList[i] = (x, y);
                    y++;
                }
                return coordList;
            }
            else
            {
                //Console.WriteLine("WEST");
                for (int i = 0; i < length; i++)
                {
                    coordList[i] = (x, y);
                    x--;
                }
                return coordList;
            }
        }

        public bool IsSunk()
        {
            if (hits == length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HitShip()
        {
            hits++;
        }

        
    }
}
