using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items;
using Pokemod.Content.Pets.CharmanderPet;
using Pokemod.Content.Pets.BulbasaurPet;
using Pokemod.Content.Pets.SquirtlePet;

namespace Pokemod.Common.Players
{
    public class PokemonPlayer : ModPlayer
	{
        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
			if(item.type == ModContent.ItemType<CharmanderPetItem>() || item.type == ModContent.ItemType<BulbasaurPetItem>() || item.type == ModContent.ItemType<SquirtlePetItem>()){
				int ballItem;
				string pokemonName = "";
				
				if(item.type == ModContent.ItemType<CharmanderPetItem>()) pokemonName = "Charmander";
				if(item.type == ModContent.ItemType<BulbasaurPetItem>()) pokemonName = "Bulbasaur";
				if(item.type == ModContent.ItemType<SquirtlePetItem>()) pokemonName = "Squirtle";

				if (Main.netMode == NetmodeID.SinglePlayer){
					ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
				}else{
					if(Main.netMode == NetmodeID.Server){
						ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
						CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
						pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
					}
				}

				item.TurnToAir();
			}
            base.PostBuyItem(vendor, shopInventory, item);
        }
    }
}