using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class ReinforcedBand : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(silver: 80);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().contactMult += 0.1f;
			player.GetModPlayer<PokemonPlayer>().HasShellBell = 3;
			player.GetModPlayer<PokemonPlayer>().HasRockyHelmet = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<MuscleBand>(1)
				.AddIngredient<ShellBell>(1)
				.AddIngredient<RockyHelmet>(1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}