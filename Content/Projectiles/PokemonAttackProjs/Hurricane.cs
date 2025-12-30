using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Hurricane : PokemonAttack
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

		public override void SetDefaults()
        {
            Projectile.width = 162;
            Projectile.height = 44;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 100;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.Opacity = 0.8f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
                int nProjs = 8;
                float scale = 0.7f;
                if (Main.IsItRaining)
                {
                    nProjs = 6;
                    scale = 2f;
                }
                Vector2 projPosition = targetCenter + new Vector2(0,16*10);
				for(int i = 0; i < Math.Min(pokemonOwner.nAttackProjs, nProjs); i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Hurricane>(), (int)(0.5f *pokemonOwner.GetPokemonAttackDamage(GetType().Name)), 4f, pokemon.owner, projPosition.X, projPosition.Y)];
                        pokemonOwner.attackProjs[i].scale = scale;
                        pokemonOwner.attackProjs[i].frame += i%6;
                        pokemonOwner.attackProjs[i].timeLeft -= 3*(nProjs-1-i);
						SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, pokemon.position);
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;

                        scale += 0.1f;
                        projPosition += (scale-0.1f)*new Vector2(0,-44);
                    }
				} 
			}
		}

        public override void AI()
        {
            Projectile.Center = new Vector2(Projectile.ai[0] + 10f*((float)Math.Sin(12f*(Projectile.frame*5+Projectile.frameCounter))),Projectile.ai[1]);

            UpdateAnimation();

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
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, Projectile.position);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = (target.Center-Projectile.Center).X > 0?1:-1;
        }
    }
}
