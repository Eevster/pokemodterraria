using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Common.Systems;
using Pokemod.Common.UI.StarterPanelUI;
using Pokemod.Content.Dusts;
using Pokemod.Content.Items;
using Pokemod.Content.Items.Accessories;
using Pokemod.Content.Items.GeneticSamples;
using Pokemod.Content.Items.MegaStones;
using Pokemod.Content.Items.Pokeballs;
using Pokemod.Content.Tiles;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Pokemod.Content.NPCs.MerchantNPCs
{
	[AutoloadHead]
	class PokemonProfessor : ModNPC
	{
		public static PokemonProfessorShop Shop;

		private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public static List<Item> shopItems = [];


        public override void AddShops() {
			Shop = new PokemonProfessorShop(NPC.type);

			Shop.Add<PokeballItem>();
			Shop.Add<GreatballItem>();
			Shop.Add<UltraballItem>();

			Shop.AddPool("VarietyBalls", slots: 3, true)
				.Add<DuskballItem>()
				.Add<MoonballItem>()
				.Add<SafariballItem>()
				.Add<FastballItem>()
				.Add<HealballItem>()
				.Add<FeatherballItem>()
				.Add<FriendballItem>()
				.Add<LevelballItem>()
				.Add<HeavyballItem>();

            Shop.Add<Everstone>();
			Shop.Add<SynchroMachine>();
			Shop.Add<GeoResonatorBlueprint>(TownNPCRespawnSystem.scientistArrivedCondition);

			Shop.AddPool("MegaStones", slots: 1, true)
				.Add<BlastoiseMegaStoneItem>(Condition.DownedMechBossAny)
				.Add<CharizardMegaStoneItemX>(Condition.DownedMechBossAny)
                .Add<CharizardMegaStoneItemY>(Condition.DownedMechBossAny)
                .Add<VenusaurMegaStoneItem>(Condition.DownedMechBossAny)
				.Add<AlakazamMegaStoneItem>(Condition.DownedMechBossAny)
                .Add<GengarMegaStoneItem>(Condition.DownedMechBossAny)
                .Add<GyaradosMegaStoneItem>(Condition.DownedMechBossAny);

			Shop.Register();
		}

		public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25;

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 60;
			NPCID.Sets.AttackType[Type] = 3; // Swings a weapon. This NPC attacks in roughly the same manner as Stylist
			NPCID.Sets.AttackTime[Type] = 12;
			NPCID.Sets.AttackAverageChance[Type] = 1;
			NPCID.Sets.HatOffsetY[Type] = 4;
			NPCID.Sets.ShimmerTownTransform[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 2f, // Draws the NPC in the bestiary as if its walking +2 tiles in the x direction
				Direction = -1 // -1 is left and 1 is right.
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Like)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Nurse, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Princess, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate);

            NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
			);
		}

		public override void SetDefaults() {
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Stylist;
		}

		public void UpdateShop() {
			shopItems.Clear();
   			shopItems.AddRange(Shop.GenerateNewInventoryList());

			// In multi player, ensure the shop items are synced with clients (see TravelingMerchantSystem.cs)
			if (Main.netMode == NetmodeID.Server) {
				// We recommend modders avoid sending WorldData too often, or filling it with too much data, lest too much bandwidth be consumed sending redundant data repeatedly
				// Consider sending a custom packet instead of WorldData if you have a significant amount of data to synchronise
				NetMessage.SendData(MessageID.WorldData);
   			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface
			});
		}

		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 1 : 5;
			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Sparkle>());
			}

			/*// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				// Retrieve the gore types. This NPC has shimmer variants for head, arm, and leg gore. It also has a custom hat gore. (7 gores)
				// This NPC will spawn either the assigned party hat or a custom hat gore when not shimmered. When shimmered the top hat is part of the head and no hat gore is spawned.
				int hatGore = NPC.GetPartyHatGore();
				// If not wearing a party hat, and not shimmered, retrieve the custom hat gore 
				if (hatGore == 0 && !NPC.IsShimmerVariant) {
					hatGore = Mod.Find<ModGore>($"{Name}_Gore_Hat").Type;
				}
				string variant = "";
				if (NPC.IsShimmerVariant) variant += "_Shimmer";
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}*/
		}

		public override bool UsesPartyHat() {
			// ExampleTravelingMerchant likes to keep his hat on while shimmered.
			if (NPC.IsShimmerVariant) {
				return false;
			}
			return true;
		}

        public override void OnSpawn(IEntitySource source)
        {
			UpdateShop();
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) // Requirements for the town NPC to spawn.
        {
            return true;
        }

        public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Prof Oak",
			};
		}
        public override void AI()
        {
            base.AI();
            if (Main.time == 1 && Main.dayTime)
			{
				UpdateShop();
			}
        }

        public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonProfessor.StandardDialogue1"));
			chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonProfessor.StandardDialogue2"));
			chat.Add(Language.GetTextValue("Mods.Pokemod.Dialogue.PokemonProfessor.StandardDialogue3"));

			string dialogueLine = chat; // chat is implicitly cast to a string.

			return dialogueLine;
		}

		public override void SetChatButtons(ref string button, ref string button2) {
			button = Language.GetTextValue("LegacyInterface.28");
			if(!Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().HasStarter) button2 = "Starter Pokemon";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
                shop = Shop.Name; // Opens the shop
			}else{
				if (!Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().HasStarter) ModContent.GetInstance<StarterPanelUISystem>().ShowMyUI();
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PokeballItem>()));
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 15;
			randExtraCooldown = 8;
		}

		public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) {
			itemWidth = itemHeight = 40;
		}

		public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset) {
			Main.GetItemDrawFrame(ModContent.ItemType<PokeballItem>(), out item, out itemFrame);
			itemSize = 40;
			// This adjustment draws the swing the way town npcs usually do.
			if (NPC.ai[1] > NPCID.Sets.AttackTime[NPC.type] * 0.66f) {
				offset.Y = 12f;
			}
		}
	}

	// You have the freedom to implement custom shops however you want
	// This example uses a 'pool' concept where items will be randomly selected from a pool with equal weight
	// We copy a bunch of code from NPCShop and NPCShop.Entry, allowing this shop to be easily adjusted by other mods.
	// 
	// This uses some fairly advanced C# to avoid being excessively long, so make sure you learn the language before trying to adapt it significantly
	public class PokemonProfessorShop : AbstractNPCShop
	{
		public new record Entry(Item Item, List<Condition> Conditions) : AbstractNPCShop.Entry
		{
			IEnumerable<Condition> AbstractNPCShop.Entry.Conditions => Conditions;

			public bool Disabled { get; private set; }

			public Entry Disable() {
				Disabled = true;
				return this;
			}

			public bool ConditionsMet() => Conditions.All(c => c.IsMet());
		}

		public record Pool(string Name, int Slots, List<Entry> Entries, bool Ordered = false)
		{
			public Pool Add(Item item, params Condition[] conditions) {
				Entries.Add(new Entry(item, conditions.ToList()));
				return this;
			}

			public Pool Add<T>(params Condition[] conditions) where T : ModItem => Add(ModContent.ItemType<T>(), conditions);
			public Pool Add(int item, params Condition[] conditions) => Add(ContentSamples.ItemsByType[item], conditions);

			// Picks a number of items (up to Slots) from the entries list, provided conditions are met.
			public Item PickNextItem()	{
				var list = Entries.Where(e => !e.Disabled && e.ConditionsMet()).ToList();

				if (list.Count == 0)
					return null;

				int k = Main.rand.Next(list.Count);

				if (Ordered) {
					if (Order.TryGetValue(Name, out List<int> value)) { //Uses the orderd list until it runs out, using truely random values to fill the remainder of the slots.
						if (value.Count > 0) {
							k = value[0];
							Order[Name].RemoveAt(0);
						}
					}
				}
				return list[k].Item;
			}
		}

		public List<Pool> Pools { get; } = new();

		public PokemonProfessorShop(int npcType) : base(npcType) { }

		public override IEnumerable<Entry> ActiveEntries => Pools.SelectMany(p => p.Entries).Where(e => !e.Disabled);

		public Pool AddPool(string name, int slots, bool ordered = false) {
			var pool = new Pool(name, slots, new List<Entry>(), ordered);
			Pools.Add(pool);
			return pool;
		}

		// Some methods to add a pool with a single item
		public void Add(Item item, params Condition[] conditions) => AddPool(item.ModItem?.FullName ?? $"Terraria/{item.type}", slots: 1).Add(item, conditions);
		public void Add<T>(params Condition[] conditions) where T : ModItem => Add(ModContent.ItemType<T>(), conditions);
		public void Add(int item, params Condition[] conditions) => Add(ContentSamples.ItemsByType[item], conditions);

        public static Dictionary<string, List<int>> Order = new();

        public void RerollOrder(string Name = "") {
            foreach (var pool in Pools) {
                if (Name != "" && pool.Name != Name) 
					continue;

                if (pool.Ordered) {
					Order.Remove(pool.Name);
					Order.Add(pool.Name, []); // gets a new empty list
                    for (var i = 0; i < pool.Entries.Count; i++) {
                        Order[pool.Name].Add(i); //fills it with ordered integers
                    }
					for (var i = 0; i < Order[pool.Name].Count; i++)
					{
						int j = Main.rand.Next(i + 1);
						(Order[pool.Name][i], Order[pool.Name][j]) = (Order[pool.Name][j], Order[pool.Name][i]);
                    }
                }
            }
        }

		// Here is where we actually 'roll' the contents of the shop
		public List<Item> GenerateNewInventoryList() {
			var items = new List<Item>();

			if (Pools.Count == 0) return items;

			foreach (var pool in Pools)
			{
				for (int i = 0; i < pool.Slots; i++)
				{
					if (pool.Ordered && (!Order.TryGetValue(pool.Name, out List<int> value) || value.Count <= 0))
					{  //Rerolls when it reaches the end of the ordered list
						RerollOrder(pool.Name);
					}
					Item newItem = pool.PickNextItem();
					if (newItem != null) items.Add(newItem);
				}
			}
            return items;
		}

		public override void FillShop(ICollection<Item> items, NPC npc) {
			// use the items which were selected when the NPC spawned.
			foreach (var item in PokemonProfessor.shopItems) {
				// make sure to add a clone of the item, in case any ModifyActiveShop hooks adjust the item when the shop is opened
				items.Add(item.Clone());
			}
		}

		public override void FillShop(Item[] items, NPC npc, out bool overflow) {
			overflow = false;
			int i = 0;
			// use the items which were selected when the NPC spawned.
			foreach (var item in PokemonProfessor.shopItems) {

				if (i == items.Length - 1) {
					// leave the last slot empty for selling
					overflow = true;
					return;
				}

				// make sure to add a clone of the item, in case any ModifyActiveShop hooks adjust the item when the shop is opened
				items[i++] = item.Clone();
			}
		}
	}
}
