using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.GlobalNPCs;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles
{
	public abstract class PokemonAttack : ModProjectile
	{
		private int expGained = 0;
		public int attackMode = 0;

		public Player Owner => Main.player[Projectile.owner];
		public PokemonPlayer Trainer => Owner.GetModPlayer<PokemonPlayer>();

		public NPC targetEnemy;
		public Player targetPlayer;
		public bool foundTarget = false;

		public Vector2 positionAux;
		public Projectile pokemonProj;
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)attackMode);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackMode = reader.ReadByte();
			base.ReceiveExtraAI(reader);
		}

        public override void OnSpawn(IEntitySource source)
        {
			attackMode = Main.player[Projectile.owner].GetModPlayer<PokemonPlayer>().attackMode;
            base.OnSpawn(source);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			//SetExpGained(target, hit);
			if(pokemonProj != null){
				if(pokemonProj.active){
					if(target.life <= 0 && target.GetGlobalNPC<HitByPokemonNPC>().pokemonProj != pokemonProj){
						PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
						pokemonMainProj?.SetExtraExp(HitByPokemonNPC.SetExpGained(target));
					}
					target.GetGlobalNPC<HitByPokemonNPC>().pokemonProj = pokemonProj;
				}
			}
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if(target.ModNPC is PokemonWildNPC wildNPC){
				modifiers.DefenseEffectiveness *= 0;
				modifiers.FinalDamage -= 2f;
				modifiers.FinalDamage /= wildNPC.finalStats[2]/10f;
				modifiers.FinalDamage += 2f;
			}

            base.ModifyHitNPC(target, ref modifiers);
        }

        /*public void SetExpGained(NPC target, NPC.HitInfo hit){
			if(target.life <= 0 || hit.InstantKill){
				int exp = (int)Math.Sqrt(target.value);
				if(exp < 1) exp = 1;
				expGained += exp;
			}
		}*/

        /*public int GetExpGained(){
			int exp = expGained;
			expGained = 0;
			return exp;
		}*/

		public void SearchTarget(float distanceFromTarget, bool canAttackThroughWalls = true){
			Vector2 targetCenter = Projectile.Center;

			targetEnemy = null;
			targetPlayer = null;

			if(Main.netMode != NetmodeID.SinglePlayer){
				float sqrMaxDetectDistance = distanceFromTarget*distanceFromTarget;
				for (int k = 0; k < Main.maxPlayers; k++) {
					if(Main.player[k] != null){
						Player target = Main.player[k];
						if(target.whoAmI != Projectile.owner){
							if(target.active && !target.dead){
								float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
								bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
								bool closeThroughWall = Vector2.Distance(target.Center, Projectile.Center) < 100f || canAttackThroughWalls;

								// Check if it is within the radius
								if (sqrDistanceToTarget < sqrMaxDetectDistance && (lineOfSight || closeThroughWall)) {
									if(target.hostile){
										sqrMaxDetectDistance = sqrDistanceToTarget;
										targetCenter = target.Center;
										targetPlayer = target;
										foundTarget = true;
									}
								}
							}
						}
					}
				}
			}

			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy()) {
					float between = Vector2.Distance(npc.Center, Projectile.Center);
					bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
					bool inRange = between < distanceFromTarget;

					bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
					bool closeThroughWall = between < 100f || canAttackThroughWalls;

					if(npc.boss){
						targetEnemy = npc;
						foundTarget = true;
						break;
					}

					if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall) && !(npc.GetGlobalNPC<PokemonNPCData>().isPokemon && npc.GetGlobalNPC<PokemonNPCData>().shiny)) {
						distanceFromTarget = between;
						targetCenter = npc.Center;
						targetEnemy = npc;
						foundTarget = true;
					}
				}
			}

			if(targetPlayer != null && targetEnemy != null){
				if(Vector2.Distance(Projectile.Center, targetPlayer.Center) >= Vector2.Distance(Projectile.Center, targetEnemy.Center)){
					targetEnemy = null;
				}else{
					targetPlayer = null;
				}
			}
		}
    }
}
