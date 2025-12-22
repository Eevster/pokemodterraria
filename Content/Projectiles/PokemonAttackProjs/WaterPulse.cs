using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
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
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 10f*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<WaterPulse>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item21, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
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
				Projectile.scale += 0.06f;

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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 4;
            height = 4;
            fallThrough = true;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
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
    }
}
