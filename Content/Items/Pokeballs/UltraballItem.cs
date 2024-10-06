using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class UltraballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<UltraballProj>();
		protected override int BallValue => 15000;
		protected override float CatchRate => 2f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<YellowApricorn>(), 3)
				.AddIngredient(ItemID.Obsidian, 1)
				.AddRecipeGroup(RecipeGroupID.IronBar, 2)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class UltraballProj : BallProj{}
}
