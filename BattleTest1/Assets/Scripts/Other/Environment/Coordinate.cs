using UnityEditor;
using UnityEngine;
using System;

namespace Assets.Scripts.Types
{
    class Coordinate
    {
        int x;
        int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Tuple<int, int> getBoth()
        {
            Tuple<int, int> coordinate = new Tuple<int, int>(x, y);
            return coordinate;
        }

    }
}