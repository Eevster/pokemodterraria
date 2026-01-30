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
	public class GigaDrain : PokemonAttack
	{
		private Vector2 targetPosition;
		public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";

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
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
            Projectile.Opacity = 0.6f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
			Projectile.hide = true;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item4, pokemon.position);
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter, Vector2.Zero, ModContent.ProjectileType<GigaDrain>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			for(int i = 0; i < 20; i++){
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos2, new Color(176, 255, 106, 0) * 0.5f, new Color(45, 185, 0), 0.5f, 0f, 0.5f, 0.5f, 1f, Projectile.rotation+ MathHelper.ToRadians(i*360f/20f) + MathHelper.ToRadians(Main.rand.Next(-8,9)), new Vector2(2f, Utils.Remap(0.5f, 0f, 1f, 4f, 1f)) * Projectile.scale, 2*Vector2.One * Projectile.scale);
            }

            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0.1f;

            base.OnSpawn(source);
        }

        public override void AI()
        {
			if(Projectile.timeLeft >= 70)
			{
				if(attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
					SearchTarget(64f);
				}else if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
					if(Trainer.targetPlayer != null){
						targetPlayer = Trainer.targetPlayer;
					}else if(Trainer.targetNPC != null){
						targetEnemy = Trainer.targetNPC;
					}
				}
			}

			Lighting.AddLight(Projectile.Center, Projectile.Opacity*0.3f, Projectile.Opacity, Projectile.Opacity*0.3f);

			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				if(Trainer.targetPlayer != null){
					targetPlayer = Trainer.targetPlayer;
				}else if(Trainer.targetNPC != null){
					targetEnemy = Trainer.targetNPC;
				}
			}

            if(Projectile.scale < 1.5f){
                Projectile.scale += 0.02f/Projectile.scale;
            }

			if(Projectile.timeLeft < 20){
				Projectile.Opacity = Projectile.timeLeft*0.05f;
			}

			if(targetEnemy != null || targetPlayer != null){
				if(targetEnemy != null){
					if(targetEnemy.active){
						targetPosition = targetEnemy.Center;
					}else{
						targetEnemy = null;
					}
				}
				if(targetPlayer != null){
					if(targetPlayer.active && !targetPlayer.dead){
						targetPosition = targetPlayer.Center;
					}else{
						targetPlayer = null;
					}
				}
				if(targetEnemy != null || targetPlayer != null){
					Projectile.Center = targetPosition;
				}
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(target.CanBeChasedBy()){
                HealEffect(hit.Damage);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            HealEffect(info.Damage);

            base.OnHitPlayer(target, info);
        }

		public void HealEffect(int healAmount){
			Vector2 targetCenter = Owner.Center;

			if (pokemonProj.ModProjectile is PokemonPetProjectile pokemonPetProj && pokemonPetProj.GetHPRatio() < 1f)
			{
				HealEffect(pokemonPetProj, healAmount);
				targetCenter = pokemonProj.Center;
			}
			else
			{
				HealEffect(Owner, Owner.statLifeMax2>300?2:1);
			}

			for(int i = 0; i < 10; i++){
				int dustIndex = Dust.NewDust(Projectile.Center-0.5f*new Vector2(32,32), 64, 64, DustID.DryadsWard, 0f, 0f, 200, default(Color), 1f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = 16f*Vector2.Normalize(targetCenter-Main.dust[dustIndex].position);
			}
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 94, 24)*Projectile.Opacity;
        }

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
		}

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
			Texture2D sparkleTexture = TextureAssets.Extra[ExtrasID.SharpTears].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
    }
}
