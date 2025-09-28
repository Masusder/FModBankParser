using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Enums;

public enum EPlaylistSelectionMode : int
{
    PlaylistSelectionMode_SelectOnce = 0x0,
    PlaylistSelectionMode_SelectNormal = 0x1,
    PlaylistSelectionMode_Undefined = 0x2,
    PlaylistSelectionMode_Max = 0x3
}
