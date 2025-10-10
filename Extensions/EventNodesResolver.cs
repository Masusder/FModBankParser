
using Fmod5Sharp.FmodTypes;
using FModUEParser.Nodes;
using FModUEParser.Objects;

namespace FModUEParser.Extensions;

// TODO: This is just a prototype
public static class EventNodesResolver
{
    public static Dictionary<FModGuid, List<FmodSample>> ResolveAudioEvents(FModReader reader)
    {
        var result = new Dictionary<FModGuid, List<FmodSample>>();
        foreach (var (eventGuid, evNode) in reader.EventNodes)
        {
            var samples = ResolveEventNodesWithAudio(reader, evNode);
            if (samples.Count > 0) result[eventGuid] = samples;
        }

        return result;
    }

    private static List<FmodSample> ResolveEventNodesWithAudio(FModReader reader, EventNode evNode)
    {
        var result = new HashSet<FmodSample>();
        var visited = new HashSet<FModGuid>();
        var stack = new Stack<FModGuid>();

        if (reader.TimelineNodes.TryGetValue(evNode.TimelineGuid, out var tmlNode))
        {
            foreach (var box in tmlNode.TriggerBoxes) stack.Push(box.Guid);
            foreach (var box in tmlNode.TimeLockedTriggerBoxes) stack.Push(box.Guid);
        }
        else
        {
            stack.Push(evNode.TimelineGuid);
        }

        foreach (var param in evNode.ParameterLayouts) stack.Push(param);

        foreach (var paramGuid in evNode.ParameterLayouts)
        {
            if (reader.ParameterLayoutNodes.TryGetValue(paramGuid, out var paramLayoutNode))
            {
                foreach (var instGuid in paramLayoutNode.Instruments) stack.Push(instGuid);
                foreach (var controllerGuid in paramLayoutNode.Controllers) stack.Push(controllerGuid);
                foreach (var triggerBoxGuid in paramLayoutNode.TriggerBoxes) stack.Push(triggerBoxGuid);
            }
        }

        foreach (var inst in evNode.EventTriggeredInstruments) stack.Push(inst);

        while (stack.Count > 0)
        {
            var guid = stack.Pop();
            if (!visited.Add(guid)) continue;

            if (reader.PlaylistNodes.TryGetValue(guid, out var plstNode))
            {
                foreach (var plstEntry in plstNode.Entries) stack.Push(plstEntry.Guid);
            }

            if (reader.InstrumentNodes.TryGetValue(guid, out var instNode)) stack.Push(instNode.TimelineGuid);

            if (reader.TimelineNodes.TryGetValue(guid, out var tmlNode2))
            {
                foreach (var box in tmlNode2.TriggerBoxes) stack.Push(box.Guid);
                foreach (var box in tmlNode2.TimeLockedTriggerBoxes) stack.Push(box.Guid);
            }

            if (reader.WaveformInstrumentNodes.TryGetValue(guid, out var wavGuid) &&
                reader.WavEntries.TryGetValue(wavGuid, out var entry) &&
                reader.SoundBankData.Count > 0 &&
                entry.SoundBankIndex < reader.SoundBankData[entry.SubsoundIndex].Samples.Count)
            {
                result.Add(reader.SoundBankData[entry.SubsoundIndex].Samples[entry.SoundBankIndex]);
                continue;
            }
        }

        return [.. result];
    }

    private static HashSet<string> GetAllResolvedSampleNames(Dictionary<FModGuid, List<FmodSample>> resolvedEvents)
    {
        var allResolvedNames = new HashSet<string>();

        foreach (var samples in resolvedEvents.Values)
            foreach (var sample in samples)
                allResolvedNames.Add(sample.Name!);

        return allResolvedNames;
    }

    public static Dictionary<FModGuid, FmodSample> GetUnreferencedSamplesWithGuids(FModReader reader, HashSet<string> allResolved)
    {
        var unreferenced = new Dictionary<FModGuid, FmodSample>();

        foreach (var kvp in reader.WavEntries)
        {
            var wavGuid = kvp.Key;
            var entry = kvp.Value;

            if (reader.SoundBankData.Count <= 0 || entry.SoundBankIndex >= reader.SoundBankData[entry.SubsoundIndex].Samples.Count)
                continue;

            var sample = reader.SoundBankData[entry.SubsoundIndex].Samples[entry.SoundBankIndex];

            if (!allResolved.Contains(sample.Name!))
            {
                unreferenced[wavGuid] = sample;
            }
        }

        return unreferenced;
    }

    public static void PrintMissingSamples(FModReader reader, Dictionary<FModGuid, List<FmodSample>> resolvedEvents)
    {
        Console.WriteLine($"----------------");
        int sampleCount = resolvedEvents.Values.Sum(samples => samples?.Count ?? 0);

        Console.WriteLine($"+ Resolved {sampleCount} audio sample(s)");

        var allResolved = GetAllResolvedSampleNames(resolvedEvents);
        var unreferencedSamples = GetUnreferencedSamplesWithGuids(reader, allResolved);

        if (unreferencedSamples.Count == 0)
        {
            Console.WriteLine("All audio samples were resolved");
            return;
        }

        Console.WriteLine($"- Unresolved {unreferencedSamples.Count} audio sample(s):");
        foreach (var sample in unreferencedSamples)
        {
            Console.WriteLine($"'{sample.Value.Name}' sample wasn't resolved (GUID: {sample.Key})");
        }
    }
}
