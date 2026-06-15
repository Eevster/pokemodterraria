using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.GlobalNPCs;
using Pokemod.Content.NPCs;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class AcidArmor : PokemonAttack
    {
        float animOffset = 0;

        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 9;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 45;

            Projectile.tileCollide = false;  

            Projectile.penetrate = -1;
            Projectile.Opacity = 0.9f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<AcidArmor>(), 0, 0f, pokemon.owner,  targetCenter.X, targetCenter.Y, 1.5f*pokemonOwner.hitboxWidth)];
						SoundEngine.PlaySound(SoundID.Item21 with { Pitch = -0.3f }, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.ApplyStatMod(1, 2);
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

        public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int nFrames = Main.projFrames[Projectile.type];

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Frame(1,nFrames,0,0).Width * 0.5f, texture.Frame(1,nFrames,0,0).Height * 0.5f);

            float auxPos = animOffset;
            float initialScale = Projectile.ai[2]/(2f*drawOrigin.X);
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY-Projectile.ai[2]*0.5f);

            while (auxPos <= Projectile.ai[2])
            {
                float currentScale = (initialScale+0.5f*(auxPos/Projectile.ai[2]))*Projectile.scale;
                Color color = Projectile.GetAlpha(lightColor)*Projectile.Opacity;
                if(auxPos < 0.4f*Projectile.ai[2])
                {
                    color *= 0.5f+0.5f*(auxPos/(0.4f*Projectile.ai[2]));
                }else if (auxPos > 0.8f*Projectile.ai[2])
                {
                    color *= (Projectile.ai[2]-auxPos)/(0.2f*Projectile.ai[2]);
                }
                Main.EntitySpriteDraw(texture, drawPos + new Vector2(0, auxPos), texture.Frame(1,nFrames,0,Projectile.frame), color, Projectile.rotation, drawOrigin, currentScale, SpriteEffects.None, 0);
                auxPos += 0.25f*Projectile.ai[2];
            }

            animOffset = (animOffset+0.02f*Projectile.ai[2])%(0.25f*Projectile.ai[2]);

			return false;
		}

        public override void AI()
        {
            if (Projectile.timeLeft == 30)
            {
                SetExpTarget(out NPC target);
            }

            UpdateAnimation();

            if(Projectile.timeLeft < 10f)
            {
                Projectile.Opacity = 0.9f*Projectile.timeLeft/10f;
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    SoundEngine.PlaySound(SoundID.Shimmer1, Projectile.Center);
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
