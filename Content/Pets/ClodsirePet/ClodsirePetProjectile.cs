using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ClodsirePet
{
	public class ClodsirePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 82;
        public override int hitboxHeight => 38;

        public override int totalFrames => 49;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [13, 27];
        public override int[] walkStartEnd => [8, 12];
        public override int[] jumpStartEnd => [8, 12];
        public override int[] fallStartEnd => [10, 10];
        public override int[] attackStartEnd => [0, 7];
        public override bool canSwim => true;
        public override int[] idleSwimStartEnd => [33, 40];
        public override int[] walkSwimStartEnd => [40, 48];
        public override int[] attackSwimStartEnd => [28, 32];


    }

	public class ClodsirePetProjectileShiny : ClodsirePetProjectile{}
}
