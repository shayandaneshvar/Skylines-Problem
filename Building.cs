using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skylines_Problem
{
    public class Building
    {
        public int Right { get; set; }

        public int Left { get; set; }

        public int Height { get; set; }

        public int Width() => Right - Left;

        public Building(int left, int right, int height)
        {
            Left = left;
            Right = right;
            Height = height;
        }

        public override String ToString()
        {
            return $"({Left}, {Right}, {Height})";
        }
    }
}
