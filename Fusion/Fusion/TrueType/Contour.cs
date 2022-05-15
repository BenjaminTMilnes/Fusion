using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class Contour
    {
        public IList<Point> Points { get; set; }

        public Contour()
        {
            Points = new List<Point>();
        }
    }
}
