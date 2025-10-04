using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace FModUEParser.Nodes;

public class TimelineNode
{
    public readonly FModGuid BaseGuid;
    public readonly FTriggerBox[] TriggerBoxes = [];
    public readonly FTriggerBox[] TimeLockedTriggerBoxes = [];
    public readonly FTimelineNamedMarker[] TimelineNamedMarkers = [];
    public readonly FTimelineTempoMarker[] TimelineTempoMarkers = [];
    //public readonly uint[] LegacyUIntArray = [];

    public TimelineNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);

        if (FModReader.Version >= 0x6d)
        {
            TriggerBoxes = FModReader.ReadElemListImp<FTriggerBox>(Ar);
            TimeLockedTriggerBoxes = FModReader.ReadElemListImp<FTriggerBox>(Ar);

            FModReader.ReadX16(Ar); // TODO: Sustain Points array
            TimelineNamedMarkers = FModReader.ReadVersionedElemListImp<FTimelineNamedMarker>(Ar);
            TimelineTempoMarkers = FModReader.ReadElemListImp<FTimelineTempoMarker>(Ar);
        }

        if (FModReader.Version < 0x84)
        {
            // LegacyUIntArray, I don't care
        }
    }
}