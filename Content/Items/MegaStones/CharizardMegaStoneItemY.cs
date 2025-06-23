
using Pokemod.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.MegaStones
{
	public class CharizardMegaStoneItemY : ModItem
	{
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;

			Item.value = Item.buyPrice(silver: 1);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().MegaStone = Name;
			player.GetModPlayer<PokemonPlayer>().HasMegaStone = 3;
		}
	}
}
