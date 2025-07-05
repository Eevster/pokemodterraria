using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Dig : PokemonAttack
	{
		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 48;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.timeLeft = 30;

			Projectile.knockBack = 10f;

			Projectile.tileCollide = false;
			Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

			base.SetDefaults();
		}

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if (pokemon.owner == Main.myPlayer)
			{
				if (Collision.SolidCollision(pokemon.Bottom, pokemon.width - 3, 1))
				{ //Grounded
					for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
					{
						if (pokemonOwner.attackProjs[i] == null)
						{
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Bottom, Vector2.Zero, ModContent.ProjectileType<Dig>(), 0, 0f, pokemon.owner)];
                            pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
                            pokemonOwner.timer = pokemonOwner.attackDuration;
                            pokemonOwner.canAttack = false;
                            pokemonOwner.canAttackOutTimer = true;
                            SoundEngine.PlaySound(SoundID.Item69, pokemon.position);
                            DustBurst(pokemon.Bottom);
                            break;
                        }
					}
				}
			}
		}

        public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

            if (pokemon.owner == Main.myPlayer)
            {
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 5)
                {
                    int remainProjs = 1;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
							Vector2 targetPosition = FindFloor(targetCenter, out bool floorFound);
                            if (!floorFound)
							{
								targetPosition = pokemon.Bottom;
							}
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetPosition, Vector2.Zero, ModContent.ProjectileType<Dig>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 10f, pokemon.owner)];
                            pokemon.position = targetPosition - Vector2.UnitY * (16 + pokemon.height / 2);
                            SoundEngine.PlaySound(SoundID.Item70, pokemon.position);
                            DustBurst(pokemon.Bottom);
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

        public override void ExtraChanges(Projectile pokemon)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			if (!pokemonOwner.canAttack && pokemonOwner.timer > 0)
			{
				pokemonOwner.immune = true;
                pokemonOwner.hurtTime = 2;
                pokemon.Opacity = 0;
                pokemon.velocity = Vector2.Zero;
            }

            if (pokemonOwner.canAttack)
            {
                pokemon.Opacity = 1;
            }
        }

        public static Vector2 FindFloor(Vector2 target, out bool floorFound)
        {
            int searchRange = 100;
            floorFound = false;
            Vector2 floorPosition = Vector2.Zero;

            int tileX = (int)(target.X / 16f);
            int tileY = (int)(target.Y / 16f);

            int searchTarget = tileY + searchRange;
            if (searchTarget > Main.maxTilesY)
            {
                searchTarget = Main.maxTilesY;
            }

            for (int y = tileY; y < searchTarget; y++)
            {
                Tile tile = Main.tile[tileX, y];
                if (tile.HasTile && Main.tileSolid[tile.TileType])
                {
                    floorPosition = new Vector2(tileX * 16, y * 16);
                    floorFound = true;
                    break;
                }

            }
            return floorPosition;
        }

        public void DustBurst(Vector2 position)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(position, Projectile.width, 4, DustID.Dirt, Main.rand.Next(-2, 3), Main.rand.Next(-3,-1));
            }
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(position, Projectile.width, 4, DustID.DirtSpray, Main.rand.Next(-2, 3), -1);
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.Opacity = 1f;
            base.OnSpawn(source);
        }
        public override void AI()
		{
			Projectile.Opacity = (float)Math.Sqrt(Projectile.timeLeft / 30f);

			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
			}
		}
	}
}
