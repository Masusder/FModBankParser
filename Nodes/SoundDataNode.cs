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
