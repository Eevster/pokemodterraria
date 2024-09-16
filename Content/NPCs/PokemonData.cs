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
        public int[] EVs;
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
            this.EVs = EVs;
            this.ultrabeast = ultrabeast;
        }

        public static int[] CalcAllStats(int level, int[] stats, int[] EVs)
        {
            int[] allStats = {0,0,0,0,0,0};
            for(int i = 0; i < allStats.Length; i++){
                allStats[i] = StatFunc(i==0,stats[i],EVs[i],0,level);
            }

            return allStats;
        }

        public static int StatFunc(bool HP,int baseStat,int IV,int EV, int Level)
        {
            int done = 0;
            if(HP) {done = ((2 * baseStat + IV + (EV/4) * Level)/100) + Level + 10;}
            if(!HP) { done = ((2 * baseStat + IV + (EV / 4) * Level) / 100) + 5;}
            return done;
        }

        public static int[] GenerateIVs()
        {
            int[] EVs = [0,0,0,0,0,0,0];
            for(int i = 0; i < EVs.Length; i++){
                EVs[i] = GenerateIV();
            }

            return EVs;
        }

        public static int GenerateIV()
        {
            return Main.rand.Next(32);
        }
    }
}