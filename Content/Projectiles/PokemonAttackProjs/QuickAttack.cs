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

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class QuickAttack : PokemonAttack
	{
		public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override void AI()
        {
            if(Main.rand.NextBool())
            {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Blue, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, default(Color), 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(0.5f,1.5f);
			}

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }
    }
}
