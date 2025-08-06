using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.OmanytePet
{
	public class OmanytePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 14;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [8,8];
        public override int[] attackStartEnd => [8,13];

        public override string[] evolutions => ["Omastar"];
		public override int levelToEvolve => 40;
		public override int levelEvolutionsNumber => 1;
	}

	public class OmanytePetProjectileShiny : OmanytePetProjectile{}
}
