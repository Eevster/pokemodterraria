using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class Leftovers : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = 0;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().LeftoversEffect();
		}
	}
}