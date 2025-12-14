using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class KabutopsCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 26;
        public override int animationSpeed => 9;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] jumpStartEnd => [18, 18];
        public override int[] fallStartEnd => [16, 16];
        public override int[] attackStartEnd => [8, 14];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [15, 18];
        public override int[] walkSwimStartEnd => [15, 18];
        public override int[] attackSwimStartEnd => [19, 25];
        public override float catchRate => 25;
        public override int minLevel => 40;
        
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "DomeFossilItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}
		
	}

	public class KabutopsCritterNPCShiny : KabutopsCritterNPC{}
}