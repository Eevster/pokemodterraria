using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
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
	public class BoneRush : PokemonAttack
	{
		public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 40;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 2;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;

            Projectile.stopsDealingDamageAfterPenetrateHits = true;
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
						pokemonOwner.remainAttacks = Main.rand.Next(2,6);
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack  && pokemonOwner.timer%4==0){
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.remainAttacks <= 0){
							pokemonOwner.canAttackOutTimer = false;
							break;
						}
						if(pokemonOwner.attackProjs[i] == null){
                            Vector2 shootDirection = Vector2.Normalize(targetCenter-pokemon.Center);
							pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center + Main.rand.NextFloat(-16f,16)*shootDirection.RotatedBy(0.5f*Math.PI), 35f*shootDirection, ModContent.ProjectileType<BoneRush>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 8f, pokemon.owner)];
							SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
							pokemonOwner.remainAttacks--;
							break;
						}
					} 
				}
			}
		}

        public override void AI()
        {
			Projectile.velocity *= 0.9f;

            if(Projectile.velocity.Length() > 0) Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            if(Projectile.timeLeft < 20f)
            {
                Projectile.Opacity = Projectile.timeLeft/20f;
            }

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }
    }
}
