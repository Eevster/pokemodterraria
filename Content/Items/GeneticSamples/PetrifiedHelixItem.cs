
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items.Consumables;
using Terraria.DataStructures;

namespace Pokemod.Content.Items.GeneticSamples
{
	public class PetrifiedHelixItem : GeneticSampleItem
    {
        public override void SetDefaults()
        {
            pokemonName = "Omanyte";
            minLevel = 5;
            maxLevel = 35;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(silver: 40);
            base.SetDefaults();
        }
    }
}
