using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class PinMissile : PokemonAttack
	{
		private Vector2 targetPosition;
		private bool canfollow = true;
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetPosition);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPosition = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
        }
		public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 70;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
        }

		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.ai[1] != 0){
				Main.instance.LoadProjectile(Projectile.type);
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				// Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++) {
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor);
					color *= 0.5f*(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
					Main.EntitySpriteDraw(texture, drawPos, texture.Bounds, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

        public override void AI()
        {
			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				if(Projectile.ai[1] == 0){
					SearchTarget(300f);
				}

				if(foundTarget){
					if(targetPlayer != null){
						if(targetPlayer.active && !targetPlayer.dead){
							targetPosition = targetPlayer.Center;
						}else{
							targetPlayer = null;
						}
					}else if(targetEnemy != null){
						if(targetEnemy.active){
							targetPosition = targetEnemy.Center;
						}else{
							targetEnemy = null;
						}
					}

					if(Projectile.timeLeft < 60){
						if(Projectile.ai[1] == 0){
							Projectile.timeLeft = 60;
							Projectile.ai[1] = 1;
						}else{
							Projectile.tileCollide = true;
						}
						float projSpeed = 16f;
						if(canfollow){
							Projectile.velocity += 0.08f*(targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
							if(Projectile.velocity.Length() > projSpeed){
								Projectile.velocity = Vector2.Normalize(Projectile.velocity)*projSpeed;
							}
							if(Vector2.Distance(Projectile.Center, targetPosition) < 100){
								Projectile.velocity = projSpeed*Vector2.Normalize(targetPosition-Projectile.Center);
								canfollow = false;
							}
						}
					}
				}
			}else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				targetPosition = Trainer.attackPosition;

				if(Projectile.timeLeft < 60){
					if(Projectile.ai[1] == 0){
						Projectile.timeLeft = 60;
						Projectile.ai[1] = 1;
					}else{
						Projectile.tileCollide = true;
					}
					float projSpeed = 16f;
					if(canfollow){
						Projectile.velocity += 0.08f*(targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
						if(Projectile.velocity.Length() > projSpeed){
							Projectile.velocity = Vector2.Normalize(Projectile.velocity)*projSpeed;
						}
						if(Vector2.Distance(Projectile.Center, targetPosition) < 100){
							Projectile.velocity = projSpeed*Vector2.Normalize(targetPosition-Projectile.Center);
							canfollow = false;
						}
					}
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(15f,0).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center - new Vector2(15f,0).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 24f, ref collisionPoint);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
			fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
