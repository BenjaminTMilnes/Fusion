using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fusion.TrueType
{
    public sealed class FontReader : BinaryReader
    {
        public FontReader(Stream stream) : base(stream) { }

        public long Position => BaseStream.Position;

        private byte[] ReadBigEndian(int count)
        {
            var bytes = base.ReadBytes(count);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        public UInt16 ReadUInt16BigEndian()
        {
            var bytes = ReadBigEndian(2);

            return BitConverter.ToUInt16(bytes, 0);
        }

        public UInt32 ReadUInt32BigEndian()
        {
            var bytes = ReadBigEndian(4);

            return BitConverter.ToUInt32(bytes, 0);
        }

        public string ReadASCII(int length)
        {
            var bytes = ReadBytes(length);
            var text = new string(bytes.Select(b => (char)b).ToArray(), 0, length);

            return text;
        }

        public void Seek(long offset)
        {
            BaseStream.Seek(offset, SeekOrigin.Begin);
        }
    }
}
