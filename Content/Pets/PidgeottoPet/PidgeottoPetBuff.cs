using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PidgeottoPet
{
        public class PidgeottoPetBuff : PokemonPetBuff
        {
        public override string PokeName => "Pidgeotto";
        public override int ProjType => ModContent.ProjectileType<PidgeottoPetProjectile>();

        public override void UpdateExtraChanges(Player player){

        }
	}

    public class PidgeottoPetBuffShiny : PokemonPetBuff
    {
        public override string PokeName => "Pidgeotto";
        public override int ProjType => ModContent.ProjectileType<PidgeottoPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){

        }
        }
}
