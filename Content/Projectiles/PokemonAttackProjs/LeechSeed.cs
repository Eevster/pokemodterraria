using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Buffs;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class LeechSeed : PokemonAttack
    {
        private int bounceCounter = 0;

        public override void SetDefaults()
        {

            Projectile.timeLeft = 150;

            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;
            Projectile.penetrate = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
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
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
                        pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.canAttack = false;
                        pokemonOwner.canAttackOutTimer = true;
                        pokemonOwner.remainAttacks = 3;
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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer%12==0)
                {
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if(pokemonOwner.remainAttacks <= 0){
							pokemonOwner.canAttackOutTimer = false;
							break;
						}
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            float shootSpeed = 12f;
                            float timeToTarget = 0;
                            Vector2 shootVelocity = shootSpeed * Vector2.Normalize(targetCenter - pokemon.Center);
                            if (shootVelocity.X != 0f)
                            {
                                timeToTarget = (targetCenter - pokemon.Center).X / shootVelocity.X;
                            }
                            if (timeToTarget > pokemonOwner.distanceToAttack / shootSpeed || timeToTarget == 0)
                            {
                                timeToTarget = pokemonOwner.distanceToAttack / shootSpeed;
                            }
                            shootVelocity.Y -= timeToTarget * 0.35f;

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<LeechSeed>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
                            SoundEngine.PlaySound(SoundID.Item5, pokemon.position);
                            pokemonOwner.remainAttacks--;
							break;
                        }
                    }
                }
            }
        }

        public override void AI()
        {
            //Gravity
            Projectile.velocity.Y += 0.7f;
            if (Projectile.velocity.Y > 20f)
            {
                Projectile.velocity.Y = 20f;
            }

            if(Projectile.velocity.Length() > 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            }

            Dust.NewDust(Projectile.Center - 0.5f*new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.DirtSpray, 0, 0, default, default, 0.5f);

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.CanBeChasedBy())
            {
                target.AddBuff(ModContent.BuffType<LeechSeedDebuff>(), 5*60);
                target.GetGlobalNPC<LeechSeedGlobalNPC>().targetPlayer = Main.player[Projectile.owner];
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<LeechSeedDebuff>(), 5*60);
            target.GetModPlayer<LeechSeedPlayer>().targetPlayer = Main.player[Projectile.owner];
            base.OnHitPlayer(target, info);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
            base.OnKill(timeLeft);
        }

    }
}
