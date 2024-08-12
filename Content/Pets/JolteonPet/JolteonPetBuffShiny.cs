using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.JolteonPet
{
    public class JolteonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Jolteon";
        public override int ProjType => ModContent.ProjectileType<JolteonPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Calm, 60); // Apply the first buff
            }
        }
    }
}