using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ExeggcutePet
{
	public class ExeggcutePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 17;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,13];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [14, 16];


        public override string[] evolutions => ["Exeggutor"];
        public override string[] itemToEvolve => ["LeafStoneItem"];
	}

	public class ExeggcutePetProjectileShiny : ExeggcutePetProjectile{}
}
