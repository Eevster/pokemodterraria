using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class ShockWave : PokemonAttack
	{
		float animOffset = 0;
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetPosition);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPosition = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }

        public override void SetDefaults()
        {
            Projectile.width = 102;
            Projectile.height = 102;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = 3;
            Projectile.Opacity = 0.8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item4, pokemon.position);
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter, Vector2.Zero, ModContent.ProjectileType<ShockWave>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			/*Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int nFrames = Main.projFrames[Projectile.type];

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Frame(1,nFrames,0,0).Width * 0.5f, texture.Frame(1,nFrames,0,0).Height * 0.5f);

            float initialScale = 0.2f;
			float finalScale = 1f;
			float currentScale = initialScale+animOffset;

			float nWaves = 3f;
			float separation = 1f/nWaves;

            while (currentScale <= finalScale)
            {
                Color color = Projectile.GetAlpha(lightColor)*Projectile.Opacity;

                color *= 0.4f+0.6f*(finalScale-currentScale)/(finalScale-initialScale);

                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(1,nFrames,0,Projectile.frame), color, Projectile.rotation, drawOrigin, currentScale*Projectile.scale, SpriteEffects.None, 0);
				if (pokemonProj != null && pokemonProj.active && pokemonProj.ModProjectile is PokemonPetProjectile)
				{
					Main.EntitySpriteDraw(texture, pokemonProj.Center - Main.screenPosition, texture.Frame(1,nFrames,0,Projectile.frame), color, Projectile.rotation, drawOrigin, currentScale*Projectile.scale, SpriteEffects.None, 0);
				}
                currentScale += separation*(finalScale-initialScale);
            }

            animOffset += 0.01f*(finalScale-initialScale);
			if(animOffset > separation * (finalScale - initialScale))
			{
				animOffset -= separation * (finalScale - initialScale);
			}*/

			return false;
		}

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, Projectile.Opacity*1f, Projectile.Opacity*0.9f, Projectile.Opacity*0.3f);

			if(Projectile.timeLeft >= 70)
			{
				if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack)
				{
					SearchTarget(64f);
				}
				else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack)
				{
					if(Trainer.targetPlayer != null)
					{
						targetPlayer = Trainer.targetPlayer;
					}
					else if(Trainer.targetNPC != null)
					{
						targetEnemy = Trainer.targetNPC;
					}
				}
			}

			if (SafeUpdateTargetPosition())
			{
				Projectile.Center = targetPosition;
			}

			SpawnDust(Projectile.Center);
			if (pokemonProj != null && pokemonProj.active && pokemonProj.ModProjectile is PokemonPetProjectile)
			{
				SpawnDust(pokemonProj.Center);
			}

			if(Projectile.timeLeft < 10){
				Projectile.Opacity = 0.8f*Projectile.timeLeft*0.1f;
			}

			//UpdateAnimation();

			if(Projectile.owner == Main.myPlayer){
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
                    Projectile.frame = 0;
                }
            }
        }

		public override bool? CanHitNPC(NPC target)
        {
			if(targetEnemy != null){
				if(targetEnemy.active){
					return target.whoAmI == targetEnemy.whoAmI;
				}
			}
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
			if(targetPlayer != null){
				if(targetPlayer.active){
					return target.whoAmI == targetPlayer.whoAmI;
				}
			}
            return false;
        }

		void SpawnDust(Vector2 position)
		{
			if(Projectile.timeLeft%15==0){
				SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
				for (int i = 0; i < 60; i++)
				{
					int dustIndex = Dust.NewDust(position, Projectile.width, Projectile.height, DustID.YellowStarDust, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), Projectile.scale);
					Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndex].noGravity = true;
					Vector2 posAux = new Vector2(12*Projectile.scale,0).RotatedBy(MathHelper.ToRadians(6*i));
					posAux.Y *= 0.2f;
					posAux = posAux.RotatedBy(Projectile.rotation);
					Main.dust[dustIndex].position = position + posAux;
					Main.dust[dustIndex].velocity = 0.8f*posAux;
				}
			}

			if(Main.rand.NextBool(5))
			{
				int dustIndex = Dust.NewDust(position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X, Projectile.velocity.Y, 100, default(Color), Projectile.scale);
				Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndex].noGravity = true;
				Vector2 posAux = new Vector2(6*Projectile.scale,0).RotatedByRandom(MathHelper.TwoPi);
				posAux.Y *= 0.2f;
				posAux = posAux.RotatedBy(Projectile.rotation);
				Main.dust[dustIndex].position = position + posAux;
				Main.dust[dustIndex].velocity = 2f*posAux;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(30,0);
			Vector2 end = Projectile.Center - Projectile.scale*new Vector2(30,0);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale*20f, ref collisionPoint);
		}
    }
}
