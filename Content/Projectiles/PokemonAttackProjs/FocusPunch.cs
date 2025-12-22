using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class FocusPunch : PokemonAttack
	{
        private Vector2 positionOffset;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 58;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 1;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            //positionOffset = new Vector2(0, 10);

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
				{
					if (pokemonOwner.attackProjs[i] == null)
					{
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<FocusPunch>(), 0, 16f, pokemon.owner, pokemonOwner.currentHp, pokemon.spriteDirection)];
                        pokemon.velocity = Vector2.Zero;
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
                if (pokemonOwner.timer > 30 && pokemonOwner.timer % 10 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item8 with { Pitch = (90f - (float)pokemonOwner.timer) / 60f }, pokemon.position);
                }

                for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                {
                    if (pokemonOwner.attackProjs[i] != null)
                    {
                        if (pokemonOwner.attackProjs[i].type == ModContent.ProjectileType<FocusPunch>())
                        {
                            if (pokemonOwner.currentHp < pokemonOwner.attackProjs[i].ai[0])
                            {
                                pokemonOwner.timer = 10;
                                pokemonOwner.canAttackOutTimer = false;
                                pokemonOwner.attackProjs[i].Kill();
                                return;
                            }
                            if (pokemonOwner.timer <= 30)
                            {
                                pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
                                pokemon.velocity = 45f * Vector2.Normalize(targetCenter - pokemon.Center);
                                for (int j = 0; j < 10; j++)
                                {
                                    Dust.NewDust(pokemon.Bottom, 4, 4, DustID.Smoke, -pokemon.velocity.X * 0.1f, -pokemon.velocity.Y * 0.1f);
                                }
                                pokemonOwner.attackProjs[i].damage = pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 5;
                                pokemonOwner.attackProjs[i].hide = true;
                                SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
                                pokemonOwner.canAttackOutTimer = false;
                                return;
                            }
                        }
                    }
                }

                pokemon.velocity.X = 0;
            }
        }
      
        public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed)
        {
            Update(pokemon, i);
        }

        public override void UpdateNoAttackProjs(Projectile pokemon, int i)
        {
            Update(pokemon, i);
        }

        public void Update(Projectile pokemon, int i)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            pokemonOwner.attackProjs[i].Center = pokemon.Center + positionOffset;
            if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack)
            {
                pokemonOwner.attackProjs[i].Center = pokemon.Center;
                pokemonOwner.attackProjs[i].velocity = pokemon.velocity;
            }
            if (pokemon.velocity.Length() < 0.1f && pokemonOwner.timer < 15)
            {
                pokemonOwner.attackProjs[i].Kill();
                if (!pokemonOwner.canAttack)
                {
                    pokemonOwner.timer = 0;
                }
            }
        }

        public override void ExtraChanges(Projectile pokemon){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack){
				if(!Main.player[pokemon.owner].GetModPlayer<PokemonPlayer>().onBattle) pokemonOwner.immune = true;
                pokemon.velocity.Y *= 0.95f;
			}
        }

        public override void AI()
        {
            if(Main.rand.NextBool() && !Projectile.hide)
            {
				int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Firework_Yellow, Main.rand.Next(-3, 4), Main.rand.Next(-3, 4), default, default, 0.5f);
				Main.dust[dust].noGravity = true;
			}

            UpdateAnimation();

            if (Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Punch(target);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Punch(target);
            base.OnHitPlayer(target, info);
        }

        public void Punch(Entity target)
        {
            for (int j = 0; j < 20; j++)
            {
                int dust = Dust.NewDust(target.Center, 50, 50, DustID.Firework_Yellow, pokemonProj.velocity.X * 0.1f + Main.rand.Next(-2, 3), pokemonProj.velocity.Y * 0.1f + Main.rand.Next(-2, 3), default, default, 0.5f);
                Main.dust[dust].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item116, Projectile.position);
            pokemonProj.velocity *= 0.3f;
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
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
