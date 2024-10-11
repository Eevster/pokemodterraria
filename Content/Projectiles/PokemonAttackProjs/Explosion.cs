using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.DataStructures;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class Explosion : PokemonAttack
    {
        const int explosionSize = 600;
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 2;

            Projectile.tileCollide = true;  

            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            for (int g = 0; g < 32; g++)
            {
                int goreIndex = Gore.NewGore(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(Main.rand.NextFloat(-explosionSize*0.5f, explosionSize*0.5f), Main.rand.NextFloat(-explosionSize*0.5f, explosionSize*0.5f)), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 2f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 0.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 0.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center-new Vector2(explosionSize*0.5f, explosionSize*0.5f), explosionSize, explosionSize, DustID.Smoke, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].velocity *= 1f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 60; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center-new Vector2(explosionSize*0.5f, explosionSize*0.5f), explosionSize, explosionSize, DustID.Torch, 0f, 0f, 100, default(Color), 5f);
                
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 4f;
                dustIndex = Dust.NewDust(Projectile.Center-new Vector2(explosionSize*0.5f, explosionSize*0.5f), explosionSize, explosionSize, DustID.Torch, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 2f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 32; g++)
            {
                int goreIndex = Gore.NewGore(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(Main.rand.NextFloat(-explosionSize*0.5f, explosionSize*0.5f), Main.rand.NextFloat(-explosionSize*0.5f, explosionSize*0.5f)), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = Main.rand.NextFloat(2f, 4f);
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 0.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 0.5f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(explosionSize*0.5f,0);
			Vector2 end = Projectile.Center - new Vector2(explosionSize*0.5f,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, explosionSize, ref collisionPoint);
		}
    }
}