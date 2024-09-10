using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.FlareonPet
{
    public class FlareonPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Flareon";
        public override int ProjType => ModContent.ProjectileType<FlareonPetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Calm, 60); // Apply the first buff
            }
        }
    }
}