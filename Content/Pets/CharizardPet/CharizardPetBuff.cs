using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharizardPet
{
	public class CharizardPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Charizard";
        public override int ProjType => ModContent.ProjectileType<CharizardPetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Featherfall, 60); // Apply the first buff
				player.AddBuff(BuffID.ObsidianSkin, 60); // Apply the first buff
            }
        }
	}

    public class CharizardPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Charizard";
        public override int ProjType => ModContent.ProjectileType<CharizardPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Featherfall, 60); // Apply the first buff
				player.AddBuff(BuffID.ObsidianSkin, 60); // Apply the first buff
            }
        }
	}
}
