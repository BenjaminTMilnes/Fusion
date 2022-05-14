using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class TableDirectorySubtableEntry
    {
        public string Tag { get; set; }
        public UInt32 CheckSum { get; set; }
        public UInt32 Offset { get; set; }
        public UInt32 Length { get; set; }
    }
}
