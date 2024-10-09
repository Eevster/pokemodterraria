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
	public class Confusion : PokemonAttack
	{
		NPC targetEnemy;
		Player targetPlayer;
		private Vector2 targetPosition;

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
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
			Projectile.light = 0.7f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Asset<Texture2D> Texture = TextureAssets.Projectile[Projectile.type];
            Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			Color color = Projectile.GetAlpha(lightColor);

			float freq = 30f;
			int maxTime = 80;

			for(int i = 0; i < 3; i++){
                if(maxTime-Projectile.timeLeft > (freq*i/3)){
					float scale = 1f*((maxTime-Projectile.timeLeft-(freq*i/3))%(int)freq)/freq;

					Main.EntitySpriteDraw(Texture.Value, drawPos2,
                    Texture.Value.Bounds, color*(1f-(scale*scale)), 0,
                    Texture.Size() / 2f, 2f*scale, SpriteEffects.None, 0);
				}
            }

            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
			SearchTarget();

            base.OnSpawn(source);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

			Lighting.AddLight(Projectile.Center, Projectile.Opacity, Projectile.Opacity, Projectile.Opacity*0.3f);

			if(Projectile.timeLeft < 20){
				Projectile.Opacity = Projectile.timeLeft*0.05f;
			}

			if(targetEnemy != null || targetPlayer != null){
				if(targetEnemy != null){
					if(targetEnemy.active){
						targetPosition = targetEnemy.Center;
					}else{
						targetEnemy = null;
					}
				}
				if(targetPlayer != null){
					if(targetPlayer.active && !targetPlayer.dead){
						targetPosition = targetPlayer.Center;
					}else{
						targetPlayer = null;
					}
				}
				if(targetEnemy != null || targetPlayer != null){
					Projectile.Center = targetPosition;
				}
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		private void SearchTarget(){
			float distanceFromTarget = 16f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;

			if (true) {
				float sqrMaxDetectDistance = distanceFromTarget*distanceFromTarget;
				for (int k = 0; k < Main.maxPlayers; k++) {
					if(Main.player[k] != null){
						Player target = Main.player[k];
						if(target.whoAmI != Projectile.owner){
							if(target.active && !target.dead){
								float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

								// Check if it is within the radius
								if (sqrDistanceToTarget < sqrMaxDetectDistance) {
									if(target.hostile){
										sqrMaxDetectDistance = sqrDistanceToTarget;
										targetPlayer = target;
									}
								}
							}
						}
					}
				}
				// This code is required either way, used for finding a target
				if(targetPlayer == null){
					for (int i = 0; i < Main.maxNPCs; i++) {
						NPC npc = Main.npc[i];

						if (npc.CanBeChasedBy()) {
							float between = Vector2.Distance(npc.Center, Projectile.Center);
							bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
							bool inRange = between < distanceFromTarget;

							if(npc.boss){
								targetEnemy = npc;
								break;
							}

							if ((closest && inRange) || !foundTarget) {
								foundTarget = true;
								distanceFromTarget = between;
								targetCenter = npc.Center;
								targetEnemy = npc;
							}
						}
					}
				}
			}
		}

		public override bool? CanHitNPC(NPC target)
        {
			if(targetEnemy != null){
				if(targetEnemy.active){
					return target.whoAmI == targetEnemy.whoAmI;
				}
			}
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
			if(targetPlayer != null){
				if(targetPlayer.active){
					return target.whoAmI == targetPlayer.whoAmI;
				}
			}
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.CanBeChasedBy()){
                target.AddBuff(BuffID.Confused, 2*60);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Confused, 60);

            base.OnHitPlayer(target, info);
        }

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
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
    }
}
