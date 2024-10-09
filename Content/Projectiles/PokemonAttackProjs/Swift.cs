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
	public class Swift : PokemonAttack
	{
		private NPC targetEnemy;
		private Vector2 targetPosition;
		private bool foundTarget = false;
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
            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
        }

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor);
				color = Color.Lerp(color, new Color(239, 141, 255), ((float)k+1f)/(float)Projectile.oldPos.Length);
				color *= (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
				Main.EntitySpriteDraw(texture, drawPos, texture.Bounds, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void AI()
        {
			Projectile.ai[0] += MathHelper.ToRadians(10);

			PokemonPlayer trainer = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>();

			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				if(Projectile.ai[1] == 0){
					SearchTarget();
				}

				if(foundTarget){
					if(targetEnemy.active){
						targetPosition = targetEnemy.Center;
					}

					if(Projectile.timeLeft < 100){
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
				targetPosition = trainer.attackPosition;

				if(Projectile.timeLeft < 100){
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

			if(Projectile.ai[1] == 1){
				Projectile.rotation += MathHelper.ToRadians(10);
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		private void SearchTarget(){
			float distanceFromTarget = 300f;
			Vector2 targetCenter = Projectile.Center;

			if (true) {
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;

						if(npc.boss){
							foundTarget = true;
							targetEnemy = npc;
							break;
						}

						if (((closest && inRange) || !foundTarget) && !(npc.GetGlobalNPC<PokemonNPCData>().isPokemon && npc.GetGlobalNPC<PokemonNPCData>().shiny)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
							targetEnemy = npc;
						}
					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(12f,0);
			Vector2 end = Projectile.Center - new Vector2(12f,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 24f, ref collisionPoint);
		}
    }
}
