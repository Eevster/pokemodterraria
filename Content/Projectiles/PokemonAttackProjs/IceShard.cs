using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class IceShard : PokemonAttack
	{

        public override void SetDefaults()
        {

            Projectile.timeLeft = 120;

            Projectile.width = 10;
            Projectile.height = 10;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -20;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.aiStyle = -1; 
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
                        SoundEngine.PlaySound(SoundID.Item20, pokemon.Center);
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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer > 15)
                {
                    float x = Main.rand.NextFloat(-1, 1);
                    float y = Main.rand.NextFloat(-1, 1);
                    int dust = Dust.NewDust(new Vector2(x, y) * 100f + pokemon.Center, 0, 0, DustID.IceRod, -x * 4f, -y * 4f);
                    Main.dust[dust].noLight = true;
                }
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20)
                {
                    int remainProjs = 1;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            Vector2 spikeDirection = (targetCenter - pokemon.Center).SafeNormalize(Vector2.Zero);

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, spikeDirection * 25f, ModContent.ProjectileType<IceShard>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 2, 12f, pokemon.owner)];
                            SoundEngine.PlaySound(SoundID.Item48, pokemon.position);
                            remainProjs--;
                            pokemonOwner.canAttackOutTimer = false;
                            if (remainProjs <= 0)
                            {
                                break;
                            }
                        }
                    }
                }
			}
		}

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
            Projectile.netUpdate = true;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            /*/Grow in 
            if (Projectile.timeLeft >= 115)
            {
                Projectile.scale = Math.Clamp((125 - Projectile.timeLeft) / 10, 0.5f, 1f);
            }*/
            if (Projectile.timeLeft == 115)
            {
                Projectile.tileCollide = true;
            }


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
            Projectile.velocity = Vector2.Zero;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 20;
            SoundEngine.PlaySound(SoundID.Item51, Projectile.Center);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item50 with { Pitch = 0.5f }, Projectile.Center);

            for (int j = 0; j < 10; j++)
            {
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.IceRod, Main.rand.Next(-2, 3), 0f);
                Main.dust[dust].noLight = true;
            }
        }
    }
}
