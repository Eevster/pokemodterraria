using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.LaprasPet
{
	public class LaprasPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 23;
		public override int animationSpeed => 12;
		public override int[] idleStartEnd => [0, 7];
		public override int[] walkStartEnd => [8, 11];
		public override int[] jumpStartEnd => [0, 0];
		public override int[] fallStartEnd => [19, 19];
		
		public override int[] attackStartEnd => [12,12];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [13,16];
		public override int[] walkSwimStartEnd => [17,21];
		public override int[] attackSwimStartEnd => [22,22];
	}

	public class LaprasPetProjectileShiny : LaprasPetProjectile{}
}
