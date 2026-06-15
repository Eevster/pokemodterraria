using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Acid : PokemonAttack
    {
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 150;

            Projectile.width = 34;
            Projectile.height = 34;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;
            Projectile.penetrate = 4;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20)
                {
                    int remainProjs = 1;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
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

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<Acid>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 5f, pokemon.owner, 0)];
                            SoundEngine.PlaySound(SoundID.Item21 with { Pitch = -0.3f }, pokemon.position);
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

        public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int nFrames = Main.projFrames[Projectile.type];

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Frame(1,nFrames,0,0).Width * 0.5f, texture.Frame(1,nFrames,0,0).Height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor);
                color *= 0.25f + 0.75f*(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1,nFrames,0,Projectile.frame), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

			return true;
		}

        public override void AI()
        {
            //Gravity
            Projectile.velocity.Y += 0.7f;
            if (Projectile.velocity.Y > 20f)
            {
                Projectile.velocity.Y = 20f;
            }

            //Rolling
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);

            for(int i = 0; i < 3; i++)
            {
                DustTrail(Projectile.velocity);
            }
            
            UpdateAnimation();
        }

		private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    SoundEngine.PlaySound(SoundID.LiquidsWaterLava, Projectile.Center);
                }
            }
        }

        private void DustTrail(Vector2 velocity)
        {
            switch (Main.rand.Next(0, 5))
            {
                case 0:
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Honey, velocity.X * 0.5f, velocity.Y * 0.5f, 100, Scale: 1f); break;
                case 1:
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Honey, velocity.X * 0.4f, velocity.Y * 0.4f, 100, Scale: 1f); break;
                case 2:
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Honey2, velocity.X * 0.4f, velocity.Y * 0.4f, 100, Scale: 1f); break;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            for(int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Honey, Main.rand.Next(-2, 3), -3, 100, default, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Honey, Main.rand.Next(-2, 3), -3, 100, default, 2f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Honey2, Main.rand.Next(-2, 3), -3, 100, default, 2f);
            }

            SoundEngine.PlaySound(SoundID.Item81, Projectile.Center);

            base.OnKill(timeLeft);
        }

        public override void OnHitPokemonPet(PokemonPetProjectile target, int damageDone)
        {
            if(Main.rand.NextBool(10)) target.ApplyStatMod(3, -1);
            base.OnHitPokemonPet(target, damageDone);
        }
    }
}
