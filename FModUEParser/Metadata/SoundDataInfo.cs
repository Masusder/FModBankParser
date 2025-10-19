using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Metadata;

public class SoundDataInfo
{
    public readonly FSoundDataHeader[] Header;

    public SoundDataInfo(BinaryReader Ar)
    {
        Header = FModReader.ReadElemListImp<FSoundDataHeader>(Ar);
    }

    public readonly struct FSoundDataHeader
    {
        public readonly uint FSBOffset;
        public readonly uint Length;

        public FSoundDataHeader(BinaryReader Ar)
        {
            FSBOffset = Ar.ReadUInt32();
            Length = Ar.ReadUInt32();
        }
    }
}
