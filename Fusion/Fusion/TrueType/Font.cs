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

        public bool HasGlyphForCharacter(char c)
        {
            foreach (var subtable in CharacterMapTable.Subtables)
            {
                if ((subtable as Format4CharacterMapSubtable).HasGlyphForCharacter(c))
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
