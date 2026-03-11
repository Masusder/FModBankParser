using FModBankParser.Enums;
using System.CommandLine;
using System.Diagnostics;
using System.Text;

namespace FModBankParser.Demo;

public class Program
{
    public static int Main(string[] args)
    {
        var pathArgument = new Argument<FileSystemInfo>("path")
        {
            Description = "Path to a FMOD .bank file or folder containing soundbanks (drag-and-drop supported)",
            Arity = ArgumentArity.ExactlyOne
        };
        var keyOption = new Option<string>("--key", ["-k"]) { Description = "Optional encryption key" };
        var exportOption = new Option<bool>("--export-audio", ["-e"]) { Description = "Whether to export audio files" };
        var outDirOption = new Option<DirectoryInfo>("--output", ["-o"])
        {
            Description = "Optional output folder path for exported audio",
            DefaultValueFactory = _ => new DirectoryInfo("ExportedAudio")
        };

        var rootCommand = new RootCommand("FMOD Soundbank Parser Demo")
        {
            pathArgument,
            keyOption,
            exportOption,
            outDirOption
        };

        rootCommand.SetAction(parseResult => RunParseAndProcess(
            parseResult.GetValue(pathArgument),
            parseResult.GetValue(keyOption),
            parseResult.GetValue(exportOption),
            parseResult.GetValue(outDirOption)
        ));

        int result = rootCommand.Parse(args).Invoke();

        if (Environment.UserInteractive)
        {
            Console.WriteLine("\nPress any key to exit..");
            Console.ReadKey();
        }

        return result;
    }

    private static void RunParseAndProcess(FileSystemInfo? fsInfo, string? keyString, bool exportAudio, DirectoryInfo? outDir)
    {
        var outputDirectory = outDir ?? new DirectoryInfo("ExportedAudio");
        byte[]? encryptionKey = string.IsNullOrEmpty(keyString) ? null : Encoding.UTF8.GetBytes(keyString);

        try
        {
            switch (fsInfo)
            {
                case FileInfo file:
                    if (!file.Extension.Equals(".bank", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Error.WriteLine($"The file must be a .bank file: {file.Name}");
                        return;
                    }

                    ProcessBankFile(file, encryptionKey, exportAudio, outputDirectory);
                    return;
                case DirectoryInfo dir:
                    ProcessBankDirectory(dir, encryptionKey, exportAudio, outputDirectory);
                    break;
                case not { Exists: true }:
                    Console.Error.WriteLine("Unsupported path type.");
                    return;
            }
        }
        catch (Exception ex)
        {
            if (Debugger.IsAttached) throw; // To preserve stack trace during debugging
            Console.Error.WriteLine($"Unhandled exception: {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void ProcessBankFile(FileInfo file, byte[]? encryptionKey, bool shouldExport, DirectoryInfo outDir)
    {
        Console.WriteLine($"Loading soundbank: {file.FullName}");
        var reader = FModBankParser.LoadSoundBank(file, encryptionKey);

        if (reader == null)
        {
            Console.WriteLine($"Failed to load soundbank: {file.Name}");
            return;
        }

        PrintReaderSummary(reader);

#if DEBUG
        FModBankParser.ResolveAudioEvents(reader);
#endif

        if (shouldExport)
        {
            var exported = FModBankParser.ExportAudio(reader, outDir);

            if (exported.Success)
            {
                Console.WriteLine($"Exported {exported.FilesExported} audio files to: {outDir.FullName}");
            }
            else
            {
                Console.WriteLine($"No audio files were exported for {file.Name}.");
            }
        }
    }

    private static void ProcessBankDirectory(DirectoryInfo dir, byte[]? encryptionKey, bool shouldExport, DirectoryInfo outDir)
    {
        Console.WriteLine($"Loading all soundbanks from: {dir.FullName}");
        var readers = FModBankParser.LoadSoundBanks(dir, encryptionKey);

        foreach (var reader in readers)
        {
            PrintReaderSummary(reader);

#if DEBUG
            FModBankParser.ResolveAudioEvents(reader);
#endif

            if (shouldExport)
            {
                var exported = FModBankParser.ExportAudio(reader, outDir);

                if (exported.Success)
                {
                    Console.WriteLine($"Exported {exported.FilesExported} audio files to: {outDir.FullName}");
                }
                else
                {
                    Console.WriteLine($"No audio files were exported for {reader.BankName}.");
                }
            }
        }
    }

    private static void PrintReaderSummary(FModReader reader)
    {
        // Metadata & Header
        var version = reader.BankInfo.FileVersion;
        Console.WriteLine($"\nSoundbank: {reader.BankName} (GUID: {reader.GetBankGuid()})");
        Console.WriteLine($"FMOD Version: {version} ({(EFModVersion)version})");

        if (reader.StringTable?.RadixTree is { Guids: var guids } tree)
        {
            Console.WriteLine("This is string bank, its purpose is to convert GUIDs to human readable strings, for example:");
            // Print just first three entries found in the string table, just to give an idea
            var entries = guids.Take(3).Select((g, i) => {
                tree.TryGetStringByIndex(i, out var p);
                return $"{g}: {p}";
            });

            foreach (var line in entries) 
                Console.WriteLine(line);

            Console.WriteLine("etc.");
            return;
        }

        // Audio Data
        Console.WriteLine($"Total Audio Samples: {reader.SoundBankData.Sum(b => b.Header.NumSamples)}");
        if (reader.SoundBankData.Count is not 0) 
            Console.WriteLine($"Audio Codecs Used: {string.Join(", ", reader.SoundBankData.Select(b => b.Header.AudioType).Distinct())}");

        if (reader.SoundTable is not null)
            Console.WriteLine($"This soundbank uses Sound Table, which replaces Waveform Entries. Total samples used in Sound Table: {reader.SoundTable.Keys.Length}");

        // Nodes
        Console.WriteLine($"Events: {reader.EventNodes.Count}");
        Console.WriteLine($"Buses: {reader.BusNodes.Count}");
        Console.WriteLine($"Effects: {reader.EffectNodes.Count}");
        Console.WriteLine($"Timelines: {reader.TimelineNodes.Count}");
        Console.WriteLine($"Transitions: {reader.TransitionNodes.Count}");
        Console.WriteLine($"Instruments: {reader.InstrumentNodes.Count}");
        Console.WriteLine($"Wav Entries: {reader.WavEntries.Count}");
        Console.WriteLine($"Parameters: {reader.ParameterNodes.Count}");
        Console.WriteLine($"Modulators: {reader.ModulatorNodes.Count}");
        Console.WriteLine($"Curves: {reader.CurveNodes.Count}");
        Console.WriteLine($"Properties: {reader.PropertyNodes.Count}");
        Console.WriteLine($"Mappings: {reader.MappingNodes.Count}");
        Console.WriteLine($"Parameter Layouts: {reader.ParameterLayoutNodes.Count}");
        Console.WriteLine($"Controllers: {reader.ControllerNodes.Count}");
        Console.WriteLine($"Snapshots: {reader.SnapshotNodes.Count}");
        Console.WriteLine($"VCAs: {reader.VCANodes.Count}");
        Console.WriteLine($"Controller Owners: {reader.ControllerOwnerNodes.Count}");
    }
}
