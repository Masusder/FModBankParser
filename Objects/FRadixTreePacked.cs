namespace FModUEParser.Objects;

// TODO
public class FRadixTreePacked
{
    public readonly FModGuid[] Guids;
    public readonly byte[] StringBlob;
    public readonly uint[] LeafIndices = [];
    public readonly uint[] ParentIndices = [];

    public FRadixTreePacked(BinaryReader Ar, EStringTableType type)
    {
        Guids = FModReader.ReadElemListImp<FModGuid>(Ar, 16); // This aint no guid
        StringBlob = ReadSimpleArray24(Ar);
        if (type is EStringTableType.StringTable_RadixTree_24Bit)
        {
            //LeafIndices = 
            //ParentIndices = 
        }
    }

    private static byte[] ReadSimpleArray24(BinaryReader Ar)
    {
        uint raw = FModReader.ReadX16(Ar);
        int count = (int)(raw >> 1);
        bool hasSizePrefix = (raw & 1) != 0;
        ushort elementSize = 1;
        if (hasSizePrefix)
            elementSize = Ar.ReadUInt16();

        if (count <= 0) return [];

        int totalBytes = checked(count * elementSize);
        var bytes = Ar.ReadBytes(totalBytes);
        if (bytes.Length != totalBytes)
            throw new EndOfStreamException($"Tried to read {totalBytes} bytes, got {bytes.Length}.");
        return bytes;
    }
}