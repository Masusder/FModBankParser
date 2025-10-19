using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using System.Diagnostics;

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

        // In case FSB5 is encrypted
        if (!FSB5Decryption.IsFSB5Header(fsbBytes))
        {
            Debug.WriteLine($"Encrypted FSB5 header at {fsbOffset}");
            FSB5Decryption.Decrypt(fsbBytes, FModReader.EncryptionKey);
        }

        try
        {
            if (FsbLoader.TryLoadFsbFromByteArray(fsbBytes, out var bank) && bank != null)
            {
                SoundBank = bank;
                Debug.WriteLine($"FSB5 parsed successfully, samples: {bank.Samples.Count}");
                for (int i = 0; i < bank.Samples.Count; i++)
                {
                    var sample = bank.Samples[i];
                    //Debug.WriteLine($"Sample: {sample.Name}, Index: {i}");
                }
            }
            else
            {
                Debug.WriteLine($"Failed to parse FSB5 at {fsbOffset}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception thrown while parsing FSB5 at {fsbOffset}: {ex.Message}");
        }
    }
}
