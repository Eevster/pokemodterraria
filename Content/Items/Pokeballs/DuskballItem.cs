using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class DuskballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<DuskballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1f;

        public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<GreenApricorn>(), 2)
				.AddIngredient(ItemID.Obsidian, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class DuskballProj : BallProj{
		public override bool FailureProb(float catchRate){
			if(!Main.dayTime || Main.player[Projectile.owner].ZoneRockLayerHeight) catchRate *= 3f;

			return RegularProb(catchRate);
		}
	}
}
