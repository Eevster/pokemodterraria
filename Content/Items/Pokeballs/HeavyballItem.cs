using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class HeavyballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<HeavyballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1.5f;
        protected override float ThrowSpeed => 8f;
        public override void AddRecipes() {
			CreateRecipe(8)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 1)
				.AddIngredient(ItemID.Obsidian, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class HeavyballProj : BallProj{
        protected override float gravityScale => 2f;
    }
}
