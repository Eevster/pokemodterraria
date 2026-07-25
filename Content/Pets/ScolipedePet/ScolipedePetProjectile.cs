using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ScolipedePet
{
	public class ScolipedePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 64;
        public override int hitboxHeight => 60;

        public override int totalFrames => 30;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [13, 23];
        public override int[] walkStartEnd => [7, 13];
        public override int[] jumpStartEnd => [23, 29];
        public override int[] fallStartEnd => [28, 29];
        public override int[] attackStartEnd => [0, 6];
        public override string[] megaEvolutions => ["ScolipedeMega"];
        public override string[] itemToMegaEvolve => ["ScolipedeMegaStoneItem"];

        public override bool canBeMounted => true;
        public override Vector2 playerMountPosition => new Vector2(-6,10);
    }

	public class ScolipedePetProjectileShiny : ScolipedePetProjectile{}
}
