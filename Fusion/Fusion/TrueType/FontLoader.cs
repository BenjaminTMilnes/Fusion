using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fusion.TrueType
{
    public class FontLoader
    {
        protected static bool IsBitSet(byte b, int position)
        {
            return (b & (1 << position)) != 0;
        }

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

                uint headTableOffset = 0;
                uint headTableLength = 0;

                uint nameTableOffset = 0;
                uint nameTableLength = 0;

                uint characterMapTableOffset = 0;
                uint characterMapTableLength = 0;

                uint horizontalHeadTableOffset = 0;
                uint horizontalHeadTableLength = 0;

                uint horizontalMetricsTableOffset = 0;
                uint horizontalMetricsTableLength = 0;

                uint maximumParametersTableOffset = 0;
                uint maximumParametersTableLength = 0;

                uint locationTableOffset = 0;
                uint locationTableLength = 0;

                uint glyphTableOffset = 0;
                uint glyphTableLength = 0;

                for (var i = 0; i < offsetSubtable.NumberOfTables; i++)
                {
                    var entry = new TableDirectorySubtableEntry();

                    entry.Tag = reader1.ReadASCII(4);
                    entry.CheckSum = reader1.ReadUInt32BigEndian();
                    entry.Offset = reader1.ReadUInt32BigEndian();
                    entry.Length = reader1.ReadUInt32BigEndian();

                    tableDirectorySubtable.Entries.Add(entry);

                    if (entry.Tag == "head")
                    {
                        headTableOffset = entry.Offset;
                        headTableLength = entry.Length;
                    }
                    if (entry.Tag == "name")
                    {
                        nameTableOffset = entry.Offset;
                        nameTableLength = entry.Length;
                    }
                    if (entry.Tag == "cmap")
                    {
                        characterMapTableOffset = entry.Offset;
                        characterMapTableLength = entry.Length;
                    }
                    if (entry.Tag == "hhea")
                    {
                        horizontalHeadTableOffset = entry.Offset;
                        horizontalHeadTableLength = entry.Length;
                    }
                    if (entry.Tag == "hmtx")
                    {
                        horizontalMetricsTableOffset = entry.Offset;
                        horizontalMetricsTableLength = entry.Length;
                    }
                    if (entry.Tag == "maxp")
                    {
                        maximumParametersTableOffset = entry.Offset;
                        maximumParametersTableLength = entry.Length;
                    }
                    if (entry.Tag == "loca")
                    {
                        locationTableOffset = entry.Offset;
                        locationTableLength = entry.Length;
                    }
                    if (entry.Tag == "glyf")
                    {
                        glyphTableOffset = entry.Offset;
                        glyphTableLength = entry.Length;
                    }
                }

                fontDirectoryTable.OffsetSubtable = offsetSubtable;
                fontDirectoryTable.TableDirectorySubtable = tableDirectorySubtable;

                font.FontDirectoryTable = fontDirectoryTable;

                reader1.Seek(headTableOffset);

                var headTable = new HeadTable();

                headTable.Version = reader1.ReadFixed32BigEndian();
                headTable.FontRevision = reader1.ReadFixed32BigEndian();
                headTable.CheckSumAdjustment = reader1.ReadUInt32BigEndian();
                headTable.MagicNumber = reader1.ReadUInt32BigEndian();
                headTable.Flags = reader1.ReadUInt16BigEndian();
                headTable.UnitsPerEm = reader1.ReadUInt16BigEndian();
                headTable.DateCreated = reader1.ReadInt64BigEndian();
                headTable.DateLastModified = reader1.ReadInt64BigEndian();
                headTable.MinimumX = reader1.ReadInt16BigEndian();
                headTable.MinimumY = reader1.ReadInt16BigEndian();
                headTable.MaximumX = reader1.ReadInt16BigEndian();
                headTable.MaximumY = reader1.ReadInt16BigEndian();
                headTable.MacStyle = reader1.ReadUInt16BigEndian();
                headTable.LowestRecommendedPPEM = reader1.ReadUInt16BigEndian();
                headTable.FontDirectionHint = reader1.ReadInt16BigEndian();
                headTable.IndexToLocFormat = reader1.ReadInt16BigEndian();
                headTable.GlyphDataFormat = reader1.ReadInt16BigEndian();

                font.HeadTable = headTable;

                reader1.Seek(nameTableOffset);

                var nameTable = new NameTable();

                nameTable.Format = reader1.ReadUInt16BigEndian();
                nameTable.Count = reader1.ReadUInt16BigEndian();
                nameTable.StringOffset = reader1.ReadUInt16BigEndian();

                for (var i = 0; i < nameTable.Count; i++)
                {
                    var nameRecord = new NameRecord();

                    nameRecord.PlatformId = reader1.ReadUInt16BigEndian();
                    nameRecord.PlatformSpecificId = reader1.ReadUInt16BigEndian();
                    nameRecord.LanguageId = reader1.ReadUInt16BigEndian();
                    nameRecord.NameId = reader1.ReadUInt16BigEndian();
                    nameRecord.Length = reader1.ReadUInt16BigEndian();
                    nameRecord.Offset = reader1.ReadUInt16BigEndian();

                    nameTable.NameRecords.Add(nameRecord);
                }

                foreach (var nameRecord in nameTable.NameRecords)
                {
                    reader1.Seek(nameTableOffset + nameTable.StringOffset + nameRecord.Offset);

                    nameRecord.Name = reader1.ReadASCII(nameRecord.Length);
                }

                font.NameTable = nameTable;

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
                        reader1.Seek(characterMapTableOffset + encodingSubtable.Offset);

                        var start = reader1.Position;

                        var format = reader1.ReadUInt16BigEndian();

                        if (format == 4)
                        {
                            var format4Table = new Format4CharacterMapSubtable();

                            format4Table.Format = format;
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

                            for (var i = 0; i < format4Table.SegmentCount; i++)
                            {
                                var startCode = format4Table.StartCodes[i];
                                var endCode = format4Table.EndCodes[i];

                                for (var j = startCode; j <= endCode; j++)
                                {
                                    var characterCode = j;
                                    var character = (char)j;

                                    if ("abcdefghijklmnopqrstuvwxyz".Any(c => c == character))
                                    {
                                        format4Table.Mappings[character] = 0;
                                    }
                                }
                            }

                            characterMapTable.Subtables.Add(format4Table);
                        }
                        else if (format == 12)
                        {
                            var format12Table = new Format12CharacterMapSubtable();

                            format12Table.Format = format;

                            reader1.ReadUInt16BigEndian();

                            format12Table.Length = reader1.ReadUInt32BigEndian();
                            format12Table.Language = reader1.ReadUInt32BigEndian();
                            format12Table.NumberOfGroups = reader1.ReadUInt32BigEndian();

                            for (var i = 0; i < format12Table.NumberOfGroups; i++)
                            {
                                var startCharacterCode = reader1.ReadUInt32BigEndian();
                                var endCharacterCode = reader1.ReadUInt32BigEndian();
                                var startGlyphCode = reader1.ReadUInt32BigEndian();

                                var group = new Tuple<UInt32, UInt32, UInt32>(startCharacterCode, endCharacterCode, startGlyphCode);

                                format12Table.Groups.Add(group);

                                for (var j = startCharacterCode; j <= endCharacterCode; j++)
                                {
                                    var character = (char)j;

                                    format12Table.Mappings[character] = (int)((j - startCharacterCode) + startGlyphCode);
                                }
                            }

                            characterMapTable.Subtables.Add(format12Table);
                        }
                    }

                    font.CharacterMapTable = characterMapTable;
                }

                reader1.Seek(horizontalHeadTableOffset);

                var horizontalHeadTable = new HorizontalHeadTable();

                horizontalHeadTable.Version = reader1.ReadFixed32BigEndian();
                horizontalHeadTable.Ascent = reader1.ReadInt16BigEndian();
                horizontalHeadTable.Descent = reader1.ReadInt16BigEndian();
                horizontalHeadTable.LineGap = reader1.ReadInt16BigEndian();
                horizontalHeadTable.MaximumAdvanceWidth = reader1.ReadUInt16BigEndian();
                horizontalHeadTable.MinimumLeftSideBearing = reader1.ReadInt16BigEndian();
                horizontalHeadTable.MinimumRightSideBearing = reader1.ReadInt16BigEndian();
                horizontalHeadTable.MaximumXExtent = reader1.ReadInt16BigEndian();
                horizontalHeadTable.CaretSlopeRise = reader1.ReadInt16BigEndian();
                horizontalHeadTable.CaretSlopeRun = reader1.ReadInt16BigEndian();
                horizontalHeadTable.CaretOffset = reader1.ReadInt16BigEndian();

                reader1.ReadInt16BigEndian();
                reader1.ReadInt16BigEndian();
                reader1.ReadInt16BigEndian();
                reader1.ReadInt16BigEndian();

                horizontalHeadTable.MetricDataFormat = reader1.ReadInt16BigEndian();
                horizontalHeadTable.NumberOfLongHorizontalMetrics = reader1.ReadUInt16BigEndian();

                font.HorizontalHeadTable = horizontalHeadTable;

                reader1.Seek(horizontalMetricsTableOffset);

                var horizontalMetricsTable = new HorizontalMetricsTable();

                for (var i = 0; i < horizontalHeadTable.NumberOfLongHorizontalMetrics; i++)
                {
                    horizontalMetricsTable.HorizontalMetrics.Add(reader1.ReadLongHorizontalMetricBigEndian());
                }

                font.HorizontalMetricsTable = horizontalMetricsTable;

                reader1.Seek(maximumParametersTableOffset);

                var maximumParametersTable = new MaximumParametersTable();

                maximumParametersTable.Version = reader1.ReadFixed32BigEndian();
                maximumParametersTable.NumberOfGlyphs = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfPoints = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfContours = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfComponentPoints = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfComponentContours = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfZones = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfTwilightPoints = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfStorageLocations = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfFunctionDefinitions = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfInstructionDefinitions = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfStackElements = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumSizeOfInstructions = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumNumberOfComponentElements = reader1.ReadUInt16BigEndian();
                maximumParametersTable.MaximumComponentDepth = reader1.ReadUInt16BigEndian();

                font.MaximumParametersTable = maximumParametersTable;

                reader1.Seek(locationTableOffset);

                if (font.HeadTable.IndexToLocFormat == 1)
                {
                    var locationTable = new LongLocationTable();

                    for (var i = 0; i <= maximumParametersTable.NumberOfGlyphs; i++)
                    {
                        locationTable.Offsets.Add(reader1.ReadUInt32BigEndian());
                    }

                    font.LocationTable = locationTable;
                }

                reader1.Seek(glyphTableOffset);

                var glyphTable = new GlyphTable();

                foreach (var offset in (font.LocationTable as LongLocationTable).Offsets)
                {
                    reader1.Seek(glyphTableOffset + offset);

                    var numberOfContours = reader1.ReadInt16BigEndian();
                    var minimumX = reader1.ReadInt16BigEndian();
                    var minimumY = reader1.ReadInt16BigEndian();
                    var maximumX = reader1.ReadInt16BigEndian();
                    var maximumY = reader1.ReadInt16BigEndian();

                    if (numberOfContours >= 0)
                    {
                        var glyph = new SimpleGlyph();

                        glyph.NumberOfContours = numberOfContours;
                        glyph.MinimumX = minimumX;
                        glyph.MinimumY = minimumY;
                        glyph.MaximumX = maximumX;
                        glyph.MaximumY = maximumY;

                        if (numberOfContours > 0)
                        {
                            for (var i = 0; i < numberOfContours; i++)
                            {
                                glyph.EndPointsOfContours.Add(reader1.ReadUInt16BigEndian());
                            }

                            var numberOfPoints = glyph.EndPointsOfContours.Last() + 1;

                            glyph.InstructionLength = reader1.ReadUInt16BigEndian();

                            for (var j = 0; j < glyph.InstructionLength; j++)
                            {
                                glyph.Instructions.Add(reader1.ReadByte());
                            }

                            var isRepeatCount = false;

                            for (var j = 0; j < numberOfPoints; j++)
                            {
                                var data = reader1.ReadByte();

                                if (isRepeatCount)
                                {
                                    var numberOfRepeats = data;

                                    j += numberOfRepeats - 1;

                                    for (var k = 0; k < numberOfRepeats; k++)
                                    {
                                        glyph.Flags.Add(glyph.Flags.Last());
                                    }

                                    isRepeatCount = false;
                                }
                                else
                                {
                                    glyph.Flags.Add(data);

                                    isRepeatCount = IsBitSet(data, 3);
                                }
                            }

                            foreach (var flag in glyph.Flags)
                            {
                                var isShortVector = IsBitSet(flag, 1);

                                if (isShortVector)
                                {
                                    glyph.XCoordinates.Add(reader1.ReadByte());
                                }
                                else
                                {
                                    glyph.XCoordinates.Add(reader1.ReadInt16BigEndian());
                                }
                            }

                            foreach (var flag in glyph.Flags)
                            {
                                var isShortVector = IsBitSet(flag, 2);

                                if (isShortVector)
                                {
                                    glyph.YCoordinates.Add(reader1.ReadByte());
                                }
                                else
                                {
                                    glyph.YCoordinates.Add(reader1.ReadInt16BigEndian());
                                }
                            }

                            var contour = new Contour();

                            var m = 0;

                            var lastX = 0;
                            var lastY = 0;

                            for (var i = 0; i < glyph.Flags.Count(); i++)
                            {
                                var flag = glyph.Flags[i];
                                var isOnCurve = IsBitSet(flag, 0);
                                var xIsShort = IsBitSet(flag, 1);
                                var yIsShort = IsBitSet(flag, 2);
                                int x = Convert.ToInt32(glyph.XCoordinates[i]);
                                int y = Convert.ToInt32(glyph.YCoordinates[i]);

                                var point = new Point();

                                if (xIsShort)
                                {
                                    var sign = (IsBitSet(flag, 4)) ? 1 : -1;

                                    point.X = sign * x;
                                }
                                else
                                {
                                    var isDelta = (IsBitSet(flag, 4)) == false;

                                    if (isDelta)
                                    {
                                        point.X = x + lastX;
                                    }
                                }

                                if (yIsShort)
                                {
                                    var sign = (IsBitSet(flag, 5)) ? 1 : -1;

                                    point.Y = sign * y;
                                }
                                else
                                {
                                    var isDelta = (IsBitSet(flag, 5)) == false;

                                    if (isDelta)
                                    {
                                        point.Y = y + lastY;
                                    }
                                }

                                lastX = point.X;
                                lastY = point.Y;

                                point.IsOnCurve = isOnCurve;

                                contour.Points.Add(point);

                                if (i == glyph.EndPointsOfContours[m])
                                {
                                    glyph.Contours.Add(contour);

                                    contour = new Contour();
                                    m++;
                                }
                            }
                        }

                        glyphTable.Glyphs.Add(glyph);
                    }
                    else
                    {
                        var glyph = new CompoundGlyph();

                        glyphTable.Glyphs.Add(glyph);
                    }
                }

                font.GlyphTable = glyphTable;

                return font;
            }
        }
    }
}
