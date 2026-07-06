using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class MegaDrain : PokemonAttack
	{
		private float sizeChangeRate = 0.05f;
		private float shadowScaleDiff = 0.20f;
		private int shadowCount = 3;

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
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = -1;
			Projectile.scale = 1f;
            Projectile.Opacity = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item4, pokemon.position);
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter, Vector2.Zero, ModContent.ProjectileType<MegaDrain>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			for(int i = 0; i < shadowCount; i++)
			{
				Color color = Projectile.GetAlpha(lightColor) * ((shadowCount - (i + 1)) / (float)shadowCount);
				float scale = Projectile.scale + (i+1)*shadowScaleDiff;
				if(scale > 1f)
				{
					scale -= 1f;
				}

				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
				texture.Bounds, color, Projectile.rotation,
				texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, Projectile.Opacity*0.3f, Projectile.Opacity, Projectile.Opacity*0.3f);

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

			if (Projectile.scale > 0f)
			{
				Projectile.scale -= sizeChangeRate;

				if(Projectile.scale <= 0f)
				{
					Projectile.scale += 1f;
				}
			}

			if (SafeUpdateTargetPosition())
			{
				Projectile.Center = targetPosition;
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

		public override void OnHitPokemonPet(PokemonPetProjectile target, int damageDone)
        {
			HealEffect(damageDone);

            base.OnHitPokemonPet(target, damageDone);
        }

		public void HealEffect(int healAmount){
			Vector2 targetCenter = Owner.Center;

			if (pokemonProj.ModProjectile is PokemonPetProjectile pokemonPetProj)
			{
				if(pokemonPetProj.GetHPRatio() < 1f){
					HealEffect(pokemonPetProj, healAmount, true);
					targetCenter = pokemonProj.Center;
				}
				else if(!pokemonPetProj.isEnemy)
				{
					HealEffect(Owner, Owner.statLifeMax2 > 300 ? 2 : 1, true);
				}
			}

			for(int i = 0; i < 10; i++){
				int dustIndex = Dust.NewDust(Projectile.Center-0.5f*new Vector2(32,32), 64, 64, DustID.DryadsWard, 0f, 0f, 200, default(Color), 1f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = 16f*Vector2.Normalize(targetCenter-Main.dust[dustIndex].position);
			}
		}
		
		/*public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center-24f*new Vector2(1,0);
            Vector2 end = Projectile.Center+24f*new Vector2(1,0);
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 48f, ref collisionPoint);
        }*/

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
		}
    }
}
