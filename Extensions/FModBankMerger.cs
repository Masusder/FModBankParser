using FModUEParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FModUEParser.Extensions;

public static class FModBankMerger
{
    // Events and samples can be divided between streams/assets/bank
    public static List<FModReader> MergeBanks(string folderPath)
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
                var fmod = new FModReader(reader);
                if (merged == null)
                {
                    merged = fmod;
                }
                else
                {
                    MergeInto(merged, fmod);
                }
            }

            if (merged != null)
                mergedReaders.Add(merged);
        }

        return mergedReaders;
    }

    private static void MergeInto(FModReader dest, FModReader src)
    {
        foreach (var kv in src.EventNodes) dest.EventNodes[kv.Key] = kv.Value;
        foreach (var kv in src.BusNodes) dest.BusNodes[kv.Key] = kv.Value;
        foreach (var kv in src.EffectNodes) dest.EffectNodes[kv.Key] = kv.Value;
        foreach (var kv in src.TimelineNodes) dest.TimelineNodes[kv.Key] = kv.Value;
        foreach (var kv in src.TransitionNodes) dest.TransitionNodes[kv.Key] = kv.Value;
        foreach (var kv in src.InstrumentNodes) dest.InstrumentNodes[kv.Key] = kv.Value;
        foreach (var kv in src.WavEntries) dest.WavEntries[kv.Key] = kv.Value;
        foreach (var kv in src.ParameterNodes) dest.ParameterNodes[kv.Key] = kv.Value;
        foreach (var kv in src.ModulatorNodes) dest.ModulatorNodes[kv.Key] = kv.Value;
        foreach (var kv in src.CurveNodes) dest.CurveNodes[kv.Key] = kv.Value;
        foreach (var kv in src.PropertyNodes) dest.PropertyNodes[kv.Key] = kv.Value;
        foreach (var kv in src.MappingNodes) dest.MappingNodes[kv.Key] = kv.Value;
        foreach (var kv in src.ParameterLayoutNodes) dest.ParameterLayoutNodes[kv.Key] = kv.Value;
        foreach (var kv in src.ControllerNodes) dest.ControllerNodes[kv.Key] = kv.Value;
        foreach (var kv in src.SnapshotNodes) dest.SnapshotNodes[kv.Key] = kv.Value;
        foreach (var kv in src.VCANodes) dest.VCANodes[kv.Key] = kv.Value;

        dest.SoundBankData.AddRange(src.SoundBankData);
        dest.ControllerOwnerNodes.AddRange(src.ControllerOwnerNodes);
        if (src.HashData is { Length: > 0 }) dest.HashData = [.. dest.HashData, .. src.HashData];
    }
}