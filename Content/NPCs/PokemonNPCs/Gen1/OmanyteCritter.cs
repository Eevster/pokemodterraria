using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class OmanyteCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 32;

        public override int totalFrames => 14;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] jumpStartEnd => [4, 4];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [8, 13];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [0, 3];
        public override int[] walkSwimStartEnd => [4, 7];
        public override int[] attackSwimStartEnd => [8, 13];
        public override float catchRate => 90;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
                {
                        bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "HelixFossilItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("In prehistoric times, it swam on the sea floor, eating plankton. Its fossils are sometimes found."));
                }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}
		
	}

	public class OmanyteCritterNPCShiny : OmanyteCritterNPC{}
}