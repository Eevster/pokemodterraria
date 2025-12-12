
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items.Consumables;

namespace Pokemod.Content.Items.GeneticSamples
{
	public class SkullFossilItem : GeneticSampleItem
	{
		public override void SetDefaults()
		{
            pokemonName = "Cranidos";
			minLevel = 5;
			maxLevel = 25;

            Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 20);
            base.SetDefaults();
        }
	}
}
