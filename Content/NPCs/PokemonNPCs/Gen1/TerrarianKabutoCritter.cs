using Pokemod.Common.Configs;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Pokemod.Common.UI;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianKabutoCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 28;

        public override int totalFrames => 12;
        public override int animationSpeed => 7;
        public override int moveStyle => 1;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 6];
        public override int[] attackStartEnd => [7, 11];

        public override int[] idleFlyStartEnd => [0, 3];
        public override int[] walkFlyStartEnd => [4, 6];
        public override int[] attackFlyStartEnd => [7, 11];

        public override bool tangible => false;

        public override float catchRate => 90;
        
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.TheDungeon, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "HauntedDomeItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Sealed long ago within the dungeon, this Pokemon inhabits the bones of a Pokemon even more ancient."));
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}

	}

	public class TerrarianKabutoCritterNPCShiny : TerrarianKabutoCritterNPC{}
}