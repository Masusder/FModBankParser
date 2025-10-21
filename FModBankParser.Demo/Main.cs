using System.CommandLine;
using System.Text;

namespace FModBankParser.Demo;

public class Program
{
    public static int Main(string[] args)
    {
        var pathOption = new Option<FileSystemInfo>("--path", ["-p"]) 
        { 
            Description = "Path to a FMOD .bank file or folder containing soundbanks",
            Required = true 
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
            pathOption,
            keyOption,
            exportOption,
            outDirOption
        };

        rootCommand.SetAction(parseResult => RunParseAndProcess(
            parseResult.GetValue(pathOption),
            parseResult.GetValue(keyOption),
            parseResult.GetValue(exportOption),
            parseResult.GetValue(outDirOption)
        ));

        return rootCommand.Parse(args).Invoke();
    }

    private static void RunParseAndProcess(FileSystemInfo? fsInfo, string? keyString, bool exportAudio, DirectoryInfo? outDir)
    {
        var outputDirectory = outDir ?? new DirectoryInfo("ExportedAudio");
        byte[]? encryptionKey = string.IsNullOrEmpty(keyString) ? null : Encoding.UTF8.GetBytes(keyString);

        try
        {
            if (fsInfo is FileInfo file)
            {
                if (!file.Extension.Equals(".bank", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine($"The file must be a .bank file: {file.Name}");
                    return;
                }

                ProcessBankFile(file, encryptionKey, exportAudio, outputDirectory);
            }
            else if (fsInfo is DirectoryInfo dir)
            {
                ProcessBankDirectory(dir, encryptionKey, exportAudio, outputDirectory);
            }
            else
            {
                Console.Error.WriteLine("Unsupported path type.");
            }
        }
        catch (Exception ex)
        {
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

    private static void PrintReaderSummary(dynamic reader)
    {
        Console.WriteLine($"\nSoundbank: {reader.BankName} (GUID: {reader.GetBankGuid()})");
        Console.WriteLine($"FMOD Version: {reader.BankInfo.FileVersion}");
        Console.WriteLine($"Event count: {reader.EventNodes.Count}");
    }
}
