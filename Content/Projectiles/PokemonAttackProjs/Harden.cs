using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.DataStructures;
using Pokemod.Content.Pets;
using Terraria.Graphics.Shaders;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class Harden : PokemonAttack
    {
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = true;  

            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

            Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Harden>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 6f, pokemon.owner)];
						SoundEngine.PlaySound(SoundID.Item37, pokemon.position);
                        DustBurst(pokemon.Center);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

        public void DustBurst(Vector2 position)
        {
            for (int i = 0; i < 7; i++)
            {
                int dust = Dust.NewDust(position, 16, 16, 63, Main.rand.Next(-6, 7), Main.rand.Next(-6, 7), default, default, 2.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
            }
        }

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			pokemonOwner.attackProjs[i].Center = pokemon.Center;
		}

        public override void ExtraChanges(Projectile pokemon){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

            if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && !pokemonOwner.canAttack) {
                pokemonOwner.immune = true;
                pokemonOwner.pokemonShader = GameShaders.Armor.GetShaderFromItemId(ItemID.ReflectiveSilverDye);
                
            }
        }
    }
}