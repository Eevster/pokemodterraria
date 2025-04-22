using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class PokeballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<PokeballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1f;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShimmerballItem>();
        }

        public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<RedApricorn>(), 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	public class PokeballProj : BallProj{}
}
