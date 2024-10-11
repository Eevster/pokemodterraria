using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class ConfuseRay : PokemonAttack
	{
		private Vector2 targetPosition;
		bool exploded = false;
		public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";

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

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
            Projectile.Opacity = 0.6f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
			Projectile.hide = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			if(exploded){
				drawPos2 = Projectile.Center + new Vector2(-32f*(float)Math.Cos(6f*MathHelper.Pi*(60f-Projectile.timeLeft)/60f), 32f-64f*(Projectile.timeLeft/60f)) - Main.screenPosition;
			}
			for(int i = 0; i < 20; i++){
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos2, new Color(255, 247, 227, 0) * 0.5f, new Color(255, 195, 35), 0.5f, 0f, 0.5f, 0.5f, 1f, Projectile.rotation+ MathHelper.ToRadians(i*360f/20f) + MathHelper.ToRadians(Main.rand.Next(-8,9)), new Vector2(2f, Utils.Remap(0.5f, 0f, 1f, 4f, 1f)) * Projectile.scale, 2*Vector2.One * Projectile.scale);
            }

            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.05f;

			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				SearchTarget(64f);
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
			}else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				float projSpeed = 10f;
				Projectile.velocity = (Trainer.attackPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			}

            base.OnSpawn(source);
        }

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, Projectile.Opacity*0.3f, Projectile.Opacity, Projectile.Opacity*0.3f);

            if(Projectile.scale < 0.2f){
                Projectile.scale += 0.005f/Projectile.scale;
            }else{
				Projectile.scale = 0.2f;
			}

			if(Projectile.timeLeft < 20){
				Projectile.Opacity = Projectile.timeLeft*0.05f;
			}


			if(!exploded){
				if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
					SearchTarget(64f);

					float projSpeed = 10f;

					if(targetPlayer != null){
						if(targetPlayer.active && !targetPlayer.dead){
							targetPosition = targetPlayer.Center;
							Projectile.velocity += 0.2f*(targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
						}else{
							targetPlayer = null;
						}
					}else if(targetEnemy != null){
						if(targetEnemy.active){
							targetPosition = targetEnemy.Center;
							Projectile.velocity += 0.2f*(targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
						}else{
							targetEnemy = null;
						}
					}

					if(Projectile.velocity.Length() > projSpeed){
						Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
					}
				}
			}else{
				Projectile.velocity = Vector2.Zero;

				if(targetPlayer != null){
					if(targetPlayer.active && !targetPlayer.dead){
						Projectile.Center = targetPlayer.Center;
					}else{
						targetPlayer = null;
					}
				}else if(targetEnemy != null){
					if(targetEnemy.active){
						Projectile.Center = targetEnemy.Center;
					}else{
						targetEnemy = null;
					}
				}
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(!exploded){
                exploded = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 60;
				targetEnemy = target;
				foundTarget = true;
            }
			target.AddBuff(BuffID.Confused, 7*60);

            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if(!exploded){
                exploded = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 60;
				targetPlayer = target;
				foundTarget = true;
            }
			target.AddBuff(BuffID.Confused, 7*60);

            base.OnHitPlayer(target, info);
        }

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if(!exploded || Projectile.timeLeft%20 < 10){
				behindNPCs.Add(index);
			}else{
				overPlayers.Add(index);
			}
        }
    }
}
