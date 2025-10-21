using FModBankParser.Enums;
using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes;

public class SnapshotNode
{
    public readonly FModGuid BaseGuid;
    public readonly int Priority;
    public readonly FSnapshot[] Snapshots;
    public readonly bool BlendingSnapshot;
    public readonly EAutomationConflictResolutionMethod GroupResolutionMethod;
    public readonly float Intensity;

    public SnapshotNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        Priority = Ar.ReadInt32();
        Snapshots = FModReader.ReadElemListImp<FSnapshot>(Ar);
        BlendingSnapshot = Ar.ReadBoolean();
        GroupResolutionMethod = (EAutomationConflictResolutionMethod)Ar.ReadUInt32();
        Intensity = Ar.ReadSingle();
    }
}
