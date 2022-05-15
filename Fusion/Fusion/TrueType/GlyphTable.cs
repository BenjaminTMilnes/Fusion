using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class GlyphTable : Table
    {
        public IList<Glyph> Glyphs { get; set; }

        public GlyphTable()
        {
            Glyphs = new List<Glyph>();
        }
    }
}
