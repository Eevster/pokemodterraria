using Pokemod.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.GeneticSamples
{
    public class MalformedSample : GeneticSampleItem
    {
        public override void SetDefaults()
        {
            pokemonName = "TerrarianMewtwo";
            minLevel = 45;
            maxLevel = 55;
            sampleQuantity = 10;

            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(silver: 20);
            base.SetDefaults();
        }
    }
}
