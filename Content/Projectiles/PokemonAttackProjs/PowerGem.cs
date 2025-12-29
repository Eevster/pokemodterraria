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
	public class PowerGem : PokemonAttack
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
			Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
        }
		public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						pokemonOwner.canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer >= 40 && pokemonOwner.timer%5==0){
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
							SoundEngine.PlaySound(SoundID.Item39, Projectile.position);
							pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, -10f*Vector2.UnitY.RotatedByRandom(MathHelper.Pi), ModContent.ProjectileType<PowerGem>(), (int)(0.5f*pokemonOwner.GetPokemonAttackDamage(GetType().Name)), 2f, pokemon.owner)];
							break;
						}
					} 
				}
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.ai[1] != 0){
				Main.instance.LoadProjectile(Projectile.type);
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				// Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = new Vector2(texture.Frame(1,8,0,7).Width * 0.5f, texture.Frame(1,8,0,7).Height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++) {
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor);
					color *= (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
					Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1,8,0,7), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

        public override void AI()
        {
			if(Projectile.ai[1]==0){
				Projectile.rotation = Vector2.Distance(Vector2.Zero, Projectile.velocity);
				Projectile.velocity *= 0.9f;
			}

			Lighting.AddLight(Projectile.Center, new Vector3(1,0.5f,0.5f));

			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				if(Projectile.ai[1] == 0){
					SearchTarget(500f);
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
							SoundEngine.PlaySound(SoundID.Item69, Projectile.position);
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
						SoundEngine.PlaySound(SoundID.Item69, Projectile.position);
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

			UpdateAnimation();

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame <  Main.projFrames[Projectile.type]-1)
                {
                    Projectile.frame++;
                }
            }
        }
    }
}
