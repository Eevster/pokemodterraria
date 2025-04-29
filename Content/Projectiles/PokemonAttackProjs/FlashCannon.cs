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
	public class FlashCannon : PokemonAttack
	{
		const float projSpeed = 20f;

		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((double)Projectile.scale);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.scale = (float)reader.ReadDouble();
            base.ReceiveExtraAI(reader);
        }

		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
			Main.projFrames[Projectile.type] = 2;
        }
		public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 50;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 5;

			Projectile.tileCollide = false;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<FlashCannon>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 4f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item43, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].frame == 0){
				pokemonOwner.attackProjs[i].Center = pokemon.Center;
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].frame == 0){
				pokemonOwner.attackProjs[i].Center = pokemon.Center;
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.frame == 1){
				Main.instance.LoadProjectile(Projectile.type);
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				// Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = texture.Frame(1, 2).Size() / 2f;
				for (int k = 0; k < Projectile.oldPos.Length; k++) {
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Color.Lerp(Projectile.GetAlpha(lightColor), new Color(100, 100, 100), (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)*((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 2, 0, 1), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

        public override void OnSpawn(IEntitySource source)
        {
			Projectile.scale = 0.1f;

            base.OnSpawn(source);
        }

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, new Vector3(0.5f,0.5f,0.5f));

			if(Projectile.frame == 0){
				for (int i = 0; i < 3; i++)
				{
					int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 100, default(Color), 1f);
					Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(3) * 0.2f;
					Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].position = Projectile.Center + new Vector2(32,0).RotatedByRandom(MathHelper.TwoPi);
					Main.dust[dustIndex].velocity = 0.2f*(Projectile.Center-Main.dust[dustIndex].position);
				}

				if(Projectile.scale >= 1f){
					if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
						SearchTarget(1000f);

						if(targetPlayer != null){
							if(targetPlayer.active && !targetPlayer.dead){
								ShootProj(targetPlayer.Center);
							}else{
								targetPlayer = null;
							}
						}else if(targetEnemy != null){
							if(targetEnemy.active){
								ShootProj(targetEnemy.Center);
							}
						}
					}else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
						ShootProj(Trainer.attackPosition);
					}
				}else{
					Projectile.scale += 0.025f;
				}
			}else{
				Projectile.rotation = Projectile.velocity.ToRotation();

				if(Projectile.timeLeft%3==0){
					for (int i = 0; i < 30; i++)
					{
						int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 100, default(Color), 1f);
						Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(3) * 0.2f;
						Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
						Main.dust[dustIndex].noGravity = true;
						Main.dust[dustIndex].position = Projectile.Center + new Vector2((float)(20*Math.Cos(MathHelper.ToRadians(12*i))),(float)(40*Math.Sin(MathHelper.ToRadians(12*i)))).RotatedBy(Projectile.rotation);
						Main.dust[dustIndex].velocity = 0.05f*(Main.dust[dustIndex].position - Projectile.Center);
					}
				}
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		private void ShootProj(Vector2 targetCenter){
			Projectile.velocity = projSpeed*Vector2.Normalize(targetCenter - Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item96, Projectile.position);
			Projectile.timeLeft = 60;
			Projectile.frame = 1;
		}

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item67, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center-new Vector2(8*Projectile.scale, 8*Projectile.scale), (int)(16*Projectile.scale), (int)(16*Projectile.scale), DustID.WhiteTorch, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3), 100, default(Color), 2f);
            }
        }
    }
}
