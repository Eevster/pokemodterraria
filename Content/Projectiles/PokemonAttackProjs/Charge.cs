using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.DataStructures;
using Pokemod.Content.Pets;
using Terraria.Graphics.Shaders;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Common.GlobalNPCs;
using System.Collections.Generic;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class Charge : PokemonAttack
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  

            Projectile.penetrate = -1;

            Projectile.Opacity = 0.8f;
            Projectile.light = 1f;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Charge>(), 0, 0f, pokemon.owner,  targetCenter.X, targetCenter.Y)];
						SoundEngine.PlaySound(SoundID.Item43, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.ApplyStatMod(3, 1);
                        pokemonOwner.isCharged = true;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			pokemonOwner.attackProjs[i].Center = pokemon.Center;
		}

        public override void AI()
        {
            if (Projectile.timeLeft == 45)
            {
                SetExpTarget(out NPC target);
            }

            if(Projectile.timeLeft > 36)
            {
                Projectile.ai[2] = Math.Clamp((60-Projectile.timeLeft)/10f,0.1f,1f);
                Projectile.scale = Projectile.ai[2];

                int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(16,16), 32, 32, DustID.YellowStarDust, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].position = Projectile.Center + 60*Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                Main.dust[dustIndex].velocity = 3f*Vector2.Normalize(Projectile.Center-Main.dust[dustIndex].position);
            }
            else
            {
                Projectile.scale = 1f;
                if(Projectile.frame < 3) Projectile.frame = 3;
            }

            UpdateAnimation();

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if(Projectile.timeLeft > 45)
                {
                    if (++Projectile.frame >= 3)
                    {
                        Projectile.frame = 0;
                    }
                }
                else
                {
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame--;
                    }
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
                            if (!target.GetGlobalNPC<HitByPokemonNPC>().pokemonProjs.Contains(pokemonProj))
                            {
                                if (target.life <= 0)
                                {
                                    PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
                                    pokemonMainProj?.SetGainedExp(HitByPokemonNPC.SetExpGained(target, target.GetGlobalNPC<HitByPokemonNPC>().pokemonProjs.Count));
                                }
                                target.GetGlobalNPC<HitByPokemonNPC>().pokemonProjs.Add(pokemonProj);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}