using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VenusaurPet
{
	public class VenusaurPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Venusaur";
        public override int ProjType => ModContent.ProjectileType<VenusaurPetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Heartreach, 60); // Apply the first buff
                player.AddBuff(BuffID.RapidHealing, 60); // Apply the first buff
            }
        }
	}

    public class VenusaurPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Venusaur";
        public override int ProjType => ModContent.ProjectileType<VenusaurPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Heartreach, 60); // Apply the first buff
                player.AddBuff(BuffID.RapidHealing, 60); // Apply the first buff
            }
        }
	}
}
