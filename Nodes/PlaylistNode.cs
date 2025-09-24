using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class PlaylistNode
{
    public readonly EPlaylistPlayMode PlayMode;
    public readonly EPlaylistSelectionMode SelectionMode;
    public readonly FPlaylistEntry[] Entries;

    public PlaylistNode(BinaryReader Ar)
    {
        PlayMode = (EPlaylistPlayMode)Ar.ReadInt32();
        SelectionMode = (EPlaylistSelectionMode)Ar.ReadInt32();

        uint packedSize = Ar.ReadUInt32();
        ushort unpackedSize = (ushort)(packedSize & 0xFFFF);
        int numEntries = (unpackedSize - 1) / 2;

        Entries = new FPlaylistEntry[numEntries];
        for (int i = 0; i < numEntries; i++)
        {
            var guid = new FModGuid(Ar);
            var weight = Ar.ReadSingle();
            Entries[i] = new FPlaylistEntry(guid, weight);
        }
    }
}

public readonly struct FPlaylistEntry
{
    public FModGuid Guid { get; }
    public float Weight { get; }

    public FPlaylistEntry(FModGuid guid, float weight)
    {
        Guid = guid;
        Weight = weight;
    }
}
