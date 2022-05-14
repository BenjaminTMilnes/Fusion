using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class FontLoader
    {
        public static Font LoadFont(string filePath)
        {
            var font = new Font();

            using (var reader1 = new FontReader(File.OpenRead(filePath)))
            {
                var fontDirectoryTable = new FontDirectoryTable();
                var offsetSubtable = new OffsetSubtable();
                var tableDirectorySubtable = new TableDirectorySubtable();

                offsetSubtable.ScalerType = reader1.ReadUInt32BigEndian();
                offsetSubtable.NumberOfTables = reader1.ReadUInt16BigEndian();
                offsetSubtable.SearchRange = reader1.ReadUInt16BigEndian();
                offsetSubtable.EntrySelector = reader1.ReadUInt16BigEndian();
                offsetSubtable.RangeShift = reader1.ReadUInt16BigEndian();

                uint characterMapTableOffset = 0;
                uint characterMapTableLength = 0;

                for (var i = 0; i < offsetSubtable.NumberOfTables; i++)
                {
                    var entry = new TableDirectorySubtableEntry();

                    entry.Tag = reader1.ReadASCII(4);
                    entry.CheckSum = reader1.ReadUInt32BigEndian();
                    entry.Offset = reader1.ReadUInt32BigEndian();
                    entry.Length = reader1.ReadUInt32BigEndian();

                    tableDirectorySubtable.Entries.Add(entry);

                    if (entry.Tag == "cmap")
                    {
                        characterMapTableOffset = entry.Offset;
                        characterMapTableLength = entry.Length;
                    }
                }

                fontDirectoryTable.OffsetSubtable = offsetSubtable;
                fontDirectoryTable.TableDirectorySubtable = tableDirectorySubtable;

                font.FontDirectoryTable = fontDirectoryTable;

                reader1.Seek(characterMapTableOffset);

                var characterMapTable = new CharacterMapTable();

                characterMapTable.Version = reader1.ReadUInt16BigEndian();
                characterMapTable.NumberOfSubtables = reader1.ReadUInt16BigEndian();

                for (var i = 0; i < characterMapTable.NumberOfSubtables; i++)
                {
                    var encodingSubtable = new CharacterMapEncodingSubtable();

                    encodingSubtable.PlatformId = reader1.ReadUInt16BigEndian();
                    encodingSubtable.PlatformSpecificId = reader1.ReadUInt16BigEndian();
                    encodingSubtable.Offset = reader1.ReadUInt32BigEndian();

                    characterMapTable.EncodingSubtables.Add(encodingSubtable);
                }

                foreach (var encodingSubtable in characterMapTable.EncodingSubtables)
                {
                    if (encodingSubtable.PlatformId == 0 && encodingSubtable.PlatformSpecificId == 4)
                    {
                        reader1.Seek(encodingSubtable.Offset);

                        var start = reader1.Position;

                        var format4Table = new Format4CharacterMapSubtable();

                        format4Table.Format = reader1.ReadUInt16BigEndian();
                        format4Table.Length = reader1.ReadUInt16BigEndian();
                        format4Table.Language = reader1.ReadUInt16BigEndian();
                        format4Table.SegmentCount = (UInt16)(reader1.ReadUInt16BigEndian() / 2);
                        format4Table.SearchRange = reader1.ReadUInt16BigEndian();
                        format4Table.EntrySelector = reader1.ReadUInt16BigEndian();
                        format4Table.RangeShift = reader1.ReadUInt16BigEndian();

                        for (var i = 0; i < format4Table.SegmentCount; i++)
                        {
                            format4Table.EndCodes.Add(reader1.ReadUInt16BigEndian());
                        }

                        format4Table.ReservedPad = reader1.ReadUInt16BigEndian();

                        for (var i = 0; i < format4Table.SegmentCount; i++)
                        {
                            format4Table.StartCodes.Add(reader1.ReadUInt16BigEndian());
                        }

                        for (var i = 0; i < format4Table.SegmentCount; i++)
                        {
                            format4Table.IdDeltas.Add(reader1.ReadUInt16BigEndian());
                        }

                        for (var i = 0; i < format4Table.SegmentCount; i++)
                        {
                            format4Table.IdRangeOffsets.Add(reader1.ReadUInt16BigEndian());
                        }

                        var read = start - reader1.Position;
                        var glyphCount = (format4Table.Length - read) / sizeof(ushort);

                        for (var i = 0; i < glyphCount; i++)
                        {
                            format4Table.GlyphIndices.Add(reader1.ReadUInt16BigEndian());
                        }

                        characterMapTable.Subtables.Add(format4Table);
                    }

                    font.CharacterMapTable = characterMapTable;
                }

                return font;
            }
        }
    }
}
