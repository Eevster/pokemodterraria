using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class GreatballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<GreatballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1.5f;
		public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 2)
				.AddIngredient(ModContent.ItemType<RedApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class GreatballProj : BallProj{}
}
