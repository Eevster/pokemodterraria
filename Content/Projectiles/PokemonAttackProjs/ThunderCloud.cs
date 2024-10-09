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
	public class ThunderCloud : PokemonAttack
	{
        bool shooted = false;
        float attackLength = 0;
        Vector2 finalPosition;
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
		
		private static Asset<Texture2D> chainTexture;
        private static Asset<Texture2D> lightTexture;
        
        public override void Load()
        { 
            chainTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderProj");
            lightTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ThunderCloudLight");
        }

        public override void Unload()
        { 
            chainTexture = null;
            lightTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 28;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.hide = true;
        }

        public override bool PreDrawExtras()
        {
            if(shooted){
                Vector2 center = Projectile.Center;
                Vector2 directionToOrigin = finalPosition - center;

                float distanceToOrigin = directionToOrigin.Length();
                int frameAux = 0;

                while (distanceToOrigin > chainTexture.Width()-4 && !float.IsNaN(distanceToOrigin))
                {
                    directionToOrigin /= distanceToOrigin; 
                    directionToOrigin *= chainTexture.Width()-4; 

                    center += directionToOrigin; 
                    directionToOrigin = finalPosition - center; 
                    distanceToOrigin = directionToOrigin.Length();

                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Frame(1, 7, 0, 1+(frameAux+Projectile.frame)%5), Color.White, 0,
                        chainTexture.Frame(1, 7).Size() / 2f, 1f, SpriteEffects.None, 0);
                    
                    frameAux++;
                }

                Main.EntitySpriteDraw(chainTexture.Value, Projectile.Center - Main.screenPosition,
                        chainTexture.Frame(1, 7, 0, 0), Color.White, 0,
                        chainTexture.Frame(1, 7).Size() / 2f, 1f, SpriteEffects.None, 0);

                Main.EntitySpriteDraw(chainTexture.Value, finalPosition - Main.screenPosition,
                        chainTexture.Frame(1, 7, 0, 6), Color.White, 0,
                        chainTexture.Frame(1, 7).Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Main.EntitySpriteDraw(lightTexture.Value, Projectile.Center - Main.screenPosition,
                lightTexture.Frame(1, 6, 0, Projectile.frame), Color.White, Projectile.rotation,
                lightTexture.Frame(1, 6).Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            base.PostDraw(lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.Opacity = 0;
            base.OnSpawn(source);
        }

        public override void AI()
        {
			ShootAttack();

            if(Projectile.Opacity < 1f){
                Projectile.Opacity += 0.1f;
            }

			UpdateAnimation();

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
                }
                if(Projectile.frame >= 3){
                    if(Projectile.frame == 3){
                        SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
                    }
                    shooted = true;
                }else{
                    shooted = false;
                }
            }
        }

		private void ShootAttack(){
            float averageLengthSample = 0f;

            if(shooted){
                float[] laserScanResults = new float[3];
                Collision.LaserScan(Projectile.Center, new Vector2(0,1), 22 * Projectile.scale, 2500, laserScanResults);
                for (int i = 0; i < laserScanResults.Length; ++i) {
                    averageLengthSample += laserScanResults[i];
                }
                averageLengthSample /= 3;

                if(averageLengthSample < 420){
                    averageLengthSample = 420;
                }

                attackLength += 200;
            }
            else{
                attackLength = 0;
            }

            finalPosition = Projectile.Center + new Vector2(0,1)*Math.Clamp(attackLength, 0, averageLengthSample);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			if(!shooted){
				return false;
			}else{
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, finalPosition, 32f, ref collisionPoint);
			}
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}
