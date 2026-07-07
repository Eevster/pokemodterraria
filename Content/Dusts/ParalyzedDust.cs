using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Dusts
{
	public class ParalyzedDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = true; // Makes the dust emit no light.
			dust.frame = new Rectangle(0, 0, 18, 18);

			dust.velocity = Main.rand.NextFloat(1f,2f)*Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.rotation = dust.velocity.ToRotation() + MathHelper.PiOver4;

			dust.alpha += 5;

			if(dust.alpha > 200)
			{
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}
