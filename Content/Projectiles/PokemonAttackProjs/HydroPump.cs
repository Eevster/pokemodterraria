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
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class HydroPump : PokemonAttack
	{
		float projSpeed = 20f;
		private bool canTrack = true;
        private static Asset<Texture2D> bodyTexture;
        private const int nBody = 10;
        private Vector2[] bodyPositions;
        private Vector2[] bodyRotations;
        
        public override void Load()
        { 
            bodyTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/HydroPump_Tail");
        }
        public override void Unload()
        {
            bodyTexture = null;
        }
		public override void OnSpawn(IEntitySource source)
        {
			Projectile.damage = (int)(Projectile.damage*0.5f); 

			if(Projectile.owner == Main.myPlayer){
				bodyPositions = new Vector2[nBody];
				bodyRotations = new Vector2[nBody];
				for(int i = 0; i < nBody; i++){
					bodyPositions[i] = Projectile.Center;
					bodyRotations[i] = new Vector2(1,0);
				}
			}
            base.OnSpawn(source);
        }
		public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			base.SetDefaults();
        }

		public override bool PreDraw(ref Color lightColor)
        {
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nBody; i++){
					Main.EntitySpriteDraw(bodyTexture.Value, bodyPositions[i] - Main.screenPosition,
					bodyTexture.Value.Bounds, lightColor*Projectile.Opacity,
					bodyRotations[i].ToRotation(), bodyTexture.Size() * 0.5f, 1f*((1f*nBody-i)/(1f*nBody))*Projectile.scale, SpriteEffects.None, 0);
				}
			}

            return true;
        }

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();
		
			if(Projectile.timeLeft < 115){
				if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
					foundTarget = true;
				}else{
					SearchTarget(800f);
				}
			}

			if(attackMode == (int)PokemonPlayer.AttackMode.No_Attack || !canTrack){
				foundTarget = false;
			}

			if(foundTarget){
				if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
					Projectile.velocity +=  0.2f*(Trainer.attackPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;

					if(Projectile.velocity.Length() > projSpeed){
						Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
					}

					if(Vector2.Distance(Projectile.Center, Trainer.attackPosition) < 2*Projectile.velocity.Length()){
						canTrack = false;
						foundTarget = false;
					}
				}else{
					if(targetPlayer != null){
						if(targetPlayer.active && !targetPlayer.dead){
							Projectile.velocity +=  0.2f*(targetEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;

							if(Projectile.velocity.Length() > projSpeed){
								Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
							}
						}else{
							targetPlayer = null;
						}
					}else if(targetEnemy != null){
						if(targetEnemy.active){
							Projectile.velocity +=  0.2f*(targetEnemy.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;

							if(Projectile.velocity.Length() > projSpeed){
								Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
							}
						}else{
							targetEnemy = null;
						}
					}else{
						Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
					}
				}
			}else{
				Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
			}

			if(Projectile.owner == Main.myPlayer){
				for(int i = nBody-1; i > 0; i--){
					bodyPositions[i] = (bodyPositions[i]+bodyPositions[i-1])/2;
				}
				const int bodyMaxDist = 6;
				
				if(Vector2.Distance(bodyPositions[0], Projectile.Center) > 0.5f*bodyMaxDist){
					bodyPositions[0] = Projectile.Center+0.5f*bodyMaxDist*Vector2.Normalize(bodyPositions[0]-Projectile.Center);
				}
				bodyRotations[0] = Vector2.Normalize(bodyPositions[0]-Projectile.Center);

				for(int i = 1; i < nBody; i++){
					if(Vector2.Distance(bodyPositions[i], bodyPositions[i-1]) > bodyMaxDist){
						bodyPositions[i] = bodyPositions[i-1]+bodyMaxDist*Vector2.Normalize(bodyPositions[i]-bodyPositions[i-1]);
					}
					bodyRotations[i] = Vector2.Normalize(bodyPositions[i]-bodyPositions[i-1]);
				}
			}

			if(Projectile.timeLeft < 10){
				Projectile.Opacity = 0.1f*Projectile.timeLeft;
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + Projectile.scale*20*Vector2.Normalize(Projectile.velocity);
			Vector2 end = Projectile.Center - Projectile.scale*20*Vector2.Normalize(Projectile.velocity);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*40f, ref collisionPoint);
		}

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center-new Vector2(15*Projectile.scale, 15*Projectile.scale), (int)(30*Projectile.scale), (int)(30*Projectile.scale), DustID.Water, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3), 100, default(Color), 2f);
            }
        }
    }
}
