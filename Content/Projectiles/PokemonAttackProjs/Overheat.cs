using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class OverHeat : PokemonAttack
    {
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/LavaPlume";

        public override void SetDefaults()
        {
            Projectile.width = 512;
            Projectile.height = 512;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.Opacity = 0.6f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;

            Projectile.hide = true;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			for(int i = 0; i < 20; i++){
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos2, new Color(255, 166, 45, 0) * 0.5f, new Color(255, 77, 0), 0.5f, 0f, 0.5f, 0.5f, 1f, Projectile.rotation+ MathHelper.ToRadians(i*360f/20f) + MathHelper.ToRadians(Main.rand.Next(-8,9)), 4*new Vector2(2f, Utils.Remap(0.5f, 0f, 1f, 4f, 1f)) * Projectile.scale, 8*Vector2.One * Projectile.scale);
            }

            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.1f;

            base.OnSpawn(source);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.timeLeft = 20;

            Lighting.AddLight(Projectile.Center, Projectile.Opacity, Projectile.Opacity*0.4f, Projectile.Opacity*0.3f);

            if(Projectile.scale < 1.5f){
                Projectile.scale += 0.02f/Projectile.scale;
                if(Projectile.scale>1f){
                    Projectile.Opacity = 1.2f*Math.Clamp(1.5f-Projectile.scale, 0f, 1f);
                }
            }else{
                Projectile.Kill();
            }

            if(++Projectile.localAI[0] > 10){
                for (int i = 0; i < 40; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Torch, 0, 0, 150, default(Color), 1f);
                    Main.dust[dustIndex].scale = i<20?1f:1.5f;
                    Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].position = Projectile.Center;
                    Main.dust[dustIndex].velocity = -(i<20?12f:10f)*new Vector2(1,0).RotatedBy((i+1)*MathHelper.Pi/10);
                }
                Projectile.localAI[0] = 0;
            };
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            if(target.CanBeChasedBy()){
                if(Vector2.Distance(target.Center, player.Center) > 0){
                    target.velocity += 4*Vector2.Normalize(target.Center-player.Center)*target.knockBackResist;
                }
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Player player = Main.player[Projectile.owner];

            if(Vector2.Distance(target.Center, player.Center) > 0){
                target.velocity += 4*Vector2.Normalize(target.Center-player.Center);
            }
            base.OnHitPlayer(target, info);
        }


        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 94, 24)*Projectile.Opacity;
        }

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center - Projectile.scale*256*new Vector2(0,-1).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + Projectile.scale*256*new Vector2(0,-1).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*512f, ref collisionPoint);
		}

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}
