using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Accessories
{
    public class MuscleBand : ModItem
	{
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(silver: 50);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.HellstoneBar, 5)
				.AddIngredient(ItemID.SoulofNight, 10)
				.AddIngredient(ItemID.SoulofLight, 10)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}