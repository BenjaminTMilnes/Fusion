using System;
using System.Collections.Generic;
using System.Text;

namespace Fusion.TrueType
{
    public class MaximumParametersTable : Table
    {
        public Fixed32 Version { get; set; }
        public UInt16 NumberOfGlyphs { get; set; }
        public UInt16 MaximumNumberOfPoints { get; set; }
        public UInt16 MaximumNumberOfContours { get; set; }
        public UInt16 MaximumNumberOfComponentPoints { get; set; }
        public UInt16 MaximumNumberOfComponentContours { get; set; }
        public UInt16 MaximumNumberOfZones { get; set; }
        public UInt16 MaximumNumberOfTwilightPoints { get; set; }
        public UInt16 MaximumNumberOfStorageLocations { get; set; }
        public UInt16 MaximumNumberOfFunctionDefinitions { get; set; }
        public UInt16 MaximumNumberOfInstructionDefinitions { get; set; }
        public UInt16 MaximumNumberOfStackElements { get; set; }
        public UInt16 MaximumSizeOfInstructions { get; set; }
        public UInt16 MaximumNumberOfComponentElements { get; set; }
        public UInt16 MaximumComponentDepth { get; set; }
    }
}
