using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class NightShade : PokemonAttack
	{
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
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 50;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
			Projectile.light = 0.7f;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){

                        SoundEngine.PlaySound(SoundID.Item103 with { Volume = 0.6f }, pokemon.position);

                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter, Vector2.Zero, ModContent.ProjectileType<NightShade>(), 1, 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
                        break;
					}
				} 
			}
		}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
            CombatText.NewText(Projectile.Hitbox, Color.White, "?");
            base.ModifyHitNPC(target, ref modifiers);
			PokemonPetProjectile pokemonOwner = (PokemonPetProjectile)pokemonProj.ModProjectile;
            
            //modifiers.FinalDamage *= 1;
            modifiers.FinalDamage += (int)(pokemonOwner.pokemonLvl * 10f) - 1;
            CombatText.NewText(Projectile.Hitbox, Color.White, "Working");
        }


        public override void OnSpawn(IEntitySource source)
		{
			if (attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack)
			{
				SearchTarget(64f);
			}
			else if (attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack)
			{
				if (Trainer.targetPlayer != null)
				{
					targetPlayer = Trainer.targetPlayer;
				}
				else if (Trainer.targetNPC != null)
				{
					targetEnemy = Trainer.targetNPC;
				}
			}

			if (!Main.dedServ)
			{
				for (int j = 0; j < 10; j++)
				{

					Dust newDust = Main.dust[Dust.NewDust(Projectile.Center, 64, 64, DustID.Shadowflame)];
					newDust.noGravity = true;

				}
			}

			base.OnSpawn(source);
		}

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, 0.4f, 0.1f, 0.4f);

			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				if(Trainer.targetPlayer != null){
					targetPlayer = Trainer.targetPlayer;
				}else if(Trainer.targetNPC != null){
					targetEnemy = Trainer.targetNPC;
				}
			}

			Projectile.Opacity = Projectile.timeLeft*0.025f;

			if (targetEnemy != null || targetPlayer != null)
			{
				if (targetEnemy != null)
				{
					if (targetEnemy.active)
					{
						targetPosition = targetEnemy.Center;
					}
					else
					{
						targetEnemy = null;
					}
				}
				if (targetPlayer != null)
				{
					if (targetPlayer.active && !targetPlayer.dead)
					{
						targetPosition = targetPlayer.Center;
					}
					else
					{
						targetPlayer = null;
					}
				}
				Projectile.Center = targetPosition;
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		/*private void SearchTarget(){
			if(attackMode == (int)PokemonPlayer.AttackMode.No_Attack) return;

			float distanceFromTarget = 16f;
			Vector2 targetCenter = Projectile.Center;
			bool foundTarget = false;

			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				return;
			}

			if (true) {
				float sqrMaxDetectDistance = distanceFromTarget*distanceFromTarget;
				for (int k = 0; k < Main.maxPlayers; k++) {
					if(Main.player[k] != null){
						Player target = Main.player[k];
						if(target.whoAmI != Projectile.owner){
							if(target.active && !target.dead){
								float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

								// Check if it is within the radius
								if (sqrDistanceToTarget < sqrMaxDetectDistance) {
									if(target.hostile){
										sqrMaxDetectDistance = sqrDistanceToTarget;
										targetPlayer = target;
									}
								}
							}
						}
					}
				}
				// This code is required either way, used for finding a target
				if(targetPlayer == null){
					for (int i = 0; i < Main.maxNPCs; i++) {
						NPC npc = Main.npc[i];

						if (npc.CanBeChasedBy()) {
							float between = Vector2.Distance(npc.Center, Projectile.Center);
							bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
							bool inRange = between < distanceFromTarget;

							if(npc.boss){
								targetEnemy = npc;
								break;
							}

							if ((closest && inRange) || !foundTarget) {
								foundTarget = true;
								distanceFromTarget = between;
								targetCenter = npc.Center;
								targetEnemy = npc;
							}
						}
					}
				}
			}
		}*/

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

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
		}
    }
}
