using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class SimpleGlyph : Glyph
    {
        public Int16 NumberOfContours { get; set; }
        public Int16 MinimumX { get; set; }
        public Int16 MinimumY { get; set; }
        public Int16 MaximumX { get; set; }
        public Int16 MaximumY { get; set; }
        public IList<UInt16> EndPointsOfContours { get; set; }
        public UInt16 InstructionLength { get; set; }
        public IList<byte> Instructions { get; set; }
        public IList<byte> Flags { get; set; }
        public IList<object> XCoordinates { get; set; }
        public IList<object> YCoordinates { get; set; }

        public SimpleGlyph()
        {
            EndPointsOfContours = new List<UInt16>();
            Instructions = new List<byte>();
            Flags = new List<byte>();
            XCoordinates = new List<object>();
            YCoordinates = new List<object>();
        }
    }
}
