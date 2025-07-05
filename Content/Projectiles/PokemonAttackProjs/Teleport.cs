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
	public class Teleport : PokemonAttack
	{
        private int fleeDistance = 800;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
		{
			Projectile.width = 62;
			Projectile.height = 62;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.timeLeft = 18;

			Projectile.knockBack = 0f;

			Projectile.tileCollide = false;
			Projectile.penetrate = -1;

			base.SetDefaults();
		}

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			if (pokemon.owner == Main.myPlayer)
			{
                for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                {
                    if (pokemonOwner.attackProjs[i] == null)
                    {
                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Teleport>(), 0, 0f, pokemon.owner)];
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
                        pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.canAttack = false;
                        pokemonOwner.canAttackOutTimer = true;
                        SoundEngine.PlaySound(SoundID.Item24, pokemon.position);
                        DustBurst(pokemon.Center);
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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 10)
                {
                    int remainProjs = 1;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            int fleeDirection = (Math.Sign(pokemon.Center.X - targetCenter.X));
                            Vector2 targetPosition = FindAir(pokemon.TopLeft + Vector2.UnitX * fleeDirection * fleeDistance, out bool airFound) - Vector2.UnitY * pokemon.height;
                            if (!airFound) targetPosition = pokemon.TopLeft;
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetPosition, Vector2.Zero, ModContent.ProjectileType<Teleport>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 10f, pokemon.owner)];
                            pokemon.position = targetPosition;
                            SoundEngine.PlaySound(SoundID.Item30, pokemon.position);
                            DustBurst(pokemon.Center);
                            remainProjs--;
                            pokemonOwner.canAttackOutTimer = false;
                            pokemonOwner.canAttack = true;
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

        public static Vector2 FindAir(Vector2 target, out bool airFound)
        {
            int searchRange = 500;
            airFound = false;
            Vector2 airPosition = Vector2.Zero;

            int tileX = (int)(target.X / 16f);
            int tileY = (int)(target.Y / 16f);

            int searchTarget = tileY - searchRange;
            if (searchTarget < 0)
            {
                searchTarget = 0;
            }

            for (int y = tileY; y > searchTarget; y--)
            {
                Tile tile = Main.tile[tileX, y];
                if (!tile.HasTile)
                {
                    airPosition = new Vector2(tileX * 16, y * 16);
                    airFound = true;
                    break;
                }

            }
            return airPosition;
        }

        public void DustBurst(Vector2 position)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, DustID.Confetti_Pink, 0, 0);
                }
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, DustID.PinkTorch, 0, 0);
                }
            }
        }

        public override void AI()
		{
            UpdateAnimation();

			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
			}
		}
        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                }
            }
        }
    }
}
