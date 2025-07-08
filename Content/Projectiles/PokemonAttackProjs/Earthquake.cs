using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class Earthquake : PokemonAttack
    {
        public List<NPC> targets = new List<NPC>();

        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hide = true;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 20;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 5)
                {
                    int remainProjs = 1;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Bottom, Vector2.Zero, ModContent.ProjectileType<Earthquake>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
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
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy() && npc.life > 0)
                {
                    if ((npc.Center - Projectile.Center).Length() < PokemonData.pokemonAttacks["Psychic"].distanceToAttack)
                    {
                        if (Collision.SolidCollision(npc.Bottom, npc.width, 4))
                        {
                            targets.Add(npc);
                        }
                    }
                }
            }

            SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
            PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 7f, 5f, 25, PokemonData.pokemonAttacks["Psychic"].distanceToAttack * 2f, FullName);
            Main.instance.CameraModifiers.Add(modifier);

            base.OnSpawn(source);
        }

        public override void AI()
        {
            bool targetsRemaining = false;

            if (targets.Count > 0)
            {
                foreach (NPC npc in targets) {
                    if (npc != null)
                    {
                        targetsRemaining = true;
                        if (Collision.SolidCollision(npc.Bottom, npc.width, 4))
                        {
                            Projectile.height = npc.height;
                            Projectile.position = npc.Center - new Vector2(Projectile.width, Projectile.height) / 2f;

                            for (int i = 0; i < 10; i++)
                            {
                                Dust.NewDust(Projectile.Bottom, 10, 6, DustID.Dirt, 0, -3);
                            }
                            targets.Remove(npc);
                            break;
                        }
                    }
                }
            }
            if (!targetsRemaining)
            {
                Projectile.Kill();
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }
    }
}