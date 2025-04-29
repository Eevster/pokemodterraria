using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
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
	public class VineWhip : PokemonAttack
	{
		private bool canDamage = true;
		private int projDirection = 1;
		Vector2 vectorToTarget;
		Vector2 positionAjust;
		float positionRotation;
		public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(positionAux);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			positionAux = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
        }
		public override void SetDefaults()
        {
            Projectile.width = 156;
            Projectile.height = 76;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.attackProjs[0] == null || pokemonOwner.attackProjs[1] == null){
					pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
					SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
					if(pokemonOwner.attackProjs[0] == null){
						pokemonOwner.attackProjs[0] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<VineWhip>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner, 0, Math.Sign((targetCenter - pokemon.Center).X))];
					}
					if(pokemonOwner.attackProjs[1] == null){
						pokemonOwner.attackProjs[1] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<VineWhip_Front>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner, 0, Math.Sign((targetCenter - pokemon.Center).X))];
					}
				}
			}
			pokemonOwner.timer = pokemonOwner.attackDuration;
			pokemonOwner.canAttack = false;
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].ModProjectile is PokemonAttack){
				((PokemonAttack)pokemonOwner.attackProjs[i]?.ModProjectile).positionAux = pokemon.Center;
			}
			pokemonOwner.attackProjs[i].netUpdate = true;
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			pokemonOwner.attackProjs[i].Kill();
		}

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.Opacity = 0;
			canDamage = false;
			Projectile.hide = true;
			projDirection = (int)Projectile.ai[1];

			vectorToTarget = new Vector2(projDirection,0);
			positionRotation = vectorToTarget.ToRotation();
			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);

			base.OnSpawn(source);
        }

        public override void AI()
        {
			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				SearchTarget(160f);
			}

			Projectile.timeLeft = 10;

			Projectile.direction = Projectile.spriteDirection = projDirection;

			if(foundTarget){
				if(targetPlayer != null){
					if(targetPlayer.active && !targetPlayer.dead){
						vectorToTarget = targetPlayer.Center-Projectile.Center;
					}else{
						targetPlayer = null;
					}
				}else if(targetEnemy != null){
					if(targetEnemy.active){
						vectorToTarget = targetEnemy.Center-Projectile.Center;
					}else{
						targetEnemy = null;
					}
				}
			}
			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				vectorToTarget = Trainer.attackPosition-Projectile.Center;
			}

			UpdateAnimation();

			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);

			Projectile.Center = positionAux+positionAjust;

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }
	
        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }

				if(Projectile.Opacity != 0){
					if(Projectile.frame <= 1 || Projectile.frame >= 6){
						canDamage = true;
					}else{
						canDamage = false;
					}
				}

				if(Projectile.frame == 3){
					if(Projectile.Opacity == 0){
						Projectile.Opacity = 1f;
					}
					if(vectorToTarget.Length() > 20f){
						if(vectorToTarget.X>0){
							Projectile.rotation = vectorToTarget.ToRotation();
							Projectile.direction = Projectile.spriteDirection = projDirection = 1;
						}
						if(vectorToTarget.X<0){
							Projectile.rotation = (float)Math.PI + vectorToTarget.ToRotation();
							Projectile.direction = Projectile.spriteDirection = projDirection = -1;
						}
						positionRotation = vectorToTarget.ToRotation();
					}
				}
            }
        }

        public override bool? CanDamage()
        {
            return canDamage;
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center - new Vector2(78,0).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + new Vector2(78,0).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 76, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }

	public class VineWhip_Front : PokemonAttack
	{
		private bool canDamage = true;
		private int projDirection = 1;
		Vector2 vectorToTarget;
		Vector2 positionAjust;
		float positionRotation;
		public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(positionAux);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			positionAux = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
        }
		public override void SetDefaults()
        {
            Projectile.width = 156;
            Projectile.height = 76;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
			projDirection = (int)Projectile.ai[1];
			vectorToTarget = new Vector2(projDirection,0);
        	positionRotation = vectorToTarget.ToRotation();
			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);

			base.OnSpawn(source);
        }

        public override void AI()
        {
			if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
				SearchTarget(160f);
			}

			Projectile.timeLeft = 10;

			Projectile.direction = Projectile.spriteDirection = projDirection;

			if(foundTarget){
				if(targetPlayer != null){
					if(targetPlayer.active && !targetPlayer.dead){
						vectorToTarget = targetPlayer.Center-Projectile.Center;
					}else{
						targetPlayer = null;
					}
				}else if(targetEnemy != null){
					if(targetEnemy.active){
						vectorToTarget = targetEnemy.Center-Projectile.Center;
					}else{
						targetEnemy = null;
					}
				}
			}
			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				vectorToTarget = Trainer.attackPosition-Projectile.Center;
			}

			UpdateAnimation();

			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);

			Projectile.Center = positionAux+positionAjust;

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }
	
        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }

				if(Projectile.Opacity != 0){
					if(Projectile.frame >= 2 && Projectile.frame <= 6){
						canDamage = true;
					}else{
						canDamage = false;
					}
				}

				if(Projectile.frame == 8){
					if(vectorToTarget.Length() > 20f){
						if(vectorToTarget.X>0){
							Projectile.rotation = vectorToTarget.ToRotation();
							Projectile.direction = Projectile.spriteDirection = projDirection = 1;
						}
						if(vectorToTarget.X<0){
							Projectile.rotation = (float)Math.PI + vectorToTarget.ToRotation();
							Projectile.direction = Projectile.spriteDirection = projDirection = -1;
						}
						positionRotation = vectorToTarget.ToRotation();
					}
				}
            }
        }

        public override bool? CanDamage()
        {
            return canDamage;
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center - new Vector2(78,0).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + new Vector2(78,0).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 76, ref collisionPoint);
		}
    }
}
