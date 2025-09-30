using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pokemod.Common.Configs;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Content.NPCs
{
    internal class PokemonNPCData : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool isPokemon = false;
        public string pokemonName = "";
        public bool shiny = false;
        public int lvl;
        public int[] baseStats;
        public int[] IVs = new int[6];
        public int nature;
        public bool ultrabeast = false;
        public string variant = "";

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            bitWriter.WriteBit(isPokemon);
            bitWriter.WriteBit(shiny);
            bitWriter.WriteBit(ultrabeast);
            binaryWriter.Write(pokemonName);
            binaryWriter.Write(lvl);
            binaryWriter.Write(nature);
            for (int i = 0; i < 6; i++)
            {
                if (i < IVs.Length) binaryWriter.Write(IVs[i]);
                else binaryWriter.Write(0);
			}
		}

        // Make sure you always read exactly as much data as you sent!
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            isPokemon = bitReader.ReadBit();
            shiny = bitReader.ReadBit();
            ultrabeast = bitReader.ReadBit();
            pokemonName = binaryReader.ReadString();
            lvl = binaryReader.ReadInt32();
            nature = binaryReader.ReadInt32();
            IVs = [0,0,0,0,0,0];
            for (int i = 0; i < IVs.Length; i++)
            {
                IVs[i] = binaryReader.ReadInt32();
            }
		}
        

        public void SetPokemonNPCData(string pokemonName, bool shiny = false, int lvl = 5, int[] baseStats = null, int[] IVs = null, int nature = -1, bool ultrabeast = false, string variant = ""){
            isPokemon = true;
            this.pokemonName = pokemonName;
            this.shiny = shiny;
            this.lvl = lvl;
            this.baseStats = baseStats;
            this.IVs = IVs;
            if(nature < 0) nature = 10*Main.rand.Next(5)+Main.rand.Next(5);
            this.nature = nature;
            this.ultrabeast = ultrabeast;
            this.variant = variant;
        }

        public static int[] CalcAllStats(int level, int[] stats, int[] IVs, int[] EVs, int nature)
        {
            int[] allStats = {0,0,0,0,0,0};
            for(int i = 0; i < allStats.Length; i++){
                allStats[i] = StatFunc(i,stats[i],IVs[i],EVs[i],level, nature);
            }

            return allStats;
        }

        public static int StatFunc(int index, int baseStat,int IV,int EV, int Level, int nature)
        {
            int done;

            if(index==0) done = (((2 * baseStat + IV + (EV/4)) * Level/100) + Level + 10) * 5;
            else done = (int)(((2 * (baseStat + IV + (EV / 4)) * Level / 100) + 5)*GetNatureMult(index, nature));

            return done;
        }

        public int GetWildCalcStat(int index)
        {
            return StatFunc(index, baseStats[index], IVs[index], 0, lvl, nature);
        }

        public static int[] GenerateIVs()
        {
            int[] IVs = [0, 0, 0, 0, 0, 0, 0];
            for (int i = 0; i < IVs.Length; i++)
            {
                IVs[i] = GenerateIV();
            }

            return IVs;
        }

        public static int GenerateIV()
        {
            return Main.rand.Next(32);
        }

        public static float GetNatureMult(int statIndex, int nature){
            statIndex = Math.Clamp(statIndex-1, 0, 4);
            float result = 1f;

			if(statIndex == nature/10) result += 0.1f;
			if(statIndex == nature%10) result -= 0.1f;

            return result;
        }

        public static string[] GetStarters(){
            string[] starters = {"","",""};

            if(ModContent.GetInstance<GameplayConfig>().RandomizedStarters){
                List<string> blackList = new List<string>();
                for(int i = 0; i < starters.Length; i++){
                    starters[i] = GetStarter(ref blackList);
                }
            }else{
                string[] grassStarters = {"Eevee", "Bulbasaur","Chikorita"};
                string[] fireStarters = {"Pikachu", "Charmander","Cyndaquil"};
                string[] waterStarters = {"Squirtle","Totodile"};

                starters[0] = grassStarters[Main.rand.Next(grassStarters.Length)];
                starters[1] = fireStarters[Main.rand.Next(fireStarters.Length)];
                starters[2] = waterStarters[Main.rand.Next(waterStarters.Length)];
            }

            return starters;
        }

        private static string GetStarter(ref List<string> blackList){
            string[] pokemonNames = PokemonData.pokemonInfo.Keys.ToArray();
            bool canBeStarter = false;
            int pokemonIndex;
            string pokemonName = "";

            while(!canBeStarter){
                pokemonIndex = Main.rand.Next(PokemonData.pokemonInfo.Keys.Count);
                pokemonName = pokemonNames[pokemonIndex];

                if(!blackList.Contains(pokemonName) && PokemonData.pokemonInfo[pokemonName].completed && PokemonData.pokemonInfo[pokemonName].pokemonStage == 0 && !PokemonData.pokemonInfo[pokemonName].legendary && Enumerable.Sum(PokemonData.pokemonInfo[pokemonName].pokemonStats) < 350){
                    canBeStarter = true;
                    blackList.Add(pokemonName);
                }
            }

            return pokemonName;
        }

        public static string GetRandomEvolution(string pokemonName){
            int stage = 0;
            if(PokemonData.pokemonInfo[pokemonName].pokemonStage < 2 || PokemonData.pokemonInfo[pokemonName].pokemonStage == 3){
                if(PokemonData.pokemonInfo[pokemonName].pokemonStage < 2){
                    stage = PokemonData.pokemonInfo[pokemonName].pokemonStage+1;
                }

                var filterList = PokemonData.pokemonInfo.Where(i => i.Value.pokemonStage == stage);
                var posibleEvolutions = filterList.ToDictionary(i => i.Key, i => i.Value);
                int pokemonIndex = 0;
                bool canBeEvo = false;

                while(!canBeEvo){
                    pokemonIndex = Main.rand.Next(posibleEvolutions.Keys.Count);
                    if(PokemonData.pokemonInfo[posibleEvolutions.Keys.ToArray()[pokemonIndex]].completed){
                        canBeEvo = true;
                    }
                }

                return posibleEvolutions.Keys.ToArray()[pokemonIndex];
            }

            return "";
        }

        public static string GetTypeColor(int type){
            string[] typeColors = ["ffffff", "ff7800", "79c3e8", "a004ff", "732400",
            "957465", "95bf36", "522c57", "829ea9", "ff0f0f",
            "009bff", "0db602", "f9ca0e", "ff27a0", "00f7ff",
            "3450de", "514d3e", "ff73f0"];

            type = Math.Clamp(type, 0, typeColors.Length);

            return typeColors[type];
        }
    }

    internal class PokemonData{
        public static Dictionary<string, PokemonInfo> pokemonInfo = new(){
            //Gen 1
            {"Bulbasaur", new PokemonInfo(0001, [45, 49, 49, 65, 65, 45], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("Tackle"), new MoveLvl("VineWhip", 3), new MoveLvl("RazorLeaf", 12), new MoveLvl("PoisonPowder", 15), new MoveLvl("BulletSeed", 18), new MoveLvl("GigaDrain", 27), new MoveLvl("DoubleEdge", 33), new MoveLvl("SolarBeam", 36)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Ivysaur", new PokemonInfo(0002, [60, 62, 63, 80, 80, 60], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("Tackle", 1), new MoveLvl("VineWhip", 3), new MoveLvl("RazorLeaf", 12), new MoveLvl("PoisonPowder", 15), new MoveLvl("BulletSeed", 20), new MoveLvl("GigaDrain", 35), new MoveLvl("DoubleEdge", 45), new MoveLvl("SolarBeam", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Venusaur", new PokemonInfo(0003, [80, 82, 83, 100, 100, 80], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("LeafStorm"), new MoveLvl("Tackle", 1), new MoveLvl("VineWhip", 1), new MoveLvl("RazorLeaf", 12), new MoveLvl("PoisonPowder", 15), new MoveLvl("BulletSeed", 20), new MoveLvl("GigaDrain", 37), new MoveLvl("DoubleEdge", 51), new MoveLvl("SolarBeam", 58)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Charmander", new PokemonInfo(0004,[39, 52, 43, 60, 50, 65], [(int)TypeIndex.Fire], [new MoveLvl("Tackle"), new MoveLvl("Ember", 4), new MoveLvl("Smokescreen", 8), new MoveLvl("DragonBreath", 12), new MoveLvl("Slash", 20), new MoveLvl("Flamethrower", 24), new MoveLvl("FlameWheel", 32), new MoveLvl("FireBlast", 40)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Charmeleon", new PokemonInfo(0005,[58, 64, 58, 80, 65, 80], [(int)TypeIndex.Fire], [new MoveLvl("Tackle", 1), new MoveLvl("Ember", 1), new MoveLvl("Smokescreen", 1), new MoveLvl("DragonBreath", 12), new MoveLvl("Slash", 24), new MoveLvl("Flamethrower", 30), new MoveLvl("FlameWheel", 42), new MoveLvl("FireBlast", 54)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Charizard", new PokemonInfo(0006,[78, 84, 78, 109, 85, 100], [(int)TypeIndex.Fire,(int)TypeIndex.Flying], [new MoveLvl("AirSlash"), new MoveLvl("Tackle", 1), new MoveLvl("Ember", 1), new MoveLvl("Smokescreen", 1), new MoveLvl("DragonBreath", 12), new MoveLvl("Slash", 24), new MoveLvl("Flamethrower", 30), new MoveLvl("FlameWheel", 46), new MoveLvl("FireBlast", 62)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Squirtle", new PokemonInfo(0007,[44, 48, 65, 50, 64, 43], [(int)TypeIndex.Water], [new MoveLvl("Tackle"), new MoveLvl("WaterGun", 3), new MoveLvl("WaterPulse", 12), new MoveLvl("Crunch", 15), new MoveLvl("Harden", 18), new MoveLvl("BubbleBeam", 24), new MoveLvl("Waterfall", 30), new MoveLvl("HydroPump", 33), new MoveLvl("DoubleEdge", 36)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Wartortle", new PokemonInfo(0008, [59, 63, 80, 65, 80, 58], [(int)TypeIndex.Water], [new MoveLvl("Tackle", 1), new MoveLvl("WaterGun", 1), new MoveLvl("WaterPulse", 12), new MoveLvl("Crunch", 15), new MoveLvl("Harden", 20), new MoveLvl("BubbleBeam", 30), new MoveLvl("Waterfall", 40), new MoveLvl("HydroPump", 45), new MoveLvl("DoubleEdge", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Blastoise", new PokemonInfo(0009, [79, 83, 100, 85, 105, 78], [(int)TypeIndex.Water], [new MoveLvl("FlashCannon"), new MoveLvl("Tackle", 1), new MoveLvl("WaterGun", 1), new MoveLvl("WaterPulse", 12), new MoveLvl("Crunch", 15), new MoveLvl("Harden", 20), new MoveLvl("BubbleBeam", 30), new MoveLvl("Waterfall", 42), new MoveLvl("HydroPump", 49), new MoveLvl("DoubleEdge", 56)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Caterpie", new PokemonInfo(0010, [45, 30, 35, 20, 20, 45], [(int)TypeIndex.Bug], [new MoveLvl("Tackle"), new MoveLvl("StringShot")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Metapod", new PokemonInfo(0011, [50, 20, 55, 25, 25, 30], [(int)TypeIndex.Bug], [new MoveLvl("Harden")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Butterfree", new PokemonInfo(0012, [60, 45, 50, 90, 80, 70], [(int)TypeIndex.Bug,(int)TypeIndex.Flying], [new MoveLvl("Confusion"), new MoveLvl("Gust"), new MoveLvl("Harden", 1), new MoveLvl("StringShot", 1), new MoveLvl("PoisonPowder", 12), new MoveLvl("Psybeam", 16), new MoveLvl("AirSlash", 24)], (int)StageIndex.Stage2, (int)ExpTypes.MediumFast)},
            
            {"Weedle", new PokemonInfo(0013, [40, 35, 30, 20, 20, 50], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], [new MoveLvl("PoisonSting"), new MoveLvl("StringShot")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Kakuna", new PokemonInfo(0014, [45, 25, 50, 25, 25, 35], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], [new MoveLvl("Harden")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Beedrill", new PokemonInfo(0015, [65, 90, 40, 45, 80, 75], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], [new MoveLvl("FuryCutter"), new MoveLvl("QuickAttack"), new MoveLvl("Harden", 1), new MoveLvl("StringShot", 1), new MoveLvl("PoisonSting", 17), new MoveLvl("PinMissile", 32), new MoveLvl("SludgeBomb", 35)], (int)StageIndex.Stage2, (int)ExpTypes.MediumFast)},
            
            {"Pidgey", new PokemonInfo(0016, [40, 45, 40, 35, 35, 56], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("Tackle"), new MoveLvl("Gust", 9), new MoveLvl("QuickAttack", 13), new MoveLvl("WingAttack", 33), new MoveLvl("AirSlash", 49)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Pidgeotto", new PokemonInfo(0017, [63, 60, 55, 50, 50, 71], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("QuickAttack", 13), new MoveLvl("WingAttack", 37), new MoveLvl("AirSlash", 57)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Pidgeot", new PokemonInfo(0018, [83, 80, 75, 70, 70, 101], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("WingAttack", 38), new MoveLvl("AirSlash", 62)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Rattata", new PokemonInfo(0019, [30, 56, 35, 25, 35, 72], [(int)TypeIndex.Normal], [new MoveLvl("Tackle"), new MoveLvl("QuickAttack", 4), new MoveLvl("Crunch", 22), new MoveLvl("HyperFang", 28), new MoveLvl("DoubleEdge", 31)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Raticate", new PokemonInfo(0020, [55, 81, 60, 50, 70, 97], [(int)TypeIndex.Normal], [new MoveLvl("Tackle", 1), new MoveLvl("QuickAttack", 4), new MoveLvl("Crunch", 24), new MoveLvl("HyperFang", 34), new MoveLvl("DoubleEdge", 39)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Spearow", new PokemonInfo(0021, [40, 60, 30, 31, 31, 70], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("Gust")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Fearow", new PokemonInfo(0022, [65, 90, 65, 61, 61, 100], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("WingAttack")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Ekans", new PokemonInfo(0023, [35, 60, 44, 40, 54, 55], [(int)TypeIndex.Poison], [new MoveLvl("PoisonSting"), new MoveLvl("SludgeBomb", 33)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Arbok", new PokemonInfo(0024, [60, 95, 69, 65, 79, 80], [(int)TypeIndex.Poison], [new MoveLvl("IceFang"), new MoveLvl("PoisonSting"), new MoveLvl("Crunch"), new MoveLvl("SludgeBomb", 39)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Pikachu", new PokemonInfo(0025, [35, 55, 40, 50, 50, 90], [(int)TypeIndex.Electric], [new MoveLvl("Tackle"), new MoveLvl("ThunderWave", 4), new MoveLvl("QuickAttack", 8), new MoveLvl("ElectroBall", 12), new MoveLvl("Swift", 20), new MoveLvl("Discharge", 28), new MoveLvl("Thunderbolt", 36), new MoveLvl("Thunder", 44)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Raichu", new PokemonInfo(0026, [60, 90, 55, 90, 80, 110], [(int)TypeIndex.Electric], [new MoveLvl("Thunderbolt"), new MoveLvl("QuickAttack", 1), new MoveLvl("ElectroBall", 1), new MoveLvl("Discharge", 1)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Sandshrew", new PokemonInfo(0027, [50, 75, 85, 20, 30, 40], [(int)TypeIndex.Ground], [new MoveLvl("QuickAttack")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Sandslash", new PokemonInfo(0028, [75, 100, 110, 45, 55, 65], [(int)TypeIndex.Ground], [new MoveLvl("Dig"), new MoveLvl("MudShot")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"NidoranF", new PokemonInfo(0029, [46, 57, 40, 40, 40, 50], [(int)TypeIndex.Poison], [new MoveLvl("PoisonSting"), new MoveLvl("DoubleKick", 25), new MoveLvl("Toxic", 40), new MoveLvl("Crunch", 50), new MoveLvl("Earthquake", 55)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Nidorina", new PokemonInfo(0030, [70, 62, 67, 55, 55, 56], [(int)TypeIndex.Poison], [new MoveLvl("PoisonSting"), new MoveLvl("DoubleKick", 29), new MoveLvl("Toxic", 50), new MoveLvl("Crunch", 64), new MoveLvl("Earthquake", 71)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Nidoqueen", new PokemonInfo(0031, [90, 92, 87, 75, 85, 76], [(int)TypeIndex.Poison,(int)TypeIndex.Ground], [new MoveLvl("Toxic"), new MoveLvl("Crunch"), new MoveLvl("Earthquake", 43), new MoveLvl("FocusPunch", 58)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"NidoranM", new PokemonInfo(0032, [55, 47, 52, 40, 40, 41], [(int)TypeIndex.Poison], [new MoveLvl("PoisonSting"), new MoveLvl("DoubleKick", 25), new MoveLvl("Toxic", 40), new MoveLvl("SludgeBomb", 50), new MoveLvl("Earthquake", 55)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Nidorino", new PokemonInfo(0033, [61, 72, 57, 55, 55, 65], [(int)TypeIndex.Poison], [new MoveLvl("PoisonSting"), new MoveLvl("DoubleKick", 29), new MoveLvl("Toxic", 50), new MoveLvl("SludgeBomb", 64), new MoveLvl("Earthquake", 71)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Nidoking", new PokemonInfo(0034, [81, 102, 77, 85, 75, 85], [(int)TypeIndex.Poison,(int)TypeIndex.Ground], [new MoveLvl("Toxic"), new MoveLvl("SludgeBomb"), new MoveLvl("Earthquake", 43), new MoveLvl("FocusPunch", 58)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Clefairy", new PokemonInfo(0035, [70, 45, 48, 60, 65, 35], [(int)TypeIndex.Fairy], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.Fast, completed: false)},
            {"Clefable", new PokemonInfo(0036, [95, 70, 73, 95, 90, 60], [(int)TypeIndex.Fairy], [new MoveLvl("HealPulse"), new MoveLvl("Tackle")], (int)StageIndex.Stage1, (int)ExpTypes.Fast, completed: false)},
            
            {"Vulpix", new PokemonInfo(0037, [38, 41, 40, 50, 65, 65], [(int)TypeIndex.Fire], [new MoveLvl("Ember"), new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Ninetales", new PokemonInfo(0038, [73, 76, 75, 81, 100, 100], [(int)TypeIndex.Fire], [new MoveLvl("LavaPlume")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Jigglypuff", new PokemonInfo(0039, [115, 45, 20, 45, 25, 20], [(int)TypeIndex.Normal,(int)TypeIndex.Fairy], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.Fast, completed: false)},
            {"Wigglytuff", new PokemonInfo(0040, [140, 70, 45, 85, 50, 45], [(int)TypeIndex.Normal,(int)TypeIndex.Fairy], [new MoveLvl("DoubleEdge"), new MoveLvl("Swift")], (int)StageIndex.Stage1, (int)ExpTypes.Fast, completed: false)},
            
            {"Zubat", new PokemonInfo(0041, [40, 45, 35, 30, 40, 55], [(int)TypeIndex.Poison,(int)TypeIndex.Flying], [new MoveLvl("ConfuseRay")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Golbat", new PokemonInfo(0042, [75, 80, 70, 65, 75, 90], [(int)TypeIndex.Poison,(int)TypeIndex.Flying], [new MoveLvl("AirSlash")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Oddish",new PokemonInfo(0043, [45, 50, 55, 75, 65, 30], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("RazorLeaf")], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Gloom", new PokemonInfo(0044, [60, 65, 70, 85, 75, 40], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("Toxic"),new MoveLvl("MegaDrain")], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Vileplume", new PokemonInfo(0045, [75, 80, 85, 110, 90, 50], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("GigaDrain")], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            
            {"Paras", new PokemonInfo(0046, [35, 70, 55, 45, 55, 25], [(int)TypeIndex.Bug,(int)TypeIndex.Grass], [new MoveLvl("Tackle"), new MoveLvl("PoisonPowder", 6), new MoveLvl("VineWhip", 11), new MoveLvl("FuryCutter", 17), new MoveLvl("Slash", 27), new MoveLvl("GigaDrain", 38), new MoveLvl("LeafBlade", 49)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Parasect", new PokemonInfo(0047, [60, 95, 80, 60, 80, 30], [(int)TypeIndex.Bug,(int)TypeIndex.Grass], [new MoveLvl("Tackle", 1), new MoveLvl("PoisonPowder", 6), new MoveLvl("VineWhip", 11), new MoveLvl("FuryCutter", 17), new MoveLvl("Slash", 29), new MoveLvl("GigaDrain", 44), new MoveLvl("LeafBlade", 59)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Venonat", new PokemonInfo(0048, [60, 55, 50, 40, 55, 45], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], [new MoveLvl("Confusion")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Venomoth", new PokemonInfo(0049, [70, 65, 60, 90, 75, 90], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], [new MoveLvl("Psychic")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Diglett", new PokemonInfo(0050, [10, 55, 25, 35, 45, 95], [(int)TypeIndex.Ground], [new MoveLvl("Tackle"), new MoveLvl("MudShot", 9), new MoveLvl("Dig", 17), new MoveLvl("RockThrow", 25), new MoveLvl("Slash", 33), new MoveLvl("Earthquake", 41)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Dugtrio", new PokemonInfo(0051, [35, 100, 50, 50, 70, 120], [(int)TypeIndex.Ground], [new MoveLvl("Tackle", 1), new MoveLvl("MudShot", 9), new MoveLvl("Dig", 17), new MoveLvl("RockThrow", 25), new MoveLvl("Slash", 37), new MoveLvl("Earthquake", 49)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Meowth", new PokemonInfo(0052, [40, 45, 35, 40, 40, 90], [(int)TypeIndex.Normal], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Persian", new PokemonInfo(0053, [65, 70, 60, 65, 65, 115], [(int)TypeIndex.Normal], [new MoveLvl("HyperFang")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Psyduck", new PokemonInfo(0054, [50, 52, 48, 65, 50, 55], [(int)TypeIndex.Water], [new MoveLvl("Tackle"), new MoveLvl("WaterGun", 3), new MoveLvl("Confusion", 6), new MoveLvl("WaterPulse", 12), new MoveLvl("PsychoCut", 18), new MoveLvl("Waterfall", 24), new MoveLvl("ConfuseRay", 30), new MoveLvl("HydroPump", 36)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Golduck", new PokemonInfo(0055, [80, 82, 78, 95, 80, 85], [(int)TypeIndex.Water], [new MoveLvl("Tackle", 1), new MoveLvl("WaterGun", 1), new MoveLvl("Confusion", 1), new MoveLvl("WaterPulse", 12), new MoveLvl("PsychoCut", 18), new MoveLvl("Waterfall", 24), new MoveLvl("ConfuseRay", 30), new MoveLvl("HydroPump", 40)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Mankey", new PokemonInfo(0056, [40, 80, 35, 35, 45, 70], [(int)TypeIndex.Fighting], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Primeape", new PokemonInfo(0057, [65, 105, 60, 60, 70, 95], [(int)TypeIndex.Fighting], [new MoveLvl("DoubleKick")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Growlithe", new PokemonInfo(0058, [55, 70, 45, 70, 50, 60], [(int)TypeIndex.Fire], [new MoveLvl("Ember"), new MoveLvl("Crunch", 32)], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Arcanine", new PokemonInfo(0059, [90, 110, 80, 100, 80, 95], [(int)TypeIndex.Fire], [new MoveLvl("FireBlast"), new MoveLvl("Crunch")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            
            {"Poliwag", new PokemonInfo(0060, [40, 50, 40, 40, 40, 90], [(int)TypeIndex.Water], [new MoveLvl("WaterGun"), new MoveLvl("Tackle"), new MoveLvl("MudShot", 12), new MoveLvl("BubbleBeam", 18), new MoveLvl("Earthquake", 36), new MoveLvl("HydroPump", 42), new MoveLvl("DoubleEdge", 54)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Poliwhirl", new PokemonInfo(0061, [65, 65, 65, 50, 50, 90], [(int)TypeIndex.Water], [new MoveLvl("WaterGun", 1), new MoveLvl("MudShot", 1), new MoveLvl("BubbleBeam", 1), new MoveLvl("Earthquake", 40), new MoveLvl("HydroPump", 48), new MoveLvl("DoubleEdge", 66)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Poliwrath", new PokemonInfo(0062, [90, 95, 95, 70, 90, 70], [(int)TypeIndex.Water,(int)TypeIndex.Fighting], [new MoveLvl("HydroPump"),new MoveLvl("FocusPunch")], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Abra", new PokemonInfo(0063, [25, 20, 15, 105, 55, 90], [(int)TypeIndex.Psychic], [new MoveLvl("Teleport")], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Kadabra", new PokemonInfo(0064, [40, 35, 30, 120, 70, 105], [(int)TypeIndex.Psychic], [new MoveLvl("Confusion"), new MoveLvl("Teleport"), new MoveLvl("Psybeam", 24), new MoveLvl("HealPulse", 30), new MoveLvl("PsychoCut", 34), new MoveLvl("Psychic", 40)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Alakazam", new PokemonInfo(0065, [55, 50, 45, 135, 95, 120], [(int)TypeIndex.Psychic], [new MoveLvl("Confusion", 1), new MoveLvl("Teleport", 1), new MoveLvl("Psybeam", 24), new MoveLvl("HealPulse", 30), new MoveLvl("PsychoCut", 34), new MoveLvl("Psychic", 40)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Machop", new PokemonInfo(0066, [70, 80, 50, 35, 35, 35], [(int)TypeIndex.Fighting], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Machoke", new PokemonInfo(0067, [80, 100, 70, 50, 60, 45], [(int)TypeIndex.Fighting], [new MoveLvl("DoubleKick")], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Machamp", new PokemonInfo(0068, [90, 130, 80, 65, 85, 55], [(int)TypeIndex.Fighting], [new MoveLvl("FocusPunch")], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            
            {"Bellsprout", new PokemonInfo(0069, [50, 75, 35, 70, 30, 40], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("VineWhip"), new MoveLvl("PoisonPowder", 12), new MoveLvl("ThunderWave", 17), new MoveLvl("PoisonSting", 23), new MoveLvl("Crunch", 29), new MoveLvl("RazorLeaf", 39), new MoveLvl("SludgeBomb", 41)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Weepinbell", new PokemonInfo(0070, [65, 90, 50, 85, 45, 55], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("VineWhip", 1), new MoveLvl("PoisonPowder", 12), new MoveLvl("ThunderWave", 17), new MoveLvl("PoisonSting", 24), new MoveLvl("Crunch", 32), new MoveLvl("RazorLeaf", 44), new MoveLvl("SludgeBomb", 47)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Victreebel", new PokemonInfo(0071, [80, 105, 65, 100, 70, 70], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("BulletSeed"), new MoveLvl("VineWhip", 1), new MoveLvl("PoisonPowder", 1), new MoveLvl("PoisonSting", 1), new MoveLvl("LeafStorm", 32), new MoveLvl("LeafBlade", 44)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Tentacool", new PokemonInfo(0072, [40, 40, 35, 50, 100, 70], [(int)TypeIndex.Water,(int)TypeIndex.Poison], [new MoveLvl("Bubble")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Tentacruel", new PokemonInfo(0073, [80, 70, 65, 80, 120, 100], [(int)TypeIndex.Water,(int)TypeIndex.Poison], [new MoveLvl("PoisonSting")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            
            {"Geodude", new PokemonInfo(0074, [40, 80, 100, 30, 30, 20], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [new MoveLvl("Tackle"), new MoveLvl("RockThrow", 10), new MoveLvl("Harden", 16), new MoveLvl("MudShot", 22), new MoveLvl("SelfDestruct", 26), new MoveLvl("Dig", 30), new MoveLvl("Earthquake", 34), new MoveLvl("Explosion", 38), new MoveLvl("DoubleEdge", 42)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Graveler", new PokemonInfo(0075, [55, 95, 115, 45, 45, 35], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [new MoveLvl("Tackle", 1), new MoveLvl("RockThrow", 10), new MoveLvl("Harden", 16), new MoveLvl("MudShot", 22), new MoveLvl("SelfDestruct", 28), new MoveLvl("Dig", 34), new MoveLvl("Earthquake", 40), new MoveLvl("Explosion", 44), new MoveLvl("DoubleEdge", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Golem", new PokemonInfo(0076, [80, 120, 130, 55, 65, 45], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [new MoveLvl("Tackle", 1), new MoveLvl("RockThrow", 10), new MoveLvl("Harden", 16), new MoveLvl("MudShot", 22), new MoveLvl("SelfDestruct", 28), new MoveLvl("Dig", 34), new MoveLvl("Earthquake", 40), new MoveLvl("Explosion", 44), new MoveLvl("DoubleEdge", 50)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Ponyta", new PokemonInfo(0077, [50, 85, 55, 65, 65, 90], [(int)TypeIndex.Fire], [new MoveLvl("Ember")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Rapidash", new PokemonInfo(0078, [65, 100, 70, 80, 80, 105], [(int)TypeIndex.Fire], [new MoveLvl("FlameWheel")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Slowpoke", new PokemonInfo(0079, [90, 65, 65, 40, 40, 15], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], [new MoveLvl("Confusion")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Slowbro", new PokemonInfo(0080, [95, 75, 110, 100, 80, 30], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], [new MoveLvl("ConfuseRay")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Magnemite", new PokemonInfo(0081, [25, 35, 70, 95, 55, 45], [(int)TypeIndex.Electric,(int)TypeIndex.Steel], [new MoveLvl("Tackle"), new MoveLvl("ConfuseRay", 4), new MoveLvl("ThunderWave", 8), new MoveLvl("ElectroBall", 12), new MoveLvl("Swift", 20), new MoveLvl("FlashCannon", 28), new MoveLvl("Discharge", 36), new MoveLvl("Thunder", 44)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Magneton", new PokemonInfo(0082, [50, 60, 95, 120, 70, 70], [(int)TypeIndex.Electric,(int)TypeIndex.Steel], [new MoveLvl("Thunderbolt"), new MoveLvl("Tackle", 1), new MoveLvl("ConfuseRay", 1), new MoveLvl("ThunderWave", 1), new MoveLvl("ElectroBall", 12), new MoveLvl("Swift", 20), new MoveLvl("FlashCannon", 28), new MoveLvl("Discharge", 40), new MoveLvl("Thunder", 52)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Farfetchd", new PokemonInfo(0083, [52, 90, 55, 58, 62, 60], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("Swift")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Doduo", new PokemonInfo(0084, [35, 85, 45, 35, 35, 75], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("QuickAttack")], (int)StageIndex.Basic, completed: false)},
            {"Dodrio", new PokemonInfo(0085, [60, 110, 70, 60, 60, 110], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], [new MoveLvl("WingAttack")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Seel", new PokemonInfo(0086, [65, 45, 55, 45, 70, 45], [(int)TypeIndex.Water], [new MoveLvl("WaterGun")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Dewgong", new PokemonInfo(0087, [90, 70, 80, 70, 95, 70], [(int)TypeIndex.Water,(int)TypeIndex.Ice], [new MoveLvl("IceFang")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Grimer", new PokemonInfo(0088, [80, 80, 50, 40, 50, 25], [(int)TypeIndex.Poison], [new MoveLvl("MudShot")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Muk", new PokemonInfo(0089, [105, 105, 75, 65, 100, 50], [(int)TypeIndex.Poison], [new MoveLvl("SludgeBomb")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Shellder", new PokemonInfo(0090, [30, 65, 100, 45, 25, 40], [(int)TypeIndex.Water], [new MoveLvl("Bubble")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Cloyster", new PokemonInfo(0091, [50, 95, 180, 85, 45, 70], [(int)TypeIndex.Water,(int)TypeIndex.Ice], [new MoveLvl("HydroPump"), new MoveLvl("IceFang")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            
            {"Gastly", new PokemonInfo(0092, [30, 35, 30, 100, 35, 80], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], [new MoveLvl("ConfuseRay"), new MoveLvl("Confusion", 12), new MoveLvl("Hex", 20), new MoveLvl("NightShade", 28), new MoveLvl("Crunch", 34), new MoveLvl("ShadowBall", 40)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Haunter", new PokemonInfo(0093, [45, 50, 45, 115, 55, 95], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], [new MoveLvl("Toxic"), new MoveLvl("ConfuseRay", 1), new MoveLvl("Confusion", 12), new MoveLvl("Hex", 20), new MoveLvl("NightShade", 32), new MoveLvl("Crunch", 40), new MoveLvl("ShadowBall", 48)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Gengar", new PokemonInfo(0094, [60, 65, 60, 130, 75, 110], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], [new MoveLvl("Toxic", 1), new MoveLvl("ConfuseRay", 1), new MoveLvl("Confusion", 12), new MoveLvl("Hex", 20), new MoveLvl("NightShade", 32), new MoveLvl("Crunch", 40), new MoveLvl("ShadowBall", 48)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Onix", new PokemonInfo(0095, [35, 45, 160, 30, 45, 70], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [new MoveLvl("RockThrow"), new MoveLvl("Earthquake")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Drowzee", new PokemonInfo(0096, [60, 48, 45, 43, 90, 42], [(int)TypeIndex.Psychic], [new MoveLvl("Confusion")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Hypno", new PokemonInfo(0097, [85, 73, 70, 73, 115, 67], [(int)TypeIndex.Psychic], [new MoveLvl("Psybeam"), new MoveLvl("Psychic")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Krabby", new PokemonInfo(0098, [30, 105, 90, 25, 25, 50], [(int)TypeIndex.Water], [new MoveLvl("Bubble")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Kingler", new PokemonInfo(0099, [55, 130, 115, 50, 50, 75], [(int)TypeIndex.Water], [new MoveLvl("WaterPulse")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Voltorb", new PokemonInfo(0100, [40, 30, 50, 55, 55, 100], [(int)TypeIndex.Electric], [new MoveLvl("Tackle"), new MoveLvl("ThunderWave", 4), new MoveLvl("RockThrow", 11), new MoveLvl("Swift", 16), new MoveLvl("ElectroBall", 22), new MoveLvl("SelfDestruct", 28), new MoveLvl("Thunderbolt", 32), new MoveLvl("Discharge", 37), new MoveLvl("Explosion", 41)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Electrode", new PokemonInfo(0101, [60, 50, 70, 80, 80, 150], [(int)TypeIndex.Electric], [new MoveLvl("Tackle", 1), new MoveLvl("ThunderWave", 4), new MoveLvl("RockThrow", 11), new MoveLvl("Swift", 16), new MoveLvl("ElectroBall", 22), new MoveLvl("SelfDestruct", 28), new MoveLvl("Thunderbolt", 34), new MoveLvl("Discharge", 41), new MoveLvl("Explosion", 47)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Exeggcute", new PokemonInfo(0102, [60, 40, 80, 60, 45, 40], [(int)TypeIndex.Grass,(int)TypeIndex.Psychic], [new MoveLvl("Confusion")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Exeggutor", new PokemonInfo(0103, [95, 95, 85, 125, 75, 55], [(int)TypeIndex.Grass,(int)TypeIndex.Psychic], [new MoveLvl("GigaDrain")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            
            {"Cubone", new PokemonInfo(0104, [50, 50, 95, 40, 50, 35], [(int)TypeIndex.Ground], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Marowak", new PokemonInfo(0105, [60, 80, 110, 50, 80, 45], [(int)TypeIndex.Ground], [new MoveLvl("Earthquake")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Hitmonlee", new PokemonInfo(0106, [50, 120, 53, 35, 110, 87], [(int)TypeIndex.Fighting], [new MoveLvl("DoubleKick")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Hitmonchan", new PokemonInfo(0107, [50, 105, 79, 35, 110, 76], [(int)TypeIndex.Fighting], [new MoveLvl("DoubleKick")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Lickitung", new PokemonInfo(0108, [90, 55, 75, 60, 75, 30], [(int)TypeIndex.Normal], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Koffing", new PokemonInfo(0109, [40, 65, 95, 60, 45, 35], [(int)TypeIndex.Poison], [new MoveLvl("Tackle"), new MoveLvl("PoisonPowder"), new MoveLvl("Smokescreen", 8), new MoveLvl("Crunch", 16), new MoveLvl("SelfDestruct", 24), new MoveLvl("SludgeBomb", 32), new MoveLvl("Toxic", 36), new MoveLvl("Explosion", 44)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Weezing", new PokemonInfo(0110, [65, 90, 120, 85, 70, 60], [(int)TypeIndex.Poison], [new MoveLvl("DoubleKick"), new MoveLvl("Tackle", 1), new MoveLvl("PoisonPowder", 1), new MoveLvl("Smokescreen", 8), new MoveLvl("Crunch", 16), new MoveLvl("SelfDestruct", 24), new MoveLvl("SludgeBomb", 32), new MoveLvl("Toxic", 38), new MoveLvl("Explosion", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Rhyhorn", new PokemonInfo(0111, [80, 85, 95, 30, 30, 25], [(int)TypeIndex.Ground,(int)TypeIndex.Rock], [new MoveLvl("Tackle")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Rhydon", new PokemonInfo(0112, [105, 130, 120, 45, 45, 40], [(int)TypeIndex.Ground,(int)TypeIndex.Rock], [new MoveLvl("RockThrow"), new MoveLvl("Earthquake")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            
            {"Chansey", new PokemonInfo(0113, [250, 5, 5, 35, 105, 50], [(int)TypeIndex.Normal], [new MoveLvl("Tackle"), new MoveLvl("Swift"), new MoveLvl("Harden", 8), new MoveLvl("DoubleKick", 16), new MoveLvl("Slash", 24), new MoveLvl("HealPulse", 32), new MoveLvl("DoubleEdge", 40)], (int)StageIndex.Basic, (int)ExpTypes.Fast)},
            {"Tangela", new PokemonInfo(0114, [65, 55, 115, 100, 40, 60], [(int)TypeIndex.Grass], [new MoveLvl("MagicalLeaf")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Kangaskhan", new PokemonInfo(0115, [105, 95, 80, 40, 80, 90], [(int)TypeIndex.Normal], [new MoveLvl("Tackle"), new MoveLvl("Crunch", 36)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Horsea", new PokemonInfo(0116, [30, 40, 70, 70, 25, 60], [(int)TypeIndex.Water], [new MoveLvl("Bubble")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Seadra", new PokemonInfo(0117, [55, 65, 95, 95, 45, 85], [(int)TypeIndex.Water], [new MoveLvl("HydroPump"), new MoveLvl("DragonBreath")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Goldeen", new PokemonInfo(0118, [45, 67, 60, 35, 50, 63], [(int)TypeIndex.Water], [new MoveLvl("BubbleBeam")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Seaking", new PokemonInfo(0119, [80, 92, 65, 65, 80, 68], [(int)TypeIndex.Water], [new MoveLvl("WaterPulse")], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Staryu", new PokemonInfo(0120, [30, 45, 55, 70, 55, 85], [(int)TypeIndex.Water], [new MoveLvl("Bubble")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Starmie", new PokemonInfo(0121, [60, 75, 85, 100, 85, 115], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], [new MoveLvl("Confusion")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            
            {"MrMime", new PokemonInfo(0122, [40, 45, 65, 100, 120, 90], [(int)TypeIndex.Psychic,(int)TypeIndex.Fairy], [new MoveLvl("Confusion")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Scyther", new PokemonInfo(0123, [70, 110, 80, 55, 80, 105], [(int)TypeIndex.Bug,(int)TypeIndex.Flying], [new MoveLvl("FuryCutter")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Jynx", new PokemonInfo(0124, [65, 50, 35, 115, 95, 95], [(int)TypeIndex.Ice,(int)TypeIndex.Psychic], [new MoveLvl("IceFang")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Electabuzz", new PokemonInfo(0125, [65, 83, 57, 95, 85, 105], [(int)TypeIndex.Electric], [new MoveLvl("Thunder")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Magmar", new PokemonInfo(0126, [65, 95, 57, 100, 85, 93], [(int)TypeIndex.Fire], [new MoveLvl("FlameWheel")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Pinsir", new PokemonInfo(0127, [65, 125, 100, 55, 70, 85], [(int)TypeIndex.Bug], [new MoveLvl("FuryCutter")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Tauros", new PokemonInfo(0128, [75, 100, 95, 40, 70, 110], [(int)TypeIndex.Normal], [new MoveLvl("Tackle"), new MoveLvl("DoubleEdge")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            
            {"Magikarp", new PokemonInfo(0129, [20, 10, 55, 15, 20, 80], [(int)TypeIndex.Water], [new MoveLvl("Splash"), new MoveLvl("Tackle", 15)], (int)StageIndex.Basic, (int)ExpTypes.Slow)},
            {"Gyarados", new PokemonInfo(0130, [95, 125, 79, 60, 100, 81], [(int)TypeIndex.Water,(int)TypeIndex.Flying], [new MoveLvl("WaterPulse"), new MoveLvl("IceFang"), new MoveLvl("Crunch", 24), new MoveLvl("Waterfall", 32), new MoveLvl("HydroPump", 40), new MoveLvl("DoubleEdge", 48), new MoveLvl("HyperBeam", 52)], (int)StageIndex.Stage1, (int)ExpTypes.Slow)},
            
            {"Lapras", new PokemonInfo(0131, [130, 85, 80, 85, 95, 60], [(int)TypeIndex.Water,(int)TypeIndex.Ice], [new MoveLvl("IceFang")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Ditto", new PokemonInfo(0132, [48, 48, 48, 48, 48, 48], [(int)TypeIndex.Normal], [new MoveLvl("Swift")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Eevee", new PokemonInfo(0133, [55, 55, 50, 45, 65, 55], [(int)TypeIndex.Normal], [new MoveLvl("Tackle"), new MoveLvl("QuickAttack", 10), new MoveLvl("Swift", 20), new MoveLvl("DoubleEdge", 50)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Vaporeon", new PokemonInfo(0134, [130, 65, 60, 110, 95, 65], [(int)TypeIndex.Water], [new MoveLvl("WaterGun"), new MoveLvl("Tackle", 1), new MoveLvl("QuickAttack", 10), new MoveLvl("IceFang", 20), new MoveLvl("WaterPulse", 25), new MoveLvl("AquaRing", 30), new MoveLvl("Waterfall", 40), new MoveLvl("HydroPump", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Jolteon", new PokemonInfo(0135, [65, 65, 60, 110, 95, 130], [(int)TypeIndex.Electric], [new MoveLvl("ThunderWave"), new MoveLvl("Tackle", 1), new MoveLvl("QuickAttack", 10), new MoveLvl("DoubleKick", 20), new MoveLvl("Thunderbolt", 25), new MoveLvl("PinMissile", 30), new MoveLvl("Discharge", 40), new MoveLvl("Thunder", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Flareon", new PokemonInfo(0136, [65, 130, 60, 95, 110, 65], [(int)TypeIndex.Fire], [new MoveLvl("Ember"), new MoveLvl("Tackle", 1), new MoveLvl("QuickAttack", 10), new MoveLvl("Smokescreen", 20), new MoveLvl("Crunch", 25), new MoveLvl("FlameWheel", 30), new MoveLvl("LavaPlume", 40), new MoveLvl("Overheat", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Porygon", new PokemonInfo(0137, [65, 60, 70, 85, 75, 40], [(int)TypeIndex.Normal], [new MoveLvl("Discharge")], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            
            {"Omanyte", new PokemonInfo(0138, [35, 40, 100, 90, 55, 35], [(int)TypeIndex.Rock,(int)TypeIndex.Water], [new MoveLvl("WaterGun"), new MoveLvl("MudShot", 25), new MoveLvl("AncientPower", 30), new MoveLvl("HydroPump", 60)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Omastar", new PokemonInfo(0139, [70, 60, 125, 115, 70, 55], [(int)TypeIndex.Rock,(int)TypeIndex.Water], [new MoveLvl("Crunch"), new MoveLvl("WaterGun", 1), new MoveLvl("MudShot", 25), new MoveLvl("AncientPower", 30), new MoveLvl("HydroPump", 70)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Kabuto", new PokemonInfo(0140, [30, 80, 90, 55, 45, 55], [(int)TypeIndex.Rock,(int)TypeIndex.Water], [new MoveLvl("Harden"), new MoveLvl("Tackle", 5), new MoveLvl("WaterGun", 15), new MoveLvl("MudShot", 25), new MoveLvl("AncientPower", 30), new MoveLvl("GigaDrain", 45), new MoveLvl("Waterfall", 50)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Kabutops", new PokemonInfo(0141, [60, 115, 105, 65, 70, 80], [(int)TypeIndex.Rock,(int)TypeIndex.Water], [new MoveLvl("Harden", 1), new MoveLvl("Tackle", 5), new MoveLvl("WaterGun", 15), new MoveLvl("MudShot", 25), new MoveLvl("AncientPower", 30), new MoveLvl("GigaDrain", 49), new MoveLvl("Waterfall", 56)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            {"Aerodactyl", new PokemonInfo(0142, [80, 105, 65, 60, 75, 130], [(int)TypeIndex.Rock,(int)TypeIndex.Flying], [new MoveLvl("AncientPower"), new MoveLvl("WingAttack", 10), new MoveLvl("Crunch", 30), new MoveLvl("HyperBeam", 55)], (int)StageIndex.Basic, (int)ExpTypes.Slow)},
            {"Snorlax", new PokemonInfo(0143, [160, 110, 65, 65, 110, 30], [(int)TypeIndex.Normal], [new MoveLvl("Tackle"), new MoveLvl("Crunch", 24), new MoveLvl("DoubleEdge", 44)], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            
            {"Articuno", new PokemonInfo(0144, [90, 85, 100, 95, 125, 85], [(int)TypeIndex.Ice,(int)TypeIndex.Flying], [new MoveLvl("IceFang"), new MoveLvl("HydroPump")], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Zapdos", new PokemonInfo(0145, [90, 90, 85, 125, 90, 100], [(int)TypeIndex.Electric,(int)TypeIndex.Flying], [new MoveLvl("Thunder"), new MoveLvl("Thunderbolt")], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Moltres", new PokemonInfo(0146, [90, 100, 90, 125, 85, 90], [(int)TypeIndex.Fire,(int)TypeIndex.Flying], [new MoveLvl("LavaPlume"), new MoveLvl("Flamethrower")], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            
            {"Dratini", new PokemonInfo(0147, [41, 64, 45, 50, 50, 50], [(int)TypeIndex.Dragon], [new MoveLvl("Swift")], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Dragonair", new PokemonInfo(0148, [61, 84, 65, 70, 70, 70], [(int)TypeIndex.Dragon], [new MoveLvl("WingAttack")], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Dragonite", new PokemonInfo(0149, [91, 134, 95, 100, 100, 80], [(int)TypeIndex.Dragon,(int)TypeIndex.Flying], [new MoveLvl("DragonBreath"), new MoveLvl("HyperBeam")], (int)StageIndex.Stage2, (int)ExpTypes.Slow, completed: false)},
            
            {"Mewtwo", new PokemonInfo(0150, [106, 110, 90, 154, 90, 130], [(int)TypeIndex.Psychic], [new MoveLvl("Psychic")], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Mew", new PokemonInfo(0151, [100, 100, 100, 100, 100, 100], [(int)TypeIndex.Psychic], [new MoveLvl("Psychic")], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, legendary: true, completed: false)},
            
            //Gen 2
            {"Chikorita", new PokemonInfo(0152, [45, 49, 65, 49, 65, 45], [(int)TypeIndex.Grass], [new MoveLvl("Tackle"), new MoveLvl("RazorLeaf", 6), new MoveLvl("PoisonPowder", 9), new MoveLvl("AquaRing", 17), new MoveLvl("MagicalLeaf", 23), new MoveLvl("DoubleEdge", 31), new MoveLvl("HealPulse", 39), new MoveLvl("SolarBeam", 45)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Bayleef", new PokemonInfo(0153, [60, 62, 80, 63, 80, 60], [(int)TypeIndex.Grass], [new MoveLvl("Tackle", 1), new MoveLvl("RazorLeaf", 6), new MoveLvl("PoisonPowder", 9), new MoveLvl("AquaRing", 18), new MoveLvl("MagicalLeaf", 26), new MoveLvl("DoubleEdge", 36), new MoveLvl("HealPulse", 46), new MoveLvl("SolarBeam", 54)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Meganium", new PokemonInfo(0154, [80, 82, 100, 83, 100, 80], [(int)TypeIndex.Grass], [new MoveLvl("LeafStorm"), new MoveLvl("Tackle", 1), new MoveLvl("RazorLeaf", 6), new MoveLvl("PoisonPowder", 9), new MoveLvl("AquaRing", 18), new MoveLvl("MagicalLeaf", 26), new MoveLvl("DoubleEdge", 40), new MoveLvl("HealPulse", 54), new MoveLvl("SolarBeam", 66), new MoveLvl("LeafBlade", 70)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Cyndaquil", new PokemonInfo(0155, [39, 52, 43, 60, 50, 65], [(int)TypeIndex.Fire], [new MoveLvl("Tackle"), new MoveLvl("Smokescreen", 6), new MoveLvl("Ember", 10), new MoveLvl("FlameWheel", 19), new MoveLvl("Swift", 28), new MoveLvl("LavaPlume", 37), new MoveLvl("Flamethrower", 40), new MoveLvl("Overheat", 46), new MoveLvl("DoubleEdge", 55)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Quilava", new PokemonInfo(0156, [58, 64, 58, 80, 65, 80], [(int)TypeIndex.Fire], [new MoveLvl("Tackle", 1), new MoveLvl("Smokescreen", 6), new MoveLvl("Ember", 10), new MoveLvl("FlameWheel", 20), new MoveLvl("Swift", 31), new MoveLvl("LavaPlume", 42), new MoveLvl("Flamethrower", 46), new MoveLvl("Overheat", 53), new MoveLvl("DoubleEdge", 64)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Typhlosion", new PokemonInfo(0157, [78, 84, 78, 109, 85, 100], [(int)TypeIndex.Fire], [new MoveLvl("Tackle", 1), new MoveLvl("Smokescreen", 6), new MoveLvl("Ember", 10), new MoveLvl("FlameWheel", 20), new MoveLvl("Swift", 31), new MoveLvl("LavaPlume", 43), new MoveLvl("Flamethrower", 48), new MoveLvl("Overheat", 56), new MoveLvl("DoubleEdge", 69), new MoveLvl("FireBlast", 74)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            {"Totodile", new PokemonInfo(0158, [50, 65, 64, 44, 48, 43], [(int)TypeIndex.Water], [new MoveLvl("Tackle"), new MoveLvl("WaterGun", 6), new MoveLvl("MudShot", 13), new MoveLvl("IceFang", 20), new MoveLvl("Crunch", 27), new MoveLvl("Slash", 34), new MoveLvl("Waterfall", 43), new MoveLvl("FocusPunch", 48), new MoveLvl("HydroPump", 50)], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Croconaw", new PokemonInfo(0159, [65, 80, 80, 59, 63, 58], [(int)TypeIndex.Water], [new MoveLvl("Tackle", 1), new MoveLvl("WaterGun", 6), new MoveLvl("MudShot", 13), new MoveLvl("IceFang", 21), new MoveLvl("Crunch", 30), new MoveLvl("Slash", 39), new MoveLvl("Waterfall", 48), new MoveLvl("FocusPunch", 57), new MoveLvl("HydroPump", 60)], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Feraligatr", new PokemonInfo(0160, [85, 105, 100, 79, 83, 78], [(int)TypeIndex.Water], [new MoveLvl("Tackle", 1), new MoveLvl("WaterGun", 6), new MoveLvl("MudShot", 13), new MoveLvl("IceFang", 21), new MoveLvl("Crunch", 32), new MoveLvl("Slash", 45), new MoveLvl("Waterfall", 58), new MoveLvl("FocusPunch", 71), new MoveLvl("HydroPump", 76)], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            
            //Gen 5
            {"Joltik", new PokemonInfo(0595, [50, 47, 50, 57, 50, 65], [(int)TypeIndex.Bug,(int)TypeIndex.Electric], [new MoveLvl("FuryCutter"), new MoveLvl("StringShot", 8), new MoveLvl("ThunderWave", 16), new MoveLvl("ElectroBall", 20), new MoveLvl("Crunch", 26), new MoveLvl("Slash", 32), new MoveLvl("Discharge", 37), new MoveLvl("PinMissile", 44)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Galvantula", new PokemonInfo(0596, [70, 77, 60, 97, 60, 108], [(int)TypeIndex.Bug,(int)TypeIndex.Electric], [new MoveLvl("FuryCutter", 1), new MoveLvl("StringShot", 8), new MoveLvl("ThunderWave", 16), new MoveLvl("ElectroBall", 20), new MoveLvl("Crunch", 26), new MoveLvl("Slash", 32), new MoveLvl("Discharge", 39), new MoveLvl("PinMissile", 50)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            
            //Gen 7
            {"Zeraora", new PokemonInfo(0807, [88, 112, 75, 102, 80, 143], [(int)TypeIndex.Electric], [new MoveLvl("Thunderbolt")], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            
            //Megas
            {"VenusaurMega", new PokemonInfo(0003, [80, 100, 123, 122, 120, 80], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], [new MoveLvl("Earthquake")], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
            {"CharizardMegaX", new PokemonInfo(0006, [78, 130, 111, 130, 85, 100], [(int)TypeIndex.Fire,(int)TypeIndex.Dragon], [new MoveLvl("Crunch")], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
            {"CharizardMegaY", new PokemonInfo(0006, [78, 104, 78, 159, 115, 100], [(int)TypeIndex.Fire,(int)TypeIndex.Flying], [new MoveLvl("SolarBeam")], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
            {"BlastoiseMega", new PokemonInfo(0009, [79, 103, 120, 135, 115, 78], [(int)TypeIndex.Water], [new MoveLvl("FocusPunch")], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
            {"AlakazamMega", new PokemonInfo(0065, [55, 50, 65, 175, 95, 150], [(int)TypeIndex.Psychic], [new MoveLvl("ShadowBall")], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
            {"GengarMega", new PokemonInfo(0094, [60, 65, 80, 170, 95, 130], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], [new MoveLvl("Psychic")], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
            {"GyaradosMega", new PokemonInfo(0130, [95, 155, 109, 70, 130, 81], [(int)TypeIndex.Water,(int)TypeIndex.Dark], [new MoveLvl("DragonBreath")], (int)StageIndex.Mega, (int)ExpTypes.Slow)},

            //Terrarian Regional Forms
            {"TerrarianOmanyte", new PokemonInfo(0138, [35, 40, 100, 90, 55, 35], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], [new MoveLvl("ConfuseRay"), new MoveLvl("WaterPulse", 10), new MoveLvl("Psybeam", 20), new MoveLvl("AncientPower", 30), new MoveLvl("BubbleBeam", 45), new MoveLvl("Psychic", 50), new MoveLvl("HydroPump", 55)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"TerrarianOmastar", new PokemonInfo(0139, [70, 60, 125, 115, 70, 55], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], [new MoveLvl("IceFang"), new MoveLvl("ConfuseRay", 1), new MoveLvl("WaterPulse", 10), new MoveLvl("Psybeam", 20), new MoveLvl("AncientPower", 30), new MoveLvl("BubbleBeam", 50), new MoveLvl("Psychic", 60), new MoveLvl("HydroPump", 70)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"TerrarianKabuto", new PokemonInfo(0140, [30, 70, 80, 65, 45, 65], [(int)TypeIndex.Rock,(int)TypeIndex.Ghost], [new MoveLvl("Harden"), new MoveLvl("Tackle", 5), new MoveLvl("ConfuseRay", 15), new MoveLvl("Slash", 25), new MoveLvl("AncientPower", 30), new MoveLvl("RockThrow", 42), new MoveLvl("FuryCutter", 47), new MoveLvl("ShadowBall", 52)], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"TerrarianKabutops", new PokemonInfo(0141, [60, 95, 85, 85, 70, 100], [(int)TypeIndex.Rock,(int)TypeIndex.Ghost], [new MoveLvl("Hex"), new MoveLvl("Harden", 1), new MoveLvl("Tackle", 5), new MoveLvl("ConfuseRay", 15), new MoveLvl("Slash", 25), new MoveLvl("AncientPower", 30), new MoveLvl("RockThrow", 45), new MoveLvl("FuryCutter", 50), new MoveLvl("ShadowBall", 60)], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"TerrarianAerodactyl", new PokemonInfo(0142, [80, 90, 65, 95, 75, 110], [(int)TypeIndex.Dark,(int)TypeIndex.Flying], [new MoveLvl("WingAttack"), new MoveLvl("Gust"), new MoveLvl("AncientPower", 10), new MoveLvl("Crunch", 20), new MoveLvl("DragonBreath", 30), new MoveLvl("NightShade", 40), new MoveLvl("Flamethrower", 50), new MoveLvl("AirSlash", 60), new MoveLvl("HyperBeam", 70)], (int)StageIndex.Basic, (int)ExpTypes.Slow)},
        };

        public static int maxID = 0807;

        public static int GetMaxPokemonIndex()
        {
            return pokemonInfo.Values.ToList().FindIndex(x => x.pokemonID == maxID);
        }

        public static string GetPokemonByID(int ID)
        {
            int index = pokemonInfo.Values.ToList().FindIndex(x => x.pokemonID == ID);

            if (index < 0) return "";

            return pokemonInfo.Keys.ToList()[index];
        }

        public static string[][] PokemonNatures = [
            ["Hardy", "Lonely", "Adamant", "Naughty", "Brave"],
            ["Bold", "Docile", "Impish", "Lax", "Relaxed"],
            ["Modest", "Mild", "Bashful", "Rash", "Quiet"],
            ["Calm", "Gentle", "Careful", "Quirky", "Sassy"],
            ["Timid", "Hasty", "Jolly", "Naive", "Serious"],
        ];

        public static Dictionary<string, PokemonAttackInfo> pokemonAttacks = new(){
            {"AirSlash", new PokemonAttackInfo(75,90,60,800f,true,true,(int)TypeIndex.Flying, true)},
            {"AncientPower", new PokemonAttackInfo(60,0,120,600f,true,false,(int)TypeIndex.Rock, true)},
            {"AquaRing", new PokemonAttackInfo(0,90,60,100f,true,false,(int)TypeIndex.Water, true)},
            {"Bubble", new PokemonAttackInfo(20,10,60,600f,false,false,(int)TypeIndex.Water, true)},
            {"BubbleBeam", new PokemonAttackInfo(65,30,60,800f,false,false,(int)TypeIndex.Water, true)},
            {"BulletSeed", new PokemonAttackInfo(25,30,60,800f,false,false,(int)TypeIndex.Grass)},
            {"ConfuseRay", new PokemonAttackInfo(0,30,60,800f,true,true,(int)TypeIndex.Ghost, true)},
            {"Confusion", new PokemonAttackInfo(50,56,60,800f,false,true,(int)TypeIndex.Psychic, true)},
            {"Crunch", new PokemonAttackInfo(80,30,60,200f,false,false,(int)TypeIndex.Dark)},
            {"Dig", new PokemonAttackInfo(80,90,60,800f,false,true,(int)TypeIndex.Ground)},
            {"Discharge", new PokemonAttackInfo(80,60,60,200f,true,false,(int)TypeIndex.Electric, true)},
            {"DoubleEdge", new PokemonAttackInfo(120,40,60,400f,false,false,(int)TypeIndex.Normal)},
            {"DoubleKick", new PokemonAttackInfo(30,30,60,200f,false,false,(int)TypeIndex.Fighting)},
            {"DragonBreath", new PokemonAttackInfo(60,90,60,800f,false,false,(int)TypeIndex.Dragon, true)},
            {"Earthquake", new PokemonAttackInfo(100,50,60,500f,false,true,(int)TypeIndex.Ground)},
            {"ElectroBall", new PokemonAttackInfo(40,30,60,800f,false,false,(int)TypeIndex.Electric)},
            {"Ember", new PokemonAttackInfo(40,30,60,800f,false,false,(int)TypeIndex.Fire, true)},
            {"Explosion", new PokemonAttackInfo(250,32,60,800f,false,false,(int)TypeIndex.Normal)},
            {"FireBlast", new PokemonAttackInfo(110,40,60,800f,true,true,(int)TypeIndex.Fire, true)},
            {"Flamethrower", new PokemonAttackInfo(90,40,80,300f,true,false,(int)TypeIndex.Fire, true)},
            {"FlameWheel", new PokemonAttackInfo(60,60,60,500f,false,false,(int)TypeIndex.Fire)},
            {"FlashCannon", new PokemonAttackInfo(80,40,60,800f,false,true,(int)TypeIndex.Steel, true)},
            {"FocusPunch", new PokemonAttackInfo(150,90,60,600f,false,false,(int)TypeIndex.Fighting)},
            {"FuryCutter", new PokemonAttackInfo(40,42,60,200f,false,false,(int)TypeIndex.Bug)},
            {"GigaDrain", new PokemonAttackInfo(75,56,60,800f,false,true,(int)TypeIndex.Grass, true)},
            {"Gust", new PokemonAttackInfo(40,30,60,800f,false,false,(int)TypeIndex.Flying, true)},
            {"Harden", new PokemonAttackInfo(0,60,60,64f,false,false,(int)TypeIndex.Normal)},
            {"HealPulse", new PokemonAttackInfo(0,30,60,200f,false,false,(int)TypeIndex.Normal, true)},
            {"Hex", new PokemonAttackInfo(65,30,60,800f,true,true,(int)TypeIndex.Ghost, true)},
            {"HydroPump", new PokemonAttackInfo(110,42,60,800f,false,true,(int)TypeIndex.Water, true)},
            {"HyperBeam", new PokemonAttackInfo(150,35,150,800f,false,true,(int)TypeIndex.Normal, true)},
            {"HyperFang", new PokemonAttackInfo(80,42,60,400f,false,false,(int)TypeIndex.Normal)},
            {"IceFang", new PokemonAttackInfo(65,30,60,200f,false,false,(int)TypeIndex.Ice)},
            {"LavaPlume", new PokemonAttackInfo(80,20,60,800f,true,true,(int)TypeIndex.Fire, true)},
            {"LeafBlade", new PokemonAttackInfo(90,40,60,500f,false,false,(int)TypeIndex.Grass)},
            {"LeafStorm", new PokemonAttackInfo(130,40,60,800f,false,false,(int)TypeIndex.Grass, true)},
            {"MagicalLeaf", new PokemonAttackInfo(60,60,60,500f,false,true,(int)TypeIndex.Grass, true)},
            {"MegaDrain", new PokemonAttackInfo(40,56,60,600f,false,true,(int)TypeIndex.Grass, true)},
            {"MudShot", new PokemonAttackInfo(55,45,60,800f,false,false,(int)TypeIndex.Ground, true)},
            {"NightShade", new PokemonAttackInfo(0,30,40,800f,true,true,(int)TypeIndex.Ghost, true)},
            {"Overheat", new PokemonAttackInfo(130,60,60,400f,true,true,(int)TypeIndex.Fire, true)},
            {"PinMissile", new PokemonAttackInfo(25,30,60,800f,false,false,(int)TypeIndex.Bug)},
            {"PoisonPowder", new PokemonAttackInfo(0,20,60,800f,true,false,(int)TypeIndex.Poison, true)},
            {"PoisonSting", new PokemonAttackInfo(15,45,60,800f,false,false,(int)TypeIndex.Poison)},
            {"Psybeam", new PokemonAttackInfo(65,60,40,800f,false,true,(int)TypeIndex.Psychic, true)},
            {"Psychic", new PokemonAttackInfo(90,45,60,800f,false,true,(int)TypeIndex.Psychic, true)},
            {"PsychoCut", new PokemonAttackInfo(70,50,60,500f,true,true,(int)TypeIndex.Psychic)},
            {"QuickAttack", new PokemonAttackInfo(40,40,60,400f,false,false,(int)TypeIndex.Normal)},
            {"RazorLeaf", new PokemonAttackInfo(55,45,60,800f,false,false,(int)TypeIndex.Grass)},
            {"RockThrow", new PokemonAttackInfo(50,45,60,700f,false,false,(int)TypeIndex.Rock)},
            {"SelfDestruct", new PokemonAttackInfo(200,32,60,100f,false,false,(int)TypeIndex.Normal)},
            {"ShadowBall", new PokemonAttackInfo(80,30,60,800f,false,true,(int)TypeIndex.Ghost, true)},
            {"Slash", new PokemonAttackInfo(70,20,60,400f,false,false,(int)TypeIndex.Normal)},
            {"SludgeBomb", new PokemonAttackInfo(90,45,60,800f,false,false,(int)TypeIndex.Poison, true)},
            {"Smokescreen", new PokemonAttackInfo(0,60,60,600f,true,false,(int)TypeIndex.Normal, true)},
            {"SolarBeam", new PokemonAttackInfo(120,60,120,800f,false,true,(int)TypeIndex.Grass, true)},
            {"Splash", new PokemonAttackInfo(0,30,60,600f,false,false,(int)TypeIndex.Water)},
            {"StringShot", new PokemonAttackInfo(0,60,60,800f,false,false,(int)TypeIndex.Bug)},
            {"Swift", new PokemonAttackInfo(60,0,120,500f,true,true,(int)TypeIndex.Normal, true)},
            {"Tackle", new PokemonAttackInfo(40,20,60,200f,false,false,(int)TypeIndex.Normal)},
            {"Teleport", new PokemonAttackInfo(0,20,60,200f,false,false,(int)TypeIndex.Psychic)},
            {"Thunder", new PokemonAttackInfo(110,30,60,800f,false,false,(int)TypeIndex.Electric, true)},
            {"Thunderbolt", new PokemonAttackInfo(90,30,60,800f,true,false,(int)TypeIndex.Electric, true)},
            {"ThunderWave", new PokemonAttackInfo(0,40,60,800f,false,false,(int)TypeIndex.Electric, true)},
            {"Toxic", new PokemonAttackInfo(0,36,60,800f,false,true,(int)TypeIndex.Poison, true)},
            {"VineWhip", new PokemonAttackInfo(45,45,60,140f,true,false,(int)TypeIndex.Grass)},
            {"Waterfall", new PokemonAttackInfo(80,42,60,600f,false,true,(int)TypeIndex.Water)},
            {"WaterGun", new PokemonAttackInfo(40,30,60,600f,false,false,(int)TypeIndex.Water, true)},
            {"WaterPulse", new PokemonAttackInfo(60,40,60,800f,false,true,(int)TypeIndex.Water, true)},
            {"WingAttack", new PokemonAttackInfo(60,40,60,600f,false,false,(int)TypeIndex.Flying)},
        };
    }

    public enum TypeIndex
    {
        Normal, Fighting, Flying, Poison, Ground,
        Rock, Bug, Ghost, Steel, Fire,
        Water, Grass, Electric, Psychic, Ice,
        Dragon, Dark, Fairy
    }

    public enum StageIndex
    {
        Basic,
        Stage1,
        Stage2,
        Baby,
        Mega
    }

    public enum ExpTypes
    {
        Slow,
        MediumSlow,
        MediumFast,
        Fast,
        Erratic,
        Fluctuating
    }

    internal class MoveLvl
    {
        public string moveName;
        public int levelToLearn;

        public MoveLvl(string moveName)
        {
            this.moveName = moveName;
            this.levelToLearn = 0;
        }

        public MoveLvl(string moveName, int levelToLearn)
        {
            this.moveName = moveName;
            this.levelToLearn = levelToLearn;
        }
    }

    internal class PokemonInfo
    {
        public int pokemonID;
        public int[] pokemonStats;
        public int[] pokemonTypes;
        public MoveLvl[] movePool;
        public int pokemonStage;
        public int expType;

        public bool legendary;

        public bool completed;

        public PokemonInfo(int pokemonID, int[] pokemonStats, int[] pokemonTypes, MoveLvl[] movePool, int pokemonStage = 0, int expType = 0, bool legendary = false, bool completed = true)
        {
            this.pokemonID = pokemonID;
            this.pokemonStats = pokemonStats;
            this.pokemonTypes = [(pokemonTypes.Length <= 0 ? -1 : pokemonTypes[0]), (pokemonTypes.Length <= 1 ? -1 : pokemonTypes[1])];
            this.movePool = (movePool.Length > 0) ? movePool : [new MoveLvl("Swift", 0)];
            this.pokemonStage = pokemonStage;
            this.expType = expType;
            this.legendary = legendary;
            this.completed = completed;
        }

        public bool HasType(TypeIndex type)
        {
            foreach (int pokemonType in pokemonTypes)
            {
                if (pokemonType == (int)type) return true;
            }

            return false;
        }
    }

    internal class PokemonAttackInfo
    {
        public int attackPower;
        public int attackDuration;
		public int cooldown;
        public float distanceToAttack;
        public bool canMove;
        public bool canPassThroughWalls;

		public int attackType;
		public bool isSpecial;

        public PokemonAttackInfo(int attackPower, int attackDuration, int cooldown, float distanceToAttack, bool canMove, bool canPassThroughWalls, int attackType, bool isSpecial = false){
            if(attackPower < 10) attackPower = 10;
            this.attackPower = attackPower;
            this.attackDuration = attackDuration;
            this.cooldown = cooldown;
            this.distanceToAttack = distanceToAttack;
            this.canMove = canMove;
            this.canPassThroughWalls = canPassThroughWalls;
            this.attackType = attackType;
            this.isSpecial = isSpecial;
        }
    }
}