using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.EeveePet
{
    public class EeveePetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Eevee";
        public override int ProjType => ModContent.ProjectileType<EeveePetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Calm, 60); // Apply the first buff
            }
        }
    }
}