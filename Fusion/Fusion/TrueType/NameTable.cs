using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class NameTable : Table
    {
        public UInt16 Format { get; set; }
        public UInt16 Count { get; set; }
        public UInt16 StringOffset { get; set; }
        public IList<NameRecord> NameRecords { get; set; }

        public NameTable()
        {
            NameRecords = new List<NameRecord>();
        }
    }
}
