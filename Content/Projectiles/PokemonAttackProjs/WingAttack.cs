using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Dusts;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class WingAttack : PokemonAttack
	{
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/QuickAttack";
		public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;
            
            Projectile.knockBack = 6f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<FeatherDust>(), Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 50, default(Color), 0f);
			Main.dust[dust].scale = Main.rand.NextFloat(0.5f,1f);

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }
    }
}
