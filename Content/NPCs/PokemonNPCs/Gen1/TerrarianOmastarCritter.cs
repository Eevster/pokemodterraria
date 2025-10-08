using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianOmastarCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 36;
        public override int hitboxHeight => 46;

        public override int totalFrames => 18;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [6, 6];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [9, 17];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [0, 5];
        public override int[] walkSwimStartEnd => [6, 9];
        public override int[] attackSwimStartEnd => [9, 17];

        public override float catchRate => 25;
        public override int minLevel => 40;
        
        public override int[][] spawnConditions => [];


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
                {
                        bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "EldritchHelixItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                new FlavorTextBestiaryInfoElement("This Pokemon's many eyes allow it to peer into deep space."));
                }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}
		
	}

	public class TerrarianOmastarCritterNPCShiny : TerrarianOmastarCritterNPC { }
}