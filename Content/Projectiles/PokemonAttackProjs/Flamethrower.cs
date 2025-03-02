using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Flamethrower : PokemonAttack
	{
		private ref float scaleAux => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true; 

            Projectile.timeLeft = 24;

            Projectile.light = 1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            scaleAux = 0.3f;
            Projectile.damage = (int)(Projectile.damage*0.25f); 
            Projectile.Opacity = 0.2f;
            Projectile.rotation = 0;
            base.OnSpawn(source);
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(40);

            Projectile.scale = scaleAux;
            scaleAux += 0.04f;

            Projectile.velocity.Y -= 0.1f;

            if(Projectile.timeLeft > 12){
                Projectile.Opacity += 0.02f;
            }

            if(Projectile.timeLeft < 5){
                Projectile.Opacity -= 0.1f;
            }

            if (Main.myPlayer == Projectile.owner){
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 5*60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 5*60);
            base.OnHitPlayer(target, info);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center+Projectile.scale*new Vector2(40,0).RotatedBy(Projectile.rotation);
            Vector2 end = Projectile.Center-Projectile.scale*new Vector2(40,0).RotatedBy(Projectile.rotation);
            float projWidth = 80;
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, projWidth*Projectile.scale, ref collisionPoint);
        }

        public override bool OnTileCollide (Vector2 oldVelocity){
			Projectile.velocity.Y = 0;

			// If the projectile hits the left or right side of the tile, reverse the X velocity
			if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon) {
                Projectile.velocity.X = 0;
			}

			// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
			if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon) {
				Projectile.velocity.Y = 0;
			}

			return false;
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            fallThrough = true;
			
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
