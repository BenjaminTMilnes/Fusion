using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class CharacterMapTable : Table
    {
        public UInt16 Version { get; set; }
        public UInt16 NumberOfSubtables { get; set; }
        public IList<CharacterMapEncodingSubtable> EncodingSubtables { get; set; }
        public IList<CharacterMapSubtable> Subtables { get; set; }

        public CharacterMapTable()
        {
            EncodingSubtables = new List<CharacterMapEncodingSubtable>();
            Subtables = new List<CharacterMapSubtable>();
        }
    }
}
