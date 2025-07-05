using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Psychic : PokemonAttack
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
		{

			Projectile.timeLeft = 60;

			Projectile.width = 26;
			Projectile.height = 26;
            Projectile.light = 0.2f;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.tileCollide = false;
			Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
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
                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Psychic>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner, pokemonOwner.GetPokemonAttackDamage(GetType().Name))];
                        SoundEngine.PlaySound(SoundID.Item9, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
                        pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.canAttack = false;
                        pokemonOwner.canAttackOutTimer = true;
                        break;
					}
				}
			}
		}

        public override void AI()
        {
            Vector2 homingTarget = FindTarget();
            float currentSpeed = 40f * (float)Math.Pow(1.0f - (float)Projectile.timeLeft / 60f, 1f);
            float homingStrength = 0.03f + 0.3f * (1.0f - (float)Projectile.timeLeft / 60f);

            if (!((Projectile.velocity == Vector2.Zero || Projectile.velocity.Length() <= float.Epsilon) && (homingTarget - Projectile.Center).Length() <= float.Epsilon))
            {
                Projectile.velocity *= 1 - homingStrength;
                Projectile.velocity += homingStrength * currentSpeed * Vector2.Normalize(homingTarget - Projectile.Center);

                if (Projectile.velocity.Length() > (homingTarget - Projectile.Center).Length())
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.position = homingTarget - Vector2.One * Projectile.width / 2f;
                }
            }

            UpdateAnimation();

            if (!Main.dedServ)
            {
                if (Projectile.timeLeft % 5 == 0)
                {
                    Dust newDust = Main.dust[Dust.NewDust(Projectile.Center, 10, 10, DustID.PinkTorch)];
                    newDust.noGravity = true;
                }
            }

            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.timeLeft == 1)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PsychicBlast>(), 0, 2f, Projectile.owner, (int)Projectile.ai[0]);
                }

                Projectile.netUpdate = true;
            }
        }

        private Vector2 FindTarget()
        {
            Vector2 targetPosition = Vector2.Zero;

            if (Projectile.owner == Main.myPlayer)
            {
                PokemonPlayer trainer = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>();
                if (trainer.attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack)
                {
                    targetPosition = trainer.attackPosition;
                }
                else
                {
                    float runningClosestDistance = PokemonData.pokemonAttacks["Psychic"].distanceToAttack;

                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.CanBeChasedBy() && npc.life > 0)
                        {
                            if ((npc.Center - Projectile.Center).Length() < runningClosestDistance)
                            {
                                runningClosestDistance = (npc.Center - Projectile.Center).Length();
                                targetPosition = npc.Center;
                            }
                        }
                    }

                }
                if (targetPosition == Vector2.Zero)
                {
                    targetPosition = Projectile.Center;
                }
            }
            return targetPosition;
        }

        public void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    
                }
            }
        }
    }
}
