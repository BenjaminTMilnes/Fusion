using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class HeadTable : Table
    {
        public Fixed32 Version { get; set; }
        public Fixed32 FontRevision { get; set; }
        public UInt32 CheckSumAdjustment { get; set; }
        public UInt32 MagicNumber { get; set; }
        public UInt16 Flags { get; set; }
        public UInt16 UnitsPerEm { get; set; }
        public Int64 DateCreated { get; set; }
        public Int64 DateLastModified { get; set; }
        public Int16 MinimumX { get; set; }
        public Int16 MaximumX { get; set; }
        public Int16 MinimumY { get; set; }
        public Int16 MaximumY { get; set; }
        public UInt16 MacStyle { get; set; }
        public UInt16 LowestRecommendedPPEM { get; set; }
        public Int16 FontDirectionHint { get; set; }
        public Int16 IndexToLocFormat { get; set; }
        public Int16 GlyphDataFormat { get; set; }
    }
}
