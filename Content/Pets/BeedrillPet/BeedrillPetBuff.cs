using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BeedrillPet
{
	public class BeedrillPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Beedrill";
        public override int ProjType => ModContent.ProjectileType<BeedrillPetProjectile>();
    }

    public class BeedrillPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Beedrill";
        public override int ProjType => ModContent.ProjectileType<BeedrillPetProjectileShiny>();
    }
}
