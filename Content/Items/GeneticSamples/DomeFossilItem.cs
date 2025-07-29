
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items.Consumables;

namespace Pokemod.Content.Items.GeneticSamples
{
	public class DomeFossilItem : GeneticSampleItem
	{
		public override void SetDefaults()
		{
            pokemonName = "Kabuto";
			minLevel = 5;
			maxLevel = 35;

            Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 20);
            base.SetDefaults();
        }
	}
}
