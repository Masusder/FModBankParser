using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FModUEParser.Extensions;
using FModUEParser.Objects;

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

            //var stringData = mergedReaders
            //    .Select(r => r.StringData?.RadixTree)
            //    .FirstOrDefault(tree => tree != null);

            foreach (var fmodReader in mergedReaders)
            {
                var resolvedEvents = EventNodesResolver.ResolveAudioEvents(fmodReader);
                EventNodesResolver.PrintMissingSamples(fmodReader, resolvedEvents);

                //if (stringData == null) continue;

                //foreach (var eventNode in fmodReader.EventNodes)
                //{
                //    var guid = eventNode.Key;
                //    if (stringData.TryGetString(guid, out var path))
                //    {
                //        Console.WriteLine($"GUID {guid} -> {path}");
                //    }
                //    else
                //    {
                //        Console.WriteLine($"Could not resolve GUID {guid}");
                //    }
                //}
            }
        }
    }
}
