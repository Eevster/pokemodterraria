using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class FeatherballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<FeatherballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeSapphireSeed, 2)
				.AddIngredient(ItemID.Feather, 2)
				.AddRecipeGroup(RecipeGroupID.IronBar, 3)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class FeatherballProj : BallProj{
		protected override bool hasGravity => false;
	}
}
