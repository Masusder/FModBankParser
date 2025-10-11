using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class PluginEffectNode : BaseEffectNode
{
    public readonly FModGuid BaseGuid;
    public readonly string PluginName;
    public readonly string Name;
    public ParameterizedEffectNode? ParamEffectBody;

    public PluginEffectNode(BinaryReader Ar)
    {
        BaseGuid = new FModGuid(Ar);
        PluginName = FModReader.ReadString(Ar);
        Name = FModReader.ReadString(Ar);

        bool legacyBypass = Ar.ReadBoolean();
    }
}
