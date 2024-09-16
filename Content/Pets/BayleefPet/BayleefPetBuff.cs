using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BayleefPet
{
	public class BayleefPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Bayleef";
        public override int ProjType => ModContent.ProjectileType<BayleefPetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Lifeforce, 60); // Apply the first buff
            }
        }
    }
}
