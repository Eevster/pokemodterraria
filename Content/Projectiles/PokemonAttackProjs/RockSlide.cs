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
	public class RockSlide : PokemonAttack
	{

        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/RockThrow";

        public override void SetDefaults()
        {

            Projectile.timeLeft = 40;
			
			Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.aiStyle = -1; 
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
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

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if (pokemon.owner == Main.myPlayer)
			{
				if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer % 10 == 1 && pokemonOwner.timer <= 21)
				{
					for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
					{
						if (pokemonOwner.attackProjs[i] == null)
						{
							if(pokemonOwner.timer == 21)
							{
                                SoundEngine.PlaySound(SoundID.Item69, pokemon.position);
                            }
							Vector2 positionOffset = new(Main.rand.Next(-24, 25), Main.rand.Next(-24, 25));
							pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter + positionOffset - Vector2.UnitY * 90, Vector2.UnitY * 3f, ModContent.ProjectileType<RockSlide>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 10f, pokemon.owner, targetCenter.Y)];
							SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
							for (int j = 0; j < 5; j++)
							{
								Dust.NewDust(pokemonOwner.attackProjs[i].Center, 6, 6, DustID.Stone, Main.rand.Next(-3, 4), 0f);
							}
							break;
						}
					}
				}
			}
		}
		

        public override void AI()
        {
			//Gravity
			Projectile.velocity.Y += 0.7f;
            if (Projectile.velocity.Y > 10f)
            {
                Projectile.velocity.Y = 10f;
            }

			//Applies Tile Collision after passing the target location vertically. 
			if (Projectile.Center.Y >= Projectile.ai[0])
			{
				Projectile.tileCollide = true;
			}

            //Fade out
            if (Projectile.timeLeft < 20){
                Projectile.Opacity = 1f * Projectile.timeLeft * 0.05f;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
			fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

            for (int j = 0; j < 5; j++)
            {
                Dust.NewDust(Projectile.Center, 6, 6, DustID.Stone, Main.rand.Next(-3, 4), 0f);
            }
        }

    }
}
