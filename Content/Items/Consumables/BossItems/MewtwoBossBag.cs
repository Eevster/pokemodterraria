using Pokemod.Content.Items.Armor.BossSets;
using Pokemod.Content.Items.GeneticSamples;
using Pokemod.Content.NPCs.Bosses.Mewtwo;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Consumables.BossItems
{
	// Basic code for a boss treasure bag
	public class MewtwoBossBag : ModItem
	{
		public override void SetStaticDefaults() {
			// This set is one that every boss bag should have.
			// It will create a glowing effect around the item when dropped in the world.
			// It will also let our boss bag drop dev armor..
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true; // This makes sure that "Expert" displays in the tooltip and the item name color changes
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			// We have to replicate the expert drops from MinionBossBody here

			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<MewtwoBossMask>(), 7));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MalformedSample>(), 1, 8, 16));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PerfectedSample>(), 1, 0, 2));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<MewtwoBossBody>()));
		}
	}
}
