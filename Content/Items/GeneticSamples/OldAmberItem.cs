
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items.Consumables;
using Terraria.DataStructures;

namespace Pokemod.Content.Items.GeneticSamples
{
	public class OldAmberItem : GeneticSampleItem
    {
        public override void SetDefaults()
        {
            pokemonName = "Aerodactyl";
            minLevel = 20;
            maxLevel = 100;

            dropChance = 0.3f;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 60);
            base.SetDefaults();
        }
    }
}
