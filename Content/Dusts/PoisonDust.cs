using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Dusts
{
	public class PoisonDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = true; // Makes the dust emit no light.
			dust.frame = new Rectangle(0, 0, 18, 18);

			dust.scale = Main.rand.NextFloat(0.5f,1f);
			dust.velocity = -Main.rand.NextFloat(0.5f,1.2f)*Vector2.UnitY;
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.scale += 0.01f;

			if (dust.scale > 1f) {
				dust.alpha += 10;
			}

			if(dust.alpha > 200)
			{
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}
