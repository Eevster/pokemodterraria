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
    }
}
