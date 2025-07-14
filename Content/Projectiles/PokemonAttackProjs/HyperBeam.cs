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
	public class HyperBeam : PokemonAttack
	{
		Vector2 enemyCenter;
        float maxLenght = 1500;
		
		private static Asset<Texture2D> chainTexture;
        private static Asset<Texture2D> effectTexture;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void Load()
        { 
            chainTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/HyperBeamRay");
            effectTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/FocusPunch");
        }

        public override void Unload()
        { 
            chainTexture = null;
            effectTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 70;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

			Projectile.light = 1f;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
                        Vector2 positionOffset = 0.4f * Vector2.UnitX * pokemon.width * pokemon.spriteDirection;

                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center + positionOffset, Vector2.Normalize(targetCenter - (pokemon.Center + positionOffset)), ModContent.ProjectileType<HyperBeam>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 4f, pokemon.owner, pokemon.spriteDirection)];
                        SoundEngine.PlaySound(SoundID.Item68 with { Pitch = -0.8f }, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            Vector2 positionOffset = 0.4f * Vector2.UnitX * pokemon.width * pokemon.spriteDirection + 0.05f * Vector2.UnitY * pokemon.height;

            pokemonOwner.attackProjs[i].Center = pokemon.Center + positionOffset;
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            Vector2 positionOffset = 0.4f * Vector2.UnitX * pokemon.width * pokemon.spriteDirection + 0.05f * Vector2.UnitY * pokemon.height;

            pokemonOwner.attackProjs[i].Center = pokemon.Center + positionOffset;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 1f;

            enemyCenter = Projectile.Center + maxLenght*Projectile.velocity;
            Projectile.velocity = Vector2.Zero;

            base.OnSpawn(source);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(effectTexture.Value, Projectile.Center - Main.screenPosition,
                effectTexture.Frame(1, 5, 0, Projectile.frame), Color.White, 0f,
                effectTexture.Frame(1, 5).Size() / 2f, (float)Math.Pow(Projectile.scale, 2), SpriteEffects.None, 0);

            if (Projectile.timeLeft > 35)
            {
                Vector2 center = Projectile.Center;
                Vector2 directionToOrigin = enemyCenter - Projectile.Center;
                float chainScale = (105f - Projectile.timeLeft) / 70f;
                if (chainScale <= 0.1f) { chainScale = 0.1f; }
                if (chainScale > 1f) { chainScale = 1f; }

                float distanceToOrigin = directionToOrigin.Length();

                //Beam Head
                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                       chainTexture.Frame(1, 3, 0, 0), Color.White, directionToOrigin.ToRotation() - MathHelper.PiOver2,
                       chainTexture.Frame(1, 3).Size() / 2f, chainScale, SpriteEffects.None, 0);

                while (distanceToOrigin > chainTexture.Width() * chainScale && !float.IsNaN(distanceToOrigin))
                {
                    directionToOrigin /= distanceToOrigin;
                    directionToOrigin *= chainTexture.Width() * chainScale;

                    center += directionToOrigin;
                    directionToOrigin = enemyCenter - center;
                    distanceToOrigin = directionToOrigin.Length();

                    //Beam Tail
                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Frame(1, 3, 0, (distanceToOrigin > chainTexture.Width()) ? 1 : 2), Color.White, directionToOrigin.ToRotation() - MathHelper.PiOver2,
                        chainTexture.Frame(1, 3).Size() / 2f, chainScale, SpriteEffects.None, 0);
                }
                return false;
            }
            return true;
        }

        public override void AI()
        {            
            PokemonPlayer trainer = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>();

            if (Main.rand.Next(0, 4) == 0)
            {
                int dust = Dust.NewDust(Projectile.Center, 2, 2, DustID.Firework_Red, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), default, default, 0.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
            }

            if (Projectile.timeLeft <= 35){
                Projectile.scale -= 0.028f;
            }else{
                if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
                    SearchTarget(1000f, true);

                    if(targetPlayer != null){
                        if(targetPlayer.active && !targetPlayer.dead){
                            Vector2 directionToCenter = Projectile.Center + maxLenght*Vector2.Normalize(targetPlayer.Center - Projectile.Center)-enemyCenter;
                            if(directionToCenter.Length()>float.Epsilon){
                                directionToCenter = Math.Clamp(directionToCenter.Length(), 0f, 16f)*Vector2.Normalize(directionToCenter);
                            }
                            enemyCenter += directionToCenter;
                        }else{
                            targetPlayer = null;
                        }
                    }else if(targetEnemy != null){
                        if(targetEnemy.active){
                            Vector2 directionToCenter = Projectile.Center + maxLenght*Vector2.Normalize(targetEnemy.Center - Projectile.Center)-enemyCenter;
                            if(directionToCenter.Length()>float.Epsilon){
                                directionToCenter = Math.Clamp(directionToCenter.Length(), 0f, 16f)*Vector2.Normalize(directionToCenter);
                            }
                            enemyCenter += directionToCenter;
                        }else{
                            targetEnemy = null;
                        }
                    }
                }else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
                    Vector2 directionToCenter = Projectile.Center + maxLenght*Vector2.Normalize(Trainer.attackPosition - Projectile.Center)-enemyCenter;
                    if(directionToCenter.Length()>float.Epsilon){
                        directionToCenter = Math.Clamp(directionToCenter.Length(), 0f, 16f)*Vector2.Normalize(directionToCenter);
                    }
                    enemyCenter += directionToCenter;
                    foundTarget = true;
                }
            }

            UpdateAnimation();

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center;
			Vector2 end = (Projectile.timeLeft >= 35 && foundTarget)?enemyCenter:Projectile.Center;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 26f, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
