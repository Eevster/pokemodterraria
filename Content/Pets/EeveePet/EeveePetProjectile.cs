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
		public override int hitboxWidth => 22;
		public override int hitboxHeight => 30;

		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];

		public override string[] evolutions => ["Flareon", "Jolteon", "Vaporeon", "Espeon", "Umbreon"];
		public override string[] itemToEvolve => ["FireStoneItem", "ThunderStoneItem", "WaterStoneItem"];
		public override string[] specialConditionToEvolve => ["HappinessDay", "HappinessNight"];
	}

	public class EeveePetProjectileShiny : EeveePetProjectile{}
}
