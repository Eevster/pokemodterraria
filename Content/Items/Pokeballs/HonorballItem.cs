using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class HonorballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<HonorballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ItemID.GemTreeDiamondSeed, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class HonorballProj : BallProj{}
}
