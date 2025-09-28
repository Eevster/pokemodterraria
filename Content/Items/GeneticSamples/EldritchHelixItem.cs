
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items.Consumables;
using Terraria.DataStructures;

namespace Pokemod.Content.Items.GeneticSamples
{
	public class EldritchHelixItem : GeneticSampleItem
    {
        public override void SetDefaults()
        {
            pokemonName = "TerrarianOmanyte";
            minLevel = 5;
            maxLevel = 35;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(silver: 40);
            base.SetDefaults();
        }
    }
}
