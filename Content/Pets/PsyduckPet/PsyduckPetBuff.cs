using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PsyduckPet
{
	public class PsyduckPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Psyduck";
        public override int ProjType => ModContent.ProjectileType<PsyduckPetProjectile>();
    }

    public class PsyduckPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Psyduck";
        public override int ProjType => ModContent.ProjectileType<PsyduckPetProjectileShiny>();
    }
}
