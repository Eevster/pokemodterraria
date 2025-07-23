using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.GlobalNPCs;
using Pokemod.Common.Players;
using Pokemod.Content.DamageClasses;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles
{
	public abstract class PokemonAttack : ModProjectile
	{
		//Attack Data
		public int attackType;
		public bool isSpecial;

		private int expGained = 0;
		public int attackMode = 0;

		public Player Owner => Main.player[Projectile.owner];
		public PokemonPlayer Trainer => Owner.GetModPlayer<PokemonPlayer>();

		public NPC targetEnemy;
		public Player targetPlayer;
		public bool foundTarget = false;

		public Vector2 positionAux;
		public Projectile pokemonProj;

        public ArmorShaderData shader = null;
		public Color effectColor = Color.White;

        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<PokemonDamageClass>();
			base.SetDefaults();
        }
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

		public virtual void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){

		}

		public virtual void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){

		}

		public virtual void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){

		}

		public virtual void UpdateNoAttackProjs(Projectile pokemon, int i){

		}

		public virtual void ExtraChanges(Projectile pokemon){

		}

        public override void OnSpawn(IEntitySource source)
        {
			attackType = PokemonData.pokemonAttacks[GetType().Name.Replace("_Front", "")].attackType;
			isSpecial = PokemonData.pokemonAttacks[GetType().Name.Replace("_Front", "")].isSpecial;
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
			if (target.ModNPC is PokemonWildNPC wildNPC)
			{
				modifiers.DefenseEffectiveness *= 0;
				modifiers.FinalDamage /= (!isSpecial ? wildNPC.finalStats[2] : wildNPC.finalStats[4]) / 10f;
				//modifiers.FinalDamage /= (!isSpecial ? wildNPC.NPC.GetGlobalNPC<PokemonNPCData>().GetWildCalcStat(2) : wildNPC.NPC.GetGlobalNPC<PokemonNPCData>().GetWildCalcStat(4)) / 10f;
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

			foundTarget = false;

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

					if (inRange && (closest || !foundTarget) && (lineOfSight || closeThroughWall) && !npc.GetGlobalNPC<PokemonNPCData>().isPokemon) {
						if(npc.boss){
							targetEnemy = npc;
							foundTarget = true;
							break;
						}
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

        public override bool PreDraw(ref Color lightColor)
        {
			if (shader != null)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                shader.Apply(Projectile);
			}
			return base.PreDraw(ref lightColor);
        }

        public override void PostDraw(Color lightColor)
        {
            base.PostDraw(lightColor);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }
    }
}
