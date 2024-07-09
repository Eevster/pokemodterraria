using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class MasterballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<MasterballProj>();
		protected override int BallValue => 100000;
		protected override float CatchRate => 255f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ItemID.GemTreeAmethystSeed, 2)
				.AddIngredient(ItemID.RodofDiscord, 1)
				.AddIngredient(ItemID.LunarBar, 3)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class MasterballProj : BallProj{}
}
