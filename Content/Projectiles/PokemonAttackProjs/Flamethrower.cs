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
	public class Flamethrower : PokemonAttack
	{
		Vector2 enemyCenter;
        float maxLenght = 400;
        int soundTimer = 0;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(enemyCenter);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            enemyCenter = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }

		public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

			Projectile.light = 1f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.1f;
            enemyCenter = Projectile.Center + maxLenght*Vector2.Normalize(Projectile.velocity);
            Projectile.velocity = Vector2.Zero;

            base.OnSpawn(source);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D chainTexture = TextureAssets.Projectile[Type].Value;
            
            Vector2 center = Projectile.Center;
            Vector2 directionToOrigin = enemyCenter - Projectile.Center;

            float distanceToOrigin = directionToOrigin.Length();
            float chainScale = 0.3f;
            float rotationAux = 0;

            while (distanceToOrigin > 20 && !float.IsNaN(distanceToOrigin))
            {
                directionToOrigin /= distanceToOrigin; 
                directionToOrigin *= 20; 

                center += directionToOrigin; 
                directionToOrigin = enemyCenter - center; 
                distanceToOrigin = directionToOrigin.Length();

                rotationAux += MathHelper.ToRadians(10);

                Main.EntitySpriteDraw(chainTexture, center - Main.screenPosition + (-20f*((float)soundTimer/20f)+10f)*Vector2.Normalize(directionToOrigin),
                    chainTexture.Bounds, Color.White*0.5f, directionToOrigin.ToRotation()+MathHelper.ToRadians(18f*soundTimer)+rotationAux,
                    chainTexture.Bounds.Size() / 2f, chainScale, SpriteEffects.None, 0);

                chainScale += 0.04f;
            }
            
            return false;
        }

        public override void AI()
        {
            PokemonPlayer trainer = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>();
            
            if(--soundTimer < 0){
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                soundTimer = 20;
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
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire, 5*60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.AddBuff(BuffID.OnFire, 5*60);
            base.OnHitPlayer(target, info);
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center;
			Vector2 end = enemyCenter;
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 48f, ref collisionPoint);
		}
    }
}
