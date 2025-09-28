using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianAerodactylCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 60;
        public override int hitboxHeight => 64;

        public override int totalFrames => 15;
        public override int animationSpeed => 6;
        public override int moveStyle => 1;

        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 9];
        public override int[] attackStartEnd => [10, 14];

        public override int[] idleFlyStartEnd => [0, 4];
        public override int[] walkFlyStartEnd => [5, 9];
        public override int[] attackFlyStartEnd => [10, 14];
        public override float catchRate => 25;

        public override int minLevel => 20;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(new CustomItemBestiaryInfoElement() { itemName = "InfernalAmberItem" }, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("In prehistoric times, this Pokémon flew freely and fearlessly through the skies."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return 0f;
		}
		
	}

	public class TerrarianAerodactylCritterNPCShiny : TerrarianAerodactylCritterNPC { }
}