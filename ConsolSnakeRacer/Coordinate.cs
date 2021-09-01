using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolSnakeRacer
{
    // 2 wymiarowa matryca
    // klasa do przechowywania wspolrzednych
    public class Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }

        public Coordinate() { x = 0; y = 0; }

        public Coordinate(int newX, int newY)
        {
            x = newX;
            y = newY;
        }
    }
}
