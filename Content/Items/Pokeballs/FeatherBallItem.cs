using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class FeatherballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<FeatherballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1f;
		public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 1)
				.AddIngredient(ItemID.Feather, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class FeatherballProj : BallProj{
		protected override bool hasGravity => false;
        
    }
}
