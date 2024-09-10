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

        public void SetPokemonNPCData(string pokemonName, bool shiny = false, bool ultrabeast = false){
            isPokemon = true;
            this.pokemonName = pokemonName;
            this.shiny = shiny;
            this.ultrabeast = ultrabeast;
        }
    }
}