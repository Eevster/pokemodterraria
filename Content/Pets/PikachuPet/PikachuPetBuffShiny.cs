using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PikachuPet
{
	public class PikachuPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Pikachu";
        public override int ProjType => ModContent.ProjectileType<PikachuPetProjectileShiny>();
        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Shine, 60); // Apply the first buff
                player.AddBuff(BuffID.Swiftness, 60); // Apply the first buff
            }
        }
	}
}
