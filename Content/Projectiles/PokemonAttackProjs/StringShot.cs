using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Buffs;
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
	public class StringShot : PokemonAttack
	{
		public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 1;
        }

        public override bool PreDrawExtras()
        {
            Asset<Texture2D> chainTexture = TextureAssets.Projectile[Projectile.type];

            Vector2 center = Projectile.Center;
            Vector2 directionToOrigin = pokemonProj.Center - Projectile.Center;

            float distanceToOrigin = directionToOrigin.Length();

            while (distanceToOrigin > chainTexture.Width() && !float.IsNaN(distanceToOrigin))
            {
                directionToOrigin /= distanceToOrigin; 
                directionToOrigin *= chainTexture.Width(); 

                center += directionToOrigin; 
                directionToOrigin = pokemonProj.Center - center; 
                distanceToOrigin = directionToOrigin.Length();

                Color drawColor = Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16));

                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                    chainTexture.Value.Bounds, drawColor, directionToOrigin.ToRotation(),
                    chainTexture.Size() / 2f, 1f, SpriteEffects.None, 0);
            }
            
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(target.boss){
                target.AddBuff(ModContent.BuffType<StringShotDebuff>(), 20);
            }else{
                target.AddBuff(ModContent.BuffType<StringShotDebuff>(), 60);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.AddBuff(ModContent.BuffType<StringShotDebuff>(), 60);
            base.OnHitPlayer(target, info);
        }
    }
}
