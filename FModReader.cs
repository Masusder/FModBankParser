using Fmod5Sharp.FmodTypes;
using FModUEParser.Enums;
using FModUEParser.Nodes;
using FModUEParser.Nodes.Buses;
using FModUEParser.Nodes.Effects;
using FModUEParser.Objects;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace FModUEParser;

public class FModReader
{
    public static int Version => FormatInfo.FileVersion;
    public static FormatInfo FormatInfo = null!;
    public static SoundDataHeaderNode? SoundDataHeader;
    public StringDataNode? StringData;
    public BankInfoNode? BankInfo;

    public readonly Dictionary<FModGuid, EventNode> EventNodes = [];
    public readonly Dictionary<FModGuid, BusNode> BusNodes = [];
    public readonly Dictionary<FModGuid, EffectNode> EffectNodes = [];
    public readonly Dictionary<FModGuid, CommandInstrumentNode> CommandInstrumentNodes = [];
    public readonly Dictionary<FModGuid, TimelineNode> TimelineNodes = [];
    public readonly Dictionary<FModGuid, TransitionRegionNode> TransitionRegionNodes = [];
    public readonly Dictionary<FModGuid, TransitionTimelineNode> TransitionTimelineNodes = [];
    public readonly Dictionary<FModGuid, PlaylistNode> PlaylistNodes = [];
    public readonly Dictionary<FModGuid, InstrumentNode> InstrumentNodes = [];
    public readonly Dictionary<FModGuid, WaveformResourceNode> WavEntries = [];
    public readonly Dictionary<FModGuid, ScattererInstrumentNode> ScattererInstrumentNodes = [];
    public readonly Dictionary<FModGuid, ParameterNode> ParameterNodes = [];
    public readonly Dictionary<FModGuid, ModulatorNode> ModulatorNodes = [];
    public readonly Dictionary<FModGuid, CurveNode> CurveNodes = [];
    public readonly Dictionary<FModGuid, PropertyNode> PropertyNodes = [];
    public readonly Dictionary<FModGuid, MappingNode> MappingNodes = [];
    public readonly Dictionary<FModGuid, ParameterLayoutNode> ParameterLayoutNodes = [];
    public readonly Dictionary<FModGuid, ControllerNode> ControllerNodes = [];
    public readonly Dictionary<FModGuid, FModGuid> WaveformInstrumentNodes = [];

    public List<FmodSoundBank> SoundBankData = [];
    public FHashData[] HashData = [];

    public FModReader(BinaryReader Ar)
    {
        ParseHeader(Ar);
        ParseNodes(Ar, Ar.BaseStream.Position, Ar.BaseStream.Length);
    }

    private void ParseHeader(BinaryReader Ar)
    {
        if (Ar.BaseStream.Length < 12)
            throw new Exception("File too small to be a valid RIFF header");

        string riff = Encoding.ASCII.GetString(Ar.ReadBytes(4));
        if (riff != "RIFF") throw new Exception("Not a valid RIFF file");

        int riffSize = Ar.ReadInt32();
        string fileType = Encoding.ASCII.GetString(Ar.ReadBytes(4));
        if (fileType != "FEV ") throw new Exception("Not a valid FMOD bank");

        long expectedSize = riffSize + 8;
        long actualSize = Ar.BaseStream.Length;

        if (actualSize < expectedSize)
            throw new Exception($"Truncated file: expected {expectedSize} bytes, got {actualSize}");
        else if (actualSize > expectedSize)
            Console.WriteLine($"Warning: file larger than RIFF size (expected {expectedSize}, got {actualSize})");

        Console.WriteLine($"FMod bank detected, size={riffSize}, type={fileType}");
    }

