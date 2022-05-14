using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    /// <summary>
    /// A fixed-point number consisting of 16 bits for the integral part and 16 bits for the fractional part.
    /// </summary>
    public class Fixed32
    {
        public Int16 IntegralPart { get; set; }
        public Int16 FractionalPart { get; set; }
    }
}
