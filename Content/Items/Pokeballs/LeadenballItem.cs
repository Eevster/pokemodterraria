using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class LeadenballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<LeadenballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 2f;
        protected override float ThrowSpeed => 8f;
        public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ItemID.GemTreeSapphireSeed, 1)
				.AddIngredient(ItemID.Obsidian, 5)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class LeadenballProj : BallProj{
        protected override float gravityScale => 3f;
    }
}
