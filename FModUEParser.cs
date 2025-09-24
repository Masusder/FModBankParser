using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FModUEParser.Extensions;

namespace FModUEParser;

public class FModUEParser
{
    static void Main()
    {
        string bankPath = @"path/to/FMod/soundbank/here.bank";

        using var Ar = new BinaryReader(File.OpenRead(bankPath));
        var fmodReader = new FModReader(Ar);

        EventNodesResolver.PrintMissingEventsAndSampleCounts(fmodReader);
    }
}
