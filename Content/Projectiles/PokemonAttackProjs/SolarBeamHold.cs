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
		Vector2 enemyCenter;
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

            Projectile.hide = true;
            base.SetDefaults();
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
                    SearchTarget(1000f, false);

                    if(targetPlayer != null){
                        if(targetPlayer.active && !targetPlayer.dead){
                            enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(targetPlayer.Center - Projectile.Center);
                        }else{
                            targetPlayer = null;
                        }
                    }else if(targetEnemy != null){
                        if(targetEnemy.active){
                            enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(targetEnemy.Center - Projectile.Center);
                        }else{
                            targetEnemy = null;
                        }
                    }
                }else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
                    enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(Trainer.attackPosition - Projectile.Center);
                    foundTarget = true;
                }
            }else{
                if(canPlaySound){
                    SoundEngine.PlaySound(SoundID.Item67, Projectile.position);
                    canPlaySound = false;
                }
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center;
			Vector2 end = (Projectile.timeLeft < 35 && foundTarget)?enemyCenter:Projectile.Center;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 26f, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
