using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianKabutopsCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 15;
        public override int animationSpeed => 9;
        public override int moveStyle => 1;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] attackStartEnd => [8, 14];

        public override int[] idleFlyStartEnd => [0, 3];
        public override int[] walkFlyStartEnd => [4, 7];
        public override int[] attackFlyStartEnd => [8, 14];
        public override bool tangible => false;
        public override float catchRate => 25;
        public override int minLevel => 40;
        
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.TheDungeon, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
                {
                        bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "HauntedDomeItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}
		
	}

	public class TerrarianKabutopsCritterNPCShiny : TerrarianKabutopsCritterNPC { }
}