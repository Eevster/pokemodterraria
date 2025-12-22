using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class DragonRush : PokemonAttack
	{
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 80;

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
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<DragonRush>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 2, 7f, pokemon.owner)];
						pokemon.velocity = 45*Vector2.Normalize(targetCenter-pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item119, pokemon.position);
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
				if(!Main.player[pokemon.owner].GetModPlayer<PokemonPlayer>().onBattle) pokemonOwner.immune = true;
				pokemonOwner.pokemonShader = GameShaders.Armor.GetShaderFromItemId(ItemID.PurpleOozeDye);
                pokemon.velocity.Y *= 0.95f;
            }
        }

        public override void AI()
        {
            if(Main.rand.NextBool() && Main.rand.NextBool())
            {
				for (int i = 0; i < 10; i++)
				{
					float spreadX = Main.rand.NextFloat(-4f, 4f);
					float spreadY = Main.rand.NextFloat(-4f, 4f);
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.YellowTorch : DustID.PurpleTorch, spreadX + Projectile.velocity.X / 4, spreadY + Projectile.velocity.Y / 4, Scale: 2f);
					Main.dust[dust].noGravity = true;
				}
			}

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public void HitEffect(Vector2 position)
		{
			SoundEngine.PlaySound(SoundID.Item68 with { Pitch = -0.2f }, position);
			for (int i = 0;  i < 20; i++)
			{
                Dust.NewDust(position, 0, 0, DustID.Enchanted_Pink, -Projectile.oldVelocity.X, -Projectile.oldVelocity.Y);
            }
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
			HitEffect(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
			HitEffect(target.Center);
        }
    }
}
