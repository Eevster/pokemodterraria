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
	public class FlameWheel : PokemonAttack
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

		public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 40;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.light = 1f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<FlameWheel>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 0f, pokemon.owner)];
						pokemon.velocity = 36f*Vector2.Normalize(targetCenter-pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item20, pokemon.position);
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
			if(pokemon.velocity.Length() < 1f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 1f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}

        public override void ExtraChanges(Projectile pokemon)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(!pokemonOwner.canAttack && pokemonOwner.timer > 0){
				pokemonOwner.immune = true;
                pokemon.velocity.Y *= 0.95f;
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Projectile.scale*new Vector3(2,0.6f,0.2f));

            if(Main.rand.NextBool())
            {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, default(Color), 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(0.5f,1.5f);
			}

            UpdateAnimation();

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 2;
                    SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                }
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
