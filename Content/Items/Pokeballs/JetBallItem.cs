using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class JetballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<WingballProj>();
		protected override int BallValue => 5000;
		protected override float CatchRate => 2.0f;
		protected override float ThrowSpeed => 25f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<BlueApricorn>(), 2)
				.AddIngredient(ItemID.Feather, 3)
				.AddRecipeGroup(RecipeGroupID.IronBar, 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class JetballProj : BallProj{
		protected override bool hasGravity => false;
	}
}
