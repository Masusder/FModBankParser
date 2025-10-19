using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Objects;

public readonly struct FSustainPoint
{
    public readonly uint Position;
    public readonly List<FEvaluator> Evaluators;

    public FSustainPoint(BinaryReader Ar)
    {
        Position = Ar.ReadUInt32();
        Evaluators = FEvaluator.ReadEvaluatorList(Ar);
    }
}