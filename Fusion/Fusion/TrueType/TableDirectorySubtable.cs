using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class TableDirectorySubtable
    {
        public IList<TableDirectorySubtableEntry> Entries { get; set; }

        public TableDirectorySubtable()
        {
            Entries = new List<TableDirectorySubtableEntry>();
        }
    }
}
