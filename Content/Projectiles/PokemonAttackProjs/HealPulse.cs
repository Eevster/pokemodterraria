using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class HealPulse : PokemonAttack
	{
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = 3;
            Projectile.Opacity = 0.6f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
			Projectile.hide = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			base.SetDefaults();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.1f;

            base.OnSpawn(source);
        }

		public override bool PreDraw(ref Color lightColor) {
			return false;
		}

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, Projectile.Opacity*0.3f, Projectile.Opacity, Projectile.Opacity*0.3f);

			Projectile.scale += 0.02f/Projectile.scale;

			if(Projectile.timeLeft%20==0){
				SoundEngine.PlaySound(SoundID.Item4 with {Volume = 0.5f}, Projectile.position);
				for (int i = 0; i < 30; i++)
				{
					int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.DryadsWard, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), Projectile.scale);
					Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndex].noGravity = true;
					Vector2 posAux = new Vector2(12*Projectile.scale,0).RotatedBy(MathHelper.ToRadians(12*i));
					posAux.Y *= 0.2f;
					posAux = posAux.RotatedBy(Projectile.rotation);
					Main.dust[dustIndex].position = Projectile.Center + posAux;
					Main.dust[dustIndex].velocity = 0.4f*posAux;
				}
				Vector2 start = Projectile.Center + Projectile.scale*new Vector2(50,0);
				Vector2 end = Projectile.Center - Projectile.scale*new Vector2(50,0);
				for (int k = 0; k < Main.maxPlayers; k++) {
					if(Main.player[k] != null){
						float collisionPoint = 0f;
						if(Collision.CheckAABBvLineCollision(Main.player[k].Hitbox.TopLeft(), Main.player[k].Hitbox.Size(), start, end, Projectile.scale*20f, ref collisionPoint)){
							HealEffect(Main.player[k]);
						}
					}
				}
			}

			if(Projectile.timeLeft < 10){
				Projectile.Opacity = 0.6f*Projectile.timeLeft*0.1f;
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public void HealEffect(Player player){
			player.Heal(player.statLifeMax2>300?3:2);

			for(int i = 0; i < 15; i++){
				int dustIndex = Dust.NewDust(player.Center-0.5f*new Vector2(player.width,player.height), 64, 64, DustID.DryadsWard, 0f, 0f, 200, default(Color), 1f);
				Main.dust[dustIndex].noGravity = true;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(50,0);
				Vector2 end = Projectile.Center - Projectile.scale*new Vector2(50,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*20f, ref collisionPoint);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}
