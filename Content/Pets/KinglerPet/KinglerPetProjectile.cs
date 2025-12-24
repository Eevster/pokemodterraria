using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KinglerPet
{
	public class KinglerPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 72;
		public override int hitboxHeight => 46;

		public override int totalFrames => 9;
		public override int animationSpeed => 15;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [4,4];
		public override int[] attackStartEnd => [8, 8];
	}

	public class KinglerPetProjectileShiny : KinglerPetProjectile{}
}
