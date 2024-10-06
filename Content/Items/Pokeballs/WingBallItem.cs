using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class WingballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<WingballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 1.5f;
		protected override float ThrowSpeed => 20f;
		public override void AddRecipes() {
			CreateRecipe(4)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 2)
				.AddIngredient(ItemID.Feather, 3)
				.AddRecipeGroup(RecipeGroupID.IronBar, 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class WingballProj : BallProj{
		protected override bool hasGravity => false;
	}
}
