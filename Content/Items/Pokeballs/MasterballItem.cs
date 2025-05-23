using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class MasterballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<MasterballProj>();
		protected override int BallValue => 100000;
		protected override float CatchRate => 255f;
		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<PinkApricorn>(), 10)
				.AddIngredient(ItemID.RodofDiscord, 1)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class MasterballProj : BallProj{}
}
