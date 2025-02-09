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
	public class Toxic : PokemonAttack
	{
		public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/QuickAttack";
		private Vector2 targetPosition;
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
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 5;

			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;

			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			base.SetDefaults();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
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

            base.OnSpawn(source);
        }

        public override void AI()
        {
			for (int i = 0; i < 3; i++)
			{
				int dustIndex = Dust.NewDust(Projectile.Center-0.5f*new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.Venom, 0, 0, 0, default(Color), 1f);
				Main.dust[dustIndex].scale = 1.2f + (float)Main.rand.Next(3) * 0.2f;
				Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndex].noGravity = true;
			}

			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				if(Trainer.targetPlayer != null){
					targetPlayer = Trainer.targetPlayer;
				}else if(Trainer.targetNPC != null){
					targetEnemy = Trainer.targetNPC;
				}
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
				target.AddBuff(BuffID.Venom, 5*60);
			}
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Venom, 5*60);
            base.OnHitPlayer(target, info);
        }

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
		}

		public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center-new Vector2(8*Projectile.scale, 8*Projectile.scale), (int)(16*Projectile.scale), (int)(16*Projectile.scale), DustID.Venom, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3,3), 0, default(Color), 2f);
				Main.dust[dustIndex].scale = 1.6f + (float)Main.rand.Next(3) * 0.2f;
				Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}
