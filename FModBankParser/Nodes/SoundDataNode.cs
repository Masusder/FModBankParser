using Fmod5Sharp;
using Fmod5Sharp.FmodTypes;
using SubstreamSharp;
using System.Diagnostics;

namespace FModBankParser.Nodes;

public class SoundDataNode
{
    public readonly FmodSoundBank? SoundBank;

    public SoundDataNode(BinaryReader Ar, long nodeStart, uint size, int soundDataIndex)
    {
        uint fsbOffset = FModReader.SoundDataInfo!.Header[soundDataIndex].FSBOffset;
        var relativeOffset = fsbOffset - nodeStart - 8;
        Stream fsbStream = Ar.BaseStream.Substream(fsbOffset, size);

        // In case FSB5 is encrypted
        if (!FSB5Decryption.IsFSB5Header(fsbStream))
        {
            Debug.WriteLine($"Encrypted FSB5 header at {fsbOffset}");
            fsbStream = FSB5Decryption.Decrypt(fsbStream, FModReader.EncryptionKey);
        }

        try
        {
            if (FsbLoader.TryLoadFsbFromStream(fsbStream, out var bank) && bank != null)
            {
                SoundBank = bank;
                Ar.BaseStream.Position = fsbOffset - relativeOffset + size;
                Debug.WriteLine($"FSB5 parsed successfully, samples: {bank.Samples.Count}");
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
