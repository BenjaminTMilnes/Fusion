using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public abstract class CharacterMapSubtable
    {
        public UInt16 Format { get; set; }

        public abstract int GetGlyphIndex(char c);
        public abstract bool HasGlyphForCharacter(char c);
    }
}
