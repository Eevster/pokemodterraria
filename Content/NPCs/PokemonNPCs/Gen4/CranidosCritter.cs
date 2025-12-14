using Pokemod.Common.Configs;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Pokemod.Common.UI;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CranidosCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 40;

        public override int totalFrames => 13;
        public override int animationSpeed => 8;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [8, 8];
        public override int[] fallStartEnd => [6, 6];
        public override int[] attackStartEnd => [10, 12];

        public override float catchRate => 45;
        
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "SkullFossilItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}

	}

	public class CranidosCritterNPCShiny : CranidosCritterNPC{}
}