using Microsoft.Xna.Framework;
using Pokemod.Common.GlobalNPCs;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Teleport : PokemonAttack
	{
        public static int fleeDistance = 300;
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
                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Teleport>(), 0, 0f, pokemon.owner, targetCenter.X, targetCenter.Y)];
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
                            Vector2 targetPosition = FindAir(pokemon.Center, fleeDirection, out bool airFound);
                            if (!airFound) targetPosition = pokemon.Center;
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetPosition, Vector2.Zero, ModContent.ProjectileType<Teleport>(), 0, 0f, pokemon.owner, targetCenter.X, targetCenter.Y)];
                            pokemon.position = targetPosition - new Vector2(pokemon.width, pokemon.height) * 0.5f;
                            DustBurst(pokemon.Center);
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

        public static Vector2 FindAir(Vector2 target, int fleeDirection, out bool airFound)
        {
            int searchRange = (int)(fleeDistance / 16f);
            airFound = false;
            bool groundFound = false;
            Vector2 airPosition = target;

            int tileX = (int)(target.X / 16f);
            int tileY = (int)(target.Y / 16f);

            int searchTargetY = tileY - searchRange;
            if (searchTargetY < 0)
            {
                searchTargetY = 0;
            }

            for (int i = 0; i < searchRange; i++)
            {
                int x = tileX + i * fleeDirection;
                Tile tileSolid = Main.tile[x, tileY];
                if (tileSolid.HasTile && Main.tileSolid[tileSolid.TileType])
                {
                    groundFound = true;
                    for (int y = tileY; y > searchTargetY; y--)
                    {
                        Tile tileAir = Main.tile[x, y];
                        if (!tileAir.HasTile || !Main.tileSolid[tileAir.TileType])
                        {
                            tileY = y;
                            airPosition = new Vector2(x * 16, tileY * 16);
                            airFound = true;
                            break;
                        }

                    }

                    if ((target - airPosition).Length() >= fleeDistance)
                    {
                        break;
                    }
                }
            }
            if (!groundFound)
            {
                airPosition = target + Vector2.UnitX * fleeDistance * fleeDirection;
                airFound = true;
            }
            return airPosition;
        }

        public void DustBurst(Vector2 position)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, DustID.Gastropod, 0, 0);
                }
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(position, Projectile.width, Projectile.height, DustID.PinkTorch, 0, 0);
                }
            }
        }

        public bool SetExpTarget(out NPC target)
        {
            target = null;
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 aimingTarget = new Vector2(Projectile.ai[0], Projectile.ai[1]);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc != null)
                    {
                        if (npc.CanBeChasedBy() || npc.CountsAsACritter || npc.ModNPC is PokemonWildNPC)
                        {
                            if ((new Rectangle((int)aimingTarget.X - 12, (int)aimingTarget.Y - 12, 24, 24)).Intersects(npc.getRect()))
                            {
                                target = npc;
                                break;
                            }
                        }
                    }
                }
                
                if (target != null)
                {
                    if (pokemonProj != null)
                    {
                        if (pokemonProj.active)
                        {
                            if (target.life <= 0 && target.GetGlobalNPC<HitByPokemonNPC>().pokemonProj != pokemonProj)
                            {
                                PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
                                pokemonMainProj?.SetExtraExp(HitByPokemonNPC.SetExpGained(target));
                            }
                            target.GetGlobalNPC<HitByPokemonNPC>().pokemonProj = pokemonProj;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void AI()
		{
            if (Projectile.timeLeft == 18)
            {
                SetExpTarget(out NPC target);
            }

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
