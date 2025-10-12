using FModUEParser.Enums;
using FModUEParser.Objects;
using System;
using System.IO;
using System.Text;

namespace FModUEParser.Metadata;

// String table is used to convert event FModGuids to human readable names
public class StringTable
{
    public readonly EStringTableType Type;
    public readonly FRadixTreePacked? RadixTree;

    public StringTable(BinaryReader Ar)
    {
        Type = (EStringTableType)Ar.ReadUInt32();

        if (Type == EStringTableType.StringTable_RadixTree_24Bit)
        {
            RadixTree = new FRadixTreePacked(Ar, Type);
        }
    }
}
