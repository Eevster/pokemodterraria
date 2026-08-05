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
    internal class CottonGuard : PokemonAttack
    {
        public override bool CanExistIfNotActualMove => false;

        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 45;

            Projectile.tileCollide = false;  

            Projectile.penetrate = -1;

            Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<CottonGuard>(), 0, 0f, pokemon.owner,  targetCenter.X, targetCenter.Y)];
						SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.ApplyStatMod(1, 3);
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

            if (Main.rand.NextBool(3))
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Main.rand.Next(61,64), 1f);
                Main.gore[goreIndex].position = Projectile.Center + 60*Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                Main.gore[goreIndex].velocity = 3f*Vector2.Normalize(Projectile.Center-Main.gore[goreIndex].position);
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
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