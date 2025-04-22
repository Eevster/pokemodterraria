using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class GalaxyballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<GalaxyballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 3.0f;
		protected override float ThrowSpeed => 25f;

		public override void AddRecipes() {
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<ShimmerballItem>(), 1)
				.AddIngredient(ItemID.LunarBar, 3)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}

	public class GalaxyballProj : BallProj{
		protected override bool hasGravity => false;
        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.light = 0.5f;
        }
	}
}
