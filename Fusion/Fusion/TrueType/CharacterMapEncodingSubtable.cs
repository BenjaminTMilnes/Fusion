using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public enum Platform
    {
        Unicode = 0,
        Macintosh = 1,
        Microsoft = 3
    }

    public class CharacterMapEncodingSubtable
    {
        public UInt16 PlatformId { get; set; }
        public UInt16 PlatformSpecificId { get; set; }
        public UInt32 Offset { get; set; }
    }
}
