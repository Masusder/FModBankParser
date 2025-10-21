using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Effects;

public class ParameterizedEffectNode
{
    public readonly FEffectParameter[] Parameters;
    public readonly bool SideChainEnabled;

    public ParameterizedEffectNode(BinaryReader Ar)
    {
        int paramCount = Ar.ReadInt32();
        Parameters = new FEffectParameter[paramCount];
        for (int i = 0; i < paramCount; i++)
        {
            Parameters[i] = new FEffectParameter(Ar);
        }

        if (FModReader.Version >= 0x6e)
        {
            SideChainEnabled = Ar.ReadBoolean();
        }
    }
}
