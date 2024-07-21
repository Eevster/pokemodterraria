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

namespace Pokemod.Content.Pets.BulbasaurPet
{
	public class VineWhipBack : PokemonAttack
	{
		private NPC targetEnemy;
		private bool foundTarget = false;
		private bool canDamage = true;
		private int projDirection = 1;
		Vector2 vectorToTarget;
		Vector2 positionAjust;
		float positionRotation;
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(vectorToTarget);
			writer.Write((double)projDirection);
			writer.WriteVector2(positionAjust);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            vectorToTarget = reader.ReadVector2();
			projDirection = (int)reader.ReadDouble();
			positionAjust = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
        }
		public override void SetDefaults()
        {
            Projectile.width = 156;
            Projectile.height = 76;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.Opacity = 0;
			canDamage = false;
			Projectile.hide = true;
			projDirection = (int)Projectile.ai[1];

			vectorToTarget = new Vector2(projDirection,0);
			positionRotation = vectorToTarget.ToRotation();
			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);
        }

        public override void AI()
        {
			SearchTarget();

			Projectile.timeLeft = 10;

			Projectile.direction = Projectile.spriteDirection = projDirection;

			if(foundTarget){
				vectorToTarget = targetEnemy.Center-Projectile.Center;
			}

			if(vectorToTarget.Length() > 20f){
				if(vectorToTarget.X>0){
					Projectile.rotation = vectorToTarget.ToRotation();
					Projectile.direction = Projectile.spriteDirection = projDirection = 1;
				}
				if(vectorToTarget.X<0){
					Projectile.rotation = (float)Math.PI + vectorToTarget.ToRotation();
					Projectile.direction = Projectile.spriteDirection = projDirection = -1;
				}
				positionRotation = vectorToTarget.ToRotation();
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}

			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);

			Projectile.Center += positionAjust;

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
				if(Projectile.Opacity == 0 && Projectile.frame == 3){
					Projectile.Opacity = 1f;
					canDamage = true;
				}
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool? CanDamage()
        {
            return canDamage;
        }

        private void SearchTarget(){
			float distanceFromTarget = 160f;
			Vector2 targetCenter = Projectile.Center;

			foundTarget = false;

			if (!foundTarget) {
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;

						if(npc.boss){
							foundTarget = true;
							targetEnemy = npc;
							break;
						}

						if ((closest && inRange) || !foundTarget) {
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
			Vector2 start = Projectile.Center - new Vector2(78,0).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + new Vector2(78,0).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 76, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }

	public class VineWhipFront : PokemonAttack
	{
		private NPC targetEnemy;
		private bool foundTarget = false;
		private bool canDamage = true;
		private int projDirection = 1;
		Vector2 vectorToTarget;
		Vector2 positionAjust;
		float positionRotation;
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(vectorToTarget);
			writer.Write((double)projDirection);
			writer.WriteVector2(positionAjust);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            vectorToTarget = reader.ReadVector2();
			projDirection = (int)reader.ReadDouble();
			positionAjust = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
        }
		public override void SetDefaults()
        {
            Projectile.width = 156;
            Projectile.height = 76;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.light = 1f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
			projDirection = (int)Projectile.ai[1];
			vectorToTarget = new Vector2(projDirection,0);
        	positionRotation = vectorToTarget.ToRotation();
			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);
        }

        public override void AI()
        {
			SearchTarget();

			Projectile.timeLeft = 10;

			Projectile.direction = Projectile.spriteDirection = projDirection;

			if(foundTarget){
				vectorToTarget = targetEnemy.Center-Projectile.Center;
			}

			if(vectorToTarget.Length() > 20f){
				if(vectorToTarget.X>0){
					Projectile.rotation = vectorToTarget.ToRotation();
					Projectile.direction = Projectile.spriteDirection = projDirection = 1;
				}
				if(vectorToTarget.X<0){
					Projectile.rotation = (float)Math.PI + vectorToTarget.ToRotation();
					Projectile.direction = Projectile.spriteDirection = projDirection = -1;
				}
				positionRotation = vectorToTarget.ToRotation();
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}

			positionAjust = new Vector2(56,-18*projDirection).RotatedBy(positionRotation);

			Projectile.Center += positionAjust;

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

        public override bool? CanDamage()
        {
            return canDamage;
        }

        private void SearchTarget(){
			float distanceFromTarget = 300f;
			Vector2 targetCenter = Projectile.Center;

			foundTarget = false;

			if (!foundTarget) {
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;

						if(npc.boss){
							foundTarget = true;
							targetEnemy = npc;
							break;
						}

						if ((closest && inRange) || !foundTarget) {
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
			Vector2 start = Projectile.Center - new Vector2(78,0).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + new Vector2(78,0).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 76, ref collisionPoint);
		}
    }
}
