using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public void SetPokemonNPCData(string pokemonName, bool shiny = false, int lvl = 5, int[] baseStats = null, int[] EVs = null, bool ultrabeast = false){
            isPokemon = true;
            this.pokemonName = pokemonName;
            this.shiny = shiny;
            this.lvl = lvl;
            this.baseStats = baseStats;
            this.IVs = EVs;
            this.ultrabeast = ultrabeast;
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

        public static Dictionary<string, int[]> pokemonStats = new(){
            {"Bulbasaur", [45, 49, 49, 65, 65, 45]},
            {"Ivysaur", [60, 62, 63, 80, 80, 60]},
            {"Venusaur", [80, 82, 83, 100, 100, 80]},
            {"Charmander", [39, 52, 43, 60, 50, 65]},
            {"Charmeleon", [58, 64, 58, 80, 65, 80]},
            {"Charizard", [78, 84, 78, 109, 85, 100]},
            {"Squirtle", [44, 48, 65, 50, 64, 43]},
            {"Wartortle", [59, 63, 80, 65, 80, 58]},
            {"Blastoise", [79, 83, 100, 85, 105, 78]},
            {"Pikachu", [35, 55, 40, 50, 50, 90]},
            {"Raichu", [60, 90, 55, 90, 80, 110]},
            {"Eevee", [55, 55, 50, 45, 65, 55]},
            {"Vaporeon", [130, 65, 60, 110, 95, 65]},
            {"Jolteon", [65, 65, 60, 110, 95, 130]},
            {"Flareon", [65, 130, 60, 95, 110, 65]},
            {"Chikorita", [45, 49, 65, 49, 65, 45]},
            {"Bayleef", [60, 62, 80, 63, 80, 60]},
            {"Meganium", [80, 82, 100, 83, 100, 80]},
            {"Cyndaquil", [39, 52, 43, 60, 50, 65]},
        }; 
    }
}