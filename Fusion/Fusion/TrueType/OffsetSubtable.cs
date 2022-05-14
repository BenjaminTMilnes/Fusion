using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class OffsetSubtable
    {
        public UInt32 ScalerType { get; set; }
        public UInt16 NumberOfTables { get; set; }
        public UInt16 SearchRange { get; set; }
        public UInt16 EntrySelector { get; set; }
        public UInt16 RangeShift { get; set; }
    }
}
