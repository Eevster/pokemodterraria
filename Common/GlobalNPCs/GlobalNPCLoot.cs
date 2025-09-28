using System;
using System.Linq;
using Pokemod.Content.Items.Accessories;
using Pokemod.Content.Items.Badges;
using Pokemod.Content.Items.Consumables.TMs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Common.GlobalNPCs
{
	public class GlobalNPCLoot : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
            switch (npc.type)
			{

				case NPCID.WyvernHead:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AirBalloon>(), 3));
					break;

				case NPCID.Mimic:
					int[] TMs = {
					ModContent.ItemType<TMElectric>(), ModContent.ItemType<TMFighting>(), ModContent.ItemType<TMFire>(),
					ModContent.ItemType<TMFlying>(), ModContent.ItemType<TMGhost>(), ModContent.ItemType<TMGrass>(),
					ModContent.ItemType<TMGround>(), ModContent.ItemType<TMIce>(), ModContent.ItemType<TMNormal>(),
					ModContent.ItemType<TMPoison>(), ModContent.ItemType<TMPsychic>(), ModContent.ItemType<TMSteel>(),
					ModContent.ItemType<TMWater>()
					};

					npcLoot.Add(ItemDropRule.OneFromOptions(10, TMs));
					break;

                case NPCID.KingSlime:
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<BoulderBadge>(), 1, 1, 1, null));
                    break;

                case NPCID.EyeofCthulhu:
                        npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<CascadeBadge>(), 1, 1, 1, null));
                    break;

                case NPCID.QueenBee:
                case NPCID.Deerclops:
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<ThunderBadge>(), 1, 1, 1, null));
                    break;

                case NPCID.SkeletronHead:
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<RainbowBadge>(), 1, 1, 1, null));
                    break;

                case NPCID.WallofFlesh:
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<SoulBadge>(), 1, 1, 1, null));
                    break;

                    // The Twins will always drop the Marsh Badge, but the Destroyer and Skeletron Prime will only drop the Volcano Badge if all other mech bosses are down, otherwise they drop the marsh badge.
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                        npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<MarshBadge>(), 1, 1, 1, new Conditions.MissingTwin()));
                    break;
                case NPCID.SkeletronPrime:
                case NPCID.TheDestroyer:
                        npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<MarshBadge>(), 1, 1, 1, new AndCondition(new Conditions.DownedAllMechBosses(), new Conditions.MechanicalBossesDummyCondition(), true)));
                        npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<VolcanoBadge>(), 1, 1, 1, new AndCondition(new Conditions.DownedAllMechBosses(), new Conditions.MechanicalBossesDummyCondition())));
                    break;

                case NPCID.Plantera:
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<EarthBadge>(), 1, 1, 1, null));
                    break;
            }
		}
	}

    public class AndCondition : IItemDropRuleCondition
    {
        private readonly IItemDropRuleCondition stable; // The first condition determines the bestiary visibility and description
        private readonly IItemDropRuleCondition unstable;
        private bool invert; 
        public AndCondition(IItemDropRuleCondition conditionStable, IItemDropRuleCondition conditionUnstable, bool NotXAndY = false)
        {
            stable = conditionStable;
            unstable = conditionUnstable;
            invert = NotXAndY;
        }
        public bool CanDrop(DropAttemptInfo info)
        {
            if (stable.CanDrop(info) != invert && unstable.CanDrop(info)){
                return true;
            }
            return false;
        }
        public bool CanShowItemDropInUI() 
        {
            if (stable.CanShowItemDropInUI())
            {
                return true;
            }
            return false;
        }
        public string GetConditionDescription() => invert? "NOT" : "" + stable.GetConditionDescription();
    }
}