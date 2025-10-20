<div align="center">
  
# FModUEParser

</div>
<div align="center">
  
[![License](https://img.shields.io/github/license/Masusder/FModUEParser?style=for-the-badge&color=blue)](https://github.com/Masusder/FModUEParser/blob/main/LICENSE)
[![Stars](https://img.shields.io/github/stars/Masusder/FModUEParser?style=for-the-badge&color=F7DF1E)](https://github.com/Masusder/FModUEParser/stargazers)
[![Releases](https://img.shields.io/github/downloads/Masusder/FModUEParser/total?style=for-the-badge&color=00ffa2&label=Downloads)](https://github.com/Masusder/FModUEParser/releases)

</div>

> **C# library for parsing and extracting FMOD Studio sound banks, with a primary focus on Unreal Engine projects.**

FModUEParser allows you to **analyze, extract, and inspect** FMOD `.bank` files of any kind, including those used in commercial games and standalone FMOD projects.
It supports a wide range of FMOD versions, including those used by UE4 and UE5 titles.

This library was originally developed for the [FModel](https://github.com/4sval/FModel) project, if you are only interested in exploring or extracting FMOD audio from UE games you should check it out first.

---

## Disclaimer

This project was **reverse-engineered by studying publicly available SDKs, documentation, and game binaries**, and is **not affiliated with or endorsed by Firelight Technologies** in any way.
All work was independently performed to better understand FMOD soundbank formats within Unreal Engine projects.

No proprietary, confidential, or internal source code from Firelight Technologies was used in the development of this library.

---

## Features

This library is intended **only for parsing, reading, and extracting** FMOD `.bank`, `.assets.bank`, `.streams.bank`, and `.strings.bank` files.  
It is **not designed** and will **never** be used to rebuild or modify soundbanks.

---

## Installation

If you have a local copy of the project:

```bash
git clone https://github.com/Masusder/FModUEParser.git
```

You can include it directly in your .NET project:

```bash
dotnet add reference ../FModUEParser/FModUEParser.csproj
```

---

## Demo CLI Usage

Project comes with an example FModUEParser.Demo that you can use.

Compile and run the demo executable from a terminal:

```bash
FModUEParser.Demo --path <file_or_folder> [--key <encryptionKey>] [--export-audio] [--output <outputFolder>]
```

### Options

| Option                | Description                                                            |
|-----------------------|------------------------------------------------------------------------|
| `--help` `-h`         | Print help options                                                     |
| `--path` `-p`         | Path to a FMOD `.bank` file or folder containing soundbanks (required) |
| `--key` `-k`          | Optional encryption key string for encrypted soundbanks                |
| `--export-audio` `-e` | Flag if you want to export audio from the soundbanks                   |
| `--output` `-o`       | Optional output folder for exported audio (default: `ExportedAudio`)   |

## API Usage Examples

### Load a Single FMOD Bank

```csharp
using FModUEParser;

var reader = FModUEParser.LoadSoundBank("Master.bank");

Console.WriteLine($"Soundbank: {reader.BankName} (GUID: {reader.GetBankGuid()})");
Console.WriteLine($"FMOD Version: {reader.BankInfo.FileVersion}");
Console.WriteLine($"Event count: {reader.EventNodes.Count}");
```

### Load All Banks in a Directory

```csharp
using FModUEParser;

var readers = FModUEParser.LoadSoundBanks("C:\\Games\\MyProject\\Content\\FMOD");

foreach (var reader in readers)
    Console.WriteLine($"Loaded bank: {reader.BankName}");
```

### Audio Export
> [!Important]
> FSB5 audio extraction is handled via [Fmod5Sharp](https://github.com/SamboyCoding/Fmod5Sharp) library, make sure to check it out.

```csharp
using FModUEParser;

// Load a sound bank
var reader = FModUEParser.LoadSoundBank("Master.bank")
var outDir = new DirectoryInfo("ExportedAudio")

// Export all embedded audio
var exportedAudio = FModUEParser.ExportAudio(reader, outDir);

if (exportedAudio.Success)
{
    Console.WriteLine($"Exported {exportedAudio.FilesExported} audio files to: {outDir.FullName}");
}
else
{
    Console.WriteLine($"No audio files were exported for {file.Name}.");
}
```

## Tested FMOD Versions

| FMOD Version  | Game(s)                                                                                               |
|---------------|-------------------------------------------------------------------------------------------------------|
| `0x33`        | All is Dust                                                                                           |
| `0x3E`        | Interloper                                                                                            |
| `0x40`        | Ancestory                                                                                             |
| `0x44`        | Quanero VR                                                                                            |
| `0x4A`        | Quanero VR                                                                                            |
| `0x87`        | Train Life – A Railway Simulator (UE 4.27)                                                            |
| `0x8E`        | Dispatch Demo, Militsioner, The Day Before, Ghostrunner 2, Epic Mickey Rebrushed, Daimon Blades, etc. |
| `0x92`        | Dead as Disco Demo, Rage Quit, Spongebob: Titans of the Tide, Groovity, Dreadbone, Lilith, etc.       |

<br>

> [!NOTE]
> For a full list of tested versions check [this](https://github.com/Masusder/FModUEParser/blob/main/FModUEParser/FModUEParser.cs#L13) summary.

---

## License
FModUEParser is licensed under [Apache License 2.0](https://github.com/Masusder/FModUEParser/blob/main/LICENSE), licenses of third-party libraries used are listed [here](https://github.com/Masusder/FModUEParser/blob/main/NOTICE).