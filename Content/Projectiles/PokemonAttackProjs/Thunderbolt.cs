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
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Thunderbolt : PokemonAttack
	{
        public override bool CanExistIfNotActualMove => false;
        Vector2 targetPosition;
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
		
		private static Asset<Texture2D> chainTexture;
        
        public override void Load()
        { 
            chainTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderboltChain");
        }

        public override void Unload()
        { 
            chainTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 30;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;
            Projectile.Opacity = 0.6f;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
			SoundEngine.PlaySound(SoundID.Item94, pokemon.position);
			if(pokemon.owner == Main.myPlayer){
				pokemonOwner.attackProjs[0] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Thunderbolt>(), pokemonOwner.GetPokemonDamage(90, true), 2f, pokemon.owner, targetCenter.X, targetCenter.Y)];
			}
			pokemonOwner.timer = pokemonOwner.attackDuration;
			pokemonOwner.canAttack = false;
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			pokemon.velocity.X *= 0.9f;
			maxFallSpeed = 2f;
			pokemonOwner.attackProjs[0].Center = pokemon.Center;
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			pokemonOwner.attackProjs[0].Kill();
		}

        public override bool PreDrawExtras()
        {
            if(foundTarget){
                Vector2 center = Projectile.Center;
                if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack && targetEnemy != null) targetPosition = targetEnemy.Center;

                Vector2 directionToOrigin = targetPosition - Projectile.Center;

                float distanceToOrigin = directionToOrigin.Length();

                while (distanceToOrigin > chainTexture.Width() && !float.IsNaN(distanceToOrigin))
                {
                    directionToOrigin /= distanceToOrigin; 
                    directionToOrigin *= chainTexture.Width(); 

                    center += directionToOrigin; 
                    directionToOrigin = targetPosition - center; 
                    distanceToOrigin = directionToOrigin.Length();

                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Frame(1, 3, 0, Projectile.frame), Color.White * Projectile.Opacity, directionToOrigin.ToRotation(),
                        chainTexture.Frame(1, 3).Size() / 2f, 1f, SpriteEffects.None, 0);
                }

                var AttackTexture = ModContent.Request<Texture2D>(Texture);

                Main.EntitySpriteDraw(AttackTexture.Value, targetPosition - Main.screenPosition,
                        AttackTexture.Frame(1, 3, 0, Projectile.frame), Color.White * Projectile.Opacity, directionToOrigin.ToRotation(),
                        AttackTexture.Frame(1, 3).Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            
            return false;
        }

        public override void AI()
        {
            if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
                if (foundTarget)
                {
                    SearchTargetFromPoint(targetPosition, 64f, false);
                }
                else targetPosition = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                foundTarget = true;
            }else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
                targetPosition = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>().attackPosition;
                foundTarget = true;
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
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool? CanDamage()
        {
            return Projectile.timeLeft <= 25 && !inPokemonBattle;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(24,0);
			Vector2 end = Projectile.Center - new Vector2(24,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			if(!foundTarget){
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 48f, ref collisionPoint);
			}else{
                if(targetEnemy != null) targetPosition = targetEnemy.Center;
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 48f, ref collisionPoint) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, targetPosition, 32f, ref collisionPoint);
			}
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
