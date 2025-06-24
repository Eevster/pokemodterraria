using Pokemod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class AirBalloon : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = 0;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().HasAirBalloon = 3;
		}
	}
}