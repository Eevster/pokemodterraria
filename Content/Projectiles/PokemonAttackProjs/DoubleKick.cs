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
	public class DoubleKick : PokemonAttack
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 5;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            base.SetDefaults();
        }

        public override void AI()
        {
            UpdateAnimation();

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.CanBeChasedBy() && !target.boss){
                target.velocity += 4*(target.Center-Projectile.Center).SafeNormalize(Vector2.UnitX);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.velocity += 4*(target.Center-Projectile.Center).SafeNormalize(Vector2.UnitX);
            base.OnHitPlayer(target, info);
        }
    }
}
