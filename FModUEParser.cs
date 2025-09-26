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
        string fmodPath = @"path/to/FMod/soundbanks/here.bank";

        if (fmodPath.EndsWith(".bank", StringComparison.OrdinalIgnoreCase))
        {
            using var reader = new BinaryReader(File.OpenRead(fmodPath));
            var fmodReader = new FModReader(reader);

            var resolvedEvents = EventNodesResolver.ResolveAudioEvents(fmodReader);
            EventNodesResolver.PrintMissingSamples(fmodReader, resolvedEvents);
        }
        else
        {
            var mergedReaders = FModBankMerger.MergeBanks(fmodPath);
            foreach (var fmodReader in mergedReaders)
            {
                var resolvedEvents = EventNodesResolver.ResolveAudioEvents(fmodReader);
                EventNodesResolver.PrintMissingSamples(fmodReader, resolvedEvents);
            }
        }
    }
}
