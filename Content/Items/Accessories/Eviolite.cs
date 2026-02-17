using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class Eviolite : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(gold: 20);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().HasEverstone = 3;
			player.GetModPlayer<PokemonPlayer>().HasEviolite = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Everstone>(1)
				.AddIngredient(ItemID.CrystalShard, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}