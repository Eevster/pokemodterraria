using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class SolarBeamHold : PokemonAttack
	{
        private NPC targetEnemy;
		Vector2 enemyCenter;
		bool foundTarget = false;
        float maxLenght = 1500;
        bool canPlaySound = true;
		
		private static Asset<Texture2D> chainTexture;
        
        public override void Load()
        { 
            chainTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/SolarBeam");
        }

        public override void Unload()
        { 
            chainTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 70;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

			Projectile.light = 1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.1f;

            base.OnSpawn(source);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft < 35 && foundTarget){
                Vector2 center = Projectile.Center;
                Vector2 directionToOrigin = enemyCenter - Projectile.Center;

                float distanceToOrigin = directionToOrigin.Length();

                 Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Frame(1, 3, 0, 0), Color.White, directionToOrigin.ToRotation()-MathHelper.PiOver2,
                        chainTexture.Frame(1, 3).Size() / 2f, 1f, SpriteEffects.None, 0);

                while (distanceToOrigin > chainTexture.Width() && !float.IsNaN(distanceToOrigin))
                {
                    directionToOrigin /= distanceToOrigin; 
                    directionToOrigin *= chainTexture.Width(); 

                    center += directionToOrigin; 
                    directionToOrigin = enemyCenter - center; 
                    distanceToOrigin = directionToOrigin.Length();


                    Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                        chainTexture.Frame(1, 3, 0, (distanceToOrigin > chainTexture.Width())?1:2), Color.White, directionToOrigin.ToRotation()-MathHelper.PiOver2,
                        chainTexture.Frame(1, 3).Size() / 2f, 1f, SpriteEffects.None, 0);
                }
                return false;
            }
            
            return true;
        }

        public override void AI()
        {
            PokemonPlayer trainer = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>();

			if(Projectile.timeLeft > 35){
                if(Projectile.scale < 1f){
                    Projectile.scale += 0.03f;
                }else{
                    Projectile.scale = 1f;
                }
                if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
                    SearchTarget();
                }else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
                    foundTarget = true;
                }
            }else{
                if(canPlaySound){
                    SoundEngine.PlaySound(SoundID.Item67, Projectile.position);
                    canPlaySound = false;
                }
                if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
                    if(targetEnemy != null){
                        if(targetEnemy.active){
                            enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(targetEnemy.Center - Projectile.Center);
                        }
                    }
                }else{
                    enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(trainer.attackPosition - Projectile.Center);
                }
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		private void SearchTarget(){
			float distanceFromTarget = 1000f;
			Vector2 targetCenter = Projectile.Center;

			if (true) {
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
                            enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(npc.Center - Projectile.Center);
							break;
						}

						if (((closest && inRange) || !foundTarget) && !(npc.GetGlobalNPC<PokemonNPCData>().isPokemon && npc.GetGlobalNPC<PokemonNPCData>().shiny)) {
							distanceFromTarget = between;
                            targetEnemy = npc;
							targetCenter = npc.Center;
							foundTarget = true;
                            enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(npc.Center - Projectile.Center);
						}
					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center;
			Vector2 end = (Projectile.timeLeft < 35 && foundTarget)?enemyCenter:Projectile.Center;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 26f, ref collisionPoint);
		}
    }
}
