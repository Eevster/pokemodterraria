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
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class FireBlast : PokemonAttack
	{
        bool exploded = false;
        private static Asset<Texture2D> explosionTexture;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
        }
        
        public override void Load()
        { 
            explosionTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/FireBlastExplosion");
        }

        public override void Unload()
        { 
            explosionTexture = null;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(exploded);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            exploded = reader.ReadBoolean();
            base.ReceiveExtraAI(reader);
        }
		public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
            base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if(exploded){
                Main.EntitySpriteDraw(explosionTexture.Value, Projectile.Center - Main.screenPosition,
                    explosionTexture.Frame(1, 8, 0, Projectile.frame), Color.White, 0,
                    explosionTexture.Frame(1, 8).Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

                return false;
            }
            return true;
        }

        public override void AI()
        {
            if(!exploded){
                Projectile.rotation = Projectile.velocity.ToRotation();
                
                int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 200, default(Color), 2f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].position = Projectile.Center+new Vector2(0,-1).RotatedBy(Projectile.rotation)*Main.rand.Next(-20,21);
            }else{
                Projectile.rotation = 0;
            }

            UpdateAnimation();

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= (exploded?8:Main.projFrames[Projectile.type]))
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(!exploded){
                exploded = true;
                Projectile.frame = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale = 1.5f;
                if(Projectile.timeLeft < 60){
                    Projectile.timeLeft = 60;
                }
            }
            target.AddBuff(BuffID.OnFire, 8*60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if(!exploded){
                exploded = true;
                Projectile.frame = 0;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale = 1.5f;
                if(Projectile.timeLeft < 60){
                    Projectile.timeLeft = 60;
                }
            }
			target.AddBuff(BuffID.OnFire, 8*60);
            base.OnHitPlayer(target, info);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center-new Vector2(8*Projectile.scale, 8*Projectile.scale), (int)(16*Projectile.scale), (int)(16*Projectile.scale), DustID.Torch, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3), 100, default(Color), 2f);
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(exploded?66:28,0);
			Vector2 end = Projectile.Center - Projectile.scale*new Vector2(exploded?66:28,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*(exploded?132f:56f), ref collisionPoint);
		}
    }
}
