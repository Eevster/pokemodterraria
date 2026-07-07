using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.Dusts
{
	public class SleepDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true; // Makes the dust have no gravity.
			dust.noLight = true; // Makes the dust emit no light.
			dust.frame = new Rectangle(0, 0, 18, 18);

			dust.velocity = -1.2f*Vector2.UnitY;
		}

		public override bool Update(Dust dust) { // Calls every frame the dust is active
			dust.scale += 0.01f;

			if (dust.scale > 0.3f)
			{
				dust.position += dust.velocity;
				dust.velocity.X += 0.02f;

				if(Math.Abs(dust.velocity.X) >= 0.5f)
				{
					dust.alpha += 10;
				}

				if(dust.alpha > 200)
				{
					dust.active = false;
				}
			}

			return false; // Return false to prevent vanilla behavior.
		}
	}
}
