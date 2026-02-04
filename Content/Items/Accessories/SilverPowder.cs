using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.NPCs;
using Pokemod.Content.Items.Accessories.Gems;

namespace Pokemod.Content.Items.Accessories
{
    public class SilverPowder : TypeDamageItem
	{
		public override int pokemonType => (int)TypeIndex.Bug;
		public override float damageMult => 0.15f;

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<BugGem>(3)
				.AddIngredient(ItemID.WaterCandle, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}
	}
}