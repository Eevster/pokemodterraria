using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class BigRoot : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(silver: 50);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().HasBigRoot = 3;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Vine, 3)
				.AddIngredient(ItemID.Mushroom, 10)
				.AddIngredient(ItemID.BottledHoney, 3)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}