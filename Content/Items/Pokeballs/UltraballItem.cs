using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class UltraballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<UltraballProj>();
		protected override int BallValue => 15000;
		protected override float CatchRate => 2f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeAmberSeed, 3)
				.AddIngredient(ItemID.Obsidian, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 2)
				.AddTile(TileID.WorkBenches)
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeTopazSeed, 3)
				.AddIngredient(ItemID.Obsidian, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class UltraballProj : BallProj{}
}
