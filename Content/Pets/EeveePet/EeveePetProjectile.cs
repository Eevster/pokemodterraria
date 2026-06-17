using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.EeveePet
{
	public class EeveePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 20;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [9,10];
		public override int[] fallStartEnd => [12,14];
		public override int[] attackStartEnd => [18,18];

		public override string[] evolutions => ["Flareon", "Jolteon", "Vaporeon", "Espeon", "Umbreon"];
		public override string[] itemToEvolve => ["FireStoneItem", "ThunderStoneItem", "WaterStoneItem"];
		public override string[] specialConditionToEvolve => ["HappinessDay", "HappinessNight"];

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);
	}

	public class EeveePetProjectileShiny : EeveePetProjectile{}
}
