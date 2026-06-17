using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Dusts
{
	public class StarDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = true; // Makes the dust emit no light.
			dust.frame = new Rectangle(0, 0, 22, 22);

			dust.rotation = MathHelper.ToRadians(Main.rand.NextFloat(-180,180));
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.position += dust.velocity;
			dust.scale -= 0.01f;
			dust.velocity *= 0.9f;

			if (dust.scale < 0.5f) {
				dust.alpha += 5;
			}

			if(dust.alpha > 200)
			{
				dust.active = false;
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}
