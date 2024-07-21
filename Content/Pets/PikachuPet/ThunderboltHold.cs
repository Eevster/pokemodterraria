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

namespace Pokemod.Content.Pets.PikachuPet
{
	public class ThunderboltHold : PokemonAttack
	{
		NPC targetEnemy;
		bool foundTarget = false;
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
		
		private static Asset<Texture2D> chainTexture;
        
        public override void Load()
        { 
            chainTexture = ModContent.Request<Texture2D>("Pokemod/Content/Pets/PikachuPet/ThunderboltChain");
        }

        public override void Unload()
        { 
            chainTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 30;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;

			Projectile.hide = true;
        }

        public override bool PreDrawExtras()
        {
            if(foundTarget){
                Vector2 center = Projectile.Center;
                Vector2 directionToOrigin = targetEnemy.Center - Projectile.Center;

                float distanceToOrigin = directionToOrigin.Length();

                while (distanceToOrigin > chainTexture.Width() && !float.IsNaN(distanceToOrigin))
                {
                    directionToOrigin /= distanceToOrigin; 
                    directionToOrigin *= chainTexture.Width(); 

                    center += directionToOrigin; 
                    directionToOrigin = targetEnemy.Center - center; 
                    distanceToOrigin = directionToOrigin.Length();


                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Frame(1, 3, 0, Projectile.frame), Color.White, directionToOrigin.ToRotation(),
                        chainTexture.Frame(1, 3).Size() / 2f, 1f, SpriteEffects.None, 0);
                }
            }
            
            return false;
        }

        public override void AI()
        {
			SearchTarget();

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
            }
        }

		private void SearchTarget(){
			float distanceFromTarget = 600f;
			Vector2 targetCenter = Projectile.Center;

			if(foundTarget){
				if(!targetEnemy.active || targetEnemy.life <=0){
					foundTarget = false;
				}
			}

			if (!foundTarget) {
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 100f;

						if(npc.boss){
							foundTarget = true;
							targetEnemy = npc;
							break;
						}

						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
							targetEnemy = npc;
						}
					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(24,0);
			Vector2 end = Projectile.Center - new Vector2(24,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			if(!foundTarget){
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 48f, ref collisionPoint);
			}else{
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 48f, ref collisionPoint) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, targetEnemy.Center, 32f, ref collisionPoint);
			}
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}
