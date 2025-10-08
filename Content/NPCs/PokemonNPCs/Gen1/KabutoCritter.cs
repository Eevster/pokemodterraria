using Pokemod.Common.Configs;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Pokemod.Common.UI;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class KabutoCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 28;

        public override int totalFrames => 16;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] jumpStartEnd => [8, 8];
        public override int[] fallStartEnd => [11, 11];
        public override int[] attackStartEnd => [8, 11];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [12, 15];
        public override int[] walkSwimStartEnd => [12, 15];
        public override int[] attackSwimStartEnd => [8, 11];
        public override float catchRate => 90;
        
        public override int[][] spawnConditions => [];


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "DomeFossilItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("This Pokémon lived in ancient times. On rare occasions, it has been discovered as a living fossil."));
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}

	}

	public class KabutoCritterNPCShiny : KabutoCritterNPC{}
}