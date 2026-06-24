using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class FutureSight : PokemonAttack
	{
		private bool exploded = false;
		private const float sizeChangeRate = 0.2f;
		private float size = 0.2f;
		private const float timeToExplode = 120;

		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 240;
			Projectile.penetrate = 4;
			Projectile.scale = 1f;
			Projectile.light = 1f;
			Projectile.Opacity = 0.9f;
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
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter + GetAuxPositionForMovingTarget(targetCenter, timeToExplode), Vector2.Zero, ModContent.ProjectileType<FutureSight>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item43, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if(exploded){
				Main.instance.LoadProjectile(Projectile.type);
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				Color color = Projectile.GetAlpha(lightColor)*Projectile.Opacity;

				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
				texture.Frame(1,Main.projFrames[Projectile.type],0,Projectile.frame), color, Projectile.rotation,
				texture.Frame(1,Main.projFrames[Projectile.type]).Size() * 0.5f, Projectile.scale*size, SpriteEffects.None, 0);
			}

			return false;
		}

		public override void AI()
		{
			if(Projectile.timeLeft > 240-timeToExplode)
			{
				int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.FireworkFountain_Pink, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), 0.5f);
				Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndex].noGravity = true;
				Vector2 posAux = new Vector2(Projectile.width*Projectile.scale - 8,0).RotatedBy(MathHelper.ToRadians(3*Projectile.timeLeft));
				posAux = posAux.RotatedBy(Projectile.rotation);
				Main.dust[dustIndex].position = Projectile.Center + posAux;
				Main.dust[dustIndex].velocity = Vector2.Zero;
			}
			else
			{
				if (!exploded)
				{
					SoundEngine.PlaySound(SoundID.Item105, Projectile.position);
					exploded = true;
				}
				Lighting.AddLight(Projectile.Center, Projectile.Opacity, Projectile.Opacity*0.3f, Projectile.Opacity * 0.3f);

				if (size < 2f)
				{
					size += sizeChangeRate;
				}

				UpdateAnimation();
			}

			if(Projectile.timeLeft < 20f)
            {
                Projectile.Opacity = 0.9f*Projectile.timeLeft/20f;
            }

			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
			}
		}

		public override bool? CanDamage()
        {
            return exploded && !inPokemonBattle;
        }

		private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
					SoundEngine.PlaySound(SoundID.Item43 with {Volume = 0.5f}, Projectile.position);
                    Projectile.frame = 0;
                }
            }
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center + size*Projectile.scale*0.5f*new Vector2(Projectile.width,0);
			Vector2 end = Projectile.Center - size*Projectile.scale*0.5f*new Vector2(Projectile.width,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, size*Projectile.scale*Projectile.height, ref collisionPoint);
		}
	}
}
