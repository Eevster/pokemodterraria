using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class PremierballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<PremierballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<WhiteApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class PremierballProj : BallProj{}
}
