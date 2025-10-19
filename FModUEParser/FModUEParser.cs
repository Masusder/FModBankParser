using Fmod5Sharp.FmodTypes;
using FModUEParser.Extensions;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser;

public class FModUEParser
{
    /// <summary>
    /// Reads and parses a single FMOD bank file.
    /// </summary>
    public static FModReader LoadSoundBank(string bankPath, byte[]? encryptionKey = null)
    {
        if (string.IsNullOrWhiteSpace(bankPath))
            throw new ArgumentException("Soundbank path cannot be null or empty.", nameof(bankPath));

        using var reader = new BinaryReader(File.OpenRead(bankPath));
        return new FModReader(reader);
    }

    /// <summary>
    /// Merges all FMOD banks in a directory and returns readers for each one.
    /// </summary>
    public static IEnumerable<FModReader> LoadSoundBanks(string directoryPath, byte[]? encryptionKey = null)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));

        return FModBankMerger.MergeBanks(directoryPath, encryptionKey);
    }

    /// <summary>
    /// Resolves audio events for a given FMOD reader.
    /// Debug mode will print missing samples.
    /// </summary>
    public static Dictionary<FModGuid, List<FmodSample>> ResolveAudioEvents(FModReader fmodReader)
    {
        var resolvedEvents = EventNodesResolver.ResolveAudioEvents(fmodReader);
#if DEBUG
        EventNodesResolver.PrintMissingSamples(fmodReader, resolvedEvents);
#endif
        return resolvedEvents;
    }
}
