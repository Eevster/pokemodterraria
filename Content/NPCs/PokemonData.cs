using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.NPCs
{
    internal class PokemonNPCData : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool isPokemon = false;
        public string pokemonName = "";
        public bool shiny = false;
        public bool ultrabeast = false;

        public void SetPokemonNPCData(string pokemonName, bool shiny = false, bool ultrabeast = false){
            isPokemon = true;
            this.pokemonName = pokemonName;
            this.shiny = shiny;
            this.ultrabeast = ultrabeast;
        }
    }
}