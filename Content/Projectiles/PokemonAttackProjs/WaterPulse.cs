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
	public class WaterPulse : PokemonAttack
	{
		private NPC targetEnemy;
		private bool foundTarget = false;
		bool exploded = false;
        private static Asset<Texture2D> explosionTexture;
        
        public override void Load()
        { 
            explosionTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/WaterPulseExplosion");
        }

        public override void Unload()
        { 
            explosionTexture = null;
        }

		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(exploded);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            exploded = reader.ReadBoolean();
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

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
        }

		public override bool PreDraw(ref Color lightColor) {
			if(exploded){
                Main.EntitySpriteDraw(explosionTexture.Value, Projectile.Center - Main.screenPosition,
                    explosionTexture.Value.Bounds, lightColor*Projectile.Opacity, 0,
                    explosionTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

                return false;
            }else{
				Main.instance.LoadProjectile(Projectile.type);
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				// Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++) {
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

        public override void AI()
        {	
			if(!exploded){
				Projectile.rotation = Projectile.velocity.ToRotation();

				if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
					SearchTarget();
				}

				if(foundTarget){
					if(targetEnemy.active){
						float projSpeed = 10f;
						Projectile.velocity =  (targetEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
					}
				}
			}else{
				Projectile.velocity = Vector2.Zero;
				Projectile.rotation = 0;
				Projectile.scale += 0.06f;

				if(foundTarget){
					if(targetEnemy.active){
						Projectile.Center = targetEnemy.Center;
					}
				}

				if(Projectile.timeLeft < 10){
					Projectile.Opacity = 0.1f*Projectile.timeLeft;
				}
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(exploded?32:11,0);
			Vector2 end = Projectile.Center - Projectile.scale*new Vector2(exploded?32:11,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*(exploded?64f:22f), ref collisionPoint);
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(!exploded){
                exploded = true;
                Projectile.frame = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale = 0.1f;
                Projectile.timeLeft = 30;
            }
			target.AddBuff(BuffID.Confused, 60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if(!exploded){
                exploded = true;
                Projectile.frame = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale = 0.1f;
                Projectile.timeLeft = 20;
            }
			target.AddBuff(BuffID.Confused, 60);
            base.OnHitPlayer(target, info);
        }

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center-new Vector2(8*Projectile.scale, 8*Projectile.scale), (int)(16*Projectile.scale), (int)(16*Projectile.scale), DustID.Water, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3), 100, default(Color), 2f);
            }
        }

        private void SearchTarget(){
			float distanceFromTarget = 300f;
			Vector2 targetCenter = Projectile.Center;

			foundTarget = false;

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
    }
}
