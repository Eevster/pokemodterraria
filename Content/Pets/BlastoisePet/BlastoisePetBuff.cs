using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BlastoisePet
{
	public class BlastoisePetBuff :  PokemonPetBuff
	{
        public override string PokeName => "Blastoise";
        public override int ProjType => ModContent.ProjectileType<BlastoisePetProjectile>();
	}

    public class BlastoisePetBuffShiny :  PokemonPetBuff
	{
        public override string PokeName => "Blastoise";
        public override int ProjType => ModContent.ProjectileType<BlastoisePetProjectileShiny>();
	}
}
