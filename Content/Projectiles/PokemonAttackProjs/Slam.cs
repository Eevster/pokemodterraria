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
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Slam : PokemonAttack
	{
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Slam>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 2, 7f, pokemon.owner)];
						pokemon.velocity = 30*Vector2.Normalize(targetCenter-pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item20 with { Pitch = -0.5f }, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 0.1f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 0.1f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}

        public override void ExtraChanges(Projectile pokemon){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(!pokemonOwner.canAttack && pokemonOwner.timer > 0){
				pokemonOwner.immune = true;
                pokemon.velocity.Y *= 0.95f;
            }
        }

        public override void AI()
        {
            if(Main.rand.NextBool() && Main.rand.NextBool())
            {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, default(Color), 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(0.5f,1.5f);
			}

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public void HitEffect()
		{
			SoundEngine.PlaySound(SoundID.Item73 with { Pitch = -0.2f }, Projectile.Center);
			for (int i = 0;  i < 10; i++)
			{
                Dust.NewDust(Projectile.position, 0, 0, DustID.Smoke, -Projectile.oldVelocity.X / 2, -Projectile.oldVelocity.Y / 2);
            }
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
			HitEffect();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
			HitEffect();
        }
    }
}
