using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.Items.Apricorns;

namespace Pokemod.Content.Items.Pokeballs
{
	public class ShimmerballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<ShimmerballProj>();
		protected override int BallValue => 1000;
		protected override float CatchRate => 1.0f;
	}

	public class ShimmerballProj : BallProj{
        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.light = 0.5f;
        }
	}
}
