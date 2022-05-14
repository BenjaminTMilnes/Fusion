using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class NameRecord
    {
        public UInt16 PlatformId { get; set; }
        public UInt16 PlatformSpecificId { get; set; }
        public UInt16 LanguageId { get; set; }
        public UInt16 NameId { get; set; }
        public UInt16 Length { get; set; }
        public UInt16 Offset { get; set; }
        public string Name { get; set; }
    }
}
