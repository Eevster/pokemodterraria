using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class CherishballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<CherishballProj>();
		protected override int BallValue => 1000000;
		protected override float CatchRate => 1f;

        
    }

	public class CherishballProj : BallProj{}
}
