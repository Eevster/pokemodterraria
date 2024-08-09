using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VaporeonPet
{
    public class VaporeonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Vaporeon";
        public override int ProjType => ModContent.ProjectileType<VaporeonPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Calm, 60); // Apply the first buff
            }
        }
    }
}