using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace Pokemod.Content.Projectiles.BossProjectiles
{
    public class MewtwoBossOverheat : PokemonBossProjectile
    {
        public int radius = 0;
        public int startRadius = 16;
        public int maxRadius = 800;
        public int duration = 300;

        public override int attackType => (int)TypeIndex.Fire;
        public override bool isSpecial => true;

        public override string Texture => "Pokemod/Content/Projectiles/BossProjectiles/MewtwoBossOverheatGlow";

        public override void SetDefaults()
        {
            Projectile.width = startRadius; 
            Projectile.height = startRadius;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = duration;
            Projectile.alpha = 255;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            radius = (int)((maxRadius - startRadius) * Math.Pow(Math.Clamp((duration - Projectile.timeLeft) / (float)duration, 0f, 1f), 0.7f)) + startRadius;
            TrackOwner();
            FireStorm();
        }

        public void FireStorm()
        {
            if (Main.rand.NextBool(20)) SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Pitch = -0.5f }, Projectile.Center);

            for (int i = 0; i < 10; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(radius * 0.125f, radius * 1f);
                Vector2 offset = Vector2.One.RotatedBy(angle) * distance;
                Vector2 velocity = offset.RotatedBy(MathHelper.PiOver2) * 0.05f;

                int dust = Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.TerraBlade, velocity.X, velocity.Y, Scale: 3f);
                Main.dust[dust].noGravity = true;
                if (Main.rand.NextBool(3))
                {
                    dust = Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.FireworksRGB, velocity.X, velocity.Y, Alpha: 200, newColor: new Color(125, 255, 45));
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 offset = radius * 1.3f * Vector2.UnitX.RotatedBy(boss.StateTimer / 1f);
            Vector2 start = Projectile.Center - offset;
            Vector2 end = Projectile.Center + offset;
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, radius, ref collisionPoint);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D sparkleTexture = TextureAssets.Extra[ExtrasID.SharpTears].Value;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color color = new Color(125, 255, 45);
            float rotation = boss.StateTimer;
            Vector2 scale = new Vector2(1.5f, radius / 20f);
            Main.EntitySpriteDraw(sparkleTexture, Projectile.Center - Main.screenPosition, null, color, MathHelper.PiOver2 + rotation, origin, scale, SpriteEffects.None);
            return false;
        }
    }
}
