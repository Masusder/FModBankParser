using FModUEParser.Objects;
using System;
using System.IO;
using System.Text;

namespace FModUEParser.Nodes;

public class StringDataNode
{
    public readonly EStringTableType Type;
    public readonly FRadixTreePacked? RadixTree;

    public StringDataNode(BinaryReader Ar)
    {
        Type = (EStringTableType)Ar.ReadUInt32();

        if (Type == EStringTableType.StringTable_RadixTree_24Bit)
        {
            RadixTree = new FRadixTreePacked(Ar, Type);
        }
    }
}
