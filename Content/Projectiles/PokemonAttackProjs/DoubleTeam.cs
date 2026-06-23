using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.GlobalNPCs;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class DoubleTeam : PokemonAttack
	{
        public override bool CanExistIfNotActualMove => false;
        private const float amplitude = 64;
        private float xPos = 0;
        private float xVel = 0;
        private const float maxVel = 60;
        private const float xAcc = 3f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
		public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.Opacity = 0.8f;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<DoubleTeam>(), 0, 0f, pokemon.owner,  targetCenter.X, targetCenter.Y, Projectile.scale)];
						SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.ApplyStatMod(6, 1);
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

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = texture.Size() / 2f;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, texture.Bounds, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

            if(pokemonProj != null && pokemonProj.active && pokemonProj.ModProjectile is PokemonPetProjectile pokemon)
            {
                string TextureName = $"Assets/Textures/Pokesprites/Pets/{pokemon.GetType().Name}";

				if(pokemon.variant != null && pokemon.variant != "") TextureName += "_" + pokemon.variant;

                Texture2D frontTexture = Mod.Assets.Request<Texture2D>(TextureName).Value;

				Vector2 positionOffset = (frontTexture.Frame(1, pokemon.totalFrames).Size() * Vector2.UnitY * 0.5f) - Vector2.UnitY * 4f;

				Main.EntitySpriteDraw(frontTexture, pokemon.Projectile.Bottom - pokemon.Projectile.scale * positionOffset - Main.screenPosition + Vector2.UnitX * amplitude*(float)Math.Sin(xPos),
                frontTexture.Frame(1,pokemon.totalFrames,0,pokemon.Projectile.frame),Projectile.GetAlpha(lightColor), pokemon.Projectile.rotation,
                frontTexture.Frame(1,pokemon.totalFrames).Size() * 0.5f, pokemon.Projectile.scale, pokemon.Projectile.spriteDirection>0?SpriteEffects.None:SpriteEffects.FlipHorizontally);

                Main.EntitySpriteDraw(frontTexture, pokemon.Projectile.Bottom - pokemon.Projectile.scale * positionOffset - Main.screenPosition - Vector2.UnitX * amplitude*(float)Math.Sin(xPos),
                frontTexture.Frame(1,pokemon.totalFrames,0,pokemon.Projectile.frame),Projectile.GetAlpha(lightColor), pokemon.Projectile.rotation,
                frontTexture.Frame(1,pokemon.totalFrames).Size() * 0.5f, pokemon.Projectile.scale, pokemon.Projectile.spriteDirection>0?SpriteEffects.None:SpriteEffects.FlipHorizontally);
            }

			return true;
		}

        public override void AI()
        {
            if (Projectile.timeLeft == 45)
            {
                SetExpTarget(out NPC target);
            }

            if (xVel < maxVel)
            {
                xVel += xAcc;
            }

            xPos += xVel;
            if(xPos > 360)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                xPos %= 360;
            }

            Projectile.Center += new Vector2(amplitude*(float)Math.Sin(xPos), 0) * Projectile.ai[2];


            if(Projectile.timeLeft < 10f)
            {
                Projectile.Opacity = 1f*Projectile.timeLeft/10f;
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
