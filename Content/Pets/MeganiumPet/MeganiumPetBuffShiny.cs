using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MeganiumPet
{
	public class MeganiumPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Meganium";
        public override int ProjType => ModContent.ProjectileType<MeganiumPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Lifeforce, 60); // Apply the first buff
            }
        }
    }
}
