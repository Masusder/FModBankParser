using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Effects;

public class ParameterizedEffectNode
{
    public readonly List<FEffectParameter> Parameters;
    public readonly bool SideChainEnabled;

    public ParameterizedEffectNode(BinaryReader Ar)
    {
        int paramCount = Ar.ReadInt32();

        Parameters = new List<FEffectParameter>(paramCount);
        for (int i = 0; i < paramCount; i++)
        {
            Parameters.Add(new FEffectParameter(Ar));
        }

        if (FModReader.Version >= 0x6e)
        {
            SideChainEnabled = Ar.ReadBoolean();
        }
    }
}
