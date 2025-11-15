using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharmanderPet
{
	public class CharmanderPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 32;

		public override int totalFrames => 20;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [14,19];

		public override string[] evolutions => ["Charmeleon"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;
		
		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }

        public override void ChangeAttackColor(PokemonAttack attack, bool condition = false, int shaderID = 0, Color color = default)
        {
            condition = attack.attackType == (int)TypeIndex.Fire && variant == "Christmas";
            base.ChangeAttackColor(attack, condition, shaderID, color);
        }
    }
	public class CharmanderPetProjectileShiny : CharmanderPetProjectile{}
}