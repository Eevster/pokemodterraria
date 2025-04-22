using Pokemod.Content.Items.Apricorns;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class SafariballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<SafariballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

		public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<GreenApricorn>(), 1)
				.AddIngredient(ItemID.DesertFossil, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class SafariballProj : BallProj{}
}
