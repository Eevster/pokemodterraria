using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.Items.Pokeballs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.Players
{
    public class InventoryPlayer : ModPlayer
	{
		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
			return new[] {
				new Item(ModContent.ItemType<PokeballItem>(), 10),
				new Item(ModContent.ItemType<Potion>(), 5)
                new Item(ModContent.ItemType<Revive>(), 1)
            };
		}
    }
}
