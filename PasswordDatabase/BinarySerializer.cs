using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordDatabase
{
    public class BinarySerializer
    {
        private List<int> _byteCounts = new List<int>();
        private List<byte[]> _data = new List<byte[]>();
        private int curDataCursor = -1;
        public void AddData(byte[] data)
        {
            _byteCounts.Add(data.Length);
            _data.Add(data);
        }
        public byte[] Serialize()
        {
            var memory = new MemoryStream();
            var bw = new BinaryWriter(memory);
            for (int i = 0; i < _data.Count; i++)
            {
                bw.Write(_byteCounts[i]);
                bw.Write(_data[i]);
            }
            return memory.ToArray();
        }

        public void Reset()
        {
            curDataCursor = -1;
        }

        public byte[] ReadNext()
        {
            curDataCursor++;
            if (curDataCursor > _data.Count - 1)
            {
                return null;
            }
            return _data[curDataCursor];
        }

        public void Deserialize(byte[] allData)
        {
            _byteCounts.Clear();
            _data.Clear();
            var memory = new MemoryStream(allData);
            var bw = new BinaryReader(memory);
            while (bw.BaseStream.Position < bw.BaseStream.Length)
            { 
                var bytesCount = bw.ReadInt32();
                _byteCounts.Add(bytesCount);
                _data.Add(bw.ReadBytes(bytesCount));
            }
        }
    }
}
