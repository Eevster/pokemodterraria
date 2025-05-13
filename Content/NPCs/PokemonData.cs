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
        public int[] IVs;
        public int nature;
        public bool ultrabeast = false;
        public string variant = "";

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
			bitWriter.WriteBit(isPokemon);
            bitWriter.WriteBit(shiny);
            bitWriter.WriteBit(ultrabeast);
            binaryWriter.Write(pokemonName);
		}

		// Make sure you always read exactly as much data as you sent!
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
			isPokemon = bitReader.ReadBit();
            shiny = bitReader.ReadBit();
            ultrabeast = bitReader.ReadBit();
            pokemonName = binaryReader.ReadString();
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

            if(index==0) done = ((2 * baseStat + IV + (EV/4)) * Level/100) + Level + 10;
            else done = (int)(((2 * (baseStat + IV + (EV / 4)) * Level / 100) + 5)*GetNatureMult(index, nature));

            return done;
        }

        public static int[] GenerateIVs()
        {
            int[] IVs = [0,0,0,0,0,0,0];
            for(int i = 0; i < IVs.Length; i++){
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
                string[] waterStarters = {"Clefairy", "Squirtle","Totodile"};

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
            {"Bulbasaur", new PokemonInfo([45, 49, 49, 65, 65, 45], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["VineWhip"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Ivysaur", new PokemonInfo([60, 62, 63, 80, 80, 60], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["PoisonPowder"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Venusaur", new PokemonInfo([80, 82, 83, 100, 100, 80], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["SolarBeam"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Charmander", new PokemonInfo([39, 52, 43, 60, 50, 65], [(int)TypeIndex.Fire], ["Ember"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Charmeleon", new PokemonInfo([58, 64, 58, 80, 65, 80], [(int)TypeIndex.Fire], ["Flamethrower"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Charizard", new PokemonInfo([78, 84, 78, 109, 85, 100], [(int)TypeIndex.Fire,(int)TypeIndex.Flying], ["FireBlast"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Squirtle", new PokemonInfo([44, 48, 65, 50, 64, 43], [(int)TypeIndex.Water], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Wartortle", new PokemonInfo([59, 63, 80, 65, 80, 58], [(int)TypeIndex.Water], ["WaterPulse"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Blastoise", new PokemonInfo([79, 83, 100, 85, 105, 78], [(int)TypeIndex.Water], ["HydroPump"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Caterpie", new PokemonInfo([45, 30, 35, 20, 20, 45], [(int)TypeIndex.Bug], ["StringShot"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Metapod", new PokemonInfo([50, 20, 55, 25, 25, 30], [(int)TypeIndex.Bug], ["Harden"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Butterfree", new PokemonInfo([60, 45, 50, 90, 80, 70], [(int)TypeIndex.Bug,(int)TypeIndex.Flying], ["Confusion"], (int)StageIndex.Stage2, (int)ExpTypes.MediumFast)},
            {"Weedle", new PokemonInfo([40, 35, 30, 20, 20, 50], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], ["PoisonSting"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Kakuna", new PokemonInfo([45, 25, 50, 25, 25, 35], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], ["Harden"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Beedrill", new PokemonInfo([65, 90, 40, 45, 80, 75], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], ["PinMissile"], (int)StageIndex.Stage2, (int)ExpTypes.MediumFast)},
            {"Pidgey", new PokemonInfo([40, 45, 40, 35, 35, 56], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["Gust"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Pidgeotto", new PokemonInfo([63, 60, 55, 50, 50, 71], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["WingAttack"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Pidgeot", new PokemonInfo([83, 80, 75, 70, 70, 101], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["AirSlash"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Rattata", new PokemonInfo([30, 56, 35, 25, 35, 72], [(int)TypeIndex.Normal], ["QuickAttack"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Raticate", new PokemonInfo([55, 81, 60, 50, 70, 97], [(int)TypeIndex.Normal], ["HyperFang"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Spearow", new PokemonInfo([40, 60, 30, 31, 31, 70], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["Gust"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Fearow", new PokemonInfo([65, 90, 65, 61, 61, 100], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["WingAttack"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Ekans", new PokemonInfo([35, 60, 44, 40, 54, 55], [(int)TypeIndex.Poison], ["Toxic"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Arbok", new PokemonInfo([60, 95, 69, 65, 79, 80], [(int)TypeIndex.Poison], ["PoisonPowder"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Pikachu", new PokemonInfo([35, 55, 40, 50, 50, 90], [(int)TypeIndex.Electric], ["Thunderbolt"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Raichu", new PokemonInfo([60, 90, 55, 90, 80, 110], [(int)TypeIndex.Electric], ["Thunder"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Sandshrew", new PokemonInfo([50, 75, 85, 20, 30, 40], [(int)TypeIndex.Ground], ["QuickAttack"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Sandslash", new PokemonInfo([75, 100, 110, 45, 55, 65], [(int)TypeIndex.Ground], ["PinMissile"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"NidoranM", new PokemonInfo([55, 47, 52, 40, 40, 41], [(int)TypeIndex.Poison], ["DoubleKick"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Nidorina", new PokemonInfo([70, 62, 67, 55, 55, 56], [(int)TypeIndex.Poison], ["PoisonSting"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Nidoqueen", new PokemonInfo([90, 92, 87, 75, 85, 76], [(int)TypeIndex.Poison,(int)TypeIndex.Ground], ["HyperFang"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"NidoranF", new PokemonInfo([46, 57, 40, 40, 40, 50], [(int)TypeIndex.Poison], ["DoubleKick"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Nidorino", new PokemonInfo([61, 72, 57, 55, 55, 65], [(int)TypeIndex.Poison], ["Toxic"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Nidoking", new PokemonInfo([81, 102, 77, 85, 75, 85], [(int)TypeIndex.Poison,(int)TypeIndex.Ground], ["HyperFang"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Clefairy", new PokemonInfo([70, 45, 48, 60, 65, 35], [(int)TypeIndex.Fairy], ["Swift"], (int)StageIndex.Basic, (int)ExpTypes.Fast, completed: false)},
            {"Clefable", new PokemonInfo([95, 70, 73, 95, 90, 60], [(int)TypeIndex.Fairy], ["HealPulse"], (int)StageIndex.Stage1, (int)ExpTypes.Fast, completed: false)},
            {"Vulpix", new PokemonInfo([38, 41, 40, 50, 65, 65], [(int)TypeIndex.Fire], ["Ember"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Ninetales", new PokemonInfo([73, 76, 75, 81, 100, 100], [(int)TypeIndex.Fire], ["LavaPlume"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Jigglypuff", new PokemonInfo([115, 45, 20, 45, 25, 20], [(int)TypeIndex.Normal,(int)TypeIndex.Fairy], ["QuickAttack"], (int)StageIndex.Basic, (int)ExpTypes.Fast, completed: false)},
            {"Wigglytuff", new PokemonInfo([140, 70, 45, 85, 50, 45], [(int)TypeIndex.Normal,(int)TypeIndex.Fairy], ["Swift"], (int)StageIndex.Stage1, (int)ExpTypes.Fast, completed: false)},
            {"Zubat", new PokemonInfo([40, 45, 35, 30, 40, 55], [(int)TypeIndex.Poison,(int)TypeIndex.Flying], ["ConfuseRay"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Golbat", new PokemonInfo([75, 80, 70, 65, 75, 90], [(int)TypeIndex.Poison,(int)TypeIndex.Flying], ["AirSlash"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Oddish",new PokemonInfo( [45, 50, 55, 75, 65, 30], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["RazorLeaf"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Gloom", new PokemonInfo([60, 65, 70, 85, 75, 40], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["Toxic"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Vileplume", new PokemonInfo([75, 80, 85, 110, 90, 50], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["GigaDrain"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            {"Paras", new PokemonInfo([35, 70, 55, 45, 55, 25], [(int)TypeIndex.Bug,(int)TypeIndex.Grass], ["PoisonPowder"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Parasect", new PokemonInfo([60, 95, 80, 60, 80, 30], [(int)TypeIndex.Bug,(int)TypeIndex.Grass], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Venonat", new PokemonInfo([60, 55, 50, 40, 55, 45], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Venomoth", new PokemonInfo([70, 65, 60, 90, 75, 90], [(int)TypeIndex.Bug,(int)TypeIndex.Poison], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Diglett", new PokemonInfo([10, 55, 25, 35, 45, 95], [(int)TypeIndex.Ground], ["Swift"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Dugtrio", new PokemonInfo([35, 100, 50, 50, 70, 120], [(int)TypeIndex.Ground], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Meowth", new PokemonInfo([40, 45, 35, 40, 40, 90], [(int)TypeIndex.Normal], ["HyperFang"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Persian", new PokemonInfo([65, 70, 60, 65, 65, 115], [(int)TypeIndex.Normal], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Psyduck", new PokemonInfo([50, 52, 48, 65, 50, 55], [(int)TypeIndex.Water], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Golduck", new PokemonInfo([80, 82, 78, 95, 80, 85], [(int)TypeIndex.Water], ["HydroPump"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Mankey", new PokemonInfo([40, 80, 35, 35, 45, 70], [(int)TypeIndex.Fighting], ["DoubleKick"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Primeape", new PokemonInfo([65, 105, 60, 60, 70, 95], [(int)TypeIndex.Fighting], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Growlithe", new PokemonInfo([55, 70, 45, 70, 50, 60], [(int)TypeIndex.Fire], ["Ember"], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Arcanine", new PokemonInfo([90, 110, 80, 100, 80, 95], [(int)TypeIndex.Fire], ["FireBlast"], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Poliwag", new PokemonInfo([40, 50, 40, 40, 40, 90], [(int)TypeIndex.Water], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Poliwhirl", new PokemonInfo([65, 65, 65, 50, 50, 90], [(int)TypeIndex.Water], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Poliwrath", new PokemonInfo([90, 95, 95, 70, 90, 70], [(int)TypeIndex.Water,(int)TypeIndex.Fighting], ["HydroPump"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            {"Abra", new PokemonInfo([25, 20, 15, 105, 55, 90], [(int)TypeIndex.Psychic], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Kadabra", new PokemonInfo([40, 35, 30, 120, 70, 105], [(int)TypeIndex.Psychic], ["ConfuseRay"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Alakazam", new PokemonInfo([55, 50, 45, 135, 95, 120], [(int)TypeIndex.Psychic], [], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            {"Machop", new PokemonInfo([70, 80, 50, 35, 35, 35], [(int)TypeIndex.Fighting], ["DoubleKick"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Machoke", new PokemonInfo([80, 100, 70, 50, 60, 45], [(int)TypeIndex.Fighting], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Machamp", new PokemonInfo([90, 130, 80, 65, 85, 55], [(int)TypeIndex.Fighting], [], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            {"Bellsprout", new PokemonInfo([50, 75, 35, 70, 30, 40], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["VineWhip"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Weepinbell", new PokemonInfo([65, 90, 50, 85, 45, 55], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["RazorLeaf"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Victreebel", new PokemonInfo([80, 105, 65, 100, 70, 70], [(int)TypeIndex.Grass,(int)TypeIndex.Poison], ["MagicalLeaf"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Tentacool", new PokemonInfo([40, 40, 35, 50, 100, 70], [(int)TypeIndex.Water,(int)TypeIndex.Poison], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Tentacruel", new PokemonInfo([80, 70, 65, 80, 120, 100], [(int)TypeIndex.Water,(int)TypeIndex.Poison], ["PoisonSting"], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Geodude", new PokemonInfo([40, 80, 100, 30, 30, 20], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], ["SelfDestruct"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, completed: false)},
            {"Graveler", new PokemonInfo([55, 95, 115, 45, 45, 35], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow, completed: false)},
            {"Golem", new PokemonInfo([80, 120, 130, 55, 65, 45], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow, completed: false)},
            {"Ponyta", new PokemonInfo([50, 85, 55, 65, 65, 90], [(int)TypeIndex.Fire], ["Ember"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Rapidash", new PokemonInfo([65, 100, 70, 80, 80, 105], [(int)TypeIndex.Fire], ["FlameWheel"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Slowpoke", new PokemonInfo([90, 65, 65, 40, 40, 15], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Slowbro", new PokemonInfo([95, 75, 110, 100, 80, 30], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], ["ConfuseRay"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Magnemite", new PokemonInfo([25, 35, 70, 95, 55, 45], [(int)TypeIndex.Electric,(int)TypeIndex.Steel], ["ThunderWave"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Magneton", new PokemonInfo([50, 60, 95, 120, 70, 70], [(int)TypeIndex.Electric,(int)TypeIndex.Steel], ["FlashCannon"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Farfetch'd", new PokemonInfo([52, 90, 55, 58, 62, 60], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["Swift"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Doduo", new PokemonInfo([35, 85, 45, 35, 35, 75], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["Gust"], (int)StageIndex.Basic, completed: false)},
            {"Dodrio", new PokemonInfo([60, 110, 70, 60, 60, 110], [(int)TypeIndex.Normal,(int)TypeIndex.Flying], ["WingAttack"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Seel", new PokemonInfo([65, 45, 55, 45, 70, 45], [(int)TypeIndex.Water], ["WaterGun"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Dewgong", new PokemonInfo([90, 70, 80, 70, 95, 70], [(int)TypeIndex.Water,(int)TypeIndex.Ice], ["IceFang"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Grimer", new PokemonInfo([80, 80, 50, 40, 50, 25], [(int)TypeIndex.Poison], ["Toxic"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Muk", new PokemonInfo([105, 105, 75, 65, 100, 50], [(int)TypeIndex.Poison], ["PoisonPowder"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Shellder", new PokemonInfo([30, 65, 100, 45, 25, 40], [(int)TypeIndex.Water], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Cloyster", new PokemonInfo([50, 95, 180, 85, 45, 70], [(int)TypeIndex.Water,(int)TypeIndex.Ice], ["HydroPump"], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Gastly", new PokemonInfo([30, 35, 30, 100, 35, 80], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], ["ConfuseRay"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Haunter", new PokemonInfo([45, 50, 45, 115, 55, 95], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], ["Hex"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Gengar", new PokemonInfo([60, 65, 60, 130, 75, 110], [(int)TypeIndex.Ghost,(int)TypeIndex.Poison], [], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Onix", new PokemonInfo([35, 45, 160, 30, 45, 70], [(int)TypeIndex.Rock,(int)TypeIndex.Ground], [], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Drowzee", new PokemonInfo([60, 48, 45, 43, 90, 42], [(int)TypeIndex.Psychic], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Hypno", new PokemonInfo([85, 73, 70, 73, 115, 67], [(int)TypeIndex.Psychic], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Krabby", new PokemonInfo([30, 105, 90, 25, 25, 50], [(int)TypeIndex.Water], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Kingler", new PokemonInfo([55, 130, 115, 50, 50, 75], [(int)TypeIndex.Water], ["WaterPulse"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Voltorb", new PokemonInfo([40, 30, 50, 55, 55, 100], [(int)TypeIndex.Electric], ["SelfDestruct"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Electrode", new PokemonInfo([60, 50, 70, 80, 80, 150], [(int)TypeIndex.Electric], ["Explosion"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Exeggcute", new PokemonInfo([60, 40, 80, 60, 45, 40], [(int)TypeIndex.Grass,(int)TypeIndex.Psychic], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Exeggutor", new PokemonInfo([95, 95, 85, 125, 75, 55], [(int)TypeIndex.Grass,(int)TypeIndex.Psychic], ["GigaDrain"], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Cubone", new PokemonInfo([50, 50, 95, 40, 50, 35], [(int)TypeIndex.Ground], [], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Marowak", new PokemonInfo([60, 80, 110, 50, 80, 45], [(int)TypeIndex.Ground], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Hitmonlee", new PokemonInfo([50, 120, 53, 35, 110, 87], [(int)TypeIndex.Fighting], ["DoubleKick"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Hitmonchan", new PokemonInfo([50, 105, 79, 35, 110, 76], [(int)TypeIndex.Fighting], ["DoubleKick"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Lickitung", new PokemonInfo([90, 55, 75, 60, 75, 30], [(int)TypeIndex.Normal], [], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Koffing", new PokemonInfo([40, 65, 95, 60, 45, 35], [(int)TypeIndex.Poison], ["PoisonPowder"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Weezing", new PokemonInfo([65, 90, 120, 85, 70, 60], [(int)TypeIndex.Poison], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Rhyhorn", new PokemonInfo([80, 85, 95, 30, 30, 25], [(int)TypeIndex.Ground,(int)TypeIndex.Rock], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Rhydon", new PokemonInfo([105, 130, 120, 45, 45, 40], [(int)TypeIndex.Ground,(int)TypeIndex.Rock], [], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Chansey", new PokemonInfo([250, 5, 5, 35, 105, 50], [(int)TypeIndex.Normal], ["HealPulse"], (int)StageIndex.Basic, (int)ExpTypes.Fast)},
            {"Tangela", new PokemonInfo([65, 55, 115, 100, 40, 60], [(int)TypeIndex.Grass], ["MagicalLeaf"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Kangaskhan", new PokemonInfo([105, 95, 80, 40, 80, 90], [(int)TypeIndex.Normal], [], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Horsea", new PokemonInfo([30, 40, 70, 70, 25, 60], [(int)TypeIndex.Water], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Seadra", new PokemonInfo([55, 65, 95, 95, 45, 85], [(int)TypeIndex.Water], ["HydroPump"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Goldeen", new PokemonInfo([45, 67, 60, 35, 50, 63], [(int)TypeIndex.Water], ["WaterPulse"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Seaking", new PokemonInfo([80, 92, 65, 65, 80, 68], [(int)TypeIndex.Water], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Staryu", new PokemonInfo([30, 45, 55, 70, 55, 85], [(int)TypeIndex.Water], ["Bubble"], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Starmie", new PokemonInfo([60, 75, 85, 100, 85, 115], [(int)TypeIndex.Water,(int)TypeIndex.Psychic], ["Confusion"], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Mr. Mime", new PokemonInfo([40, 45, 65, 100, 120, 90], [(int)TypeIndex.Psychic,(int)TypeIndex.Fairy], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Scyther", new PokemonInfo([70, 110, 80, 55, 80, 105], [(int)TypeIndex.Bug,(int)TypeIndex.Flying], ["AirSlash"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Jynx", new PokemonInfo([65, 50, 35, 115, 95, 95], [(int)TypeIndex.Ice,(int)TypeIndex.Psychic], ["IceFang"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Electabuzz", new PokemonInfo([65, 83, 57, 95, 85, 105], [(int)TypeIndex.Electric], ["Thunder"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Magmar", new PokemonInfo([65, 95, 57, 100, 85, 93], [(int)TypeIndex.Fire], ["FlameWheel"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Pinsir", new PokemonInfo([65, 125, 100, 55, 70, 85], [(int)TypeIndex.Bug], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Tauros", new PokemonInfo([75, 100, 95, 40, 70, 110], [(int)TypeIndex.Normal], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Magikarp", new PokemonInfo([20, 10, 55, 15, 20, 80], [(int)TypeIndex.Water], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Gyarados", new PokemonInfo([95, 125, 79, 60, 100, 81], [(int)TypeIndex.Water,(int)TypeIndex.Flying], ["HydroPump"], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Lapras", new PokemonInfo([130, 85, 80, 85, 95, 60], [(int)TypeIndex.Water,(int)TypeIndex.Ice], ["IceFang"], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Ditto", new PokemonInfo([48, 48, 48, 48, 48, 48], [(int)TypeIndex.Normal], [], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Eevee", new PokemonInfo([55, 55, 50, 45, 65, 55], [(int)TypeIndex.Normal], ["Swift"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Vaporeon", new PokemonInfo([130, 65, 60, 110, 95, 65], [(int)TypeIndex.Water], ["AquaRing"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Jolteon", new PokemonInfo([65, 65, 60, 110, 95, 130], [(int)TypeIndex.Electric], ["Discharge"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Flareon", new PokemonInfo([65, 130, 60, 95, 110, 65], [(int)TypeIndex.Fire], ["LavaPlume"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            {"Porygon", new PokemonInfo([65, 60, 70, 85, 75, 40], [(int)TypeIndex.Normal], [], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Omanyte", new PokemonInfo([35, 40, 100, 90, 55, 35], [(int)TypeIndex.Rock,(int)TypeIndex.Water], ["WaterPulse"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Omastar", new PokemonInfo([70, 60, 125, 115, 70, 55], [(int)TypeIndex.Rock,(int)TypeIndex.Water], ["AquaRing"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Kabuto", new PokemonInfo([30, 80, 90, 55, 45, 55], [(int)TypeIndex.Rock,(int)TypeIndex.Water], ["QuickAttack"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast, completed: false)},
            {"Kabutops", new PokemonInfo([60, 115, 105, 65, 70, 80], [(int)TypeIndex.Rock,(int)TypeIndex.Water], ["AirSlash"], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast, completed: false)},
            {"Aerodactyl", new PokemonInfo([80, 105, 65, 60, 75, 130], [(int)TypeIndex.Rock,(int)TypeIndex.Flying], ["WingAttack"], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Snorlax", new PokemonInfo([160, 110, 65, 65, 110, 30], [(int)TypeIndex.Normal], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Articuno", new PokemonInfo([90, 85, 100, 95, 125, 85], [(int)TypeIndex.Ice,(int)TypeIndex.Flying], ["IceFang"], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Zapdos", new PokemonInfo([90, 90, 85, 125, 90, 100], [(int)TypeIndex.Electric,(int)TypeIndex.Flying], ["Thunder"], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Moltres", new PokemonInfo([90, 100, 90, 125, 85, 90], [(int)TypeIndex.Fire,(int)TypeIndex.Flying], ["LavaPlume"], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Dratini", new PokemonInfo([41, 64, 45, 50, 50, 50], [(int)TypeIndex.Dragon], [], (int)StageIndex.Basic, (int)ExpTypes.Slow, completed: false)},
            {"Dragonair", new PokemonInfo([61, 84, 65, 70, 70, 70], [(int)TypeIndex.Dragon], [], (int)StageIndex.Stage1, (int)ExpTypes.Slow, completed: false)},
            {"Dragonite", new PokemonInfo([91, 134, 95, 100, 100, 80], [(int)TypeIndex.Dragon,(int)TypeIndex.Flying], ["WingAttack"], (int)StageIndex.Stage2, (int)ExpTypes.Slow, completed: false)},
            {"Mewtwo", new PokemonInfo([106, 110, 90, 154, 90, 130], [(int)TypeIndex.Psychic], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            {"Mew", new PokemonInfo([100, 100, 100, 100, 100, 100], [(int)TypeIndex.Psychic], ["Confusion"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow, legendary: true, completed: false)},
            //Gen 2
            {"Chikorita", new PokemonInfo([45, 49, 65, 49, 65, 45], [(int)TypeIndex.Grass], ["RazorLeaf"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Bayleef", new PokemonInfo([60, 62, 80, 63, 80, 60], [(int)TypeIndex.Grass], ["MagicalLeaf"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Meganium", new PokemonInfo([80, 82, 100, 83, 100, 80], [(int)TypeIndex.Grass], ["GigaDrain"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Cyndaquil", new PokemonInfo([39, 52, 43, 60, 50, 65], [(int)TypeIndex.Fire], ["Smokescreen"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Quilava", new PokemonInfo([58, 64, 58, 80, 65, 80], [(int)TypeIndex.Fire], ["FlameWheel"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Typhlosion", new PokemonInfo([78, 84, 78, 109, 85, 100], [(int)TypeIndex.Fire], ["Overheat"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            {"Totodile", new PokemonInfo([50, 65, 64, 44, 48, 43], [(int)TypeIndex.Water], ["WaterGun"], (int)StageIndex.Basic, (int)ExpTypes.MediumSlow)},
            {"Croconaw", new PokemonInfo([65, 80, 80, 59, 63, 58], [(int)TypeIndex.Water], ["IceFang"], (int)StageIndex.Stage1, (int)ExpTypes.MediumSlow)},
            {"Feraligatr", new PokemonInfo([85, 105, 100, 79, 83, 78], [(int)TypeIndex.Water], ["Waterfall"], (int)StageIndex.Stage2, (int)ExpTypes.MediumSlow)},
            //Gen 5
            {"Joltik", new PokemonInfo([50, 47, 50, 57, 50, 65], [(int)TypeIndex.Bug,(int)TypeIndex.Electric], ["ThunderWave"], (int)StageIndex.Basic, (int)ExpTypes.MediumFast)},
            {"Galvantula", new PokemonInfo([70, 77, 60, 97, 60, 108], [(int)TypeIndex.Bug,(int)TypeIndex.Electric], [], (int)StageIndex.Stage1, (int)ExpTypes.MediumFast)},
            //Gen 7
            {"Zeraora", new PokemonInfo([88, 112, 75, 102, 80, 143], [(int)TypeIndex.Electric], ["ThunderBolt"], (int)StageIndex.Basic, (int)ExpTypes.Slow, legendary: true, completed: false)},
            //Megas
            {"CharizardMegaX", new PokemonInfo([78, 130, 111, 130, 85, 100], [(int)TypeIndex.Fire,(int)TypeIndex.Dragon], ["LavaPlume"], (int)StageIndex.Mega, (int)ExpTypes.MediumSlow)},
        };

        public static string[][] PokemonNatures = [
            ["Hardy", "Lonely", "Adamant", "Naughty", "Brave"],
            ["Bold", "Docile", "Impish", "Lax", "Relaxed"],
            ["Modest", "Mild", "Bashful", "Rash", "Quiet"],
            ["Calm", "Gentle", "Careful", "Quirky", "Sassy"],
            ["Timid", "Hasty", "Jolly", "Naive", "Serious"],
        ];

        public static Dictionary<string, PokemonAttackInfo> pokemonAttacks = new(){
            {"AirSlash", new PokemonAttackInfo(75,80,60,800f,true,true,(int)TypeIndex.Flying, true)},
            {"AquaRing", new PokemonAttackInfo(0,90,60,100f,true,false,(int)TypeIndex.Water, true)},
            {"Bubble", new PokemonAttackInfo(20,10,60,600f,false,false,(int)TypeIndex.Water, true)},
            {"ConfuseRay", new PokemonAttackInfo(0,30,60,800f,true,true,(int)TypeIndex.Ghost, true)},
            {"Confusion", new PokemonAttackInfo(50,56,60,800f,false,true,(int)TypeIndex.Psychic, true)},
            {"Discharge", new PokemonAttackInfo(80,60,60,200f,true,false,(int)TypeIndex.Electric, true)},
            {"DoubleKick", new PokemonAttackInfo(30,30,60,200f,false,false,(int)TypeIndex.Fighting)},
            {"Ember", new PokemonAttackInfo(40,30,60,800f,false,false,(int)TypeIndex.Fire, true)},
            {"Explosion", new PokemonAttackInfo(250,32,60,800f,false,false,(int)TypeIndex.Normal)},
            {"FireBlast", new PokemonAttackInfo(110,40,60,200f,true,true,(int)TypeIndex.Fire, true)},
            {"Flamethrower", new PokemonAttackInfo(90,90,60,400f,true,false,(int)TypeIndex.Fire, true)},
            {"FlameWheel", new PokemonAttackInfo(60,60,60,800f,false,false,(int)TypeIndex.Fire)},
            {"FlashCannon", new PokemonAttackInfo(80,40,60,800f,false,true,(int)TypeIndex.Steel, true)},
            {"GigaDrain", new PokemonAttackInfo(75,56,60,800f,false,true,(int)TypeIndex.Grass, true)},
            {"Gust", new PokemonAttackInfo(40,30,60,800f,false,false,(int)TypeIndex.Flying, true)},
            {"Harden", new PokemonAttackInfo(0,40,60,64f,false,false,(int)TypeIndex.Normal)},
            {"HealPulse", new PokemonAttackInfo(0,30,60,200f,false,false,(int)TypeIndex.Normal, true)},
            {"Hex", new PokemonAttackInfo(65,30,60,800f,true,true,(int)TypeIndex.Ghost, true)},
            {"HydroPump", new PokemonAttackInfo(110,42,60,800f,false,true,(int)TypeIndex.Water, true)},
            {"HyperFang", new PokemonAttackInfo(80,42,60,400f,false,false,(int)TypeIndex.Normal)},
            {"IceFang", new PokemonAttackInfo(65,30,60,200f,false,false,(int)TypeIndex.Ice)},
            {"LavaPlume", new PokemonAttackInfo(80,20,60,800f,true,true,(int)TypeIndex.Fire, true)},
            {"MagicalLeaf", new PokemonAttackInfo(60,60,60,500f,false,true,(int)TypeIndex.Grass, true)},
            {"Overheat", new PokemonAttackInfo(130,60,60,800f,true,true,(int)TypeIndex.Fire, true)},
            {"PinMissile", new PokemonAttackInfo(25,30,60,800f,false,false,(int)TypeIndex.Bug)},
            {"PoisonPowder", new PokemonAttackInfo(0,20,60,800f,true,false,(int)TypeIndex.Poison, true)},
            {"PoisonSting", new PokemonAttackInfo(15,45,60,800f,false,false,(int)TypeIndex.Poison)},
            {"QuickAttack", new PokemonAttackInfo(40,40,60,800f,false,false,(int)TypeIndex.Normal)},
            {"RazorLeaf", new PokemonAttackInfo(55,45,60,800f,false,false,(int)TypeIndex.Grass)},
            {"SelfDestruct", new PokemonAttackInfo(200,32,60,100f,false,false,(int)TypeIndex.Normal)},
            {"Smokescreen", new PokemonAttackInfo(0,60,60,800f,true,false,(int)TypeIndex.Normal, true)},
            {"SolarBeam", new PokemonAttackInfo(120,60,60,800f,false,true,(int)TypeIndex.Grass, true)},
            {"StringShot", new PokemonAttackInfo(0,60,60,800f,false,false,(int)TypeIndex.Bug)},
            {"Swift", new PokemonAttackInfo(60,0,120,800f,true,true,(int)TypeIndex.Normal, true)},
            {"Thunder", new PokemonAttackInfo(110,30,60,800f,false,false,(int)TypeIndex.Electric, true)},
            {"Thunderbolt", new PokemonAttackInfo(90,30,60,800f,true,false,(int)TypeIndex.Electric, true)},
            {"ThunderWave", new PokemonAttackInfo(0,40,60,800f,false,false,(int)TypeIndex.Electric, true)},
            {"Toxic", new PokemonAttackInfo(0,36,60,800f,false,true,(int)TypeIndex.Poison, true)},
            {"VineWhip", new PokemonAttackInfo(45,45,60,140f,true,false,(int)TypeIndex.Grass)},
            {"Waterfall", new PokemonAttackInfo(80,42,60,600f,false,true,(int)TypeIndex.Water)},
            {"WaterGun", new PokemonAttackInfo(40,30,60,600f,false,false,(int)TypeIndex.Water, true)},
            {"WaterPulse", new PokemonAttackInfo(60,40,60,800f,false,true,(int)TypeIndex.Water, true)},
            {"WingAttack", new PokemonAttackInfo(60,40,60,800f,false,false,(int)TypeIndex.Flying)},
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

    internal class PokemonInfo
    {
        public int[] pokemonStats;
        public int[] pokemonTypes;
        public string[] movePool;
        public int pokemonStage;
        public int expType;

        public bool legendary;

        public bool completed;

        public PokemonInfo(int[] pokemonStats, int[] pokemonTypes, string[] movePool, int pokemonStage = 0, int expType = 0, bool legendary = false, bool completed = true){
            this.pokemonStats = pokemonStats;
            this.pokemonTypes = [(pokemonTypes.Length<=0?-1:pokemonTypes[0]), (pokemonTypes.Length<=1?-1:pokemonTypes[1])];
            this.movePool = (movePool.Length>0)?movePool:["Swift"];
            this.pokemonStage = pokemonStage;
            this.expType = expType;
            this.legendary = legendary;
            this.completed = completed;
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