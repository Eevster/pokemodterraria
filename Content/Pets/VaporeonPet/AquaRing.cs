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

namespace Pokemod.Content.Pets.VaporeonPet
{
	public class AquaRing : PokemonAttack
	{
		private static Asset<Texture2D> ringTexture;
        
        public override void Load()
        { 
            ringTexture = ModContent.Request<Texture2D>("Pokemod/Content/Pets/VaporeonPet/AquaRingVisual");
        }

        public override void Unload()
        { 
            ringTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.Opacity = 0.5f;

			Projectile.hide = true;
        }

        public override bool PreDrawExtras()
        {
            if(Projectile.timeLeft < 90){
                for(int i = 0; i < 3; i++){
                    float timeAux = 90-(Projectile.timeLeft+5*i);
                    float ringRotation;

                    if(timeAux>=0 && timeAux<=80){
                        ringRotation = (float)(1f-Math.Cos(timeAux*MathHelper.Pi/80))*MathHelper.ToRadians(360);
                    }else{
                        ringRotation = 0;
                    }
                    
                    Color color = Color.Lerp(Color.LightBlue, Color.LightGreen, (float)(1f+Math.Sin(MathHelper.ToRadians(30)*(Projectile.timeLeft + i)))/2);

                    Main.EntitySpriteDraw(ringTexture.Value, Projectile.Center - Main.screenPosition,
                        ringTexture.Value.Bounds, color, ringRotation,
                        ringTexture.Size() / 2f, 1f, SpriteEffects.None, 0);
                }
            }
            
            return false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0,1,2));

            if(Projectile.timeLeft%10 == 9) SoundEngine.PlaySound(SoundID.Item4, Projectile.position);

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.CanBeChasedBy()){
                Player player = Main.player[Projectile.owner];
                player.Heal(player.statLifeMax2>300?2:1);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// "Hit anything between the player and the tip of the sword"
			// shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
			Vector2 start = Projectile.Center + new Vector2(76,0);
			Vector2 end = Projectile.Center - new Vector2(76,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 152f, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}
