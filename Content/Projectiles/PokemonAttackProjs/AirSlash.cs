using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class AirSlash : PokemonAttack
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

		public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 112;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 40;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.Opacity = 0.6f;

            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(45)).ToRotation();
            base.OnSpawn(source);
        }

        public override void AI()
        {
            if(Projectile.timeLeft < 10){
                Projectile.Opacity = 0.6f*Projectile.timeLeft*0.1f;
            }

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center - 56*new Vector2(0,-1).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + 56*new Vector2(0,-1).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 32f, ref collisionPoint);
		}
    }
}
