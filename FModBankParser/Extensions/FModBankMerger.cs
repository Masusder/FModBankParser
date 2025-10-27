using FModBankParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FModBankParser.Extensions;

internal static class FModBankMerger
{
    private const string BankSuffix = ".bank";
    private const string StreamsSuffix = ".streams.bank";
    private const string AssetsSuffix = ".assets.bank";

    // Events and samples can be divided between streams/assets/bank
    // Simplest way to handle this is to merge all related banks into a single FModReader
    internal static List<FModReader> MergeBanks(string folderPath, byte[]? encryptionKey = null)
    {
        var allFiles = Directory.GetFiles(folderPath, "*.bank", SearchOption.AllDirectories);
        if (allFiles.Length == 0)
            return [];

        var filesByFolder = allFiles.GroupBy(f => Path.GetDirectoryName(f) ?? "");

        var mergedReaders = new List<FModReader>();
        foreach (var folderGroup in filesByFolder)
        {
            var groupedByBase = folderGroup
                .GroupBy(f => Path.GetFileName(f)
                    .Replace(StreamsSuffix, "")
                    .Replace(AssetsSuffix, "")
                    .Replace(BankSuffix, ""))
                .ToDictionary(g => g.Key, g => g.ToArray());

            foreach (var (baseName, variants) in groupedByBase)
            {
                FModReader? merged = null;
                foreach (var file in variants)
                {
                    using var reader = new BinaryReader(File.OpenRead(file));
#if DEBUG
                    Debug.WriteLine(file);
#endif
                    var fmod = new FModReader(reader, baseName, encryptionKey);

                    merged ??= fmod;
                    if (merged != fmod)
                        merged.Merge(fmod);
                }

                if (merged != null)
                    mergedReaders.Add(merged);
            }
        }

        return mergedReaders;
    }
}