using FModBankParser.Enums;
using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes;

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
        if (FModReader.Version >= 0x65 && FModReader.Version <= 0x67) Ar.ReadBoolean();
    }
}
