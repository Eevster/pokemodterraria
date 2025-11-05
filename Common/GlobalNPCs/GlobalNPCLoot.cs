using System;
using System.Collections.Generic;
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
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DragonBreathTM>(), 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DragonRushTM>(), 30));
                    break;


                //TMs =================================================
                /*
                case NPCID.Mimic:
                    int[] TMs = {
                    ModContent.ItemType<TMElectric>(), ModContent.ItemType<TMFighting>(), ModContent.ItemType<TMFire>(),
                    ModContent.ItemType<TMFlying>(), ModContent.ItemType<TMGhost>(), ModContent.ItemType<TMGrass>(),
                    ModContent.ItemType<TMGround>(), ModContent.ItemType<TMIce>(), ModContent.ItemType<TMNormal>(),
                    ModContent.ItemType<TMPoison>(), ModContent.ItemType<TMPsychic>(), ModContent.ItemType<TMSteel>(),
                    ModContent.ItemType<TMWater>()
                    };*/

                //All TMs
                case NPCID.Mimic: case NPCID.IceMimic: case NPCID.PresentMimic: case NPCID.BigMimicJungle: case NPCID.BigMimicCorruption: case NPCID.BigMimicCrimson: case NPCID.BigMimicHallow:
                    List<int> TMs = [];
                    foreach (TechnicalMachine TM in ModContent.GetContent<TechnicalMachine>())
                    {
                        if (TM.singleMove)
                        {
                            TMs.Add(TM.Type);
                        }
                    }
                    npcLoot.Add(ItemDropRule.OneFromOptions(8, TMs.ToArray()));
					break;

                    //Specific TMs
                case NPCID.Harpy:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WingAttackTM>(), 20));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AirSlashTM>(), 100));
                    break;
                case NPCID.BlackRecluse: case NPCID.WallCreeperWall: case NPCID.WallCreeper: case 236: case 237: case 238: case 239: case 240:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FuryCutterTM>(), 30));
                    break;
                case NPCID.Hornet: case -16: case -17: case 231: case -56: case -57: case 232: case -58: case -59: case 233: case -60: case -61: case 234: case -62: case -63: case 235: case -64: case -65:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PinMissileTM>(), 80)); break;
                case NPCID.VampireBat: case NPCID.Vampire:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrunchTM>(), 10)); break;
                case NPCID.Reaper:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowBallTM>(), 10)); break;
                case NPCID.AngryNimbus: case NPCID.Paladin:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThunderboltTM>(), 10));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThunderTM>(), 30)); break;
                case NPCID.AngryBones: case -13: case -14: case 294: case 295: case 296:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrickBreakTM>(), 70)); break;
                case NPCID.BlueArmoredBones: case 274: case 275: case 276:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FocusPunchTM>(), 80)); break;
                case NPCID.Clown:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExplosionTM>(), 40)); break;
                case NPCID.IceElemental:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceBeamTM>(), 50)); break;
                case NPCID.IceGolem:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlizzardTM>(), 50)); break;
                case NPCID.FireImp:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FlamethrowerTM>(), 50)); break;
                case NPCID.Lavabat:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FireBlastTM>(), 80));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OverheatTM>(), 80)); break;
                case NPCID.DemonTaxCollector:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HexTM>(), 20)); break;
                case NPCID.Wraith:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NightSlashTM>(), 50)); break;
                case NPCID.Snatcher:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BulletSeedTM>(), 50)); break;
                case NPCID.AngryTrapper:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SolarBeamTM>(), 80)); break;
                case NPCID.SpikedJungleSlime:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GigaDrainTM>(), 50)); break;
                case NPCID.Antlion:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DigTM>(), 80)); break;
                case NPCID.DesertBeast:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EarthquakeTM>(), 50)); break;
                case NPCID.CorruptSlime: case NPCID.Crimslime:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ToxicTM>(), 50)); break;
                case NPCID.Corruptor: case NPCID.Herpling:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SludgeBombTM>(), 80)); break;
                case NPCID.Eyezor:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HyperBeamTM>(), 80)); break;
                case NPCID.Pixie:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SwiftTM>(), 30)); break;
                case NPCID.IlluminantBat:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PsychoCutTM>(), 30)); break;
                case NPCID.ChaosElemental:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PsychicTM>(), 80)); break;
                case NPCID.RockGolem:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneEdgeTM>(), 30)); break;
                case NPCID.GraniteGolem:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RockSlideTM>(), 30)); break;
                case NPCID.DarkCaster:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterPulseTM>(), 30)); break;
                case NPCID.IceTortoise: case NPCID.IcyMerman: case NPCID.ZombieMerman: case NPCID.CreatureFromTheDeep:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HydroPumpTM>(), 70)); break;
                case NPCID.DeadlySphere:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FlashCannonTM>(), 30)); break;
                //======================================================

                //Badges
                case NPCID.KingSlime:
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<BoulderBadge>(), 1, 1, 1, null));
                    break;

                case NPCID.EyeofCthulhu:
                        npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<CascadeBadge>(), 1, 1, 1, null));
                    break;

                case NPCID.QueenBee:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Leftovers>(), 5));
                    npcLoot.Add(new DropPerPlayerOnThePlayer(ModContent.ItemType<ThunderBadge>(), 1, 1, 1, null));
                    break;
                    
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