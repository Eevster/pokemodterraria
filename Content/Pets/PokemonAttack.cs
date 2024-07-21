using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets
{
	public abstract class PokemonAttack : ModProjectile
	{
		private int expGained = 0;
		public Vector2 positionAux;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			SetExpGained(target, hit);
            base.OnHitNPC(target, hit, damageDone);
        }

		public void SetExpGained(NPC target, NPC.HitInfo hit){
			if(target.life <= 0 || hit.InstantKill){
				int exp = (int)Math.Sqrt(target.value);
				if(exp < 1) exp = 1;
				expGained += exp;
			}
		}

		public int GetExpGained(){
			int exp = expGained;
			expGained = 0;
			return exp;
		}
    }
}
