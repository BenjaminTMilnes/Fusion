using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class HorizontalHeadTable : Table
    {
        public Fixed32 Version { get; set; }
        public Int16 Ascent { get; set; }
        public Int16 Descent { get; set; }
        public Int16 LineGap { get; set; }
        public UInt16 MaximumAdvanceWidth { get; set; }
        public Int16 MinimumLeftSideBearing { get; set; }
        public Int16 MinimumRightSideBearing { get; set; }
        public Int16 MaximumXExtent { get; set; }
        public Int16 CaretSlopeRise { get; set; }
        public Int16 CaretSlopeRun { get; set; }
        public Int16 CaretOffset { get; set; }
        public Int16 MetricDataFormat { get; set; }
        public UInt16 NumberOfLongHorizontalMetrics { get; set; }
    }
}
