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
    internal class CosmicPower : PokemonAttack
    {
        public override bool CanExistIfNotActualMove => false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 45;

            Projectile.tileCollide = false;  

            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

            Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<CosmicPower>(), 0, 0f, pokemon.owner,  targetCenter.X, targetCenter.Y)];
						SoundEngine.PlaySound(SoundID.Item105, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.ApplyStatMod(1, 1);
                        pokemonOwner.ApplyStatMod(3, 1);
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
            if (Projectile.timeLeft == 30)
            {
                SetExpTarget(out NPC target);
            }

            UpdateAnimation();

            if (Main.rand.NextBool(5))
            {
                int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(16,18), 1f);
                Main.gore[goreIndex].position = Projectile.Center;
                Main.gore[goreIndex].velocity = Main.rand.NextFloat(3f,5f)*Vector2.UnitX.RotatedByRandom(2f*MathHelper.Pi);
            }

            if(Projectile.timeLeft < 20f)
            {
                Projectile.Opacity = 1f*Projectile.timeLeft/20f;
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
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

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}