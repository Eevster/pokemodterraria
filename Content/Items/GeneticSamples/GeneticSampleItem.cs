
using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Content.Items.GeneticSamples
{
	public abstract class GeneticSampleItem : ModItem
	{
        public string pokemonName;
        public int minLevel;
        public int maxLevel;
        public int sampleQuantity = 5;
        public float dropChance = 1f;

        public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;

            Item.consumable = true;

            Item.ResearchUnlockCount = sampleQuantity;
            Item.maxStack = Item.CommonMaxStack;
		}

        public string pokemonType => PokemonTypeToString();
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(pokemonType);

        private string PokemonTypeToString()
        {
            string typeString = "";
            int[] types = PokemonData.pokemonInfo[pokemonName].pokemonTypes;

            if (types.Length > 0)
            {
                if (types[0] >= 0) typeString += "[c/" + PokemonNPCData.GetTypeColor(types[0]) + ":" + (TypeIndex)types[0] + "]";
                if (types[1] >= 0) typeString += "/[c/" + PokemonNPCData.GetTypeColor(types[1]) + ":" + (TypeIndex)types[1] + "]";
            }

            return typeString;
        }
    }
}
