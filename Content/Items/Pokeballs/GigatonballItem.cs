using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class GigatonballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<GigatonballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 2.5f;
        protected override float ThrowSpeed => 8f;
        public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 3)
				.AddIngredient(ItemID.Obsidian, 30)
				.AddRecipeGroup(RecipeGroupID.IronBar, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class GigatonballProj : BallProj{
        protected override float gravityScale => 4f;
    }
}
