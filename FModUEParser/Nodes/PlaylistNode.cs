using FModUEParser.Enums;
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
        Entries = FModReader.ReadElemListImp<FPlaylistEntry>(Ar);
    }
}
