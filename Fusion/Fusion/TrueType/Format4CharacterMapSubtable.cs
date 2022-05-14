using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class Format4CharacterMapSubtable : CharacterMapSubtable
    {
        public UInt16 Length { get; set; }
        public UInt16 Language { get; set; }
        public UInt16 SegmentCount { get; set; }
        public UInt16 SearchRange { get; set; }
        public UInt16 EntrySelector { get; set; }
        public UInt16 RangeShift { get; set; }
        public IList<UInt16> EndCodes { get; set; }
        public UInt16 ReservedPad { get; set; }
        public IList<UInt16> StartCodes { get; set; }
        public IList<UInt16> IdDeltas { get; set; }
        public IList<UInt16> IdRangeOffsets { get; set; }
        public IList<UInt16> GlyphIndices { get; set; }

        public Format4CharacterMapSubtable()
        {
            EndCodes = new List<UInt16>();
            StartCodes = new List<UInt16>();
            IdDeltas = new List<UInt16>();
            IdRangeOffsets = new List<UInt16>();
            GlyphIndices = new List<UInt16>();
        }

        public UInt16 GetGlyphIndex(char c)
        {
            var characterCode = (ushort)c;

            for (var i = 0; i < SegmentCount; i++)
            {
                var start = StartCodes[i];
                var end = EndCodes[i];

                if (start <= characterCode && end >= characterCode)
                {
                    var delta = IdDeltas[i];
                    var rangeOffset = IdRangeOffsets[i];

                    if (rangeOffset == 0)
                    {
                        return (UInt16)((delta + c) % 65536);
                    }

                    var index = rangeOffset + (c - start) + i;
                    var glyphId = GlyphIndices[index];

                    if (glyphId != 0)
                    {
                        return (UInt16)((glyphId + c) % 65536);
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Whether or not this subtable has a mapping for a given character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool HasGlyphForCharacter(char c)
        {
            return GetGlyphIndex(c) > 0;
        }
    }
}
