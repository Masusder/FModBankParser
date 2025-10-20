using Fmod5Sharp.FmodTypes;
using FModUEParser.Extensions;
using FModUEParser.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser;

/// <summary>
/// Tested games by FMOD version:
/// 
/// FMOD | Game(s)
/// -----|---------------
/// 0x33 | All is Dust
/// 0x3E | Interloper
/// 0x40 | Ancestory
/// 0x44 | Quanero VR
/// 0x4A | Quanero VR
/// 0x87 | Train Life - A Railway Simulator (UE 4.27)
/// 0x8E | Dispatch Demo (UE 4.27) | Militsioner (UE 5.3) | Deadly Days: Roadtrip Demo (UE 5.5) | Daimon Blades (UE 5.5) | The Day Before (UE 5.2) | UNYIELDER (UE 5.3) | Firefighting Simulator Ignite (UE 5.4) | Ghostrunner2 (UE 4.27) | Disney Epic Mickey Rebrushed Demo (UE 4.27) | GODBREAKERS (UE 5.4) | Ascend | Looking For Fael Demo | Tetherfall Demo | Cave Game | Nikoderiko: The Magical World Demo | Stereo-Mix Demo | Gothic 1 Remake - Demo (Nyras Prologue) | Vespera Bononia Demo | Always With You Demo | Steel Century Groove Demo | ShantyTown Demo | TerraTech Legion Demo | Void Flesh Demo | Bullet Ballet | A.I.L.A Demo | Shepherd Knight Demo | Echoes of Mora Demo | Beat of Rebellion Demo | Wrack Remake Demo | Poveglia Demo | Primal Echo Demo
/// 0x92 | Dead as Disco Demo (UE 5.5) | Rage Quit (UE 5.5) | Goldilock One: The Mists of Jakaira Demo | Stereo-Mix Demo | 14:Overmind Demo | Underpacked! Demo | Dreadbone Demo | Lilith Demo | Groovity Demo | Skyfire Demo | LORT Demo | SpongeBob SquarePants: Titans of the Tide Demo | Manairons Demo | Bus Bound Demo | Duskfade Demo | ASHGARD: Infinity Mask Demo
/// </summary>
public class FModUEParser
{
    /// <summary>
    /// Reads and parses a single FMOD bank file.
    /// </summary>
    public static FModReader LoadSoundBank(FileInfo bankPath, byte[]? encryptionKey = null)
    {
        if (!bankPath.Exists)
            throw new ArgumentException("Soundbank path cannot be null or empty.", nameof(bankPath));

        using var reader = new BinaryReader(File.OpenRead(bankPath.FullName));
        return new FModReader(reader, bankPath.Name, encryptionKey);
    }

    /// <summary>
    /// Merges all FMOD banks in a directory and returns readers for each one.
    /// </summary>
    public static IEnumerable<FModReader> LoadSoundBanks(DirectoryInfo directoryPath, byte[]? encryptionKey = null)
    {
        if (!directoryPath.Exists)
            throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));

        return FModBankMerger.MergeBanks(directoryPath.FullName, encryptionKey);
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

    /// <summary>
    /// Result of audio export operation.
    /// </summary>
    public class AudioExportResult
    {
        /// <summary>
        /// Number of files successfully exported.
        /// </summary>
        public int FilesExported { get; init; }

        /// <summary>
        /// Whether at least one file was exported.
        /// </summary>
        public bool Success => FilesExported > 0;
    }

    /// <summary>
    /// Exports all audio samples from a FMOD reader to disk.
    /// </summary>
    /// <param name="reader">The FModReader containing soundbanks.</param>
    /// <param name="outputDirectory">Directory to save audio files.</param>
    /// <param name="overwrite">Whether to overwrite existing files.</param>
    /// <returns>An AudioExportResult with file count and success status.</returns>
    public static AudioExportResult ExportAudio(FModReader reader, DirectoryInfo outputDirectory, bool overwrite = true)
    {
        ArgumentNullException.ThrowIfNull(reader);

        if (reader.SoundBankData == null || reader.SoundBankData.Count == 0)
            return new AudioExportResult { FilesExported = 0 };

        int exportedFiles = 0;
        outputDirectory.Create();
        foreach (var bank in reader.SoundBankData)
        {
            if (bank.Samples == null || bank.Samples.Count == 0)
                continue;

            DirectoryInfo bankFolder = new(Path.Combine(outputDirectory.FullName, reader.BankName!));
            bankFolder.Create();

            for (int i = 0; i < bank.Samples.Count; i++)
            {
                var sample = bank.Samples[i];
                if (!sample.RebuildAsStandardFileFormat(out var dataBytes, out var fileExtension))
                    continue;

                string sampleName = string.IsNullOrWhiteSpace(sample.Name)
                    ? $"Sample_{i}"
                    : sample.Name;

                FileInfo filePath = new(Path.Combine(bankFolder.FullName, $"{sampleName}.{fileExtension}"));

                if (!overwrite && filePath.Exists)
                    continue;

                File.WriteAllBytes(filePath.FullName, dataBytes);
                exportedFiles++;
            }
        }

        return new AudioExportResult { FilesExported = exportedFiles };
    }
}
