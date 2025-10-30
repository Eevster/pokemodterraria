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
	public class MudSlap : PokemonAttack
	{
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MudShot";
        public override void SetDefaults()
        {

            Projectile.timeLeft = 60;
			
			Projectile.width = 30;
            Projectile.height = 30;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
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
							float shootSpeed = 10f;
							float timeToTarget = 0;
                            Vector2 shootVelocity = shootSpeed * Vector2.Normalize(targetCenter - pokemon.Center);
							if (shootVelocity.X != 0f)
							{
                                timeToTarget = (targetCenter - pokemon.Center).X / shootVelocity.X;
							}
							if (timeToTarget > pokemonOwner.distanceToAttack / shootSpeed || timeToTarget == 0)
							{
								timeToTarget = pokemonOwner.distanceToAttack / shootSpeed;
							}
							shootVelocity.Y -= timeToTarget * 0.25f;

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<MudSlap>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 5f, pokemon.owner)];
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
			//Gravity
			Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y > 20f)
            {
                Projectile.velocity.Y = 20f;
            }

            //Rolling
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 1.5f);

            DustTrail();
        
            CheckPokemonPetCollide();
        
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            DustBomb(Projectile.velocity, target.Center);
            base.OnHitNPC(target, hit, damageDone);
            if(Projectile.timeLeft > 18)
            {
                Projectile.timeLeft = 18;
            }
            target.AddBuff(BuffID.Weak, 5 * 60);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            DustBomb(Projectile.velocity, target.Center);
            base.OnHitPlayer(target, info);
            if (Projectile.timeLeft > 18)
            {
                Projectile.timeLeft = 18;
            }
            target.AddBuff(BuffID.Weak, 5 * 60);
        }

        public override void OnHitPokemonPet(PokemonPetProjectile target, int damageDone)
        {
            base.OnHitPokemonPet(target, damageDone);
            target.ApplyStatMod(5, -1); //Accuracy Down
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

        private void DustTrail()
        {
            Dust.NewDust(Projectile.Center, 0, 0, DustID.Mud, Scale: 2f);
            if (Main.rand.NextBool(10))
            {
                SoundEngine.PlaySound(SoundID.SplashWeak with { Volume = 0.3f}, Projectile.position);
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
