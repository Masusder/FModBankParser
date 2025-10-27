using Fmod5Sharp.FmodTypes;
using FModBankParser;
using FModBankParser.Extensions;
using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser;

/// <summary>
/// Tested games by FMOD version:
/// 
/// FMOD | Game(s)
/// -----|---------------
/// 0x33 | All is Dust
/// 0x3E | Interloper
/// 0x40 | Ancestory | Medusa's Labyrinth | Planet Explorers | The Wild Eternal
/// 0x44 | Quanero VR | Vanguards | Glitchspace | Counter Agents | The Lab
/// 0x48 | VirZOOM Arcade
/// 0x4A | Quanero VR
/// 0x4F | Unearned Bounty | Battlerite | Smash Party VR | Gonio VR | Battlerite Royale
/// 0x50 | Nemesis Perspective
/// 0x58 | Kyklos Code | ERISLE | Grabity | War Robots VR: The Skirmish | Nemesis Realms | The Flesh God | Lightmatter Anniversary
/// 0x63 | To the Core | Beer'em Up | SQUIDS FROM SPACE | Death Toll | Highway Madness | Beavers Be Dammed | Vivez Versailles | Tori | Pylow | ForzeBreak | Meu | Electro Ride Prologue | ManaRocks
/// 0x64 | Hanako: Honor & Blade | Dungeon Of Zaar - Open Beta | Flora | Saint Hazel's Horsepital | VersaillesVR | Aperture Hand Lab | CONTINUE | Virtual Ricochet | RavenWeald
/// 0x65 | Old Town Stories | Plasticity | Lonely Skies | Robot Rumble 2
/// 0x67 | WarriOrb: Prologue | Malediction | KROSMAGA | Galaxy Forces VR | Cartoon Network Journeys VR | Pathos | Last Shark Standing | When The Past Was Around - Prologue | Lust from Beyond: Prologue | Decay of Logos | Nightwolf: Survive the Megadome | Skater Frog | Path of the Sramana | Rising Hell Demo | DeLight: The Journey Home | Wanderlust: Bangkok Prelude | Liberated: Free Trial | Lust from Beyond: Scarlet | Veterans Online - Open Beta | Evergreen Blues | Amends VR | Ring of Fire: Prologue | _keyboardkommander | Death Crown Demo
/// 0x77 | Sumo | Mage Mania
/// 0x79 | Rekt: Crash Test | he Edgar Mitchell Overview Effect VR Experience | Mech Mechanic Simulator: Prologue | Runo | Athanasia Demo
/// 0x7B | Dungeon Brawlers | Castle Fight | Avast Ye | Swarm | Condors Vs Ocelots | Retro Machina: Nucleonics | Nomori: Prologue
/// 0x7D | Chroma: Bloom And Blight | Mercenary Skirmish | Sunset Drive 1986 | We're All Going To Die | Octarina | Pots and Potions | Devolverland Expo | Keepers of the Trees | Raji: Prologue | Legend of Keepers: Prologue | Burn Me Twice | Way To Yaatra | Bamerang: Warm-Up Duel | Save the Date | From the Shadows | Evergate: Ki's Awakening | The Night Fisherman | The Outcast Lovers | The Imagined Leviathan: Prologue | The Change Architect | Primordials: Battle of Gods | Prison Simulator Prologue | Video World | Slay the Dragon! | Come with Me | Heart of Enya | Wonhon: Prologue | Morkredd Prøve (Demo) | Plan A | Rise of Humanity: Prologue | Arausio | Slender Threads: Prologue | The Darkest Tales — Into the Nightmare | Submorse | Nuremberg: VRdict of Nations | Wonhon: The Beginning | Necrofugitive Demo | Godsbane | Vigorus | BAWs Pharmacy | A MIRROR PUZZLE | Beneath Demo | Planisphere | Sydless Demo
/// 0x84 | Kore | Vectromirror 0™ | AGNI | Forest of Giants | LIGHT-BRINGER
/// 0x87 | Train Life - A Railway Simulator (UE 4.27) | Hunter's Arena: Revolution | Arid | Til Nord | Dichotomy | Live/Wire | Sweeping the Ruins | Hideout: Face your fears | Twin Stones: The Journey of Bukka | Payload | The Last Spark | Opus Castle | Boom Boomerang | Tails Noir: Prologue | Merchant's Rush | ToyLand | Totally Accurate Battlegrounds | Brainmelter Deluxe | A Space For The Unbound - Prologue | Rising Hell - Prologue | Whateverland: Prologue | MEGAJUMP | Torque Drift | Trainslation | Tadpole Tales | Happy Funtime Labs | The 2020 Trail | Opening Night at the Großen Schauspielhaus - Berlin 1927 | The Last Friend: First Bite | Dreams and Nightmares | Attack Of UNDO Zai | Shatter Keep | Junkpunk: Arena | Acheron's Souls | virtual beepis | Reminiscence | Void's Ballad Demo | Rust 'n Dust | Laserboy | Time on Frog Island - Prologue | Hired Ops | Yupitergrad 🚀: Sneaki Peaki (Virtual Reality Adventure) | Dialogue_EN (Lily’s Dream: Adventures of Dinosaurs) | DunDun VR | Aircraft Carrier Survival: Tutorial | Last Slice | Zleepy | Ibatic | To Carry a Sword | Space Beastz | Krai. Digital-poetry vol. 1 | Fayburrow | Project Nautilus | Downtown Jam | Jump The Gun | Triennale Game Collection 2 | For Goodness Sake | Nova Lands: Emilia's Mission | Touhou Mystery Reel | White Mirror | Townseek Demo | Space Tail: Every Journey Leads Home | Space Tail: Lost in the Sands | Possession Obsession | Aim Assault Demo | Autopanic Zero | Bloodless Demo | Crypt Keeper Demo | Dumball Rush | Farmyard Survivors Demo | GeoJet | Haneda Girl Demo" | Joutsen Ara | KIDDO Demo | Kimchi: A Stars in the Trash Story | Old Thiess: The Werewolf’s Trial | ROVE - The Wanderer's Tale Demo | Scale the Depths Demo | Sirocco | Tatari Demo | Warchief
/// 0x8D | Iragon: Prologue 18+ | Blind Fate: Edo no Yami — Dojo | Prominence Poker | Alien Removal Division | ProBee | Spectral Showdown | MITTIN: Clean Flat Surfaces | ISO | Iragon: Prologue | Diluvian Ultra: Awakening | A Buttload of Free Games | Cat Warfare | The Loneliest Artist Revamp | Dodge Dots | The Hepatica Spring | Rail Route: The Story of Jozic | The Moonlight Circus | Porcelain Tales | Comrade Quack | Missing Pictures : Catherine Hardwicke | Rolando The Majestic | Amanda the Adventurer 3 Demo | Dragon Shelter Demo | Genopanic: Prologue | Mini Tank Mayhem Demo | Mirth Melody | Nova Hearts: The Spark | Planetka | Project Juggler | Running Through The Beat | Seekers Aeterna Demo | The Séance of Blake Manor Demo | Treasure Defence
/// 0x8E | Dispatch Demo (UE 4.27) | Militsioner (UE 5.3) | Deadly Days: Roadtrip Demo (UE 5.5) | Daimon Blades (UE 5.5) | The Day Before (UE 5.2) | UNYIELDER (UE 5.3) | Firefighting Simulator Ignite (UE 5.4) | Ghostrunner2 (UE 4.27) | Disney Epic Mickey Rebrushed Demo (UE 4.27) | GODBREAKERS (UE 5.4) | Ascend | Looking For Fael Demo | Tetherfall Demo | Cave Game | Nikoderiko: The Magical World Demo | Stereo-Mix Demo | Gothic 1 Remake - Demo (Nyras Prologue) | Vespera Bononia Demo | Always With You Demo | Steel Century Groove Demo | ShantyTown Demo | TerraTech Legion Demo | Void Flesh Demo | Bullet Ballet | A.I.L.A Demo | Shepherd Knight Demo | Echoes of Mora Demo | Beat of Rebellion Demo | Wrack Remake Demo | Poveglia Demo | Primal Echo Demo | ASURAJANG | Steel Century Groove: Midnight | Stop and Breathe | What The Hack! | Voice of the Ocean | Bloodline | Pale Abyss | Sound of Survival | Anturi | Maskerade: The Deadpan Cry | Silktrails- Cats in the Grove | BULLET YEETERS | Truck World: Australia - First Haul | Mega Dimension Ripper 9000 | Lady Umbrella | Super Squad | Shaype Demo | Twilight Wars: Declassified | Fangs | Child of Lothian | NOX: Chapter 1 | Dark Table CCG | Sans Logique | Alone in the Dark Prologue | Ashen Knights: One Passage | Nowhere: Mysterious Artifacts | Negative Atmosphere: Emergency Room Prototype | The Archipelago Promise | Dust and Aliens | Aohri's Uprising | Spaceminers | Postmouse | RagBrawl | Chinese Frontiers: Prologue | Hell Of A Racket | PANDORA | MythForce Demo | Antipsychotic | KILL CRAB | SpaceMine | RegicideX | Simulakros Demo | Bloodhound: First day in hell | MONOTONIA: First Contact | Dream Bright | Scavenger | BOSS ARENA: The Goldilock One Trials | Sonder | Satori | Moon Rider | Ball Bulét | Unbreaded | Nine Realms Prologue | Digital Audio Wasteland | Ruby Dreams: Immortal Promise | Superfighters of Survival | Sanguinaria | Aard and Wyzz: The rise of minions | Goeland: Seagull Adventure Demo | BlightFog | Avante! Atlantis Demo | Hordes of Hunger Demo | Light Reforged | Downtown Drift | Adventure of Samsara Demo | Last Message | UNBEATABLE | Big Boy Boxing Demo | News Tower Demo | Invisible Wings: Chapter One | Battlecaster | Zenith: Nexus | Aeolien | Moonrakers: Luminor | Odium | Guardian | Wildaria Pre-Alpha | Astral Flux Demo | Escape First Alchemist: Prologue | Morendo | Peek a Boo | Ballads and Romances | Roboholic Demo | Esophaguys Demo | Aeolien | Crafty Survivors - Prologue | Wildaria Pre-Alpha | Melobot - A Last Song Demo | Astral Flux Demo | Abra-Cooking-Dabra Demo | Adapta Solva: Legacy | Age of Enchantment Demo | Alchemist Journey of the Soul Demo | Alchemist: Journey of the Soul Prologue | Alchemy Frogs | Ale & Tale Tavern: First Pints | Alien Tag | All Hands on Deck Demo | Angel Wings: Endless Night | Animal Adventure | Arctis Demo | Aris Arcanum Demo | Arranger: A Role-Puzzling Adventure Demo | Astra Moment:DIMENSION | Astro Prospector Prologue | Astrohaulers | Azaran: Islands of the Jinn Demo | BAPBAP | Ballrun - Demo | Battle Poet | Bed and BEAKfast | Beyond The Board Demo | Big Hops Demo | Biophobia | Biotopia Demo | Black Market | Blessed with Death Demo | BloomKeeper - Demo | BloomTale Demo | Bloomtown: A Different Story Demo | Blossom: The Seed Of Life Demo | Boogey's Wicked Game Demo | Brainrot's HAMSTER GAME | Burning Skies Arcade | By Bait or By Bullet | Calm Cove [Demo] | Cardburners Demo | Cardstronaut Demo | Castaway Soul | CatCat Demo | Cats, Guns & Robots Prologue | Chaos Classroom | Color-A-Cube | Convicted Demo | CoreRunner Demo | Corpus Edax Demo | Cozy Sanctuary Demo | Crashed | Crystal Rail Demo | Curtainfall | DONG WU: ODYSSEY | Day of the Shell: Prologue | Dead Engine Demo | Death By Misstep Demo | Death Howl Demo | Defense&Making Demo | Delverium Demo | Devilish League | Dicey Chess Singleplayer Demo | Disastory Demo | Dog And Goblin Demo | Don't Forget to Smile Demo | Don't Kill the Messenger Demo | Dragon March Demo | Drawn Tale Demo | Drone Sector Demo | Duck Paradox Demo | Echoes of Adventure Demo | Elemental Labyrinth Demo | Ella Stars Demo | Emerald Dreams: Sanity - Platformer Quest | Escape Simulator 2 Demo | Excommunicated | Exovia Demo | Eye of the Erime Demo | FP Racer | FROM ASHES, BLOOM | Fast Food Frog Demo | Finch & Archie Demo | Fix-mas | Fizz Flow: Factory Management Demo | Flibbius McDoogle and the Mysterious Flying Machine Demo | Flick Shot Rogues Demo | FlowerBots Demo | Foothold | Forgetmenots | Frayed | Free Will | GHOST CAM Demo | GNAW Demo | GOPOGO Demo | GREAT TOY SHOWDOWN | GameStonk Simulator: Demo Kiosk | Gecko Gods Demo | Ghouls Of Divinity | Giggleland: Terry’s Vegetable Patch | Go Forward Survivors Demo | Goddess Of Swing | Grill'd: The Vanishing Demo | GrindFest | Gurei Demo | Hadley's Run: A Starship Saga Demo | Hell Maiden Demo | Here Comes The Swarm - Demo | Highway to Heal Demo | Histera | Hollow Survivors: Prologue | Homura Hime Demo | INSERT GAME HERE | ITER-8 Demo | IVRI | Imperius | Into The Grid Demo | Into the Dataswarm Demo | Invasive Species | Japanese Salaryman Demo | Journey to the Void Demo | Jump the Track Demo | Keyboard Quest | Knightica: Prologue | Kona & Snowrabbit | Kotama and Academy Citadel Demo | Kriophobia Demo | Krypta FM | Kuzo Demo | L8R SK8R Demo | Lemonade Apocalypse 2: The Great Filter Demo | Lethal Honor - Order of the Apocalypse Demo | Life Below Demo | Life Slash Death Demo | Light Odyssey Demo | Locomoto Demo | Lonely Space Demo | Lumina Rush Demo | Lunar Ascendant Demo | Lunars Demo | MAiZE Demo | MULLET MAD JACK DEMO | Magic Inn: Prologue | Majulah: Shape Your City Demo | Mangut Demo | Mars Attracts Demo | Mecha Simultactics Alpha | Memoria Wake Demo | Mercapolis | Mexican Ninja Demo | Mindbug Online | Modulus Demo | Movierooms - Free Demo | Mr. Bomb Demo" | Mr. Nomad Demo | Museum Mystery: Deckbuilding Card Game | My Friend The Spider | My life with you | Mythmatch Demo | NEOTAG LEAGUE | NET.CRAWL Demo | NEXO | Nantara Adventures Demo | Neutralized: Dark moon | Nirih Demo | Njuma | Null Reference Exception | O.U.T.T. Demo | Ocean Survivors Demo | Oceaneers Demo | Of Roots and Gears Demo | Of The Lilies Demo: Part I | Oh Baby Kart | On Life And Living Demo | Oopz-Oofs | Order:the New Dawn | Oscuro Blossom's Glow Demo | Otto's Escape | Outcast Tales: The First Journey | PIO | PacaPomo | Paddle Together Demo | Painted Echoes | Pardon My French Toast Demo | Pascal's Requiem | Paw Pirates | Pentaquin: Deeds Of Twilight (Demo) | Pieces of the Past Demo | Pixgun Demo" | Pizza Slice (Demo) | Plastic Tactics Online Friend's Pass | Please, Touch The Artwork 2 | Project Apidom | Project Roll Demo | Project: Nova Demo | Psychotic Bathtub Demo | Puni the Florist Demo | Quackdzilla: Pool Cleaning Simulator | Quacolé Tennis Demo | Quadrivium - Paths of History | Quadrivium: Path of History Demo | Rainbound Demo | Reignbreaker Demo | Republic of Jungle | Revelation of Decay Demo | Rhyolite Demo | Roadside Research Demo | Robicon Demo | Roc's Odyssey Demo | Roombattle Demo | Rubinite Demo | Rue Valley Demo | Ruins To Fortress | Rumble Club | SHUFFLE SHOWDOWN | SOG: Vietnam Demo | Salvo Shuffle | Santherya Uprising Demo | Scaling Up Demo | ScanIt! Demo | Schrodinger's Cat Burglar Demo | Section Six Requiem Demo | Shard Squad Demo | Shuffle Tactics Demo | Shush | Shy Cats Hidden Tracks - Paris | Sick Samurai Demo | Slap 'em UP! Demo | Slider | Slots & Daggers Demo | Smash it Wild Demo | Smasher Demo | Soggy Beans Demo | Solar Collision Control | Solnox - Grimoire of Seasons Demo | Spark & Kling | Spectacle: Worlds Unseen | Split Personality Doctor: Prologue | Steel Carnelian: Pilot | Steel Swarm: APOCALYPSE | Sungaia Saga Demo | Sunken Engine Demo | Super Cabbage Kabumi Demo | Super Cloud Fight | TEST TEST TEST | THRASHER Demo | THYSIASTERY Demo | Tabletop Game Shop Simulator Demo | Take You Home Demo | Tavern Talk Stories: Dreamwalker Demo | Test Zero | The Chef's Shift: First Course | The Chroma: from the wasteland | The Demo of The Remake of the End of the Greatest RPG of All Time | The Last Delivery Man On Earth Demo | The Lost Son | The Midnight Barber Demo | The Siege of Jeomdo Demo | The Temple of Children (Demo) | The Tipping Point | Thing & Fist | This Is No Cave Demo | This Party Is Killer DEMO | Tiempo | Tiny Kingdom Demo | Train Reaction Demo | Unknown Fluffy Object | VRLegs | Valiants: Arena | Ventomori | Vigil | WHERE SAFETY ENDS *R* | Warcrow Adventures | Warhammer 40,000: Mechanicus II Demo | Waterpark Simulator Demo | Welcome Home | Weyrdlets : Idle Desktop Pets | Wheelborn | When the Crow Sings | Whispers of the Architects | Winnie's Hole Demo | Winter Burrow Demo | Wood and Flesh: A Candleforth Short Story | Woodo Demo | Wordatro! Demo | World's Worst Handyman: Prologue | Zoo Simulator: Prologue | [Don't] Let Go | eWorlds | 猎魔骑士 Demo | 타부티(TAVUTI)
/// 0x92 | Dead as Disco Demo (UE 5.5) | Rage Quit (UE 5.5) | Goldilock One: The Mists of Jakaira Demo | Stereo-Mix Demo | 14:Overmind Demo | Underpacked! Demo | Dreadbone Demo | Lilith Demo | Groovity Demo | Skyfire Demo | LORT Demo | SpongeBob SquarePants: Titans of the Tide Demo | Manairons Demo | Bus Bound Demo | Duskfade Demo | ASHGARD: Infinity Mask Demo | Sounds of Shadows | Realm Runner | Song of Nanzhao | Spray N' Pray | ONE WAY HOME Demo | Rhythm Towers Demo | THANKS, LIGHT. - DEMO | Hidalgo Demo | HeartWeaver Demo | Townseek Demo | A Webbing Journey Demo | Esophaguys Demo | Animas Prologue | BALL x PIT Demo | BARDA Demo | Bam Bam Boom | Blueman Demo | Compress(space) Demo | Cosmic Run Demo | Cozyrama Demo | CyberArena | Darkest Cube Demo | Dead in Antares Demo | ENDLING | ESC/APE | Echoes Of Night: The Exodus of the Stars Demo | Epoch Spire | FATHER Demo | Find Eden Demo | Generation Exile Demo | Greta Sees Ghosts! Demo | GrowBud Demo | Gunny Ascend Demo | HellPunk: Purgatorium Demo | Hemlock Demo | Hog Heist | Illumination of Mansion Demo | Incorporeal | Iron Convoy Demo | Junktown Rally Demo | Jupiter Junkworks Demo | LOST ALTR Demo | Last Line Demo | Leaks In Space: Demo | Lilith's Game Demo" | Lillium Demo | Little Rocket Lab Demo | Livber: Smoke and Mirrors Demo | Lokko Demo | Looppip Demo | Lumara Demo | Map Map - A Game About Maps Demo | Mariposa | Memories with You Demo | Mindscorn Demo | Mo The Moai Demo | Mona:The Endless Journey Demo | Monsters are Coming! Rock & Road Demo | Morbid Metal Demo | Necrologium Demo | Negative Space Demo | Noise Canceler Demo | Nullstar: Solus Demo | Passant: A Chess Roguelike Demo | Persevere - Demo | Project: Haste Demo | Quality Dreams, Reasonably Priced [Demo] | RAD: Repeat After Death Demo | Raiders of Blackveil Demo | SCHROTT Demo | Shoe it All! Demo | Snack Shop Simulator Demo | Sons of Odin | Demo | Storm Mined Demo | Stuck Together Demo | TWIN FLAMES Demo | The Bestest Alchemist Demo | The Wind and the Wisp | Tidal Rush: Tech Demo | Tri Survive Demo | Trickshotterz Demo | Watertight | Wish Me Well Demo | Witch Potions - Craft of Lust Demo | Wood & Flesh: Chapter 2 Demo | 幸存者小队 Demo | 蓬莱之旅 Demo
/// </summary>
public class FModBankParser
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

            DirectoryInfo bankFolder = new(Path.Combine(outputDirectory.FullName, reader.BankName));
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
