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
	public class MagicalLeaf : PokemonAttack
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
            Projectile.width = 28;
            Projectile.height = 28;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.light = 1f;

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
					color *= (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
					Main.EntitySpriteDraw(texture, drawPos, texture.Bounds, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Rainbow(Projectile.timeLeft/20f)*Projectile.Opacity;
        }

		public Color Rainbow(float progress)
		{
			float div = (Math.Abs(progress % 1) * 6);
			int ascending = (int) ((div % 1) * 255);
			int descending = 255 - ascending;

			switch ((int) div)
			{
				case 0:
					return new Color(255, ascending, 0);
				case 1:
					return new Color(descending, 255, 0);
				case 2:
					return new Color(0, 255, ascending);
				case 3:
					return new Color(0, descending, 255);
				case 4:
					return new Color(ascending, 0, 255);
				default: // case 5:
					return new Color(255, 0, descending);
			}
		}

        public override void AI()
        {
			if(Projectile.ai[1]==0){
				if(Projectile.timeLeft < 115){
					float gravityAmount = (float)Math.Cos(MathHelper.ToRadians(((120-Projectile.timeLeft)%40)*9));
					Projectile.velocity = new Vector2(5*Math.Sign(gravityAmount),1f-gravityAmount);
				}
			}

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
						}
						float projSpeed = 16f;
						if(canfollow){
							Projectile.velocity =  (targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
							if(Vector2.Distance(Projectile.Center, targetPosition) < 3*projSpeed){
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
					}
					float projSpeed = 16f;
					if(canfollow){
						Projectile.velocity =  (targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
						if(Vector2.Distance(Projectile.Center, targetPosition) < 3*projSpeed){
							canfollow = false;
						}
					}
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver4;

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(15f,0);
			Vector2 end = Projectile.Center - new Vector2(15f,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 30f, ref collisionPoint);
		}
    }
}
