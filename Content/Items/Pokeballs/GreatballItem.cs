using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class GreatballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<GreatballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1.5f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeSapphireSeed, 2)
				.AddIngredient(ItemID.GemTreeRubySeed, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 3)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class GreatballProj : BallProj{}
}
