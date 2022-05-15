using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class Font
    {
        public FontDirectoryTable FontDirectoryTable { get; set; }
        public HeadTable HeadTable { get; set; }
        public NameTable NameTable { get; set; }
        public CharacterMapTable CharacterMapTable { get; set; }
        public HorizontalHeadTable HorizontalHeadTable { get; set; }
        public HorizontalMetricsTable HorizontalMetricsTable { get; set; }
        public MaximumParametersTable MaximumParametersTable { get; set; }
        public LocationTable LocationTable { get; set; }
        public GlyphTable GlyphTable { get; set; }

        public Font()
        {
        }

        public Glyph GetGlyphForCharacter(char c)
        {
            foreach (var subtable in CharacterMapTable.Subtables)
            {
                if (subtable.HasGlyphForCharacter(c))
                {
                    var index = subtable.GetGlyphIndex(c);

                    return GlyphTable.Glyphs[index];
                }
            }

            throw new Exception($"Font does not have glyph for '{c}'.");
        }

        public bool HasGlyphForCharacter(char c)
        {
            foreach (var subtable in CharacterMapTable.Subtables)
            {
                if (subtable.HasGlyphForCharacter(c))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasGlyphsForCharacters(char[] cs)
        {
            foreach (var c in cs)
            {
                if (!HasGlyphForCharacter(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
