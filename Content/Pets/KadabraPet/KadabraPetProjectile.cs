using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KadabraPet
{
	public class KadabraPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 13;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 8];
        public override int[] jumpStartEnd => [8, 8];
        public override int[] fallStartEnd => [7, 7];
        public override int[] attackStartEnd => [9, 12];

        public override string[] evolutions => ["Alakazam"];
        public override string[] itemToEvolve => ["LinkingCordItem"];
	}

	public class KadabraPetProjectileShiny : KadabraPetProjectile{}
}
