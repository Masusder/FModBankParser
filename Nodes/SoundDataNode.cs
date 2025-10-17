using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes;

public class SoundDataNode
{
    public readonly FmodSoundBank? SoundBank;

    public SoundDataNode(BinaryReader Ar, long nodeStart, int size, int soundDataIndex)
    {
        byte[] sndChunk = Ar.ReadBytes(size);

        uint fsbOffset = FModReader.SoundDataInfo!.Header[soundDataIndex].FSBOffset;

        var relativeOffset = (int)(fsbOffset - nodeStart) - 8;

        byte[] fsbBytes = sndChunk[relativeOffset..];

        string fsbHeader = fsbBytes.Length >= 4
            ? Encoding.UTF8.GetString(fsbBytes, 0, 4)
            : Encoding.UTF8.GetString(fsbBytes);

        // In case FSB5 is encrypted
        if (fsbHeader != "FSB5")
        {
            Console.WriteLine($"Encrypted FSB5 header at {fsbOffset}");

            if (FModReader.EncryptionKey == null) throw new Exception("FSB5 is encrypted, but encryption key wasn't provided, cannot decrypt FSB5");

            FsbDecryption.Decrypt(fsbBytes, FModReader.EncryptionKey);

            fsbHeader = fsbBytes.Length >= 4
                ? Encoding.UTF8.GetString(fsbBytes, 0, 4)
                : Encoding.UTF8.GetString(fsbBytes);

            if (fsbHeader == "FSB5") Console.WriteLine($"Decrypted FSB5 succesfully");
        }

        try
        {
            if (FsbLoader.TryLoadFsbFromByteArray(fsbBytes, out var bank) && bank != null)
            {
                SoundBank = bank;
                Console.WriteLine($"FSB5 parsed successfully, samples: {bank.Samples.Count}");
                for (int i = 0; i < bank.Samples.Count; i++)
                {
                    var sample = bank.Samples[i];
                    //Console.WriteLine($"Sample: {sample.Name}, Index: {i}");
                }
            }
            else
            {
                Console.WriteLine($"Failed to parse FSB5 at {fsbOffset}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception thrown while parsing FSB5 at {fsbOffset}: {ex.Message}");
        }
    }
}
