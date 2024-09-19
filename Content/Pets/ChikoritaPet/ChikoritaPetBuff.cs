using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ChikoritaPet
{
	public class ChikoritaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Chikorita";
        public override int ProjType => ModContent.ProjectileType<ChikoritaPetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Lifeforce, 60); // Apply the first buff
            }
        }
    }

    public class ChikoritaPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Chikorita";
        public override int ProjType => ModContent.ProjectileType<ChikoritaPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Lifeforce, 60); // Apply the first buff
            }
        }
    }
}
