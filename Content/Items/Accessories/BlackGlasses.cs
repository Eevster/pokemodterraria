using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.NPCs;
using Pokemod.Content.Items.Accessories.Gems;

namespace Pokemod.Content.Items.Accessories
{
    public class BlackGlasses : TypeDamageItem
	{
		public override int pokemonType => (int)TypeIndex.Dark;
		public override float damageMult => 0.15f;

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<DarkGem>(3)
				.AddIngredient(ItemID.WaterCandle, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}