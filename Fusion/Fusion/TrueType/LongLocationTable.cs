using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class LongLocationTable : LocationTable
    {
        public IList<UInt32> Offsets { get; set; }

        public LongLocationTable()
        {
            Offsets = new List<UInt32>();
        }
    }
}
