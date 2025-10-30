using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class MudShot : PokemonAttack
	{
		private int bounceCounter = 0;

        public override void SetDefaults()
        {

            Projectile.timeLeft = 60;
			
			Projectile.width = 30;
            Projectile.height = 30;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;  
            Projectile.penetrate = 6;

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
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20){
					int remainProjs = 1;
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
							float shootSpeed = 20f;
                            Vector2 shootVelocity = shootSpeed * Vector2.Normalize(targetCenter - pokemon.Center);

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<MudShot>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 5f, pokemon.owner)];
							SoundEngine.PlaySound(SoundID.Item21 with { Pitch = -0.5f }, pokemon.position);
							remainProjs--;
							pokemonOwner.canAttackOutTimer = false;
							if(remainProjs <= 0){
								break;
							}
						}
					} 
				}
			}
		}



        public override void AI()
        {
            //Rolling
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);

            DustTrail(Projectile.velocity);

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            DustBomb(Projectile.velocity, target.Center);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            DustBomb(Projectile.velocity, target.Center);
            base.OnHitPlayer(target, info);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
			fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			DustBomb(oldVelocity, Projectile.Center);
			base.OnTileCollide(oldVelocity);
			return true;
		}

        private void DustTrail(Vector2 velocity)
        {
			switch (Main.rand.Next(0, 6))
			{
				case 0: 
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, 0, 0, default, default, 1.5f); break;
				case 1: 
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, velocity.X * 0.3f, velocity.Y * 0.3f);
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, velocity.X * 0.5f, velocity.Y * 0.5f); break;
                case 2: 
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, velocity.X * 0.4f, velocity.Y * 0.4f); break;
                case 3: 
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, velocity.X * 0.2f, velocity.Y * 0.2f, default, default, 1.5f);
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, velocity.X * 0.2f, velocity.Y * 0.2f, default, default, 1.5f); break;
				case 4: 
                    SoundEngine.PlaySound(SoundID.SplashWeak with { Pitch = -0.5f }, Projectile.position); break;
            }
        }

        private void DustBomb(Vector2 velocity, Vector2 targetPosition)
        {   
            for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Mud, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), default, default, 1.5f);
                Dust.NewDust(targetPosition, Projectile.width, Projectile.height, DustID.Mud, Main.rand.Next(-2, 3) + velocity.X * 0.1f, Main.rand.Next(-2, 3) + velocity.Y * 0.1f, default, default, 1f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            base.OnKill(timeLeft);
        }
    }
}
