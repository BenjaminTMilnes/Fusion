using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class Format12CharacterMapSubtable : CharacterMapSubtable
    {
        public UInt32 Length { get; set; }
        public UInt32 Language { get; set; }
        public UInt32 NumberOfGroups { get; set; }
        public IList<Tuple<UInt32, UInt32, UInt32>> Groups { get; set; }
        public IDictionary<char, int> Mappings { get; set; }

        public Format12CharacterMapSubtable()
        {
            Groups = new List<Tuple<UInt32, UInt32, UInt32>>();
            Mappings = new Dictionary<char, int>();
        }

        public override int GetGlyphIndex(char c)
        {
            return Mappings[c];
        }

        public override bool HasGlyphForCharacter(char c)
        {
            return Mappings.ContainsKey(c);
        }
    }
}
