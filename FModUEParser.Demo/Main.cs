using FModUEParser;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FModUEParser.Demo;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: FModUEParser.Demo <path> [encryptionKeyString]");
            return;
        }

        string fmodPath = args[0];
        byte[]? encryptionKey = args.Length > 1 ? Encoding.UTF8.GetBytes(args[1]) : null; // Optional encryption key if soundbanks are encrypted

        if (fmodPath.EndsWith(".bank", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Loading soundbank: {Path.GetFileName(fmodPath)}");

            var fmodReader = FModUEParser.LoadSoundBank(fmodPath, encryptionKey);
            var resolvedEvents = FModUEParser.ResolveAudioEvents(fmodReader);

            Console.WriteLine($"Resolved {resolvedEvents.Count} audio events");
        }
        else
        {
            Console.WriteLine($"Loading all soundbanks from: {fmodPath}");

            var mergedReaders = FModUEParser.LoadSoundBanks(fmodPath, encryptionKey);

            foreach (var fmodReader in mergedReaders)
            {
                Console.WriteLine($"Loaded soundbank: {fmodReader.BankName}");
                var resolvedEvents = FModUEParser.ResolveAudioEvents(fmodReader);
                Console.WriteLine($"Resolved {resolvedEvents.Count} events");
            }
        }

        Console.WriteLine("FMOD parsing completed successfully");
    }
}
