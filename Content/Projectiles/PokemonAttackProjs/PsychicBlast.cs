using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.NPCs;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class PsychicBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 120;

            Projectile.width = 62;
            Projectile.height = 62;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item96.WithVolumeScale(1.7f), Projectile.position);
            base.OnSpawn(source);
        }

        public override void AI()
        {
            UpdateAnimation();

            if (!Main.dedServ)
            {
                if (Projectile.timeLeft % 5 == 0)
                {
                    Dust newDust = Main.dust[Dust.NewDust(Projectile.Center, 10, 10, DustID.PinkTorch)];
                    newDust.noGravity = true;
                }
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }



        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    
                    Projectile.Kill();
                }
            }
        }
    }
}
