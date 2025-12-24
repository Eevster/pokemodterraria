using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class AncientPower : PokemonAttack
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
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
        }
		public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;
            
            Projectile.knockBack = 10f;

            Projectile.tileCollide = true;  
            Projectile.penetrate = 3;

			Projectile.light = 0.5f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			SoundEngine.PlaySound(SoundID.Item90, pokemon.position);
			if(pokemon.owner == Main.myPlayer){
				int remainingProjs = 4;
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
                        remainingProjs--;
                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<AncientPower>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner, remainingProjs*MathHelper.TwoPi/6)];
						if (remainingProjs <= 0)
						{
							break;
						}
					}
				} 
			}
			pokemonOwner.timer = pokemonOwner.attackDuration;
			pokemonOwner.canAttack = false;
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].ai[1] == 0){
				pokemonOwner.attackProjs[i].Center = pokemon.position + new Vector2(25,23) + 50*new Vector2(-1,0).RotatedBy(pokemonOwner.attackProjs[i].ai[0]);
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].ai[1] != 0){
				pokemonOwner.attackProjs[i] = null;
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			if (Projectile.timeLeft < 100)
			{
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] + (new Vector2(Projectile.width, Projectile.height) / 2f) - Main.screenPosition;
					Color color = Projectile.GetAlpha(lightColor);
					color = Color.Lerp(color, new Color(255, 141, 240), ((float)k + 1f) / (float)Projectile.oldPos.Length);
					color *= (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
					Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 7, 0, Projectile.frame), color, Projectile.rotation, texture.Frame(1, 7).Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
				}
				return false;
			}
			else
			{
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(1, 7, 0, Projectile.frame), lightColor, Projectile.rotation, texture.Frame(1, 7).Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
				return false;
            }
		}

        public override void AI()
        {
			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				if(Projectile.ai[1] == 0){
					SearchTarget(800f);
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

					if(Projectile.timeLeft < 100){
						if(Projectile.ai[1] == 0){
							Projectile.timeLeft = 60;
							Projectile.ai[1] = 1;
						}
						float projSpeed = 10f;
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

				if(Projectile.timeLeft < 100){
					if(Projectile.ai[1] == 0){
						Projectile.timeLeft = 60;
						Projectile.ai[1] = 1;
					}
					float projSpeed = 10f;
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

			if (Projectile.timeLeft > 100){
				UpdateAnimation();
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
					Projectile.frame = Main.projFrames[Projectile.type];
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);

            for (int i = 0; i < 40; i++)
            {
                int dust = Dust.NewDust(Projectile.Center - new Vector2(8 * Projectile.scale, 8 * Projectile.scale), (int)(16 * Projectile.scale), (int)(16 * Projectile.scale), DustID.Dirt, Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-7, 6), 50, default(Color), 1f);
                Main.dust[dust].noGravity = true;
            }

            base.OnKill(timeLeft);
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(14f,0);
			Vector2 end = Projectile.Center - new Vector2(14f,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 24f, ref collisionPoint);
		}
    }
}
