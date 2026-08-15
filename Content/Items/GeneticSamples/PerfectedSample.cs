using Pokemod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.GeneticSamples
{
    public class PerfectedSample : GeneticSampleItem
    {
        public override void SetDefaults()
        {
            pokemonName = "Mewtwo";
            minLevel = 45;
            maxLevel = 75;
            sampleQuantity = 10;


            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(silver: 20);
            base.SetDefaults();
        }
    }
}
