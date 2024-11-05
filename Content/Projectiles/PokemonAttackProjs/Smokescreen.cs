using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Smokescreen : PokemonAttack
	{
        const int explosionSize = 80;
		float explosionScale = 1f;
		bool exploded = false;
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.hostile = false; 
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true; 
			Projectile.light = 0.5f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;

			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 0, Color.Black, 0f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = (float)Main.rand.Next(100, 125) * 0.008f;

			if(!exploded){
				Projectile.velocity.Y += 0.25f;
			}else{
                float explosionFinalSize = explosionScale*explosionSize;
                for(int g = 0; g < 2; g++)
                {
                    int goreIndex = Gore.NewGore(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(Main.rand.NextFloat(-explosionFinalSize*0.5f, explosionFinalSize*0.5f), Main.rand.NextFloat(-explosionFinalSize*0.5f, explosionFinalSize*0.5f)), default(Vector2), 99, 1f);
                    Main.gore[goreIndex].scale = Main.rand.NextFloat(0.5f, 1f);
                    Main.gore[goreIndex].velocity *= 0.5f;
                }
            }
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(!exploded){
				Explode();
			}
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if(!exploded){
				Explode();
			}else{
                target.AddBuff(BuffID.Darkness, 3*60);
            }
            base.OnHitPlayer(target, info);
        }

		public override bool OnTileCollide (Vector2 oldVelocity){
			if(!exploded){
				Explode();
			}

			return false;
		}

        private void Explode(){
			Projectile.timeLeft = 90;
            Projectile.velocity = Vector2.Zero;
            exploded = true;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.oldPosition + 0.5f*new Vector2(Projectile.width, Projectile.height) - 0.5f*Projectile.width*Projectile.velocity.SafeNormalize(Vector2.UnitX);
			Vector2 end = Projectile.Center + 0.5f*Projectile.width*Projectile.velocity.SafeNormalize(Vector2.UnitX);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			if(exploded){
				start = Projectile.Center-new Vector2(explosionScale*explosionSize*0.5f, 0);
				end = Projectile.Center+new Vector2(explosionScale*explosionSize*0.5f, 0);
			}
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, exploded?explosionScale*explosionSize:Projectile.height, ref collisionPoint);
		}
    }
}
