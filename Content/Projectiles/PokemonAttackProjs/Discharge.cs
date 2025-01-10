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
	public class Discharge : PokemonAttack
	{
        Vector2 LightVector;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
		private static Asset<Texture2D> LightTexture;
        
        public override void Load()
        { 
            LightTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/DischargeProj");
        }

        public override void Unload()
        { 
            LightTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.Opacity = 0.8f;
            Projectile.light = 1f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override bool PreDrawExtras()
        {
            if(Projectile.timeLeft < 60){
                for(int i = 0; i < 3; i++){
                    float LightRotation = LightVector.RotatedBy(i*MathHelper.ToRadians(120)).ToRotation();

                    Main.EntitySpriteDraw(LightTexture.Value, Projectile.Center + Projectile.scale*new Vector2(60,0).RotatedBy(LightRotation) - Main.screenPosition,
                        LightTexture.Frame(1, 4, 0, Main.rand.Next(0,4)), Color.White, LightRotation,
                        LightTexture.Frame(1, 4).Size() / 2f, Projectile.scale*0.3f, SpriteEffects.None, 0);
                }
            }
            
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Projectile.scale*new Vector3(2,2,0));

            Projectile.scale += 0.025f;

            UpdateAnimation();

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                LightVector = new Vector2(1,0).RotatedByRandom(MathHelper.TwoPi);
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(75,0);
			Vector2 end = Projectile.Center - Projectile.scale*new Vector2(75,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*150f, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}
