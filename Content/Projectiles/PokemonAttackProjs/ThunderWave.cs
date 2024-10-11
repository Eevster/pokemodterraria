using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Buffs;
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
	public class ThunderWave : PokemonAttack
	{
		bool exploded = false;
        private static Asset<Texture2D> explosionTexture;
        
        public override void Load()
        { 
            explosionTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderWaveExplosion");
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

            Projectile.tileCollide = true;  
            Projectile.penetrate = 1;

			Projectile.light = 1f;

			Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

		public override bool PreDraw(ref Color lightColor) {
			if(exploded){
				float freq = 18f;

				for(int i = 0; i < 3; i++){
					float scale = 1f*((Projectile.timeLeft-(freq*i/3))%(int)freq)/freq;
						
					Main.EntitySpriteDraw(explosionTexture.Value, Projectile.Center - Main.screenPosition,
						explosionTexture.Frame(1, 4, 0, Projectile.frame), Color.White*(1f-(scale*scale)), 0,
						explosionTexture.Frame(1, 4).Size() / 2f, 2f*scale, SpriteEffects.None, 0);
				}

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
					SearchTarget(300f, false);
				}else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
					if(Trainer.targetPlayer != null){
						targetPlayer = Trainer.targetPlayer;
						foundTarget = true;
					}else if(Trainer.targetNPC != null){
						targetEnemy = Trainer.targetNPC;
						foundTarget = true;
					}
				}

				if(foundTarget){
					float projSpeed = 10f;
					if(targetPlayer != null){
						if(targetPlayer.active && !targetPlayer.dead){
							Projectile.velocity =  (targetPlayer.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
						}else{
							targetPlayer = null;
						}
					}else if(targetEnemy != null){
						if(targetEnemy.active){
							Projectile.velocity =  (targetEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
						}else{
							targetEnemy = null;
						}
					}
				}
			}else{
				Projectile.velocity = Vector2.Zero;
				Projectile.rotation = 0;

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
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(11,0);
			Vector2 end = Projectile.Center - Projectile.scale*new Vector2(11,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*22f, ref collisionPoint);
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(!exploded){
                exploded = true;
                Projectile.frame = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 30;
            }
			target.AddBuff(ModContent.BuffType<ParalizedDebuff>(), (target.boss?2:3)*60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if(!exploded){
                exploded = true;
                Projectile.frame = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 30;
            }
			target.AddBuff(ModContent.BuffType<ParalizedDebuff>(), 2*60);
            base.OnHitPlayer(target, info);
        }

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item94, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center-new Vector2(8*Projectile.scale, 8*Projectile.scale), (int)(16*Projectile.scale), (int)(16*Projectile.scale), DustID.YellowStarDust, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3), 100, default(Color), 2f);
            }
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