    private void ParseNodes(BinaryReader Ar, long start, long end)
    {
        Ar.BaseStream.Position = start;

        Stack<FParentContext> parentStack = [];
        bool visitedSoundNode = false;
        int soundDataIndex = 0;

        while (Ar.BaseStream.Position + 8 <= end)
        {
            long nodeStart = Ar.BaseStream.Position;
            var rawNodeValue = Ar.ReadInt32();

            // Shift to correct position if end of the node starts with null terminator
            // (usually it's end of a list but not always)
            if ((rawNodeValue & 0xFF) == 0x00)
            {
                nodeStart = Ar.BaseStream.Position - 3;
                Ar.BaseStream.Position -= 3;
                rawNodeValue = Ar.ReadInt32();
            }

            var nodeId = (ENodeId)rawNodeValue;
            int nodeSize = Ar.ReadInt32();
            long nextNode = nodeStart + 8 + nodeSize;

            if (nodeSize == 0)
            {
                Ar.BaseStream.Position = nextNode;
                continue;
            }

            switch (nodeId)
            {
                case ENodeId.CHUNKID_FORMATINFO:
                    FormatInfo = new FormatInfo(Ar);
                    break;

                case ENodeId.CHUNKID_BANKINFO:
                    BankInfo = new BankInfoNode(Ar);
                    break;

                case ENodeId.CHUNKID_LIST: // List of sub-chunks
                    var listNodeId = (ENodeId)Ar.ReadInt32();
                    ParseNodes(Ar, Ar.BaseStream.Position, nextNode);
                    break;

                case ENodeId.CHUNKID_LISTCOUNT:
                    var listCount = Ar.ReadUInt32();
                    break;

                case ENodeId.CHUNKID_INPUTBUSBODY: // Input Bus Node
                    {
                        var node = new InputBusNode(Ar);

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to bus node
                    }
                    break;

                case ENodeId.CHUNKID_GROUPBUSBODY: // Group Bus Node
                    {
                        var node = new GroupBusNode(Ar);

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to bus node
                    }
                    break;

                case ENodeId.CHUNKID_MASTERBUSBODY: // Master Bus Node
                    {
                        var node = new MasterBusNode(Ar);

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to bus node
                    }
                    break;

                case ENodeId.CHUNKID_BUS: // Bus Node
                    if (parentStack.TryPeek(out var busParent) &&
                        (busParent.NodeId == ENodeId.CHUNKID_INPUTBUSBODY ||
                        busParent.NodeId == ENodeId.CHUNKID_GROUPBUSBODY ||
                        busParent.NodeId == ENodeId.CHUNKID_MASTERBUSBODY))
                    {
                        var node = new BusNode(Ar);
                        BusNodes[busParent.Guid] = node;
                        parentStack.Pop();
                    }
                    break;

                case ENodeId.CHUNKID_BUILTINEFFECTBODY: // Built-in Effect Node
                    {
                        var node = new BuiltInEffectNode(Ar);
                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to parameterized effect node
                    }
                    break;

                case ENodeId.CHUNKID_SENDEFFECTBODY: // Send Effect Node
                    {
                        var node = new SendEffectNode(Ar);

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to effect node
                    }
                    break;

                case ENodeId.CHUNKID_SIDECHAINEFFECT: // Side Chain Effect Node
                    {
                        var node = new SideChainEffectNode(Ar);

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to effect node
                    }
                    break;

                case ENodeId.CHUNKID_PARAMETERIZEDEFFECT: // Parameterized Effect Node
                    if (parentStack.TryPeek(out var paramEffectParent) &&
                        paramEffectParent.NodeId == ENodeId.CHUNKID_BUILTINEFFECTBODY)
                    {
                        var node = new ParameterizedEffectNode(Ar);
                        parentStack.Pop();

                        parentStack.Push(new FParentContext(nodeId, paramEffectParent.Guid)); // Points to effect node
                    }
                    break;

                case ENodeId.CHUNKID_EFFECTBODY: // Effect Node
                    if (parentStack.TryPeek(out var effectParent) &&
                        (effectParent.NodeId == ENodeId.CHUNKID_PARAMETERIZEDEFFECT ||
                        effectParent.NodeId == ENodeId.CHUNKID_SENDEFFECTBODY ||
                        effectParent.NodeId == ENodeId.CHUNKID_SIDECHAINEFFECT))
                    {
                        var node = new EffectNode(Ar);
                        EffectNodes[effectParent.Guid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_PROPERTY: // Property Node
                    {
                        var node = new PropertyNode(Ar);
                        PropertyNodes[node.MappingGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_EVENTBODY: // Audio Event Node
                    {
                        var node = new EventNode(Ar);
                        EventNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_MODULATORBODY: // Modulator Node
                    {
                        var node = new ModulatorNode(Ar);
                        ModulatorNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_STRINGDATA: // String Data Node
                    {
                        StringData = new StringDataNode(Ar);
                    }
                    break;

                case ENodeId.CHUNKID_PARAMETERBODY: // Parameter Node
                    {
                        var node = new ParameterNode(Ar);
                        ParameterNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_PARAMETERLAYOUTBODY: // Parameter Layout Node
                    {
                        var node = new ParameterLayoutNode(Ar);
                        ParameterLayoutNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_WAVEFORMRESOURCE: // Single WAV Node
                    {
                        var node = new WaveformResourceNode(Ar);
                        WavEntries[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_COMMANDINSTRUMENTBODY: // Command Instrument Node
                    {
                        var node = new CommandInstrumentNode(Ar);
                        CommandInstrumentNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_SCATTERERINSTRUMENTBODY: // Scatterer Instrument Node
                    {
                        var node = new ScattererInstrumentNode(Ar);
                        ScattererInstrumentNodes[node.BaseGuid] = node;

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to playlist node
                    }
                    break;

                case ENodeId.CHUNKID_MULTIINSTRUMENTBODY: // Multi Instrument Node
                    parentStack.Push(new FParentContext(nodeId, new FModGuid(Ar))); // Points to playlist node
                    break;

                case ENodeId.CHUNKID_WAVEFORMINSTRUMENTBODY: // Waveform Instrument Node
                    {
                        var node = new WaveformInstrumentNode(Ar);
                        WaveformInstrumentNodes[node.BaseGuid] = node.WaveformResourceGuid;
                    }
                    break;

                case ENodeId.CHUNKID_INSTRUMENT: // Instrument Node
                    {
                        var node = new InstrumentNode(Ar);
                        InstrumentNodes[node.TimelineGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_TIMELINEBODY: // Timeline Node
                    {
                        var node = new TimelineNode(Ar);
                        TimelineNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_TRANSITIONREGIONBODY: // Transition Region Node
                    {
                        var node = new TransitionRegionNode(Ar);
                        TransitionRegionNodes[node.BaseGuid] = node;

                        parentStack.Push(new FParentContext(nodeId, node.BaseGuid)); // Points to transition timeline node
                    }
                    break;

                case ENodeId.CHUNKID_TRANSITIONTIMELINE: // Transition Timeline Node
                    if (parentStack.TryPeek(out var transParent) &&
                        transParent.NodeId == ENodeId.CHUNKID_TRANSITIONREGIONBODY)
                    {
                        var node = new TransitionTimelineNode(Ar);
                        TransitionTimelineNodes[transParent.Guid] = node;
                        parentStack.Pop();
                    }
                    break;

                case ENodeId.CHUNKID_PLAYLIST: // Playlist Node
                    if (parentStack.TryPeek(out var parent) &&
                        (parent.NodeId == ENodeId.CHUNKID_SCATTERERINSTRUMENTBODY ||
                         parent.NodeId == ENodeId.CHUNKID_MULTIINSTRUMENTBODY))
                    {
                        PlaylistNodes[parent.Guid] = new PlaylistNode(Ar);
                        parentStack.Pop();
                    }
                    break;

                case ENodeId.CHUNKID_HASHDATA: // Hash Node
                    {
                        HashData = new HashDataNode(Ar).HashData;
                    }
                    break;

                case ENodeId.CHUNKID_CURVE: // Curve Node
                    {
                        var node = new CurveNode(Ar);
                        CurveNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_CONTROLLEROWNER: // Controller Owner Node
                    {
                        _ = new ControllerOwnerNode(Ar);
                    }
                    break;

                case ENodeId.CHUNKID_CONTROLLER: // Controller Node
                    {
                        var node = new ControllerNode(Ar);
                        ControllerNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_MAPPING: // Mapping Node
                    {
                        var node = new MappingNode(Ar);
                        MappingNodes[node.BaseGuid] = node;
                    }
                    break;

                case ENodeId.CHUNKID_SOUNDDATAHEADER: // Sound Data Header Node
                    {
                        SoundDataHeader = new SoundDataHeaderNode(Ar);
                    }
                    break;

                case ENodeId.CHUNKID_SOUNDDATA: // Sound Data Node
                    {
                        var node = new SoundDataNode(Ar, nodeStart, nodeSize, soundDataIndex);
                        visitedSoundNode = true;
                        soundDataIndex++;
                        if (node.SoundBank != null)
                        {
                            SoundBankData.Add(node.SoundBank);
                        }
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown chunk {nodeId} at {nodeStart}, size={nodeSize}, skipped");
                    break;
            }

            // Stop if we already visited a sound node and current node is NOT sound node
            // Not sure why I need to do that but I've seen soundbanks that write duplicated FSB data outside of SND chunk
            // It's important to note there might be multiple SND chunks so we can't just stop after first SND
            if (visitedSoundNode && nodeId != ENodeId.CHUNKID_SOUNDDATA)
                break;

            if (Ar.BaseStream.Position != nextNode)
            {
                Console.WriteLine($"Warning: chunk {nodeId} did not parse fully (at {Ar.BaseStream.Position}, should be {nextNode})");
                Ar.BaseStream.Position = nextNode;
            }
        }
    }

    #region Global Readers

    public static uint ReadX16(BinaryReader Ar)
    {
        short signedLow = Ar.ReadInt16();
        ushort low = (ushort)signedLow;
        uint value = low;

        if ((low & 0x8000) != 0)
        {
            ushort high = Ar.ReadUInt16();
            value &= 0x7FFFu;
            value |= ((uint)high << 15);
        }

        return value;
    }

    public static string ReadSerializedString(BinaryReader Ar)
    {
        uint length = ReadX16(Ar);

        if (length <= 0) return string.Empty;

        var bytes = Ar.ReadBytes((int)length);

        return Encoding.UTF8.GetString(bytes);
    }

    public static T[] ReadVersionedElemListImp<T>(BinaryReader Ar)
    {
        uint raw = ReadX16(Ar);
        int count = (int)(raw >> 1);

        if (count <= 0) return [];

        var result = new T[count];
        _ = Ar.ReadUInt16(); // Payload size

        for (int i = 0; i < count; i++)
        {
            result[i] = (T)Activator.CreateInstance(typeof(T), Ar)!;

            if (i < count - 1) _ = Ar.ReadUInt16(); // Payload size
        }

        return result;
    }

    public static T[] ReadFixElemList<T>(BinaryReader Ar, Func<BinaryReader, T> readElem)
    {
        uint raw = ReadX16(Ar);
        int count = (int)(raw >> 1);
        bool hasSizePrefix = (raw & 1) != 0;

        if (count <= 0) return [];

        if (hasSizePrefix) _ = Ar.ReadUInt16(); // Payload size

        var result = new T[count];
        for (int i = 0; i < count; i++)
            result[i] = readElem(Ar);

        return result;
    }

    public static T[] ReadElemListImp<T>(BinaryReader Ar, int? expectedSize = null)
    {

        uint raw = ReadX16(Ar);
        int count = (int)(raw >> 1);
        bool hasSizePrefix = (raw & 1) != 0; // Element list "size prefix" is a single size value used for the element payloads

        if (count <= 0) return [];

        var result = new T[count];

        ushort elementSize = 0;
        if (hasSizePrefix) elementSize = Ar.ReadUInt16();

        for (int i = 0; i < count; i++)
        {
            // Pass size for debugging purposes only
            if (hasSizePrefix && expectedSize != null && elementSize != expectedSize)
            {
                Ar.BaseStream.Position += elementSize;
#if DEBUG
                Console.WriteLine($"Warning: '{typeof(T).Name}' element size {elementSize} does not match expected {expectedSize}, skipping");
#endif
            }
            else
            {
                result[i] = (T)Activator.CreateInstance(typeof(T), Ar)!;
            }
        }

        return result;
    }

    #endregion
}