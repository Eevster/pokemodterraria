using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;

namespace Pokemod.Content.Pets.RaichuPet
{
	public class RaichuPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Raichu";
        public override int ProjType => ModContent.ProjectileType<RaichuPetProjectile>();
	}

    public class RaichuPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Raichu";
        public override int ProjType => ModContent.ProjectileType<RaichuPetProjectileShiny>();
	}
}
