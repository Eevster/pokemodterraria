using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BulbasaurPet
{
	public class BulbasaurPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Bulbasaur";
        public override int ProjType => ModContent.ProjectileType<BulbasaurPetProjectile>();
	}

    public class BulbasaurPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Bulbasaur";
        public override int ProjType => ModContent.ProjectileType<BulbasaurPetProjectileShiny>();
	}
}
