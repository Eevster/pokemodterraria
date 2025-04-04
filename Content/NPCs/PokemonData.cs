using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pokemod.Common.Configs;
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

        public void SetPokemonNPCData(string pokemonName, bool shiny = false, int lvl = 5, int[] baseStats = null, int[] IVs = null, bool ultrabeast = false, string variant = ""){
            isPokemon = true;
            this.pokemonName = pokemonName;
            this.shiny = shiny;
            this.lvl = lvl;
            this.baseStats = baseStats;
            this.IVs = IVs;
            this.ultrabeast = ultrabeast;
            this.variant = variant;
        }

        public static int[] CalcAllStats(int level, int[] stats, int[] IVs, int[] EVs)
        {
            int[] allStats = {0,0,0,0,0,0};
            for(int i = 0; i < allStats.Length; i++){
                allStats[i] = StatFunc(i==0,stats[i],IVs[i],EVs[i],level);
            }

            return allStats;
        }

        public static int StatFunc(bool HP,int baseStat,int IV,int EV, int Level)
        {
            int done = 0;
            if(HP) {done = ((2 * baseStat + IV + (EV/4)) * Level/100) + Level + 10;}
            if(!HP) { done = (2 * (baseStat + IV + (EV / 4)) * Level / 100) + 5;}
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
            string[] pokemonNames = pokemonInfo.Keys.ToArray();
            bool canBeStarter = false;
            int pokemonIndex;
            string pokemonName = "";

            while(!canBeStarter){
                pokemonIndex = Main.rand.Next(pokemonInfo.Keys.Count);
                pokemonName = pokemonNames[pokemonIndex];

                if(!blackList.Contains(pokemonName) && pokemonInfo[pokemonName].completed && pokemonInfo[pokemonName].pokemonStage == 0 && !pokemonInfo[pokemonName].legendary && Enumerable.Sum(pokemonInfo[pokemonName].pokemonStats) < 350){
                    canBeStarter = true;
                    blackList.Add(pokemonName);
                }
            }

            return pokemonName;
        }

        public static string GetRandomEvolution(string pokemonName){
            int stage = 0;
            if(pokemonInfo[pokemonName].pokemonStage < 2 || pokemonInfo[pokemonName].pokemonStage == 3){
                if(pokemonInfo[pokemonName].pokemonStage < 2){
                    stage = pokemonInfo[pokemonName].pokemonStage+1;
                }

                var filterList = pokemonInfo.Where(i => i.Value.pokemonStage == stage);
                var posibleEvolutions = filterList.ToDictionary(i => i.Key, i => i.Value);
                int pokemonIndex = 0;
                bool canBeEvo = false;

                while(!canBeEvo){
                    pokemonIndex = Main.rand.Next(posibleEvolutions.Keys.Count);
                    if(pokemonInfo[posibleEvolutions.Keys.ToArray()[pokemonIndex]].completed){
                        canBeEvo = true;
                    }
                }

                return posibleEvolutions.Keys.ToArray()[pokemonIndex];
            }

            return "";
        }

        public static Dictionary<string, PokemonInfo> pokemonInfo = new(){
            //Gen 1
            {"Bulbasaur", new PokemonInfo([45, 49, 49, 65, 65, 45], 0)},
            {"Ivysaur", new PokemonInfo([60, 62, 63, 80, 80, 60], 1)},
            {"Venusaur", new PokemonInfo([80, 82, 83, 100, 100, 80], 2)},
            {"Charmander", new PokemonInfo([39, 52, 43, 60, 50, 65], 0)},
            {"Charmeleon", new PokemonInfo([58, 64, 58, 80, 65, 80], 1)},
            {"Charizard", new PokemonInfo([78, 84, 78, 109, 85, 100], 2)},
            {"Squirtle", new PokemonInfo([44, 48, 65, 50, 64, 43], 0)},
            {"Wartortle", new PokemonInfo([59, 63, 80, 65, 80, 58], 1)},
            {"Blastoise", new PokemonInfo([79, 83, 100, 85, 105, 78], 2)},
            {"Caterpie", new PokemonInfo([45, 30, 35, 20, 20, 45], 0)},
            {"Metapod", new PokemonInfo([50, 20, 55, 25, 25, 30], 1)},
            {"Butterfree", new PokemonInfo([60, 45, 50, 90, 80, 70], 2)},
            {"Weedle", new PokemonInfo([40, 35, 30, 20, 20, 50], 0)},
            {"Kakuna", new PokemonInfo([45, 25, 50, 25, 25, 35], 1)},
            {"Beedrill", new PokemonInfo([65, 90, 40, 45, 80, 75], 2)},
            {"Pidgey", new PokemonInfo([40, 45, 40, 35, 35, 56], 0)},
            {"Pidgeotto", new PokemonInfo([63, 60, 55, 50, 50, 71], 1)},
            {"Pidgeot", new PokemonInfo([83, 80, 75, 70, 70, 101], 2)},
            {"Rattata", new PokemonInfo([30, 56, 35, 25, 35, 72], 0)},
            {"Raticate", new PokemonInfo([55, 81, 60, 50, 70, 97], 1)},
            {"Spearow", new PokemonInfo([40, 60, 30, 31, 31, 70], 0, completed: false)},
            {"Fearow", new PokemonInfo([65, 90, 65, 61, 61, 100], 1, completed: false)},
            {"Ekans", new PokemonInfo([35, 60, 44, 40, 54, 55], 0, completed: false)},
            {"Arbok", new PokemonInfo([60, 95, 69, 65, 79, 80], 1, completed: false)},
            {"Pikachu", new PokemonInfo([35, 55, 40, 50, 50, 90], 0)},
            {"Raichu", new PokemonInfo([60, 90, 55, 90, 80, 110], 1)},
            {"Sandshrew", new PokemonInfo([50, 75, 85, 20, 30, 40], 0, completed: false)},
            {"Sandslash", new PokemonInfo([75, 100, 110, 45, 55, 65], 1, completed: false)},
            {"NidoranM", new PokemonInfo([55, 47, 52, 40, 40, 41], 0)},
            {"Nidorina", new PokemonInfo([70, 62, 67, 55, 55, 56], 1)},
            {"Nidoqueen", new PokemonInfo([90, 92, 87, 75, 85, 76], 2)},
            {"NidoranF", new PokemonInfo([46, 57, 40, 40, 40, 50], 0)},
            {"Nidorino", new PokemonInfo([61, 72, 57, 55, 55, 65], 1)},
            {"Nidoking", new PokemonInfo([81, 102, 77, 85, 75, 85], 2)},
            {"Clefairy", new PokemonInfo([70, 45, 48, 60, 65, 35], 0, completed: false)},
            {"Clefable", new PokemonInfo([95, 70, 73, 95, 90, 60], 1, completed: false)},
            {"Vulpix", new PokemonInfo([38, 41, 40, 50, 65, 65], 0, completed: false)},
            {"Ninetales", new PokemonInfo([73, 76, 75, 81, 100, 100], 1, completed: false)},
            {"Jigglypuff", new PokemonInfo([115, 45, 20, 45, 25, 20], 0, completed: false)},
            {"Wigglytuff", new PokemonInfo([140, 70, 45, 85, 50, 45], 1, completed: false)},
            {"Zubat", new PokemonInfo([40, 45, 35, 30, 40, 55], 0, completed: false)},
            {"Golbat", new PokemonInfo([75, 80, 70, 65, 75, 90], 1, completed: false)},
            {"Oddish",new PokemonInfo( [45, 50, 55, 75, 65, 30], 0, completed: false)},
            {"Gloom", new PokemonInfo([60, 65, 70, 85, 75, 40], 1, completed: false)},
            {"Vileplume", new PokemonInfo([75, 80, 85, 110, 90, 50], 2, completed: false)},
            {"Paras", new PokemonInfo([35, 70, 55, 45, 55, 25], 0)},
            {"Parasect", new PokemonInfo([60, 95, 80, 60, 80, 30], 1)},
            {"Venonat", new PokemonInfo([60, 55, 50, 40, 55, 45], 0, completed: false)},
            {"Venomoth", new PokemonInfo([70, 65, 60, 90, 75, 90], 1, completed: false)},
            {"Diglett", new PokemonInfo([10, 55, 25, 35, 45, 95], 0)},
            {"Dugtrio", new PokemonInfo([35, 100, 50, 50, 70, 120], 1)},
            {"Meowth", new PokemonInfo([40, 45, 35, 40, 40, 90], 0, completed: false)},
            {"Persian", new PokemonInfo([65, 70, 60, 65, 65, 115], 1, completed: false)},
            {"Psyduck", new PokemonInfo([50, 52, 48, 65, 50, 55], 0, completed: false)},
            {"Golduck", new PokemonInfo([80, 82, 78, 95, 80, 85], 1, completed: false)},
            {"Mankey", new PokemonInfo([40, 80, 35, 35, 45, 70], 0, completed: false)},
            {"Primeape", new PokemonInfo([65, 105, 60, 60, 70, 95], 1, completed: false)},
            {"Growlithe", new PokemonInfo([55, 70, 45, 70, 50, 60], 0, completed: false)},
            {"Arcanine", new PokemonInfo([90, 110, 80, 100, 80, 95], 1, completed: false)},
            {"Poliwag", new PokemonInfo([40, 50, 40, 40, 40, 90], 0, completed: false)},
            {"Poliwhirl", new PokemonInfo([65, 65, 65, 50, 50, 90], 1, completed: false)},
            {"Poliwrath", new PokemonInfo([90, 95, 95, 70, 90, 70], 2, completed: false)},
            {"Abra", new PokemonInfo([25, 20, 15, 105, 55, 90], 0, completed: false)},
            {"Kadabra", new PokemonInfo([40, 35, 30, 120, 70, 105], 1, completed: false)},
            {"Alakazam", new PokemonInfo([55, 50, 45, 135, 95, 120], 2, completed: false)},
            {"Machop", new PokemonInfo([70, 80, 50, 35, 35, 35], 0, completed: false)},
            {"Machoke", new PokemonInfo([80, 100, 70, 50, 60, 45], 1, completed: false)},
            {"Machamp", new PokemonInfo([90, 130, 80, 65, 85, 55], 2, completed: false)},
            {"Bellsprout", new PokemonInfo([50, 75, 35, 70, 30, 40], 0)},
            {"Weepinbell", new PokemonInfo([65, 90, 50, 85, 45, 55], 1)},
            {"Victreebel", new PokemonInfo([80, 105, 65, 100, 70, 70], 2)},
            {"Tentacool", new PokemonInfo([40, 40, 35, 50, 100, 70], 0, completed: false)},
            {"Tentacruel", new PokemonInfo([80, 70, 65, 80, 120, 100], 1, completed: false)},
            {"Geodude", new PokemonInfo([40, 80, 100, 30, 30, 20], 0, completed: false)},
            {"Graveler", new PokemonInfo([55, 95, 115, 45, 45, 35], 1, completed: false)},
            {"Golem", new PokemonInfo([80, 120, 130, 55, 65, 45], 2, completed: false)},
            {"Ponyta", new PokemonInfo([50, 85, 55, 65, 65, 90], 0, completed: false)},
            {"Rapidash", new PokemonInfo([65, 100, 70, 80, 80, 105], 1, completed: false)},
            {"Slowpoke", new PokemonInfo([90, 65, 65, 40, 40, 15], 0, completed: false)},
            {"Slowbro", new PokemonInfo([95, 75, 110, 100, 80, 30], 1, completed: false)},
            {"Magnemite", new PokemonInfo([25, 35, 70, 95, 55, 45], 0)},
            {"Magneton", new PokemonInfo([50, 60, 95, 120, 70, 70], 1)},
            {"Farfetch'd", new PokemonInfo([52, 90, 55, 58, 62, 60], 0, completed: false)},
            {"Doduo", new PokemonInfo([35, 85, 45, 35, 35, 75], 0, completed: false)},
            {"Dodrio", new PokemonInfo([60, 110, 70, 60, 60, 110], 1, completed: false)},
            {"Seel", new PokemonInfo([65, 45, 55, 45, 70, 45], 0, completed: false)},
            {"Dewgong", new PokemonInfo([90, 70, 80, 70, 95, 70], 1, completed: false)},
            {"Grimer", new PokemonInfo([80, 80, 50, 40, 50, 25], 0, completed: false)},
            {"Muk", new PokemonInfo([105, 105, 75, 65, 100, 50], 1, completed: false)},
            {"Shellder", new PokemonInfo([30, 65, 100, 45, 25, 40], 0, completed: false)},
            {"Cloyster", new PokemonInfo([50, 95, 180, 85, 45, 70], 1, completed: false)},
            {"Gastly", new PokemonInfo([30, 35, 30, 100, 35, 80], 0)},
            {"Haunter", new PokemonInfo([45, 50, 45, 115, 55, 95], 1)},
            {"Gengar", new PokemonInfo([60, 65, 60, 130, 75, 110], 2)},
            {"Onix", new PokemonInfo([35, 45, 160, 30, 45, 70], 0, completed: false)},
            {"Drowzee", new PokemonInfo([60, 48, 45, 43, 90, 42], 0, completed: false)},
            {"Hypno", new PokemonInfo([85, 73, 70, 73, 115, 67], 1, completed: false)},
            {"Krabby", new PokemonInfo([30, 105, 90, 25, 25, 50], 0, completed: false)},
            {"Kingler", new PokemonInfo([55, 130, 115, 50, 50, 75], 1, completed: false)},
            {"Voltorb", new PokemonInfo([40, 30, 50, 55, 55, 100], 0)},
            {"Electrode", new PokemonInfo([60, 50, 70, 80, 80, 150], 1)},
            {"Exeggcute", new PokemonInfo([60, 40, 80, 60, 45, 40], 0, completed: false)},
            {"Exeggutor", new PokemonInfo([95, 95, 85, 125, 75, 55], 1, completed: false)},
            {"Cubone", new PokemonInfo([50, 50, 95, 40, 50, 35], 0, completed: false)},
            {"Marowak", new PokemonInfo([60, 80, 110, 50, 80, 45], 1, completed: false)},
            {"Hitmonlee", new PokemonInfo([50, 120, 53, 35, 110, 87], 0, completed: false)},
            {"Hitmonchan", new PokemonInfo([50, 105, 79, 35, 110, 76], 0, completed: false)},
            {"Lickitung", new PokemonInfo([90, 55, 75, 60, 75, 30], 0, completed: false)},
            {"Koffing", new PokemonInfo([40, 65, 95, 60, 45, 35], 0)},
            {"Weezing", new PokemonInfo([65, 90, 120, 85, 70, 60], 1)},
            {"Rhyhorn", new PokemonInfo([80, 85, 95, 30, 30, 25], 0, completed: false)},
            {"Rhydon", new PokemonInfo([105, 130, 120, 45, 45, 40], 1, completed: false)},
            {"Chansey", new PokemonInfo([250, 5, 5, 35, 105, 50], 0)},
            {"Tangela", new PokemonInfo([65, 55, 115, 100, 40, 60], 0, completed: false)},
            {"Kangaskhan", new PokemonInfo([105, 95, 80, 40, 80, 90], 0, completed: false)},
            {"Horsea", new PokemonInfo([30, 40, 70, 70, 25, 60], 0, completed: false)},
            {"Seadra", new PokemonInfo([55, 65, 95, 95, 45, 85], 1, completed: false)},
            {"Goldeen", new PokemonInfo([45, 67, 60, 35, 50, 63], 0, completed: false)},
            {"Seaking", new PokemonInfo([80, 92, 65, 65, 80, 68], 1, completed: false)},
            {"Staryu", new PokemonInfo([30, 45, 55, 70, 55, 85], 0, completed: false)},
            {"Starmie", new PokemonInfo([60, 75, 85, 100, 85, 115], 1, completed: false)},
            {"Mr. Mime", new PokemonInfo([40, 45, 65, 100, 120, 90], 0, completed: false)},
            {"Scyther", new PokemonInfo([70, 110, 80, 55, 80, 105], 0, completed: false)},
            {"Jynx", new PokemonInfo([65, 50, 35, 115, 95, 95], 0, completed: false)},
            {"Electabuzz", new PokemonInfo([65, 83, 57, 95, 85, 105], 0, completed: false)},
            {"Magmar", new PokemonInfo([65, 95, 57, 100, 85, 93], 0, completed: false)},
            {"Pinsir", new PokemonInfo([65, 125, 100, 55, 70, 85], 0, completed: false)},
            {"Tauros", new PokemonInfo([75, 100, 95, 40, 70, 110], 0, completed: false)},
            {"Magikarp", new PokemonInfo([20, 10, 55, 15, 20, 80], 0, completed: false)},
            {"Gyarados", new PokemonInfo([95, 125, 79, 60, 100, 81], 1, completed: false)},
            {"Lapras", new PokemonInfo([130, 85, 80, 85, 95, 60], 0, completed: false)},
            {"Ditto", new PokemonInfo([48, 48, 48, 48, 48, 48], 0, completed: false)},
            {"Eevee", new PokemonInfo([55, 55, 50, 45, 65, 55], 0)},
            {"Vaporeon", new PokemonInfo([130, 65, 60, 110, 95, 65], 1)},
            {"Jolteon", new PokemonInfo([65, 65, 60, 110, 95, 130], 1)},
            {"Flareon", new PokemonInfo([65, 130, 60, 95, 110, 65], 1)},
            {"Porygon", new PokemonInfo([65, 60, 70, 85, 75, 40], 0, completed: false)},
            {"Omanyte", new PokemonInfo([35, 40, 100, 90, 55, 35], 0, completed: false)},
            {"Omastar", new PokemonInfo([70, 60, 125, 115, 70, 55], 1, completed: false)},
            {"Kabuto", new PokemonInfo([30, 80, 90, 55, 45, 55], 0, completed: false)},
            {"Kabutops", new PokemonInfo([60, 115, 105, 65, 70, 80], 1, completed: false)},
            {"Aerodactyl", new PokemonInfo([80, 105, 65, 60, 75, 130], 0, completed: false)},
            {"Snorlax", new PokemonInfo([160, 110, 65, 65, 110, 30], 0, completed: false)},
            {"Articuno", new PokemonInfo([90, 85, 100, 95, 125, 85], 0, legendary: true, completed: false)},
            {"Zapdos", new PokemonInfo([90, 90, 85, 125, 90, 100], 0, legendary: true, completed: false)},
            {"Moltres", new PokemonInfo([90, 100, 90, 125, 85, 90], 0, legendary: true, completed: false)},
            {"Dratini", new PokemonInfo([41, 64, 45, 50, 50, 50], 0, completed: false)},
            {"Dragonair", new PokemonInfo([61, 84, 65, 70, 70, 70], 1, completed: false)},
            {"Dragonite", new PokemonInfo([91, 134, 95, 100, 100, 80], 2, completed: false)},
            {"Mewtwo", new PokemonInfo([106, 110, 90, 154, 90, 130], 0, legendary: true, completed: false)},
            {"Mew", new PokemonInfo([100, 100, 100, 100, 100, 100], 0, legendary: true, completed: false)},
            //Gen 2
            {"Chikorita", new PokemonInfo([45, 49, 65, 49, 65, 45], 0)},
            {"Bayleef", new PokemonInfo([60, 62, 80, 63, 80, 60], 1)},
            {"Meganium", new PokemonInfo([80, 82, 100, 83, 100, 80], 2)},
            {"Cyndaquil", new PokemonInfo([39, 52, 43, 60, 50, 65], 0)},
            {"Quilava", new PokemonInfo([58, 64, 58, 80, 65, 80], 1)},
            {"Typhlosion", new PokemonInfo([78, 84, 78, 109, 85, 100], 2)},
            {"Totodile", new PokemonInfo([50, 65, 64, 44, 48, 43], 0)},
            {"Croconaw", new PokemonInfo([65, 80, 80, 59, 63, 58], 1)},
            {"Feraligatr", new PokemonInfo([85, 105, 100, 79, 83, 78], 2)},
            //Gen 5
            {"Joltik", new PokemonInfo([50, 47, 50, 57, 50, 65], 0)},
            {"Galvantula", new PokemonInfo([70, 77, 60, 97, 60, 108], 1)},
            //Gen 7
            {"Zeraora", new PokemonInfo([88, 112, 75, 102, 80, 143], 0)},
            //Megas
            {"CharizardMegaX", new PokemonInfo([78, 130, 111, 130, 85, 100], 4)},
        }; 
    }

    internal class PokemonInfo
    {
        public int[] pokemonStats;
        public int pokemonStage;

        public enum StageIndex
        {
            Basic,
            Stage1,
            Stage2,
            Baby,
            Mega
        }

        public bool legendary;

        public bool completed;

        public PokemonInfo(int[] pokemonStats, int pokemonStage = 0, bool legendary = false, bool completed = true){
            this.pokemonStats = pokemonStats;
            this.pokemonStage = pokemonStage;
            this.legendary = legendary;
            this.completed = completed;
        }
    }
}