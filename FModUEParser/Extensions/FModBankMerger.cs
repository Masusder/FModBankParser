using FModUEParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FModUEParser.Extensions;

internal static class FModBankMerger
{
    // Events and samples can be divided between streams/assets/bank
    // Simplest way to handle this is to merge all related banks into a single FModReader
    internal static List<FModReader> MergeBanks(string folderPath, byte[]? encryptionKey = null)
    {
        var files = Directory.GetFiles(folderPath, "*.bank", SearchOption.AllDirectories);
        var baseNames = files
            .Select(f => Path.GetFileName(f)
                .Replace(".streams.bank", "")
                .Replace(".assets.bank", "")
                .Replace(".bank", ""))
            .Distinct();

        var mergedReaders = new List<FModReader>();
        foreach (var baseName in baseNames)
        {
            var variants = files.Where(f =>
            {
                var fn = Path.GetFileName(f);
                return fn.Equals(baseName + ".bank", StringComparison.OrdinalIgnoreCase)
                    || fn.Equals(baseName + ".assets.bank", StringComparison.OrdinalIgnoreCase)
                    || fn.Equals(baseName + ".streams.bank", StringComparison.OrdinalIgnoreCase);
            }).ToArray();

            FModReader? merged = null;
            foreach (var file in variants.Where(File.Exists))
            {
                using var reader = new BinaryReader(File.OpenRead(file));
                var fmod = new FModReader(reader, encryptionKey);

                if (merged == null)
                {
                    merged = fmod;
                }
                else
                {
                    merged.Merge(fmod);
                }
            }

            if (merged != null)
            {
                merged.BankName = baseName;
                mergedReaders.Add(merged);
            }
        }

        return mergedReaders;
    }
}