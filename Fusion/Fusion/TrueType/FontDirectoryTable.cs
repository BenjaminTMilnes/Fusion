using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class FontDirectoryTable : Table
    {
        public OffsetSubtable OffsetSubtable { get; set; }
        public TableDirectorySubtable TableDirectorySubtable { get; set; }
    }
}
